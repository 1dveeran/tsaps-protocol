# TSAPS v1.0
## Temporal-Atomic Secure API Protocol
Deterministic, Hardware-Bound Transaction Proof for High-Value APIs

---

# 🚀 Why TSAPS Exists

Modern APIs rely heavily on **bearer tokens** (like JWTs).

Bearer tokens have one fundamental flaw:

> If someone steals the token, they can reuse it.

For low-risk APIs, this is acceptable.
For high-value financial transactions, it is not.

TSAPS exists because:

* Financial systems cannot tolerate replay attacks.
* Transaction duplication must be cryptographically prevented.
* Tokens should not be reusable secrets.
* Zero-trust architectures require proof-of-possession.
* CPU exhaustion attacks must be mitigated early.
* Global idempotency must be guaranteed.

TSAPS replaces reusable tokens with:

> A per-request cryptographic transaction proof.

Each API call proves:

* Who signed it
* What exactly was signed
* That it belongs to this TLS session
* That it cannot be replayed
* That it cannot be double-spent

No reusable bearer secrets.
No silent replays.
No cross-session duplication.

---

# 🧠 How TSAPS v1.0 Works (In Simple Terms)

Instead of sending a reusable access token, the client:

1. Hashes the request body.
2. Creates a **Semantic Transaction ID (STID)** — a fingerprint of the financial intent.
3. Binds the request to the current TLS session.
4. Adds a lightweight anti-DDoS guard.
5. Signs everything using a hardware-backed private key (Secure Enclave / TPM / HSM).

The server verifies everything in a strict order and only then processes the transaction.

---

# 🔐 Step-by-Step Flow

## 1️⃣ Create a Financial Fingerprint (STID)

The client computes:

```
STID = SHA256(method || path || body_hash || client_key_id)
```

This represents the actual financial intent.

If someone tries to submit the same transaction again — even later —
the STID will be identical.

This enables **global duplicate prevention**.

---

## 2️⃣ Bind to the Current TLS Session

The client extracts a TLS exporter value.

Then computes:

```
RID = SHA256(STID || window_index || TLS_exporter)
```

This ensures:

* The request cannot be replayed in another session.
* The request cannot be replayed later.

---

## 3️⃣ Anti-DDoS Guard (Cheap Pre-Check)

Before verifying signatures (which are expensive), the client sends:

```
Guard = HMAC(window_key, STID)
```

If this guard fails, the server rejects immediately —
without performing signature verification.

This protects CPU under attack.

---

## 4️⃣ Hardware Signature

The client signs the payload using:

* Ed25519
* Hardware-backed key (Secure Enclave / TPM / HSM)

This ensures:

* The request was signed by a legitimate device.
* It cannot be modified.
* It cannot be forged.

---

# 🛡 What the Server Verifies (Strict Order)

1. TLS 1.3 + mTLS valid
2. Time window acceptable
3. Guard valid (cheap check)
4. STID matches recomputed value
5. RID not seen in this TLS session
6. Signature valid
7. STID not already committed globally

Only then does the transaction proceed.

---

# 📊 Comparison with Common Approaches

| Feature                         | JWT (Bearer) | DPoP     | mTLS Only         | TSAPS v1.0 |
| ------------------------------- | ------------ | -------- | ----------------- | ---------- |
| Reusable token                  | Yes          | Yes      | No token          | No         |
| Replay resistance               | Weak         | Partial  | TLS-bound         | Strong     |
| Cross-session replay protection | No           | Limited  | Yes               | Yes        |
| Financial idempotency           | No           | No       | No                | Yes        |
| Hardware-bound signing          | No           | Optional | Certificate-based | Required   |
| Per-request signature           | No           | Yes      | No                | Yes        |
| CPU pre-verification guard      | No           | No       | No                | Yes        |
| Double-spend protection         | No           | No       | No                | Yes        |
| Designed for high-value finance | No           | No       | Partial           | Yes        |

---

# 🏗 Architecture Diagram (Simplified)

```
Client Device
+--------------------------------------------------+
|                                                  |
|  1. Hash Request Body                           |
|  2. Create STID (Financial Fingerprint)         |
|  3. Bind to TLS Session (RID)                   |
|  4. Compute Guard (HMAC)                        |
|  5. Sign Payload (Hardware Key)                 |
|                                                  |
+-------------------------|------------------------+
                          |
                          v
                =====================
                Secure TLS 1.3 (mTLS)
                =====================
                          |
                          v
Server
+--------------------------------------------------+
|  Step 1: Validate TLS                            |
|  Step 2: Validate Time Window                    |
|  Step 3: Verify Guard (Cheap)                    |
|  Step 4: Recompute STID                          |
|  Step 5: Check Session Replay (RID)              |
|  Step 6: Verify Signature                        |
|  Step 7: Global Idempotency Check (STID)         |
|                                                  |
|  --> Commit Transaction                          |
+--------------------------------------------------+
```

---

# 🔒 Security Guarantees

TSAPS v1.0 ensures:

* A signed request cannot be modified.
* A request cannot be replayed in another session.
* A request cannot be replayed later.
* A transaction cannot be double-settled.
* Fake signatures are rejected early.
* CPU exhaustion attacks are mitigated.
* There is no reusable bearer secret.

---

# ❌ What TSAPS Does NOT Do

TSAPS does not:

* Replace TLS.
* Replace fraud detection systems.
* Prevent misuse of a legitimately compromised key.
* Replace AML / compliance systems.
* Replace business-layer validation.

It is a **transaction integrity layer**, not a fraud engine.

---

# 🎯 Intended Use Cases

TSAPS is designed for:

* High-value financial APIs
* Inter-bank settlement rails
* Core banking systems
* CBDC transaction layers
* Zero-trust financial infrastructure
* Payment orchestration engines

It is likely unnecessary for:

* Public consumer APIs
* Basic SaaS apps
* Low-risk data services

---

# 🧩 Design Principles

TSAPS v1.0 follows:

* Deterministic verification
* No reusable bearer secrets
* Channel binding to TLS
* Hardware-backed proof-of-possession
* Global financial idempotency
* Early rejection of malicious traffic
* Zero-trust assumptions

---

# 🏁 Summary

TSAPS v1.0 turns every API request into a:

* Hardware-signed
* TLS-bound
* Replay-proof
* Double-spend-proof
* DDoS-hardened

cryptographic transaction proof.

# Repository Structure
tsaps/
│
├── README.md
├── LICENSE
├── CONTRIBUTING.md
├── COMMERCIAL-LICENSE.md
├── SECURITY.md
├── CODE_OF_CONDUCT.md
│
├── spec/
│   ├── tsaps-v1.0-rfc.md
│   ├── security-model.md
│   └── tamarin/
│       └── tsaps-v1.0.spthy
│
├── test-vectors/
│   ├── v1.0/
│   │   ├── basic-transfer.json
│   │   ├── duplicate-transfer.json
│   │   ├── invalid-guard.json
│   │   └── cross-window.json
│   └── README.md
│
├── reference-implementations/
│   │
│   ├── javascript/
│   │   ├── package.json
│   │   ├── src/
│   │   │   ├── tsaps.js
│   │   │   ├── client.js
│   │   │   └── server.js
│   │   └── README.md
│   │
│   ├── typescript/
│   │   ├── package.json
│   │   ├── tsconfig.json
│   │   ├── src/
│   │   │   ├── tsaps.ts
│   │   │   ├── client.ts
│   │   │   └── server.ts
│   │   └── README.md
│   │
│   ├── go/
│   │   ├── go.mod
│   │   ├── tsaps/
│   │   │   ├── core.go
│   │   │   ├── client.go
│   │   │   └── server.go
│   │   └── README.md
│   │
│   ├── rust/
│   │   ├── Cargo.toml
│   │   ├── src/
│   │   │   ├── lib.rs
│   │   │   ├── client.rs
│   │   │   └── server.rs
│   │   └── README.md
│   │
│   ├── dotnet/
│   │   ├── Tsaps.sln
│   │   ├── Tsaps.Core/
│   │   ├── Tsaps.Client/
│   │   ├── Tsaps.Server/
│   │   └── README.md
│   │
│   └── python/
│       ├── pyproject.toml
│       ├── tsaps/
│       │   ├── core.py
│       │   ├── client.py
│       │   └── server.py
│       └── README.md
│
└── docs/
    ├── architecture.md
    ├── comparison.md
    └── diagrams/
