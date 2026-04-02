import { ConfigurationException } from "./exceptions/configuration-exception.ts";
import { InvalidRequestException } from "./exceptions/invalid-request-exception.ts";
import { supabase } from "./lib/supabase-helper.ts";


export async function fetchFileData(fileName: string): Promise<Blob> {
  const bucket = Deno.env.get("BUCKET_NAME");
  if (!bucket) {
    throw new ConfigurationException("Missing BUCKET_NAME in environment");
  }
  const { data, error } = await supabase.storage.from(bucket).download(
    fileName
  );
  if (error) {
    if (error.cause) {
      throw new InvalidRequestException(error.message);
    }
    throw new InvalidRequestException(`Error downloading file ${fileName}`);
  }
  return data;
}
