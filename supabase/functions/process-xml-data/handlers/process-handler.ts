import { InvalidRequestException } from "../exceptions/invalid-request-exception.ts";
import { fetchReceiptData, getFileData, updatePaidReceipts } from "../helpers.ts";
import { DefaultRequestHandler } from "../lib/request-handlers.ts";

export class ProcessHandler extends DefaultRequestHandler {
  public static override Instance() {
    return new ProcessHandler();
  }

  public override async POST(req: Request) {
    const body = await req.json().catch(() => null);

    const path = body?.path || body?.name;

    if (!path) {
      throw new InvalidRequestException(`Missing 'path' in request body`);
    }

    const jsonObj = await getFileData(path);

    const receiptData = await fetchReceiptData(jsonObj.filter(item => item.AfectoDetF12 !== "N"));

    await updatePaidReceipts(receiptData);

    console.info("File data parsed successfully");

    return new Response(
      JSON.stringify({ updated_receipts: receiptData }),
      { headers: { "Content-Type": "application/json" }, status: 201 },
    );
  }

}
