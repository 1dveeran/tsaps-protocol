// tsaps.js
const crypto = require("crypto");
const nacl = require("tweetnacl");

// ---------- CONFIG ----------
const WINDOW_SIZE = 30; // seconds
const S_MASTER = crypto.randomBytes(32); // server master secret

// ---------- HELPERS ----------
function sha256(data) {
  return crypto.createHash("sha256").update(data).digest("hex");
}

function hmac(key, data) {
  return crypto.createHmac("sha256", key).update(data).digest("hex");
}

function hkdf(master, windowIndex) {
  return crypto.hkdfSync("sha256", master, Buffer.alloc(0), Buffer.from(String(windowIndex)), 32);
}

function getWindowIndex() {
  return Math.floor(Date.now() / 1000 / WINDOW_SIZE);
}

// ---------- STID ----------
function computeSTID(method, path, bodyHash, keyId) {
  return sha256(method + path + bodyHash + keyId);
}

// ---------- RID ----------
function computeRID(stid, windowIndex, tlsExporter) {
  return sha256(stid + windowIndex + tlsExporter);
}

// ---------- SIGN ----------
function signPayload(payload, privateKey) {
  const prefix = "\x19TSAPS-v4.3\x00Ed25519\x00Transaction-Signature\x00";
  const message = Buffer.from(prefix + JSON.stringify(payload));
  const signature = nacl.sign.detached(message, privateKey);
  return Buffer.from(signature).toString("hex");
}

function verifySignature(payload, signatureHex, publicKey) {
  const prefix = "\x19TSAPS-v4.3\x00Ed25519\x00Transaction-Signature\x00";
  const message = Buffer.from(prefix + JSON.stringify(payload));
  const signature = Buffer.from(signatureHex, "hex");
  return nacl.sign.detached.verify(message, signature, publicKey);
}

module.exports = {
  sha256,
  hmac,
  hkdf,
  getWindowIndex,
  computeSTID,
  computeRID,
  signPayload,
  verifySignature,
  S_MASTER
};