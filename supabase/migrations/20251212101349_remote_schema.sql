revoke delete on table "public"."comission_data" from "anon";

revoke insert on table "public"."comission_data" from "anon";

revoke references on table "public"."comission_data" from "anon";

revoke select on table "public"."comission_data" from "anon";

revoke trigger on table "public"."comission_data" from "anon";

revoke truncate on table "public"."comission_data" from "anon";

revoke update on table "public"."comission_data" from "anon";

revoke delete on table "public"."comission_data" from "authenticated";

revoke insert on table "public"."comission_data" from "authenticated";

revoke references on table "public"."comission_data" from "authenticated";

revoke select on table "public"."comission_data" from "authenticated";

revoke trigger on table "public"."comission_data" from "authenticated";

revoke truncate on table "public"."comission_data" from "authenticated";

revoke update on table "public"."comission_data" from "authenticated";

revoke delete on table "public"."comission_data" from "service_role";

revoke insert on table "public"."comission_data" from "service_role";

revoke references on table "public"."comission_data" from "service_role";

revoke select on table "public"."comission_data" from "service_role";

revoke trigger on table "public"."comission_data" from "service_role";

revoke truncate on table "public"."comission_data" from "service_role";

revoke update on table "public"."comission_data" from "service_role";

revoke delete on table "public"."comission_office_data" from "anon";

revoke insert on table "public"."comission_office_data" from "anon";

revoke references on table "public"."comission_office_data" from "anon";

revoke select on table "public"."comission_office_data" from "anon";

revoke trigger on table "public"."comission_office_data" from "anon";

revoke truncate on table "public"."comission_office_data" from "anon";

revoke update on table "public"."comission_office_data" from "anon";

revoke delete on table "public"."comission_office_data" from "authenticated";

revoke insert on table "public"."comission_office_data" from "authenticated";

revoke references on table "public"."comission_office_data" from "authenticated";

revoke select on table "public"."comission_office_data" from "authenticated";

revoke trigger on table "public"."comission_office_data" from "authenticated";

revoke truncate on table "public"."comission_office_data" from "authenticated";

revoke update on table "public"."comission_office_data" from "authenticated";

revoke delete on table "public"."comission_office_data" from "service_role";

revoke insert on table "public"."comission_office_data" from "service_role";

revoke references on table "public"."comission_office_data" from "service_role";

revoke select on table "public"."comission_office_data" from "service_role";

revoke trigger on table "public"."comission_office_data" from "service_role";

revoke truncate on table "public"."comission_office_data" from "service_role";

revoke update on table "public"."comission_office_data" from "service_role";

drop view if exists "public"."prospera_commissions";

alter table "public"."comission_data" drop constraint "comission_data_pkey";

alter table "public"."comission_office_data" drop constraint "comission_office_data_pkey";

drop index if exists "public"."comission_data_pkey";

drop index if exists "public"."comission_office_data_pkey";

drop table "public"."comission_data";

drop table "public"."comission_office_data";


  create table "public"."commission_data" (
    "month_range" int4range not null,
    "sales_limit" int8range not null,
    "percentage_agente" real,
    "percentage_manager" real generated always as (
CASE
    WHEN ((month_range @> 1) AND (month_range @> 12)) THEN abs((percentage_agente - (0.45)::double precision))
    WHEN ((month_range @> 13) AND (month_range @> 24)) THEN abs((percentage_agente - (0.04)::double precision))
    ELSE (0)::double precision
END) stored
      );


alter table "public"."commission_data" enable row level security;


  create table "public"."commission_office_data" (
    "month_range" int4range not null,
    "percentage" real
      );


alter table "public"."commission_office_data" enable row level security;

CREATE UNIQUE INDEX comission_data_pkey ON public.commission_data USING btree (month_range, sales_limit);

CREATE UNIQUE INDEX comission_office_data_pkey ON public.commission_office_data USING btree (month_range);

alter table "public"."commission_data" add constraint "comission_data_pkey" PRIMARY KEY using index "comission_data_pkey";

alter table "public"."commission_office_data" add constraint "comission_office_data_pkey" PRIMARY KEY using index "comission_office_data_pkey";

set check_function_bodies = off;

CREATE OR REPLACE FUNCTION public.get_percentage_data(p_creation_date timestamp without time zone, sales integer, p_type text)
 RETURNS numeric
 LANGUAGE plpgsql
AS $function$
declare
  diff interval;
  months_diff int;
  commission commission_data;
begin
  diff = age(p_creation_date, now());
  months_diff:= round(abs(EXTRACT(EPOCH from diff) / 60 / 60 / 24 / 30.0));

  if p_type = 'agent' then
    select * into commission from commission_data where month_range @> months_diff limit 1;
    return commission.percentage_agente;
  end if;
  if p_type = 'manager' then
    select * into commission from commission_data where month_range @> months_diff limit 1;
    return commission.percentage_manager;
  end if;

  return 0;

end;
$function$
;

CREATE OR REPLACE FUNCTION public.get_percentage_office(p_creation_date timestamp without time zone)
 RETURNS numeric
 LANGUAGE plpgsql
AS $function$
declare
  diff interval;
  months_diff int;
  commission_office commission_office_data;
begin
  diff = age(p_creation_date, now());
  months_diff:= round(abs(EXTRACT(EPOCH from diff) / 60 / 60 / 24 / 30.0));

  select * into commission_office from commission_office_data where month_range @> months_diff limit 1;
  return commission_office.percentage;
end;
$function$
;

create or replace view "public"."prospera_commissions" as  SELECT p."POLIZA" AS police,
    r."NUMERO_RECIBO" AS receipt,
    public.commission_status(( SELECT t.*::record AS t
           FROM ( SELECT p."POLIZA" AS police,
                    r."NUMERO_RECIBO" AS receipt,
                    r."ESTATUS_RECIBO" AS receipt_status) t)) AS status,
    ((public.get_percentage_office((p."INI_VIG_PLAN")::timestamp without time zone))::double precision * p."PRIMA_ANUAL") AS commission_forcast,
    pr.amount AS commission_received
   FROM ((public.polices p
     LEFT JOIN public.receipts r ON ((p."POLIZA" = r."POLIZA")))
     LEFT JOIN public.paid_receipt pr ON ((r."NUMERO_RECIBO" = pr.receipt)));


grant delete on table "public"."commission_data" to "anon";

grant insert on table "public"."commission_data" to "anon";

grant references on table "public"."commission_data" to "anon";

grant select on table "public"."commission_data" to "anon";

grant trigger on table "public"."commission_data" to "anon";

grant truncate on table "public"."commission_data" to "anon";

grant update on table "public"."commission_data" to "anon";

grant delete on table "public"."commission_data" to "authenticated";

grant insert on table "public"."commission_data" to "authenticated";

grant references on table "public"."commission_data" to "authenticated";

grant select on table "public"."commission_data" to "authenticated";

grant trigger on table "public"."commission_data" to "authenticated";

grant truncate on table "public"."commission_data" to "authenticated";

grant update on table "public"."commission_data" to "authenticated";

grant delete on table "public"."commission_data" to "service_role";

grant insert on table "public"."commission_data" to "service_role";

grant references on table "public"."commission_data" to "service_role";

grant select on table "public"."commission_data" to "service_role";

grant trigger on table "public"."commission_data" to "service_role";

grant truncate on table "public"."commission_data" to "service_role";

grant update on table "public"."commission_data" to "service_role";

grant delete on table "public"."commission_office_data" to "anon";

grant insert on table "public"."commission_office_data" to "anon";

grant references on table "public"."commission_office_data" to "anon";

grant select on table "public"."commission_office_data" to "anon";

grant trigger on table "public"."commission_office_data" to "anon";

grant truncate on table "public"."commission_office_data" to "anon";

grant update on table "public"."commission_office_data" to "anon";

grant delete on table "public"."commission_office_data" to "authenticated";

grant insert on table "public"."commission_office_data" to "authenticated";

grant references on table "public"."commission_office_data" to "authenticated";

grant select on table "public"."commission_office_data" to "authenticated";

grant trigger on table "public"."commission_office_data" to "authenticated";

grant truncate on table "public"."commission_office_data" to "authenticated";

grant update on table "public"."commission_office_data" to "authenticated";

grant delete on table "public"."commission_office_data" to "service_role";

grant insert on table "public"."commission_office_data" to "service_role";

grant references on table "public"."commission_office_data" to "service_role";

grant select on table "public"."commission_office_data" to "service_role";

grant trigger on table "public"."commission_office_data" to "service_role";

grant truncate on table "public"."commission_office_data" to "service_role";

grant update on table "public"."commission_office_data" to "service_role";


