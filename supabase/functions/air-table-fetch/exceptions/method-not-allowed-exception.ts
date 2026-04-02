export class MethodNotAllowedException extends Error {
  constructor(message?: string) {
    super(message);
    this.name = "MethodNotAllowedException";
  }
}