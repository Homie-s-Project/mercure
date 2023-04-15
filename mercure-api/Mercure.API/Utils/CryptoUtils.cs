using Crypto.AES;
using Microsoft.Extensions.Configuration;

namespace Mercure.API.Utils;

/// <summary>
/// Classe utilitaire pour le chiffrement et dé-chiffrement des données
/// </summary>
public abstract class CryptoUtils
{
    private static readonly IConfiguration Config;

    static CryptoUtils()
    {
        Config = Startup.StaticConfig;
    }
    
    /// <summary>
    /// Chiffre la valeur en paramètre
    /// </summary>
    /// <param name="toBeEncrypted">la valeur à chiffrer</param>
    /// <returns>la valeur chiffré</returns>
    public static string Encrypt(string toBeEncrypted)
    {
        return AES.EncryptString(Config["Secure:SecretKey"], toBeEncrypted);
    }

    /// <summary>
    /// Dé-chiffrer la valeur en paramètre
    /// </summary>
    /// <param name="encrypted">la valeur dé-chiffrer</param>
    /// <returns>la valeur dé-chiffrer</returns>
    public static string Decrypt(string encrypted)
    {
        return AES.DecryptString(Config["Secure:SecretKey"], encrypted);
    }
}