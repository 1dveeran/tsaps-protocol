# TSAPS v1.0 — Go Reference Implementation

This folder contains a minimal Go reference implementation of **TSAPS v1.0**.

⚠️ Reference only. Not production ready.

---

## 📦 Project Structure

```
tsaps-go/
 ├── client.go
 ├── server.go
 ├── tsaps.go
 └── go.mod
```

---

## 🔐 Cryptographic Components Used

- `crypto/ed25519` → Signature generation & verification  
- `crypto/sha256` → Hashing  
- `crypto/hmac` → Guard construction  
- `golang.org/x/crypto/hkdf` → Key derivation  

---

## 1️⃣ Initialize Module

```bash
go mod init tsaps-go
go get golang.org/x/crypto/hkdf
```

---

## ▶ Run Example

```bash
go run client.go
```

---

## ✅ Expected Output

```
---- Server Verification ----
✔ Guard valid
✔ STID valid
✔ RID unique
✔ Signature valid
✔ Transaction committed
```

---

## 🔁 Replay Protection

Re-running within the same time window should fail due to duplicate RID/STID.

---

## 🚫 Production Warning

This implementation does NOT include:

- Distributed replay cache
- Rate limiting
- Key rotation
- Persistent storage
- Hardware security modules

Use hardened SDK for production.