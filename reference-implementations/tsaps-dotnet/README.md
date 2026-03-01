# TSAPS v1.0 — .NET Reference Implementation

Minimal C# reference implementation of **TSAPS v1.0**.

⚠️ For demonstration purposes only.

---

## 📦 Project Structure

```
tsaps-dotnet/
 ├── Client.cs
 ├── Server.cs
 ├── Tsaps.cs
 └── tsaps.csproj
```

---

## 🔐 Cryptographic Components Used

- `System.Security.Cryptography.Ed25519`
- `SHA256`
- `HMACSHA256`
- `HKDF` (via custom implementation or library)

---

## 1️⃣ Create Project

```bash
dotnet new console -n tsaps-dotnet
cd tsaps-dotnet
```

Add required crypto library if needed.

---

## ▶ Run Example

```bash
dotnet run
```

---

## ✅ Expected Output

```
---- TSAPS .NET Reference ----

---- Server Verification ----
✔ TID unique
✔ STID valid
✔ Guard valid
✔ Signature valid
✔ Transaction committed

-- Replaying same request --

---- Server Verification ----
✘ Replay detected
```

---

## 🚫 Production Warning

Missing:

- Replay database
- Distributed cache
- Secure key vault integration
- Observability

Not production hardened.