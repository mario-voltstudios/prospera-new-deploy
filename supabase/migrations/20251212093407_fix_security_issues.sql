-- Migration: fix functions with role-mutable search_path
-- This migration finds the listed functions in the public schema
-- and sets an explicit, non-role-mutable search_path for each.

DO $$
DECLARE
	r RECORD;
	names TEXT[] := ARRAY[
		'get_percentage_data',
		'get_percentage_office',
		'comission_status',
		'handle_csv_upsert',
		'handle_polices_upsert',
		'validate_and_insert_parent_records'
	];
	target_search_path TEXT := 'pg_temp, public, pg_catalog';
BEGIN
	FOR r IN
		SELECT p.oid,
					 p.proname,
					 pg_get_function_identity_arguments(p.oid) AS args
		FROM pg_proc p
		JOIN pg_namespace n ON p.pronamespace = n.oid
		WHERE n.nspname = 'public' AND p.proname = ANY(names)
	LOOP
		-- Generate and execute an ALTER FUNCTION ... SET search_path = '...'
		EXECUTE format(
			'ALTER FUNCTION public.%I(%s) SET search_path = %L',
			r.proname,
			r.args,
			target_search_path
		);
	END LOOP;
END
$$ LANGUAGE plpgsql;

-- Notes:
-- - This approach does not require knowing exact argument types; it uses
--   pg_get_function_identity_arguments to build correct ALTER FUNCTION calls.
-- - The chosen search_path is 'pg_temp, public, pg_catalog' which avoids
--   role-mutable search_path behavior and is generally safe for functions.

