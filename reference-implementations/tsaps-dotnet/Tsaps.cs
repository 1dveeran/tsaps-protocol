using System.Security.Cryptography;
using System.Text;
using NSec.Cryptography;

public static class Tsaps
{
    public static string ComputeTID(string message, string nonce, long timestamp)
    {
        return ComputeSHA256($"{message}|{nonce}|{timestamp}");
    }

    public static string ComputeSTID(byte[] serverSecret, string clientId, long timestamp)
    {
        long epoch = timestamp / 60;
        return ComputeHMAC(serverSecret, $"{clientId}|{epoch}");
    }

    public static string ComputePVG(byte[] serverSecret, string tid, string tlsExporter)
    {
        return ComputeHMAC(serverSecret, $"{tid}|{tlsExporter}");
    }

    public static byte[] SignTID(Key privateKey, string tid)
    {
        var algo = SignatureAlgorithm.Ed25519;
        return algo.Sign(privateKey, Encoding.UTF8.GetBytes(tid));
    }

    public static bool VerifySignature(PublicKey publicKey, string tid, byte[] signature)
    {
        var algo = SignatureAlgorithm.Ed25519;
        return algo.Verify(publicKey, Encoding.UTF8.GetBytes(tid), signature);
    }

    private static string ComputeSHA256(string input)
    {
        using var sha = SHA256.Create();
        var hash = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hash);
    }

    private static string ComputeHMAC(byte[] key, string input)
    {
        using var hmac = new HMACSHA256(key);
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input));
        return Convert.ToHexString(hash);
    }
}