using System;
using NSec.Cryptography;

public class Client
{
    private readonly Key _privateKey;
    private readonly string _clientId;

    public Client(string clientId)
    {
        _clientId = clientId;
        _privateKey = Key.Create(SignatureAlgorithm.Ed25519);
    }

    public TsapsRequest CreateRequest(byte[] serverSecret)
    {
        string message = "Transfer 100 tokens";
        string nonce = Guid.NewGuid().ToString();
        long timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        string tlsExporter = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));

        string tid = Tsaps.ComputeTID(message, nonce, timestamp);
        string stid = Tsaps.ComputeSTID(serverSecret, _clientId, timestamp);
        string pvg = Tsaps.ComputePVG(serverSecret, tid, tlsExporter);
        byte[] signature = Tsaps.SignTID(_privateKey, tid);

        return new TsapsRequest
        {
            ClientId = _clientId,
            TID = tid,
            STID = stid,
            PVG = pvg,
            Signature = signature,
            PublicKey = _privateKey.PublicKey,
            Timestamp = timestamp,
            TlsExporter = tlsExporter
        };
    }
}

public class TsapsRequest
{
    public string ClientId { get; set; } = "";
    public string TID { get; set; } = "";
    public string STID { get; set; } = "";
    public string PVG { get; set; } = "";
    public byte[] Signature { get; set; } = Array.Empty<byte>();
    public PublicKey PublicKey { get; set; } = null!;
    public long Timestamp { get; set; }
    public string TlsExporter { get; set; } = "";
}