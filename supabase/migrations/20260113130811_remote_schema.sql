create extension if not exists "wrappers" with schema "extensions";

revoke delete on table "public"."agente_assets" from "anon";

revoke insert on table "public"."agente_assets" from "anon";

revoke references on table "public"."agente_assets" from "anon";

revoke select on table "public"."agente_assets" from "anon";

revoke trigger on table "public"."agente_assets" from "anon";

revoke truncate on table "public"."agente_assets" from "anon";

revoke update on table "public"."agente_assets" from "anon";

revoke delete on table "public"."agente_assets" from "authenticated";

revoke insert on table "public"."agente_assets" from "authenticated";

revoke references on table "public"."agente_assets" from "authenticated";

revoke select on table "public"."agente_assets" from "authenticated";

revoke trigger on table "public"."agente_assets" from "authenticated";

revoke truncate on table "public"."agente_assets" from "authenticated";

revoke update on table "public"."agente_assets" from "authenticated";

revoke delete on table "public"."agente_assets" from "service_role";

revoke insert on table "public"."agente_assets" from "service_role";

revoke references on table "public"."agente_assets" from "service_role";

revoke select on table "public"."agente_assets" from "service_role";

revoke trigger on table "public"."agente_assets" from "service_role";

revoke truncate on table "public"."agente_assets" from "service_role";

revoke update on table "public"."agente_assets" from "service_role";

revoke delete on table "public"."agentes" from "anon";

revoke insert on table "public"."agentes" from "anon";

revoke references on table "public"."agentes" from "anon";

revoke select on table "public"."agentes" from "anon";

revoke trigger on table "public"."agentes" from "anon";

revoke truncate on table "public"."agentes" from "anon";

revoke update on table "public"."agentes" from "anon";

revoke delete on table "public"."agentes" from "authenticated";

revoke insert on table "public"."agentes" from "authenticated";

revoke references on table "public"."agentes" from "authenticated";

revoke select on table "public"."agentes" from "authenticated";

revoke trigger on table "public"."agentes" from "authenticated";

revoke truncate on table "public"."agentes" from "authenticated";

revoke update on table "public"."agentes" from "authenticated";

revoke delete on table "public"."agentes" from "service_role";

revoke insert on table "public"."agentes" from "service_role";

revoke references on table "public"."agentes" from "service_role";

revoke select on table "public"."agentes" from "service_role";

revoke trigger on table "public"."agentes" from "service_role";

revoke truncate on table "public"."agentes" from "service_role";

revoke update on table "public"."agentes" from "service_role";

alter table "public"."agentes" drop constraint "agentes_Gerencia Buena_fkey";

alter table "public"."agente_assets" drop constraint "agente_assets_pkey";

alter table "public"."agentes" drop constraint "agentes_pkey";

drop index if exists "public"."agentes_pkey";

drop index if exists "public"."agente_assets_pkey";

drop table "public"."agente_assets";

drop table "public"."agentes";


  create table "public"."agents" (
    "id" uuid not null default gen_random_uuid(),
    "created_at" timestamp with time zone not null default now(),
    "key" text,
    "photo" text,
    "credentials" text
      );


alter table "public"."agents" enable row level security;

CREATE UNIQUE INDEX agente_assets_pkey ON public.agents USING btree (id);

alter table "public"."agents" add constraint "agente_assets_pkey" PRIMARY KEY using index "agente_assets_pkey";

grant delete on table "public"."agents" to "anon";

grant insert on table "public"."agents" to "anon";

grant references on table "public"."agents" to "anon";

grant select on table "public"."agents" to "anon";

grant trigger on table "public"."agents" to "anon";

grant truncate on table "public"."agents" to "anon";

grant update on table "public"."agents" to "anon";

grant delete on table "public"."agents" to "authenticated";

grant insert on table "public"."agents" to "authenticated";

grant references on table "public"."agents" to "authenticated";

grant select on table "public"."agents" to "authenticated";

grant trigger on table "public"."agents" to "authenticated";

grant truncate on table "public"."agents" to "authenticated";

grant update on table "public"."agents" to "authenticated";

grant delete on table "public"."agents" to "service_role";

grant insert on table "public"."agents" to "service_role";

grant references on table "public"."agents" to "service_role";

grant select on table "public"."agents" to "service_role";

grant trigger on table "public"."agents" to "service_role";

grant truncate on table "public"."agents" to "service_role";

grant update on table "public"."agents" to "service_role";


