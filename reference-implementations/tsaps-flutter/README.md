# TSAPS v1.0 — Flutter (Dart) Reference Implementation

Minimal Dart implementation of **TSAPS v1.0**, suitable for Flutter clients.

⚠️ Reference only. Not production secure.

---

## 📦 Project Structure

```
tsaps-flutter/
 ├── lib/
 │   ├── tsaps.dart
 │   ├── client.dart
 │   └── server_mock.dart
 └── pubspec.yaml
```

---

## 🔐 Cryptographic Components Used

- `package:cryptography` → Ed25519
- `dart:convert`
- `package:crypto` → SHA256, HMAC

---

## 1️⃣ Add Dependencies

In `pubspec.yaml`:

```yaml
dependencies:
  cryptography: ^2.5.0
  crypto: ^3.0.0
```

Then run:

```bash
flutter pub get
```

---

## ▶ Run Example

If standalone Dart:

```bash
dart run lib/client.dart
```

If Flutter:

Integrate into test or debug screen and trigger request.

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

Re-running within same time window should trigger duplicate detection.

---

## 🚫 Production Warning

Flutter implementation does NOT include:

- Secure key storage (use Keychain/Keystore)
- Biometric binding
- Certificate pinning
- Network hardening

Production mobile apps must use secure storage APIs.