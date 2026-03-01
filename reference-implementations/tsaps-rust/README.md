# TSAPS v1.0 — Rust Reference Implementation

Minimal Rust reference implementation of **TSAPS v1.0**.

⚠️ Educational use only.

---

## 📦 Project Structure

```
tsaps-rust/
 ├── Cargo.toml
 └── src/
     ├── main.rs
     ├── client.rs
     ├── server.rs
     └── tsaps.rs
```

---

## 🔐 Cryptographic Components Used

- `ed25519-dalek` → Signing & verification  
- `sha2` → SHA256  
- `hmac` → HMAC construction  
- `hkdf` → Key derivation  

---

## 1️⃣ Add Dependencies

Add to `Cargo.toml`:

```toml
[dependencies]
sha2 = "0.10"
hmac = "0.12"
rand = "0.8"
hex = "0.4"
ed25519-dalek = { version = "2", features = ["rand_core"] }
base64 = "0.21"
uuid = { version = "1", features = ["v4"] }
```

---

## ▶ Run Example

```bash
cargo run
```

---

## ✅ Expected Output

```
---- TSAPS Rust Reference ----

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

This implementation omits:

- Distributed cache
- Replay database
- HSM integration
- Audit logging

Not suitable for live deployment.