// server.js
const {
  sha256,
  computeSTID,
  computeRID,
  hkdf,
  hmac,
  verifySignature,
  S_MASTER
} = require("./tsaps");

// In-memory stores
const sessionReplay = new Set();
const globalIdempotency = new Set();

function verifyRequest({ payload, signature, guard, publicKey }) {
  console.log("---- Server Verification ----");

  const { m, u, h, w, b, stid, rid, kid } = payload;

  // 1. Guard Check
  const windowKey = hkdf(S_MASTER, w);
  const expectedGuard = hmac(windowKey, stid);

  if (guard !== expectedGuard) {
    return console.log("❌ Guard failed");
  }

  console.log("✔ Guard valid");

  // 2. Recompute STID
  const recomputedSTID = computeSTID(m, u, h, kid);
  if (recomputedSTID !== stid) {
    return console.log("❌ STID mismatch");
  }

  console.log("✔ STID valid");

  // 3. Session Replay Check
  if (sessionReplay.has(rid)) {
    return console.log("❌ Replay detected");
  }

  sessionReplay.add(rid);
  console.log("✔ RID unique");

  // 4. Signature Verification
  if (!verifySignature(payload, signature, publicKey)) {
    return console.log("❌ Signature invalid");
  }

  console.log("✔ Signature valid");

  // 5. Global Idempotency
  if (globalIdempotency.has(stid)) {
    return console.log("❌ Duplicate transaction");
  }

  globalIdempotency.add(stid);
  console.log("✔ Transaction committed");
}

module.exports = { verifyRequest };