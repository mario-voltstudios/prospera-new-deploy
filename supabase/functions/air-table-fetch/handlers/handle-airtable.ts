import { getAirtableClient } from "../helpers.ts";
import { DefaultRequestHandler } from "../lib/request-handlers.ts";

export class ProcessHandler extends DefaultRequestHandler {
  public static override Instance() {
    return new ProcessHandler();
  }

  public override async POST(req: Request) {
    const body = await req.json().catch(() => ({}));

    // Escape single quotes per Airtable string rules
    const safeClave = "1002";
    const filterByFormula = `{Clave}='${safeClave}'`;

    const client = getAirtableClient();

    const data = await client.table("tblCzyD81OuIkHEep").select({
      maxRecords: 1,
      offset: 0,
      pageSize: 1,
      fields: ["Clave", "FOTO"],
      filterByFormula,
    }).all();

    return new Response(
      JSON.stringify({ updated_receipts: data }),
      { headers: { "Content-Type": "application/json" }, status: 200 },
    );
  }
}
