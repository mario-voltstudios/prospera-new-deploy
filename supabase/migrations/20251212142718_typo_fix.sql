do $$
begin
  -- rename only when the old table exists and the target name is still free
  if to_regclass('public.paid_receipt') is not null
     and to_regclass('public.paid_receipts') is null then
    alter table public.paid_receipt rename to paid_receipts;
  end if;
end;
$$;

do $$
declare
  view_owner text;
  is_super  boolean;
begin
  select pg_get_userbyid(c.relowner)
    into view_owner
  from pg_class c
  join pg_namespace n on n.oid = c.relnamespace
  where n.nspname = 'public'
    and c.relname = 'prospera_commissions'
    and c.relkind in ('v', 'm');

  select rolsuper into is_super from pg_roles where rolname = current_user;

  if view_owner is null then
    -- No existing view, just create it.
    execute format($sql$
      create view public.prospera_commissions with (security_invoker = on) as
      select 
        p."POLIZA" as police,
        r."NUMERO_RECIBO" as receipt,
        public.commission_status(
          (
            select t
            from (
              select
                p."POLIZA"       as police,
                r."NUMERO_RECIBO" as receipt,
                r."ESTATUS_RECIBO" as receipt_status
            ) as t
          )
        ) as status,
        public.get_percentage_office(p."INI_VIG_PLAN"::timestamp) * p."PRIMA_ANUAL" as commission_forcast,
        pr.amount as commission_received
      from polices p
      left join receipts r on p."POLIZA" = r."POLIZA"
      left join public.paid_receipts pr on r."NUMERO_RECIBO" = pr.receipt;
    $sql$);
  elsif view_owner = current_user or coalesce(is_super, false) then
    -- We own it (or are superuser), replace it directly.
    execute format($sql$
      create or replace view public.prospera_commissions with (security_invoker = on) as
      select 
        p."POLIZA" as police,
        r."NUMERO_RECIBO" as receipt,
        public.commission_status(
          (
            select t
            from (
              select
                p."POLIZA"       as police,
                r."NUMERO_RECIBO" as receipt,
                r."ESTATUS_RECIBO" as receipt_status
            ) as t
          )
        ) as status,
        public.get_percentage_office(p."INI_VIG_PLAN"::timestamp) * p."PRIMA_ANUAL" as commission_forcast,
        pr.amount as commission_received
      from polices p
      left join receipts r on p."POLIZA" = r."POLIZA"
      left join public.paid_receipts pr on r."NUMERO_RECIBO" = pr.receipt;
    $sql$);
  else
    -- Try to take ownership so we can replace; if not permitted, skip without failing.
    begin
      execute 'alter view public.prospera_commissions owner to ' || quote_ident(current_user);
      execute format($sql$
        create or replace view public.prospera_commissions with (security_invoker = on) as
        select 
          p."POLIZA" as police,
          r."NUMERO_RECIBO" as receipt,
          public.commission_status(
            (
              select t
              from (
                select
                  p."POLIZA"       as police,
                  r."NUMERO_RECIBO" as receipt,
                  r."ESTATUS_RECIBO" as receipt_status
              ) as t
            )
          ) as status,
          public.get_percentage_office(p."INI_VIG_PLAN"::timestamp) * p."PRIMA_ANUAL" as commission_forcast,
          pr.amount as commission_received
        from polices p
        left join receipts r on p."POLIZA" = r."POLIZA"
        left join public.paid_receipts pr on r."NUMERO_RECIBO" = pr.receipt;
      $sql$);
    exception when others then
      raise notice 'Skipping update of view public.prospera_commissions: owned by %, current user %', view_owner, current_user;
    end;
  end if;
end;
$$;

do $$
declare
  fn_oid   oid;
  fn_owner text;
  is_super boolean;
begin
  fn_oid := to_regprocedure('public.commission_status(record)');
  if fn_oid is not null then
    select pg_get_userbyid(proowner) into fn_owner from pg_proc where oid = fn_oid;
  end if;

  select rolsuper into is_super from pg_roles where rolname = current_user;

  if fn_oid is null then
    -- Function does not exist yet
    execute format($fn$
      create function public.commission_status(p_police record)
      returns text language plpgsql as $body$
      declare
        is_paid boolean;
      begin
        select exists (
          select 1
          from paid_receipts pr
          where pr.police = p_police.police
            and pr.receipt = p_police.receipt
        ) into is_paid;

        if is_paid and p_police.receipt_status = 'APLICADO' then
          return 'COMPLETED';
        else
          return 'PENDING';
        end if;
      end;
      $body$;
    $fn$);
  elsif fn_owner = current_user or coalesce(is_super, false) then
    -- We own it (or are superuser), replace it directly.
    execute format($fn$
      create or replace function public.commission_status(p_police record)
      returns text language plpgsql as $body$
      declare
        is_paid boolean;
      begin
        select exists (
          select 1
          from paid_receipts pr
          where pr.police = p_police.police
            and pr.receipt = p_police.receipt
        ) into is_paid;

        if is_paid and p_police.receipt_status = 'APLICADO' then
          return 'COMPLETED';
        else
          return 'PENDING';
        end if;
      end;
      $body$;
    $fn$);
  else
    -- Try to take ownership so we can replace; if not permitted, skip without failing.
    begin
      execute 'alter function public.commission_status(record) owner to ' || quote_ident(current_user);
      execute format($fn$
        create or replace function public.commission_status(p_police record)
        returns text language plpgsql as $body$
        declare
          is_paid boolean;
        begin
          select exists (
            select 1
            from paid_receipts pr
            where pr.police = p_police.police
              and pr.receipt = p_police.receipt
          ) into is_paid;

          if is_paid and p_police.receipt_status = 'APLICADO' then
            return 'COMPLETED';
          else
            return 'PENDING';
          end if;
        end;
        $body$;
      $fn$);
    exception when others then
      raise notice 'Skipping update of function public.commission_status(record): owned by %, current user %', fn_owner, current_user;
    end;
  end if;
end;
$$;

