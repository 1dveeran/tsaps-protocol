use rand::rngs::OsRng;
use ed25519_dalek::{SigningKey, VerifyingKey};
use base64;
use crate::tsaps::*;
use std::time::{SystemTime, UNIX_EPOCH};

pub struct TsapsRequest {
    pub client_id: String,
    pub tid: String,
    pub stid: String,
    pub pvg: String,
    pub signature: ed25519_dalek::Signature,
    pub verifying_key: VerifyingKey,
    pub timestamp: i64,
    pub tls_exporter: String,
}

pub struct Client {
    client_id: String,
    signing_key: SigningKey,
}

impl Client {
    pub fn new(client_id: &str) -> Self {
        let mut csprng = OsRng;
        let signing_key = SigningKey::generate(&mut csprng);

        Self {
            client_id: client_id.to_string(),
            signing_key,
        }
    }

    pub fn create_request(&self, server_secret: &[u8]) -> TsapsRequest {
        let message = "Transfer 100 tokens";
        let nonce = uuid::Uuid::new_v4().to_string();
        let timestamp = SystemTime::now()
            .duration_since(UNIX_EPOCH)
            .unwrap()
            .as_secs() as i64;

        let tls_exporter = base64::encode(rand::random::<[u8; 32]>());

        let tid = compute_tid(message, &nonce, timestamp);
        let stid = compute_stid(server_secret, &self.client_id, timestamp);
        let pvg = compute_pvg(server_secret, &tid, &tls_exporter);
        let signature = sign_tid(&self.signing_key, &tid);

        TsapsRequest {
            client_id: self.client_id.clone(),
            tid,
            stid,
            pvg,
            signature,
            verifying_key: self.signing_key.verifying_key(),
            timestamp,
            tls_exporter,
        }
    }
}