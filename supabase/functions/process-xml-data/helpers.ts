import { ConfigurationException } from "./exceptions/configuration-exception.ts";
import { InvalidRequestException } from "./exceptions/invalid-request-exception.ts";
import { supabase } from "./lib/supabase-helper.ts";
import { XMLParser } from "fast-xml-parser";
import { PaidReceipt } from "./types/paid-receipts.ts";
import { PaidReceiptType, ParsedFileData } from "./types/parse-file-data.ts";

export async function fetchFileData(fileName: string): Promise<Blob> {
  const bucket = Deno.env.get("PROCESSING_BUCKET_NAME");
  if (!bucket) {
    throw new ConfigurationException(
      "Missing PROCESSING_BUCKET_NAME in environment",
    );
  }
  const { data, error } = await supabase.storage.from(bucket).download(
    fileName,
  );
  if (error) {
    if (error.cause) {
      throw new InvalidRequestException(error.message);
    }
    throw new InvalidRequestException(`Error downloading file ${fileName}`);
  }
  if (!data) {
    throw new InvalidRequestException(`No data returned for file ${fileName}`);
  }
  return data;
}

export async function getFileData(
  fileName: string,
): Promise<PaidReceiptType[]> {
  const fileData = await fetchFileData(fileName);
  const options = {
    ignoreAttributes: true, // Ignores attributes for cleaner JSON output
  };
  const parser = new XMLParser(options);

  const jsonObj = parser.parse(await fileData.text());

  if (!isParsedFileData(jsonObj)) {
    throw new InvalidRequestException(
      "Unexpected XML structure. Missing EstadodeCuenta.SECCION",
    );
  }

  return jsonObj.EstadodeCuenta.SECCION
    .filter((section) => !!section.DetalleComisiones)
    .flatMap((section) => section.DetalleComisiones)
    .flatMap((item) => item.DetalleContenido)
    .flatMap((content) => content.DetalleF12)
    .flatMap((content) => content.ITEM)
    .filter((item) => typeof item.PolizaDetF12 === "number")
    .map((item) => {
      item.FechaDetF12 = convertDate(String(item.FechaDetF12));
      item.ComisionDetF12 = convertStringToNumber(item.ComisionDetF12);
      return item;
    });
}

export async function deleteFile(fileName: string): Promise<void> {
  const bucket = Deno.env.get("PROCESSING_BUCKET_NAME");
  if (!bucket) {
    throw new ConfigurationException(
      "Missing PROCESSING_BUCKET_NAME in environment",
    );
  }
  const { error } = await supabase.storage.from(bucket).remove([fileName]);
  if (error) {
    throw new Error(`Error deleting file ${fileName}: ${error.message}`);
  }
}

export async function fetchReceiptData(
  values: PaidReceiptType[],
): Promise<PaidReceipt[]> {
  
  const p_data = values
    .map((item) => ({
      police: item.PolizaDetF12,
      receipt_date_charged: item.FechaDetF12,
      amount: item.ComisionDetF12,
    }));

  const result = await supabase.rpc("fetch_receipts_data", { p_data }).overrideTypes<PaidReceipt[]>();

  // console.info("data", p_data);
  // console.info("Fetched receipt data from the database.", result.data);
  return result.data as PaidReceipt[] || [];
}
export async function updatePaidReceipts(receipts: PaidReceipt[]) {
  console.info("Updating paid receipts in the database...");

  const { error } = await supabase.from("paid_receipts").upsert(receipts.map((receipt) => ({
    police: receipt.police,
    receipt: receipt.receipt,
    amount: receipt.amount,
  })));

  console.info("Paid receipts updated.");
  if (error) {
    throw new Error(`${error.message}`);
  }
}

export function isParsedFileData(input: unknown): input is ParsedFileData {
  if (!input || typeof input !== "object") return false;
  const candidate = input as Record<string, unknown>;
  if (
    !candidate.EstadodeCuenta || typeof candidate.EstadodeCuenta !== "object"
  ) {
    return false;
  }
  return Object.prototype.hasOwnProperty.call(
    candidate.EstadodeCuenta,
    "SECCION",
  );
}

function convertDate(input: string): string {
  if (!/^\d{7,8}$/.test(input)) {
    throw new Error("Invalid date format");
  }

  const year = input.slice(-4);
  const month = input.slice(-6, -4);
  const day = input.slice(0, input.length - 6);

  const d = Number(day);
  const m = Number(month);
  const y = Number(year);

  const date = new Date(y, m - 1, d);

  // validate real date
  if (
    date.getFullYear() !== y ||
    date.getMonth() !== m - 1 ||
    date.getDate() !== d
  ) {
    throw new Error("Invalid date value");
  }

  return `${y}-${String(m).padStart(2, "0")}-${String(d).padStart(2, "0")}`;
}

function convertStringToNumber(
  input: string | number | undefined,
): number | undefined {
  if (input === undefined) return undefined;
  const num = typeof input === "number"
    ? input
    : parseFloat(input.replace(/,/g, ""));
  return isNaN(num) ? undefined : num;
}
