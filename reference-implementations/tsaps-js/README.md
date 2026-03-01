TSAPS v1.0 — JavaScript Reference Implementation

This folder contains a minimal JavaScript reference implementation of TSAPS v1.0.

⚠️ This is a reference implementation only.
It is designed for learning, testing, and validating protocol logic — not production deployment.

📦 Project Structure
tsaps-js/
 ├── client.js        # Simulates client-side TSAPS request creation
 ├── server.js        # Simulates server-side verification
 ├── tsaps.js         # Core TSAPS cryptographic logic
 └── package.json
🔐 Cryptographic Components Used

This reference implementation uses:

tweetnacl → Ed25519 signing & verification

Node.js crypto module → SHA256, HMAC, HKDF

These primitives reflect the TSAPS v1.0 cryptographic design.

1️⃣ Install Dependencies

Initialize the project:

npm init -y

Install required dependencies:

npm install tweetnacl tweetnacl-util

No additional crypto libraries are required — Node's built-in crypto module is used for hashing and key derivation.

▶ Run the Example

Execute:

node client.js
✅ Expected Output

You should see something similar to:

---- Server Verification ----
✔ Guard valid
✔ STID valid
✔ RID unique
✔ Signature valid
✔ Transaction committed

This means:

Temporal Guard window validated

STID (Session Temporal ID) verified

RID (Request ID) not reused

Ed25519 signature verified

Request accepted and committed

🔁 Replay Protection Demonstration

If you immediately run the script again within the same time window:

node client.js

You should see a failure due to duplicate STID or RID reuse.

This demonstrates TSAPS’s built-in replay resistance.

🧪 What This Demonstrates

This reference implementation shows:

Deterministic STID construction

Guard window validation

Replay protection via RID uniqueness

Ed25519 signature verification

Atomic commit validation

🚫 Production Warning

This implementation:

Does NOT include distributed cache

Does NOT include rate limiting

Does NOT include key rotation

Does NOT include hardware-backed keys

Does NOT include persistence

For production systems, use a hardened SDK and proper infrastructure.

📘 Related Documentation

See the main repository:

/spec
/formal-model
/test-vectors

For the full TSAPS v1.0 protocol specification and security model.