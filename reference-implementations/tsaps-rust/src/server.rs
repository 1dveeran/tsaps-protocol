use std::collections::HashSet;
use crate::client::TsapsRequest;
use crate::tsaps::*;

pub struct Server {
    server_secret: Vec<u8>,
    replay_cache: HashSet<String>,
}

impl Server {
    pub fn new(server_secret: Vec<u8>) -> Self {
        Self {
            server_secret,
            replay_cache: HashSet::new(),
        }
    }

    pub fn verify(&mut self, request: &TsapsRequest) {
        println!("\n---- Server Verification ----");

        // Replay check
        if self.replay_cache.contains(&request.tid) {
            println!("✘ Replay detected");
            return;
        }
        self.replay_cache.insert(request.tid.clone());
        println!("✔ TID unique");

        // STID check
        let expected_stid =
            compute_stid(&self.server_secret, &request.client_id, request.timestamp);

        if expected_stid != request.stid {
            println!("✘ STID invalid");
            return;
        }
        println!("✔ STID valid");

        // PVG check
        let expected_pvg =
            compute_pvg(&self.server_secret, &request.tid, &request.tls_exporter);

        if expected_pvg != request.pvg {
            println!("✘ Guard invalid");
            return;
        }
        println!("✔ Guard valid");

        // Signature check
        if !verify_signature(&request.verifying_key, &request.tid, &request.signature) {
            println!("✘ Signature invalid");
            return;
        }

        println!("✔ Signature valid");
        println!("✔ Transaction committed");
    }
}