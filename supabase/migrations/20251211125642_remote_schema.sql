


SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;


COMMENT ON SCHEMA "public" IS 'standard public schema';



CREATE EXTENSION IF NOT EXISTS "pg_graphql" WITH SCHEMA "graphql";


CREATE EXTENSION IF NOT EXISTS "pg_stat_statements" WITH SCHEMA "extensions";


CREATE EXTENSION IF NOT EXISTS "pgcrypto" WITH SCHEMA "extensions";


CREATE EXTENSION IF NOT EXISTS "supabase_vault" WITH SCHEMA "vault";


CREATE EXTENSION IF NOT EXISTS "uuid-ossp" WITH SCHEMA "extensions";


CREATE TYPE "public"."ChargeBy" AS ENUM (
    'BANCO',
    'AGENTE'
);


ALTER TYPE "public"."ChargeBy" OWNER TO "postgres";


CREATE TYPE "public"."Currency" AS ENUM (
    'PESOS',
    'DOLARES'
);


ALTER TYPE "public"."Currency" OWNER TO "postgres";


CREATE TYPE "public"."PaymentType" AS ENUM (
    'MENSUAL',
    'ANUAL'
);


ALTER TYPE "public"."PaymentType" OWNER TO "postgres";


CREATE OR REPLACE FUNCTION "public"."comission_status"("p_police" "record") RETURNS "text"
    LANGUAGE "plpgsql"
    AS $$
declare
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
end;
$$;


ALTER FUNCTION "public"."comission_status"("p_police" "record") OWNER TO "postgres";


CREATE OR REPLACE FUNCTION "public"."get_percentage_data"("p_creation_date" timestamp without time zone, "sales" integer, "p_type" "text") RETURNS numeric
    LANGUAGE "plpgsql"
    AS $$
declare
  diff interval;
  months_diff int;
  comission_office comission_office_data;
  comission comission_data;
begin
  diff = age(p_creation_date, now());
  months_diff:= round(abs(EXTRACT(EPOCH from diff) / 60 / 60 / 24 / 30.0));

  if p_type = 'agent' then
    select * into comission from comission_data where month_range @> months_diff limit 1;
    return comission.percentage_agente;
  end if;
  if p_type = 'manager' then
    select * into comission from comission_data where month_range @> months_diff limit 1;
    return comission.percentage_manager;
  end if;

  return 0;

end;
$$;


ALTER FUNCTION "public"."get_percentage_data"("p_creation_date" timestamp without time zone, "sales" integer, "p_type" "text") OWNER TO "postgres";


CREATE OR REPLACE FUNCTION "public"."get_percentage_office"("p_creation_date" timestamp without time zone) RETURNS numeric
    LANGUAGE "plpgsql"
    AS $$
declare
  diff interval;
  months_diff int;
  comission_office comission_office_data;
  comission comission_data;
begin
  diff = age(p_creation_date, now());
  months_diff:= round(abs(EXTRACT(EPOCH from diff) / 60 / 60 / 24 / 30.0));

  select * into comission_office from comission_office_data where month_range @> months_diff limit 1;
  return comission_office.percentage;
end;
$$;


ALTER FUNCTION "public"."get_percentage_office"("p_creation_date" timestamp without time zone) OWNER TO "postgres";


CREATE OR REPLACE FUNCTION "public"."handle_csv_upsert"() RETURNS "trigger"
    LANGUAGE "plpgsql"
    AS $$
BEGIN
  -- 1. Check for existing record using the Unique Key
  IF EXISTS (SELECT 1 FROM public.receipts WHERE "NUMERO_RECIBO" = NEW."NUMERO_RECIBO") THEN
    
    -- 2. IF FOUND: Update the existing record with all new CSV data
    UPDATE public.receipts
    SET 
      "POLIZA" = NEW."POLIZA",
      "INI_VIG_PLAN" = NEW."INI_VIG_PLAN",
      "FIN_VIG_PLAN" = NEW."FIN_VIG_PLAN",
      "INI_VIG_ANUALIDAD" = NEW."INI_VIG_ANUALIDAD",
      "FIN_VIG_ANUALIDAD" = NEW."FIN_VIG_ANUALIDAD",
      "PLAN" = NEW."PLAN",
      "ESTATUS_POLIZA" = NEW."ESTATUS_POLIZA",
      "PRIMA_ANUAL" = NEW."PRIMA_ANUAL",
      "MONEDA" = NEW."MONEDA",
      "FORMA_DE_PAGO" = NEW."FORMA_DE_PAGO",
      "CONDUCTO_COBRO" = NEW."CONDUCTO_COBRO",
      "SUMA_ASEGURADA" = NEW."SUMA_ASEGURADA",
      "AÑO_POLIZA" = NEW."AÑO_POLIZA",
      "CVE_CONTRATANTE" = NEW."CVE_CONTRATANTE",
      "CONTRATANTE" = NEW."CONTRATANTE",
      "DIRECCION_CONT" = NEW."DIRECCION_CONT",
      "TEL_CASA_CONT" = NEW."TEL_CASA_CONT",
      "TEL_OFICINA_CONT" = NEW."TEL_OFICINA_CONT",
      "FECHA_NAC_CONT" = NEW."FECHA_NAC_CONT",
      "CVE_ASEGURADO" = NEW."CVE_ASEGURADO",
      "ASEGURADO" = NEW."ASEGURADO",
      "DIRECCION_ASEG" = NEW."DIRECCION_ASEG",
      "TEL_CASA_ASEG" = NEW."TEL_CASA_ASEG",
      "TEL_OFICINA_ASEG" = NEW."TEL_OFICINA_ASEG",
      "FECHA_NAC_ASEG" = NEW."FECHA_NAC_ASEG",
      "ESTATUS_RECIBO" = NEW."ESTATUS_RECIBO",
      "FECHA_PROG_PAGO" = NEW."FECHA_PROG_PAGO",
      "FECHA_COBRO" = NEW."FECHA_COBRO",
      "IMPORTE_DEL_RECIBO" = NEW."IMPORTE_DEL_RECIBO",
      "VALOR_GARANTIZADO_ACTUAL" = NEW."VALOR_GARANTIZADO_ACTUAL",
      "VALOR_GARANTIZADO_PROX" = NEW."VALOR_GARANTIZADO_PROX",
      "PRESTAMOS" = NEW."PRESTAMOS",
      "FONDOS_ADMON" = NEW."FONDOS_ADMON",
      "DCP" = NEW."DCP",
      "FONDOS_DE_LA_POLIZA" = NEW."FONDOS_DE_LA_POLIZA",
      "FONDO_PARA_DEDUCCIONES" = NEW."FONDO_PARA_DEDUCCIONES",
      "VALOR_RESCATE" = NEW."VALOR_RESCATE",
      "ULTIMA_DEDUCCION" = NEW."ULTIMA_DEDUCCION",
      "BENEFICIOS_ADICIONALES" = NEW."BENEFICIOS_ADICIONALES"
    WHERE "NUMERO_RECIBO" = NEW."NUMERO_RECIBO";

    -- 3. Return NULL to cancel the current row's INSERT operation
    RETURN NULL;
    
  ELSE
    -- 4. IF NOT FOUND: Proceed with the standard INSERT
    RETURN NEW;
  END IF;
END;
$$;


ALTER FUNCTION "public"."handle_csv_upsert"() OWNER TO "postgres";


CREATE OR REPLACE FUNCTION "public"."handle_polices_upsert"() RETURNS "trigger"
    LANGUAGE "plpgsql"
    AS $$
BEGIN
  -- 1. Check for existing record using the Unique Key (POLIZA)
  IF EXISTS (SELECT 1 FROM public.polices WHERE "POLIZA" = NEW."POLIZA") THEN
    
    -- 2. IF FOUND: Update the existing record with all new CSV data
    UPDATE public.polices
    SET 
      "INI_VIG_PLAN" = NEW."INI_VIG_PLAN",
      "FIN_VIG_PLAN" = NEW."FIN_VIG_PLAN",
      "INI_VIG_ANUALIDAD" = NEW."INI_VIG_ANUALIDAD",
      "FIN_VIG_ANUALIDAD" = NEW."FIN_VIG_ANUALIDAD",
      "PLAN" = NEW."PLAN",
      "ESTATUS_POLIZA" = NEW."ESTATUS_POLIZA",
      "PRIMA_ANUAL" = NEW."PRIMA_ANUAL",
      "MONEDA" = NEW."MONEDA",
      "FORMA_DE_PAGO" = NEW."FORMA_DE_PAGO",
      "CONDUCTO_COBRO" = NEW."CONDUCTO_COBRO",
      "SUMA_ASEGURADA" = NEW."SUMA_ASEGURADA",
      "AÑO_POLIZA" = NEW."AÑO_POLIZA",
      "CVE_CONTRATANTE" = NEW."CVE_CONTRATANTE",
      "CONTRATANTE" = NEW."CONTRATANTE",
      "DIRECCION_CONT" = NEW."DIRECCION_CONT",
      "TEL_CASA_CONT" = NEW."TEL_CASA_CONT",
      "TEL_OFICINA_CONT" = NEW."TEL_OFICINA_CONT",
      "FECHA_NAC_CONT" = NEW."FECHA_NAC_CONT",
      "CVE_ASEGURADO" = NEW."CVE_ASEGURADO",
      "ASEGURADO" = NEW."ASEGURADO",
      "DIRECCION_ASEG" = NEW."DIRECCION_ASEG",
      "TEL_CASA_ASEG" = NEW."TEL_CASA_ASEG",
      "TEL_OFICINA_ASEG" = NEW."TEL_OFICINA_ASEG",
      "FECHA_NAC_ASEG" = NEW."FECHA_NAC_ASEG",
      "VALOR_GARANTIZADO_ACTUAL" = NEW."VALOR_GARANTIZADO_ACTUAL",
      "VALOR_GARANTIZADO_PROX" = NEW."VALOR_GARANTIZADO_PROX",
      "PRESTAMOS" = NEW."PRESTAMOS",
      "FONDOS_ADMON" = NEW."FONDOS_ADMON",
      "DCP" = NEW."DCP",
      "FONDOS_DE_LA_POLIZA" = NEW."FONDOS_DE_LA_POLIZA",
      "FONDO_PARA_DEDUCCIONES" = NEW."FONDO_PARA_DEDUCCIONES",
      "VALOR_RESCATE" = NEW."VALOR_RESCATE",
      "ULTIMA_DEDUCCION" = NEW."ULTIMA_DEDUCCION",
      "BENEFICIOS_ADICIONALES" = NEW."BENEFICIOS_ADICIONALES"
      -- NOTE: "POLIZA" is used in the WHERE clause, no need to update it here.
    WHERE "POLIZA" = NEW."POLIZA";

    -- 3. Return NULL to cancel the current row's INSERT operation
    RETURN NULL;
    
  ELSE
    -- 4. IF NOT FOUND: Proceed with the standard INSERT
    RETURN NEW;
  END IF;
END;
$$;


ALTER FUNCTION "public"."handle_polices_upsert"() OWNER TO "postgres";


CREATE OR REPLACE FUNCTION "public"."validate_and_insert_parent_records"() RETURNS "trigger"
    LANGUAGE "plpgsql"
    AS $$
BEGIN
    -- 1. Check if the related POLIZA exists in the polices table
    IF NOT EXISTS (SELECT 1 FROM public.polices WHERE "POLIZA" = NEW."NumeroDePoliza") THEN
        -- Insert a placeholder record into polices, setting all non-key fields to NULL
        INSERT INTO public.polices (
            "POLIZA", "INI_VIG_PLAN", "FIN_VIG_PLAN", "INI_VIG_ANUALIDAD", "FIN_VIG_ANUALIDAD", "PLAN", "ESTATUS_POLIZA", "PRIMA_ANUAL", "MONEDA", "FORMA_DE_PAGO", 
            "CONDUCTO_COBRO", "SUMA_ASEGURADA", "AÑO_POLIZA", "CVE_CONTRATANTE", "CONTRATANTE", "DIRECCION_CONT", "TEL_CASA_CONT", "TEL_OFICINA_CONT", "FECHA_NAC_CONT", 
            "CVE_ASEGURADO", "ASEGURADO", "DIRECCION_ASEG", "TEL_CASA_ASEG", "TEL_OFICINA_ASEG", "FECHA_NAC_ASEG", "VALOR_GARANTIZADO_ACTUAL", "VALOR_GARANTIZADO_PROX", 
            "PRESTAMOS", "FONDOS_ADMON", "DCP", "FONDOS_DE_LA_POLIZA", "FONDO_PARA_DEDUCCIONES", "VALOR_RESCATE", "ULTIMA_DEDUCCION", "BENEFICIOS_ADICIONALES"
        )
        VALUES (
            NEW."NumeroDePoliza", NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
            NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
            NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
            NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL
        );
    END IF;

    -- 2. Check if the related NUMERO_RECIBO exists in the receipts table
    IF NOT EXISTS (SELECT 1 FROM public.receipts WHERE "NUMERO_RECIBO" = NEW."NumeroDeRecibo") THEN
        -- Insert a placeholder record into receipts, setting all non-key/non-link fields to NULL
        INSERT INTO public.receipts (
            "NUMERO_RECIBO", "POLIZA", "INI_VIG_PLAN", "FIN_VIG_PLAN", "INI_VIG_ANUALIDAD", "FIN_VIG_ANUALIDAD", "PLAN", "ESTATUS_POLIZA", "PRIMA_ANUAL", "MONEDA", 
            "FORMA_DE_PAGO", "CONDUCTO_COBRO", "SUMA_ASEGURADA", "AÑO_POLIZA", "CVE_CONTRATANTE", "CONTRATANTE", "DIRECCION_CONT", "TEL_CASA_CONT", "TEL_OFICINA_CONT", 
            "FECHA_NAC_CONT", "CVE_ASEGURADO", "ASEGURADO", "DIRECCION_ASEG", "TEL_CASA_ASEG", "TEL_OFICINA_ASEG", "ESTATUS_RECIBO", "FECHA_PROG_PAGO", "FECHA_COBRO", 
            "IMPORTE_DEL_RECIBO", "VALOR_GARANTIZADO_ACTUAL", "VALOR_GARANTIZADO_PROX", "PRESTAMOS", "FONDOS_ADMON", "DCP", "FONDOS_DE_LA_POLIZA", "FONDO_PARA_DEDUCCIONES", 
            "VALOR_RESCATE", "ULTIMA_DEDUCCION", "BENEFICIOS_ADICIONALES"
        )
        VALUES (
            NEW."NumeroDeRecibo", NEW."NumeroDePoliza", NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
            NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
            NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
            NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 
            NULL, NULL, NULL
        );
    END IF;

    -- 3. Proceed with the original insert into comisiones_GNP
    RETURN NEW;
END;
$$;


ALTER FUNCTION "public"."validate_and_insert_parent_records"() OWNER TO "postgres";

SET default_tablespace = '';

SET default_table_access_method = "heap";


CREATE TABLE IF NOT EXISTS "public"."agente_assets" (
    "id" "uuid" DEFAULT "gen_random_uuid"() NOT NULL,
    "created_at" timestamp with time zone DEFAULT "now"() NOT NULL,
    "agent" "text",
    "photo" "text",
    "credentials" "text"
);


ALTER TABLE "public"."agente_assets" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."agentes" (
    "Clave" "text" NOT NULL,
    "Nombre Completo" "text",
    "FOTO" "text",
    "CREDENCIAL" "text",
    "Date created" "date",
    "Status" "text",
    "Tipo de Agente o Lider" "text",
    "CELULAR" "text",
    "Correo" "text",
    "Entrevistado Gris" "text",
    "Grupo Generado" "text",
    "Talla Uniforme" "text",
    "Sexo" "text",
    "RFC" "text",
    "Fecha de Nacimiento2" "date",
    "Estado o CDMX" "text",
    "Date credencial added" "date",
    "Necesita Credencial" "text",
    "Tarjeta de Presentacion" "text",
    "Gerencia Buena" "text",
    "Latitud" "text",
    "Longitud" "text",
    "Cuenta Clabe" "text",
    "RESICO" "text",
    "RESICO SUSTENTO" "text",
    "Banco" "text",
    "CURP" "text",
    "Bonos copy" "text",
    "URL para administracion" "text",
    "¿Tienes Cedula de Agente de Seguros Vigente ?" "text",
    "¿Del 1 al 10 , 10 siendo excelente y 1 siendo nulo como consid" smallint,
    "¿Del 1 al 10 _1, 10 siendo excelente y 1 siendo nulo como cons" smallint,
    "¿Del 1 al 10 _2, 10 siendo excelente y 1 siendo nulo como cons" smallint,
    "¿Del 1 al 10 _3, 10 siendo excelente y 1 siendo nulo como cons" smallint,
    "Porque apasiona seguros" "text",
    "Experiencia en seguros" "text",
    "Credencial Modificadoa" "text",
    "Fecha credencial impresa" "date",
    "Fecha credencial entregada" "date",
    "Persona que entrego credencial" "text",
    "Prioridad de credencial" "text"
);


ALTER TABLE "public"."agentes" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."comission_data" (
    "month_range" "int4range" NOT NULL,
    "sales_limit" "int8range" NOT NULL,
    "percentage_agente" real,
    "percentage_manager" real GENERATED ALWAYS AS (
CASE
    WHEN (("month_range" @> 1) AND ("month_range" @> 12)) THEN "abs"(("percentage_agente" - (0.45)::double precision))
    WHEN (("month_range" @> 13) AND ("month_range" @> 24)) THEN "abs"(("percentage_agente" - (0.04)::double precision))
    ELSE (0)::double precision
END) STORED
);


ALTER TABLE "public"."comission_data" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."comission_office_data" (
    "month_range" "int4range" NOT NULL,
    "percentage" real
);


ALTER TABLE "public"."comission_office_data" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."commissions_gnp" (
    "ClaveUnica" bigint,
    "Folio" "text",
    "FolioAgente" "text",
    "Contrato" bigint,
    "FechaDeMovimiento" "text",
    "Ramo" "text",
    "TipoComprobante" "text",
    "ImporteAntesImpuestos" double precision,
    "IVAAcreditado" "text",
    "IVARetenido" "text",
    "ISRRetenido" "text",
    "ImporteDespuesImpuestos" double precision,
    "NumeroDePoliza" bigint,
    "NumeroDeRecibo" bigint,
    "NombreDeContratante" "text"
);


ALTER TABLE "public"."commissions_gnp" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."gerencia" (
    "Nombre Gerencia" "text" NOT NULL
);


ALTER TABLE "public"."gerencia" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."gnp_commission_detail" (
    "id" integer NOT NULL,
    "periodo" "text",
    "clave_unica" "text",
    "folio" "text",
    "contrato" "text",
    "pagina" integer,
    "uen" "text",
    "agente_num" "text",
    "agente_label" "text",
    "fecha_raw" "text",
    "poliza" "text",
    "endoso" "text",
    "vencimiento" "text",
    "modelo_anio" "text",
    "linea" "text",
    "concepto" "text",
    "afecto" "text",
    "porcentaje" numeric,
    "prima_neta" numeric,
    "recargo" numeric,
    "pma_total" numeric,
    "comision" numeric
);


ALTER TABLE "public"."gnp_commission_detail" OWNER TO "postgres";


CREATE SEQUENCE IF NOT EXISTS "public"."gnp_commission_detail_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE "public"."gnp_commission_detail_id_seq" OWNER TO "postgres";


ALTER SEQUENCE "public"."gnp_commission_detail_id_seq" OWNED BY "public"."gnp_commission_detail"."id";



CREATE TABLE IF NOT EXISTS "public"."gnp_statement" (
    "id" integer NOT NULL,
    "periodo" "text",
    "clave_unica" "text",
    "folio" "text",
    "contrato" "text",
    "nombre" "text",
    "rfc" "text",
    "zona" "text",
    "folio_doc" "text",
    "fecha_doc" "text",
    "total_ingresos" numeric,
    "total_impuestos" numeric,
    "total_descuentos" numeric,
    "total_neto" numeric,
    "saldo_anterior" numeric,
    "saldo_disponible" numeric,
    "total_afecto_text" "text"
);


ALTER TABLE "public"."gnp_statement" OWNER TO "postgres";


CREATE SEQUENCE IF NOT EXISTS "public"."gnp_statement_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE "public"."gnp_statement_id_seq" OWNER TO "postgres";


ALTER SEQUENCE "public"."gnp_statement_id_seq" OWNED BY "public"."gnp_statement"."id";



CREATE TABLE IF NOT EXISTS "public"."gnp_statement_movements" (
    "id" integer NOT NULL,
    "statement_id" integer NOT NULL,
    "section_type" "text",
    "concepto" "text",
    "uen" "text",
    "importe" numeric,
    "saldo" numeric,
    "fecha_raw" "text"
);


ALTER TABLE "public"."gnp_statement_movements" OWNER TO "postgres";


CREATE SEQUENCE IF NOT EXISTS "public"."gnp_statement_movements_id_seq"
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE "public"."gnp_statement_movements_id_seq" OWNER TO "postgres";


ALTER SEQUENCE "public"."gnp_statement_movements_id_seq" OWNED BY "public"."gnp_statement_movements"."id";



CREATE TABLE IF NOT EXISTS "public"."paid_receipt" (
    "police" bigint NOT NULL,
    "receipt" bigint NOT NULL,
    "created_at" timestamp with time zone DEFAULT "now"() NOT NULL,
    "amount" double precision DEFAULT '0'::double precision NOT NULL
);


ALTER TABLE "public"."paid_receipt" OWNER TO "postgres";


ALTER TABLE "public"."paid_receipt" ALTER COLUMN "police" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME "public"."paid_receipt_police_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);



ALTER TABLE "public"."paid_receipt" ALTER COLUMN "receipt" ADD GENERATED BY DEFAULT AS IDENTITY (
    SEQUENCE NAME "public"."paid_receipt_receipt_seq"
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1
);



CREATE TABLE IF NOT EXISTS "public"."police_assets" (
    "id" "uuid" DEFAULT "gen_random_uuid"() NOT NULL,
    "created_at" timestamp with time zone DEFAULT "now"() NOT NULL,
    "pdf_poliza" "text",
    "poliza" bigint,
    "solicitud" "text",
    "ine_ife" "text",
    "cartas_autorizacion" "text",
    "talones_de_pago" "text",
    "foto_cliente_con_solicitud" "text"
);


ALTER TABLE "public"."police_assets" OWNER TO "postgres";


CREATE TABLE IF NOT EXISTS "public"."polices" (
    "POLIZA" bigint NOT NULL,
    "INI_VIG_PLAN" "text",
    "FIN_VIG_PLAN" "date",
    "INI_VIG_ANUALIDAD" "date",
    "FIN_VIG_ANUALIDAD" "date",
    "PLAN" "text",
    "ESTATUS_POLIZA" "text",
    "PRIMA_ANUAL" double precision,
    "MONEDA" "text",
    "FORMA_DE_PAGO" "text",
    "CONDUCTO_COBRO" "text",
    "SUMA_ASEGURADA" "text",
    "AÑO_POLIZA" bigint,
    "CVE_CONTRATANTE" bigint,
    "CONTRATANTE" "text",
    "DIRECCION_CONT" "text",
    "TEL_CASA_CONT" "text",
    "TEL_OFICINA_CONT" "text",
    "FECHA_NAC_CONT" "text",
    "CVE_ASEGURADO" bigint,
    "ASEGURADO" "text",
    "DIRECCION_ASEG" "text",
    "TEL_CASA_ASEG" "text",
    "TEL_OFICINA_ASEG" "text",
    "FECHA_NAC_ASEG" "date",
    "VALOR_GARANTIZADO_ACTUAL" "text",
    "VALOR_GARANTIZADO_PROX" "text",
    "PRESTAMOS" "text",
    "FONDOS_ADMON" "text",
    "DCP" "text",
    "FONDOS_DE_LA_POLIZA" "text",
    "FONDO_PARA_DEDUCCIONES" "text",
    "VALOR_RESCATE" "text",
    "ULTIMA_DEDUCCION" "text",
    "BENEFICIOS_ADICIONALES" "text"
);


ALTER TABLE "public"."polices" OWNER TO "postgres";


COMMENT ON TABLE "public"."polices" IS 'This table holds the secury polices';



CREATE TABLE IF NOT EXISTS "public"."receipts" (
    "POLIZA" bigint,
    "INI_VIG_PLAN" "text",
    "FIN_VIG_PLAN" "date",
    "INI_VIG_ANUALIDAD" "date",
    "FIN_VIG_ANUALIDAD" "date",
    "PLAN" "text",
    "ESTATUS_POLIZA" "text",
    "PRIMA_ANUAL" double precision,
    "MONEDA" "text",
    "FORMA_DE_PAGO" "text",
    "CONDUCTO_COBRO" "text",
    "SUMA_ASEGURADA" "text",
    "AÑO_POLIZA" bigint,
    "CVE_CONTRATANTE" bigint,
    "CONTRATANTE" "text",
    "DIRECCION_CONT" "text",
    "TEL_CASA_CONT" "text",
    "TEL_OFICINA_CONT" "text",
    "FECHA_NAC_CONT" "date",
    "CVE_ASEGURADO" bigint,
    "ASEGURADO" "text",
    "DIRECCION_ASEG" "text",
    "TEL_CASA_ASEG" "text",
    "TEL_OFICINA_ASEG" "text",
    "FECHA_NAC_ASEG" "text",
    "NUMERO_RECIBO" bigint NOT NULL,
    "ESTATUS_RECIBO" "text",
    "FECHA_PROG_PAGO" "date",
    "FECHA_COBRO" "date",
    "IMPORTE_DEL_RECIBO" double precision,
    "VALOR_GARANTIZADO_ACTUAL" "text",
    "VALOR_GARANTIZADO_PROX" "text",
    "PRESTAMOS" "text",
    "FONDOS_ADMON" "text",
    "DCP" "text",
    "FONDOS_DE_LA_POLIZA" "text",
    "FONDO_PARA_DEDUCCIONES" "text",
    "VALOR_RESCATE" "text",
    "ULTIMA_DEDUCCION" "text",
    "BENEFICIOS_ADICIONALES" "text"
);


ALTER TABLE "public"."receipts" OWNER TO "postgres";


COMMENT ON TABLE "public"."receipts" IS 'This table holds all the generated receipts';



CREATE OR REPLACE VIEW "public"."prospera_comissions" WITH ("security_invoker"='on') AS
 SELECT "p"."POLIZA" AS "police",
    "r"."NUMERO_RECIBO" AS "receipt",
    "public"."comission_status"(( SELECT "t".*::"record" AS "t"
           FROM ( SELECT "p"."POLIZA" AS "police",
                    "r"."NUMERO_RECIBO" AS "receipt",
                    "r"."ESTATUS_RECIBO" AS "receipt_status") "t")) AS "status",
    (("public"."get_percentage_office"(("p"."INI_VIG_PLAN")::timestamp without time zone))::double precision * "p"."PRIMA_ANUAL") AS "comission_forcast",
    "pr"."amount" AS "comission_received"
   FROM (("public"."polices" "p"
     LEFT JOIN "public"."receipts" "r" ON (("p"."POLIZA" = "r"."POLIZA")))
     LEFT JOIN "public"."paid_receipt" "pr" ON (("r"."NUMERO_RECIBO" = "pr"."receipt")));


ALTER VIEW "public"."prospera_comissions" OWNER TO "postgres";


ALTER TABLE ONLY "public"."gnp_commission_detail" ALTER COLUMN "id" SET DEFAULT "nextval"('"public"."gnp_commission_detail_id_seq"'::"regclass");



ALTER TABLE ONLY "public"."gnp_statement" ALTER COLUMN "id" SET DEFAULT "nextval"('"public"."gnp_statement_id_seq"'::"regclass");



ALTER TABLE ONLY "public"."gnp_statement_movements" ALTER COLUMN "id" SET DEFAULT "nextval"('"public"."gnp_statement_movements_id_seq"'::"regclass");



ALTER TABLE ONLY "public"."polices"
    ADD CONSTRAINT "Polizas_pkey" PRIMARY KEY ("POLIZA");



ALTER TABLE ONLY "public"."receipts"
    ADD CONSTRAINT "Recibos_pkey" PRIMARY KEY ("NUMERO_RECIBO");



ALTER TABLE ONLY "public"."agente_assets"
    ADD CONSTRAINT "agente_assets_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."agentes"
    ADD CONSTRAINT "agentes_pkey" PRIMARY KEY ("Clave");



ALTER TABLE ONLY "public"."comission_data"
    ADD CONSTRAINT "comission_data_pkey" PRIMARY KEY ("month_range", "sales_limit");



ALTER TABLE ONLY "public"."comission_office_data"
    ADD CONSTRAINT "comission_office_data_pkey" PRIMARY KEY ("month_range");



ALTER TABLE ONLY "public"."gerencia"
    ADD CONSTRAINT "gerencia_pkey" PRIMARY KEY ("Nombre Gerencia");



ALTER TABLE ONLY "public"."gnp_commission_detail"
    ADD CONSTRAINT "gnp_commission_detail_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."gnp_statement_movements"
    ADD CONSTRAINT "gnp_statement_movements_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."gnp_statement"
    ADD CONSTRAINT "gnp_statement_pkey" PRIMARY KEY ("id");



ALTER TABLE ONLY "public"."paid_receipt"
    ADD CONSTRAINT "paid_receipt_pkey" PRIMARY KEY ("police", "receipt");



ALTER TABLE ONLY "public"."police_assets"
    ADD CONSTRAINT "poliza_assets_pkey" PRIMARY KEY ("id");



CREATE OR REPLACE TRIGGER "adding_mising_data_polices_receipt" BEFORE INSERT ON "public"."commissions_gnp" FOR EACH ROW EXECUTE FUNCTION "public"."validate_and_insert_parent_records"();



CREATE OR REPLACE TRIGGER "update_and_insert_polices" BEFORE INSERT ON "public"."polices" FOR EACH ROW EXECUTE FUNCTION "public"."handle_polices_upsert"();



CREATE OR REPLACE TRIGGER "upde_and_insert_receipts" BEFORE INSERT ON "public"."receipts" FOR EACH ROW EXECUTE FUNCTION "public"."handle_csv_upsert"();



ALTER TABLE ONLY "public"."commissions_gnp"
    ADD CONSTRAINT "ComisionesGNP_NumeroDePoliza_fkey" FOREIGN KEY ("NumeroDePoliza") REFERENCES "public"."polices"("POLIZA");



ALTER TABLE ONLY "public"."commissions_gnp"
    ADD CONSTRAINT "ComisionesGNP_NumeroDeRecibo_fkey" FOREIGN KEY ("NumeroDeRecibo") REFERENCES "public"."receipts"("NUMERO_RECIBO");



ALTER TABLE ONLY "public"."receipts"
    ADD CONSTRAINT "Recibos_POLIZA_fkey" FOREIGN KEY ("POLIZA") REFERENCES "public"."polices"("POLIZA");



ALTER TABLE ONLY "public"."agentes"
    ADD CONSTRAINT "agentes_Gerencia Buena_fkey" FOREIGN KEY ("Gerencia Buena") REFERENCES "public"."gerencia"("Nombre Gerencia");



ALTER TABLE ONLY "public"."gnp_statement_movements"
    ADD CONSTRAINT "gnp_statement_movements_statement_id_fkey" FOREIGN KEY ("statement_id") REFERENCES "public"."gnp_statement"("id");



ALTER TABLE ONLY "public"."paid_receipt"
    ADD CONSTRAINT "paid_receipt_police_fkey" FOREIGN KEY ("police") REFERENCES "public"."polices"("POLIZA");



ALTER TABLE ONLY "public"."paid_receipt"
    ADD CONSTRAINT "paid_receipt_receipt_fkey" FOREIGN KEY ("receipt") REFERENCES "public"."receipts"("NUMERO_RECIBO");



ALTER TABLE ONLY "public"."police_assets"
    ADD CONSTRAINT "poliza_assets_poliza_fkey" FOREIGN KEY ("poliza") REFERENCES "public"."polices"("POLIZA");



ALTER TABLE "public"."agente_assets" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."agentes" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."comission_data" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."comission_office_data" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."commissions_gnp" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."gerencia" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."gnp_commission_detail" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."gnp_statement" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."gnp_statement_movements" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."paid_receipt" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."police_assets" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."polices" ENABLE ROW LEVEL SECURITY;


ALTER TABLE "public"."receipts" ENABLE ROW LEVEL SECURITY;



ALTER PUBLICATION "supabase_realtime" OWNER TO "postgres";


GRANT USAGE ON SCHEMA "public" TO "postgres";
GRANT USAGE ON SCHEMA "public" TO "anon";
GRANT USAGE ON SCHEMA "public" TO "authenticated";
GRANT USAGE ON SCHEMA "public" TO "service_role";



GRANT ALL ON FUNCTION "public"."comission_status"("p_police" "record") TO "anon";
GRANT ALL ON FUNCTION "public"."comission_status"("p_police" "record") TO "authenticated";
GRANT ALL ON FUNCTION "public"."comission_status"("p_police" "record") TO "service_role";



GRANT ALL ON FUNCTION "public"."get_percentage_data"("p_creation_date" timestamp without time zone, "sales" integer, "p_type" "text") TO "anon";
GRANT ALL ON FUNCTION "public"."get_percentage_data"("p_creation_date" timestamp without time zone, "sales" integer, "p_type" "text") TO "authenticated";
GRANT ALL ON FUNCTION "public"."get_percentage_data"("p_creation_date" timestamp without time zone, "sales" integer, "p_type" "text") TO "service_role";



GRANT ALL ON FUNCTION "public"."get_percentage_office"("p_creation_date" timestamp without time zone) TO "anon";
GRANT ALL ON FUNCTION "public"."get_percentage_office"("p_creation_date" timestamp without time zone) TO "authenticated";
GRANT ALL ON FUNCTION "public"."get_percentage_office"("p_creation_date" timestamp without time zone) TO "service_role";



GRANT ALL ON FUNCTION "public"."handle_csv_upsert"() TO "anon";
GRANT ALL ON FUNCTION "public"."handle_csv_upsert"() TO "authenticated";
GRANT ALL ON FUNCTION "public"."handle_csv_upsert"() TO "service_role";



GRANT ALL ON FUNCTION "public"."handle_polices_upsert"() TO "anon";
GRANT ALL ON FUNCTION "public"."handle_polices_upsert"() TO "authenticated";
GRANT ALL ON FUNCTION "public"."handle_polices_upsert"() TO "service_role";



GRANT ALL ON FUNCTION "public"."validate_and_insert_parent_records"() TO "anon";
GRANT ALL ON FUNCTION "public"."validate_and_insert_parent_records"() TO "authenticated";
GRANT ALL ON FUNCTION "public"."validate_and_insert_parent_records"() TO "service_role";







GRANT ALL ON TABLE "public"."agente_assets" TO "anon";
GRANT ALL ON TABLE "public"."agente_assets" TO "authenticated";
GRANT ALL ON TABLE "public"."agente_assets" TO "service_role";



GRANT ALL ON TABLE "public"."agentes" TO "anon";
GRANT ALL ON TABLE "public"."agentes" TO "authenticated";
GRANT ALL ON TABLE "public"."agentes" TO "service_role";



GRANT ALL ON TABLE "public"."comission_data" TO "anon";
GRANT ALL ON TABLE "public"."comission_data" TO "authenticated";
GRANT ALL ON TABLE "public"."comission_data" TO "service_role";



GRANT ALL ON TABLE "public"."comission_office_data" TO "anon";
GRANT ALL ON TABLE "public"."comission_office_data" TO "authenticated";
GRANT ALL ON TABLE "public"."comission_office_data" TO "service_role";



GRANT ALL ON TABLE "public"."commissions_gnp" TO "anon";
GRANT ALL ON TABLE "public"."commissions_gnp" TO "authenticated";
GRANT ALL ON TABLE "public"."commissions_gnp" TO "service_role";



GRANT ALL ON TABLE "public"."gerencia" TO "anon";
GRANT ALL ON TABLE "public"."gerencia" TO "authenticated";
GRANT ALL ON TABLE "public"."gerencia" TO "service_role";



GRANT ALL ON TABLE "public"."gnp_commission_detail" TO "anon";
GRANT ALL ON TABLE "public"."gnp_commission_detail" TO "authenticated";
GRANT ALL ON TABLE "public"."gnp_commission_detail" TO "service_role";



GRANT ALL ON SEQUENCE "public"."gnp_commission_detail_id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."gnp_commission_detail_id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."gnp_commission_detail_id_seq" TO "service_role";



GRANT ALL ON TABLE "public"."gnp_statement" TO "anon";
GRANT ALL ON TABLE "public"."gnp_statement" TO "authenticated";
GRANT ALL ON TABLE "public"."gnp_statement" TO "service_role";



GRANT ALL ON SEQUENCE "public"."gnp_statement_id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."gnp_statement_id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."gnp_statement_id_seq" TO "service_role";



GRANT ALL ON TABLE "public"."gnp_statement_movements" TO "anon";
GRANT ALL ON TABLE "public"."gnp_statement_movements" TO "authenticated";
GRANT ALL ON TABLE "public"."gnp_statement_movements" TO "service_role";



GRANT ALL ON SEQUENCE "public"."gnp_statement_movements_id_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."gnp_statement_movements_id_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."gnp_statement_movements_id_seq" TO "service_role";



GRANT ALL ON TABLE "public"."paid_receipt" TO "anon";
GRANT ALL ON TABLE "public"."paid_receipt" TO "authenticated";
GRANT ALL ON TABLE "public"."paid_receipt" TO "service_role";



GRANT ALL ON SEQUENCE "public"."paid_receipt_police_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."paid_receipt_police_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."paid_receipt_police_seq" TO "service_role";



GRANT ALL ON SEQUENCE "public"."paid_receipt_receipt_seq" TO "anon";
GRANT ALL ON SEQUENCE "public"."paid_receipt_receipt_seq" TO "authenticated";
GRANT ALL ON SEQUENCE "public"."paid_receipt_receipt_seq" TO "service_role";



GRANT ALL ON TABLE "public"."police_assets" TO "anon";
GRANT ALL ON TABLE "public"."police_assets" TO "authenticated";
GRANT ALL ON TABLE "public"."police_assets" TO "service_role";



GRANT ALL ON TABLE "public"."polices" TO "anon";
GRANT ALL ON TABLE "public"."polices" TO "authenticated";
GRANT ALL ON TABLE "public"."polices" TO "service_role";



GRANT ALL ON TABLE "public"."receipts" TO "anon";
GRANT ALL ON TABLE "public"."receipts" TO "authenticated";
GRANT ALL ON TABLE "public"."receipts" TO "service_role";



GRANT ALL ON TABLE "public"."prospera_comissions" TO "anon";
GRANT ALL ON TABLE "public"."prospera_comissions" TO "authenticated";
GRANT ALL ON TABLE "public"."prospera_comissions" TO "service_role";



ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON SEQUENCES TO "service_role";



ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON FUNCTIONS TO "service_role";



ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES TO "postgres";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES TO "anon";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES TO "authenticated";
ALTER DEFAULT PRIVILEGES FOR ROLE "postgres" IN SCHEMA "public" GRANT ALL ON TABLES TO "service_role";




drop extension if exists "pg_net";