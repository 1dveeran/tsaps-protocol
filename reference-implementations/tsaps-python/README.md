# TSAPS v1.0 — Python Reference Implementation

Minimal Python reference implementation of **TSAPS v1.0**.

⚠️ Educational reference only.

---

## 📦 Project Structure

```
tsaps-python/
 ├── client.py
 ├── server.py
 ├── tsaps.py
 └── requirements.txt
```

---

## 🔐 Cryptographic Components Used

- `pynacl` → Ed25519
- `hashlib` → SHA256
- `hmac` → Guard construction
- `hkdf` (via cryptography library)

---

## 1️⃣ Install Dependencies

```bash
pip install pynacl cryptography
```

---

## ▶ Run Example

```bash
python client.py
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

## 🚫 Production Warning

No:

- Persistent replay store
- Rate limiting
- Secure enclave
- Logging controls

Use hardened SDK in real systems.