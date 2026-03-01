// client.js
const nacl = require("tweetnacl");
const crypto = require("crypto");
const {
  sha256,
  computeSTID,
  computeRID,
  getWindowIndex,
  hkdf,
  hmac,
  signPayload,
  S_MASTER
} = require("./tsaps");

// Generate hardware-like key pair
const keyPair = nacl.sign.keyPair();
const keyId = sha256(Buffer.from(keyPair.publicKey)).slice(0, 16);

// Simulated request
const method = "POST";
const path = "/transfer";
const body = JSON.stringify({ amount: 1000, to: "Alice" });
const bodyHash = sha256(body);

// TLS exporter simulation
const tlsExporter = crypto.randomBytes(32).toString("hex");

// Window
const windowIndex = getWindowIndex();

// Compute STID
const stid = computeSTID(method, path, bodyHash, keyId);

// Compute RID
const rid = computeRID(stid, windowIndex, tlsExporter);

// Guard
const windowKey = hkdf(S_MASTER, windowIndex);
const guard = hmac(windowKey, stid);

// Payload
const payload = {
  u: path,
  m: method,
  h: bodyHash,
  w: windowIndex,
  b: tlsExporter,
  stid,
  rid,
  kid: keyId,
  v: "4.3"
};

// Signature
const signature = signPayload(payload, keyPair.secretKey);

// Send to server
require("./server").verifyRequest({
  payload,
  signature,
  guard,
  publicKey: keyPair.publicKey
});