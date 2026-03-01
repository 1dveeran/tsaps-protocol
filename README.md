[![DOI](https://zenodo.org/badge/DOI/10.5281/zenodo.18821754.svg)](https://doi.org/10.5281/zenodo.18821754)

# TSAPS v1.0
## Temporal-Atomic Secure API Protocol

Deterministic, TLS-Bound, Hardware-Signed Transaction Proof  
for High-Value API Systems

---

# 🚀 Why TSAPS Exists

Modern APIs commonly rely on **bearer tokens** (e.g., JWTs).

Bearer tokens have a structural weakness:

> If the token is copied, it can be reused until it expires.

For low-risk systems, this tradeoff is acceptable.  
For high-value financial or settlement systems, it is not.

High-assurance APIs require:

- Deterministic replay resistance
- Strong proof-of-possession
- Transaction-level idempotency
- Strict temporal binding
- Early rejection of malicious traffic
- Zero-trust verification principles

TSAPS replaces reusable bearer tokens with:

> A per-request cryptographic transaction proof.

Each API request becomes a signed, session-bound, time-scoped proof of intent.

---

# 🧠 How TSAPS v1.0 Works (Conceptual Overview)

Instead of sending a reusable access token, the client:

1. Hashes the request body.
2. Creates a **Semantic Transaction ID (STID)** representing the financial intent.
3. Binds the request to the current TLS session.
4. Adds a lightweight anti-DDoS guard.
5. Signs the entire structure using a hardware-backed private key.

The server verifies each component in a strict deterministic order before processing the transaction.

---

# 🔐 Protocol Flow

## 1️⃣ Financial Fingerprint (STID)

The client computes:

```
STID = SHA256(method || path || body_hash || client_key_id)
```

This value represents the semantic financial intent of the request.

If an identical transaction is submitted again, the STID will be identical, enabling deterministic duplicate detection.

---

## 2️⃣ TLS Session Binding (RID)

The client derives a TLS exporter value from the active TLS 1.3 session.

It then computes:

```
RID = SHA256(STID || window_index || TLS_exporter)
```

This ensures the request is bound to:

- A specific TLS session
- A specific temporal window

Requests replayed in another session or outside the valid window will be rejected.

---

## 3️⃣ Anti-DDoS Guard (Pre-Signature Check)

Before verifying signatures, the client includes:

```
Guard = HMAC(window_key, STID)
```

The server validates this inexpensive check first.

Invalid guards are rejected before signature verification, reducing CPU exhaustion risk.

---

## 4️⃣ Hardware-Backed Signature

The client signs the final payload using:

- Ed25519
- A hardware-protected private key (Secure Enclave / TPM / HSM)

This provides:

- Proof-of-possession
- Payload integrity
- Strong client identity binding

---

# 🛡 Server Verification Order

The server processes requests in strict order:

1. Validate TLS 1.3 + mTLS
2. Validate time window
3. Validate Guard (cheap pre-check)
4. Recompute and verify STID
5. Ensure RID not seen in this session
6. Verify signature
7. Ensure STID not already globally committed

Only after all checks succeed does the transaction proceed.

This ordering ensures inexpensive checks occur before expensive cryptographic operations.

---

# 📊 Comparison with Common Approaches

| Feature                         | JWT (Bearer) | DPoP     | mTLS Only | TSAPS v1.0 |
|----------------------------------|--------------|----------|-----------|------------|
| Reusable token                   | Yes          | Yes      | No token  | No         |
| Per-request signature            | No           | Yes      | No        | Yes        |
| TLS channel binding              | Optional     | Yes      | Yes       | Yes        |
| Deterministic duplicate control  | No           | No       | No        | Yes        |
| Built-in replay window control   | Weak         | Partial  | Session   | Strong     |
| Hardware-backed signing required | No           | Optional | Certificate-based | Yes |
| Early CPU guard mechanism        | No           | No       | No        | Yes        |
| Designed for high-value finance  | No           | No       | Partial   | Yes        |

---

# 🔒 Security Properties

TSAPS v1.0 is designed to provide:

- Deterministic transaction fingerprinting (via STID)
- Session-bound replay resistance (via RID)
- Temporal window enforcement
- Hardware-backed proof-of-possession
- Global duplicate detection capability
- Early rejection of malformed or malicious traffic

These guarantees depend on correct implementation and infrastructure configuration.

---

# 🧱 Threat Model

TSAPS assumes:

- TLS 1.3 confidentiality and integrity
- Proper certificate validation
- Secure hardware key storage on client device
- Potentially malicious network attackers
- Possible replay attempts across sessions
- Possible CPU exhaustion attempts

TSAPS does NOT assume:

- Trusted internal networks
- Trusted intermediaries
- Trusted client runtime environments
- Protection against compromised hardware keys

If a legitimate client key is compromised, TSAPS cannot prevent misuse.

---

# ❌ What TSAPS Does NOT Do

TSAPS does not:

- Replace TLS
- Replace fraud detection systems
- Prevent misuse of a compromised private key
- Replace AML / compliance systems
- Replace business-layer validation logic

It is a **transaction integrity layer**, not a fraud detection engine.

---

# 🎯 Intended Use Cases

TSAPS is designed for:

- High-value financial APIs
- Inter-bank settlement rails
- Core banking systems
- CBDC infrastructure layers
- Payment orchestration systems
- Zero-trust financial environments

It is likely unnecessary for:

- Public low-risk APIs
- Standard SaaS applications
- Non-transactional services

---

# 🧩 Design Principles

TSAPS v1.0 follows:

- Deterministic verification
- No reusable bearer secrets
- Strict TLS channel binding
- Hardware-backed proof-of-possession
- Financial idempotency support
- Early rejection of invalid traffic
- Zero-trust assumptions

---

# ⚡ Performance Characteristics

TSAPS is designed to:

- Reject invalid requests before signature verification
- Avoid unnecessary database reads for invalid guards
- Support O(1) duplicate detection using STID indexing
- Scale horizontally using distributed replay stores
- Maintain low per-request latency

Actual performance depends on deployment architecture.

---

# 📌 Protocol Status

Version: v1.0  
Status: Draft Specification  
Stability: Experimental  

Breaking changes may occur in future versions.

Formal verification artifacts are available under:

```
spec/security-model.md
spec/tamarin/tsaps-v1.0.spthy
```

---

# 🏗 Repository Structure

```
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
```

---

# 🏁 Summary

TSAPS v1.0 transforms each API request into a:

- Hardware-signed  
- TLS-bound  
- Time-scoped  
- Deterministically fingerprinted  
- Replay-resistant  
- Duplicate-detectable  

cryptographic transaction proof.

It is designed for environments where transaction integrity must be explicit, verifiable, and deterministic.

# 🚀 Roadmap — TSAPS v1.1

TSAPS v1.1 focuses on formal rigor, interoperability, and ecosystem maturity.

## 🔐 1. Formal Verification Completion
- Complete full Tamarin proof coverage
- Publish formal security proof document
- Provide adversarial replay model validation

## 🧠 2. Post-Quantum Extension Layer (Optional)
- Hybrid signature mode (Ed25519 + PQC)
- Pluggable signature abstraction
- Migration strategy for future quantum resistance

## 🌐 3. Distributed Replay Store Specification
- Standardized STID indexing format
- Horizontal scaling guidance
- Multi-region duplicate control model
- Failure-mode analysis

## 🧪 4. Cross-Language Conformance Suite
- Deterministic test vectors
- Interoperability testing harness
- Automated CI validation across SDKs

## 🛠 5. Production SDK Hardening
- Secure key storage integrations
- HSM compatibility layer
- Observability and audit logging extensions
- Rate-limiting integration guidelines

## 📜 6. Draft Standardization
- Prepare IETF-style draft
- Publish security considerations section
- Collect peer review from cryptography community

---

## 🎯 Long-Term Vision (v2.x)

- Hardware attestation integration
- Secure enclave remote attestation binding
- Deterministic cross-institution settlement layer
- Native support for CBDC-style architectures

## Citation
If you use this protocol in your research or implementation, please cite it as:

Paventhan D. (2026). TASAP v1.0: Temporal-Atomic Secure API Protocol. Zenodo. https://doi.org/10.5281/zenodo.18821754