drop view if exists "public"."prospera_comissions";

drop function if exists "public"."comission_status"(p_police record);

set check_function_bodies = off;

CREATE OR REPLACE FUNCTION public.commission_status(p_police record)
 RETURNS text
 LANGUAGE plpgsql
 SET search_path TO 'public'
AS $function$declare
  is_paid boolean;
begin
  select exists (
    select 1
    from paid_receipt pr
    where pr.police = p_police.police
      and pr.receipt = p_police.receipt
  ) into is_paid;

  if is_paid and p_police.receipt_status = 'APLICADO' then
    return 'COMPLETED';
  else
    return 'PENDING';
  end if;
end;$function$
;

create or replace view "public"."prospera_commissions" as  SELECT p."POLIZA" AS police,
    r."NUMERO_RECIBO" AS receipt,
    public.commission_status(( SELECT t.*::record AS t
           FROM ( SELECT p."POLIZA" AS police,
                    r."NUMERO_RECIBO" AS receipt,
                    r."ESTATUS_RECIBO" AS receipt_status) t)) AS status,
    ((public.get_percentage_office((p."INI_VIG_PLAN")::timestamp without time zone))::double precision * p."PRIMA_ANUAL") AS comission_forcast,
    pr.amount AS commission_received
   FROM ((public.polices p
     LEFT JOIN public.receipts r ON ((p."POLIZA" = r."POLIZA")))
     LEFT JOIN public.paid_receipt pr ON ((r."NUMERO_RECIBO" = pr.receipt)));



