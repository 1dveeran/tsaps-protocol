using System;
using System.Collections.Concurrent;
using System.Security.Cryptography;

public class Server
{
    private readonly byte[] _serverSecret;
    private readonly ConcurrentDictionary<string, bool> _replayCache = new();

    public Server(byte[] serverSecret)
    {
        _serverSecret = serverSecret;
    }

    public void Verify(TsapsRequest request)
    {
        Console.WriteLine("\n---- Server Verification ----");

        // 1. Replay Check
        if (!_replayCache.TryAdd(request.TID, true))
        {
            Console.WriteLine("✘ Replay detected");
            return;
        }
        Console.WriteLine("✔ TID unique");

        // 2. Verify STID
        string expectedStid = Tsaps.ComputeSTID(_serverSecret, request.ClientId, request.Timestamp);
        if (request.STID != expectedStid)
        {
            Console.WriteLine("✘ STID invalid");
            return;
        }
        Console.WriteLine("✔ STID valid");

        // 3. Verify PVG
        string expectedPvg = Tsaps.ComputePVG(_serverSecret, request.TID, request.TlsExporter);
        if (request.PVG != expectedPvg)
        {
            Console.WriteLine("✘ Guard invalid");
            return;
        }
        Console.WriteLine("✔ Guard valid");

        // 4. Verify Signature
        if (!Tsaps.VerifySignature(request.PublicKey, request.TID, request.Signature))
        {
            Console.WriteLine("✘ Signature invalid");
            return;
        }

        Console.WriteLine("✔ Signature valid");
        Console.WriteLine("✔ Transaction committed");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("---- TSAPS .NET Reference ----");

        byte[] serverSecret = RandomNumberGenerator.GetBytes(32);

        var client = new Client("client1");
        var server = new Server(serverSecret);

        var request = client.CreateRequest(serverSecret);
        server.Verify(request);

        // Try replay
        Console.WriteLine("\n-- Replaying same request --");
        server.Verify(request);
    }
}