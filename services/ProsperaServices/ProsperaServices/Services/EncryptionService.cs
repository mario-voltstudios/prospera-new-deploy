using System.Security.Cryptography;
using System.Text;
using ProsperaServices.Interfaces;
using ProsperaServices.Interfaces.IoC;

namespace ProsperaServices.Services;

/// <summary>
/// Provides AES-256 encryption and decryption services
/// </summary>
public class EncryptionService : IEncryptionService, ISingletonDependency
{
    private readonly byte[] _key;
    private readonly byte[] _iv;
    
    /// <summary>
    /// Initializes the encryption service with a secret key
    /// </summary>
    /// <param name="configuration">Configuration to read the encryption key</param>
    public EncryptionService(IConfiguration configuration)
    {
        var secret = configuration["EncryptionSettings:Secret"]
                     ?? throw new InvalidOperationException("Encryption secret not configured");

        if (secret.Length < 32)
        {
            throw new InvalidOperationException("Encryption secret must be at least 32 characters long");
        }

        // Derive a 256-bit key and 128-bit IV from the secret using SHA256
        var keyBytes = SHA256.HashData(Encoding.UTF8.GetBytes(secret));
        _key = keyBytes; // 32 bytes for AES-256

        // Derive IV from a different part of the secret
        var ivSource = secret[(secret.Length / 2)..];
        var ivBytes = SHA256.HashData(Encoding.UTF8.GetBytes(ivSource));
        _iv = ivBytes[..16]; // First 16 bytes for the IV
    }

    /// <summary>
    /// Encrypts a plain text string using AES-256
    /// </summary>
    /// <param name="plainText">The text to encrypt</param>
    /// <returns>Base64Url encoded encrypted string (URL-safe, no padding)</returns>
    public string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText))
        {
            throw new ArgumentException("Plain text cannot be null or empty", nameof(plainText));
        }
        
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        
        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using (var swEncrypt = new StreamWriter(csEncrypt))
        {
            swEncrypt.Write(plainText);
        }
        
        var encrypted = msEncrypt.ToArray();
        return Base64UrlEncode(encrypted);
    }
    
    /// <summary>
    /// Decrypts an encrypted string using AES-256
    /// </summary>
    /// <param name="encryptedText">The Base64Url encoded encrypted text</param>
    /// <returns>Decrypted plain text string</returns>
    public string Decrypt(string encryptedText)
    {
        if (string.IsNullOrEmpty(encryptedText))
        {
            throw new ArgumentException("Encrypted text cannot be null or empty", nameof(encryptedText));
        }
        
        var cipherBytes = Base64UrlDecode(encryptedText);
        
        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        
        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var msDecrypt = new MemoryStream(cipherBytes);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        
        return srDecrypt.ReadToEnd();
    }
    
    public bool TryDecrypt(string encryptedToken, out string? plainText)
    {
        try
        {
            plainText = Decrypt(encryptedToken);
            return true;
        }
        catch
        {
            plainText = null;
            return false;
        }
    }
    
    /// <summary>
    /// Encodes byte array to Base64Url format (URL-safe, no padding)
    /// This reduces string size by ~23% compared to standard Base64
    /// </summary>
    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .TrimEnd('=')                 // Remove padding
            .Replace('+', '-')            // URL-safe character
            .Replace('/', '_');           // URL-safe character
    }
    
    /// <summary>
    /// Decodes Base64Url format back to byte array
    /// </summary>
    private static byte[] Base64UrlDecode(string input)
    {
        // Convert Base64Url back to Base64
        var base64 = input
            .Replace('-', '+')
            .Replace('_', '/');
        
        // Add padding back if needed
        switch (input.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }
        
        return Convert.FromBase64String(base64);
    }
}
