use sha2::{Sha256, Digest};
use hmac::{Hmac, Mac};
use sha2::Sha256 as HmacSha256;
use ed25519_dalek::{Signer, Verifier, SigningKey, VerifyingKey, Signature};
use hex;

type HmacSha256Type = Hmac<HmacSha256>;

pub fn compute_tid(message: &str, nonce: &str, timestamp: i64) -> String {
    let mut hasher = Sha256::new();
    hasher.update(format!("{}|{}|{}", message, nonce, timestamp));
    hex::encode(hasher.finalize())
}

pub fn compute_hmac(key: &[u8], input: &str) -> String {
    let mut mac = HmacSha256Type::new_from_slice(key).unwrap();
    mac.update(input.as_bytes());
    let result = mac.finalize().into_bytes();
    hex::encode(result)
}

pub fn compute_stid(server_secret: &[u8], client_id: &str, timestamp: i64) -> String {
    let epoch = timestamp / 60;
    compute_hmac(server_secret, &format!("{}|{}", client_id, epoch))
}

pub fn compute_pvg(server_secret: &[u8], tid: &str, tls_exporter: &str) -> String {
    compute_hmac(server_secret, &format!("{}|{}", tid, tls_exporter))
}

pub fn sign_tid(signing_key: &SigningKey, tid: &str) -> Signature {
    signing_key.sign(tid.as_bytes())
}

pub fn verify_signature(
    verifying_key: &VerifyingKey,
    tid: &str,
    signature: &Signature,
) -> bool {
    verifying_key.verify(tid.as_bytes(), signature).is_ok()
}