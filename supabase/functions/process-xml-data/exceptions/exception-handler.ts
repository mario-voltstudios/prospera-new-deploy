import { InvalidRequestException } from "./invalid-request-exception.ts";
import { MethodNotAllowedException } from "./method-not-allowed-exception.ts";

export function handleException(err: unknown): Response {
  if (err instanceof InvalidRequestException) {
    return new Response(
      JSON.stringify({ error: err.message }),
      { status: 400, headers: { "Content-Type": "application/json" } },
    );
  }

  if (err instanceof MethodNotAllowedException) {
    return new Response(
      JSON.stringify({ error: "Sorry we can process that" }),
      { status: 405, headers: { "Content-Type": "application/json" } },
    );
  }

  console.error("Unhandled exception:", err);

  return new Response(
    JSON.stringify({ error: "Internal Server Error" }),
    { status: 500, headers: { "Content-Type": "application/json" } },
  );
}
