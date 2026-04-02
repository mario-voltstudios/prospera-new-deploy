export class ConfigurationException extends Error {
  constructor(message: string) {
    super(message);
    console.error(`ConfigurationException: ${message}`);
    this.name = "ConfigurationException";
  }
}   