namespace ProsperaServices.Interfaces;

/// <summary>
/// Service for encrypting and decrypting strings using AES-256 encryption
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    /// Encrypts a plain text string
    /// </summary>
    /// <param name="plainText">The text to encrypt</param>
    /// <returns>Base64Url encoded encrypted string (URL-safe, no padding, ~23% smaller than Base64)</returns>
    string Encrypt(string plainText);
    
    /// <summary>
    /// Decrypts an encrypted string
    /// </summary>
    /// <param name="encryptedText">The Base64Url encoded encrypted text</param>
    /// <returns>Decrypted plain text string</returns>
    string Decrypt(string encryptedText);

    /// <summary>
    ///  Tries to decrypt an encrypted string
    /// </summary>
    /// <param name="encryptedText">The Base64Url encoded encrypted text</param>
    /// <param name="plainText">Decrypted plain text string</param>
    /// <returns>true is success or false if fails</returns>
    bool TryDecrypt(string encryptedText, out string? plainText);
}
