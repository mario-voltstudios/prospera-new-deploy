import { ConfigurationException } from "./exceptions/configuration-exception.ts";
import { InvalidRequestException } from "./exceptions/invalid-request-exception.ts";
import { supabase } from "./lib/supabase-helper.ts";
import Airtable from "airtable";

export function getAirtableClient() {
  const apiKey = Deno.env.get("AIRTABLE_API_KEY");
  const airtableBase = Deno.env.get("AIRTABLE_BASE_ID");

  if(!apiKey) {
    throw new ConfigurationException("Missing AIRTABLE_API_KEY in environment");
  }

  if(!airtableBase) {
    throw new ConfigurationException("Missing AIRTABLE_BASE_ID in environment");
  }

  return new Airtable({apiKey: apiKey}).base(airtableBase);
}
