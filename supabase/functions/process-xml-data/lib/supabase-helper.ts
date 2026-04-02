import { createClient as createSupabaseClient, type SupabaseClient } from "@supabase/supabase-js";

let cachedClient: SupabaseClient | null = null;

function createClient(): SupabaseClient {
  const SUPABASE_URL = Deno.env.get("SUPABASE_URL") ?? "";
  const SUPABASE_KEY = Deno.env.get("SUPABASE_SERVICE_ROLE_KEY") ??
    Deno.env.get("SUPABASE_ANON_KEY") ?? "";

  if (!SUPABASE_URL || !SUPABASE_KEY) {
    throw new Error("Missing SUPABASE_URL or SUPABASE key in environment");
  }

  if(cachedClient){
    return cachedClient;
  }

  cachedClient = createSupabaseClient(SUPABASE_URL, SUPABASE_KEY);
  return cachedClient;
}

export const supabase = createClient();
