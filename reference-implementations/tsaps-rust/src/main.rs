mod tsaps;
mod client;
mod server;

use rand::RngCore;
use client::Client;
use server::Server;

fn main() {
    println!("---- TSAPS Rust Reference ----");

    let mut server_secret = vec![0u8; 32];
    rand::thread_rng().fill_bytes(&mut server_secret);

    let client = Client::new("client1");
    let mut server = Server::new(server_secret.clone());

    let request = client.create_request(&server_secret);

    server.verify(&request);

    println!("\n-- Replaying same request --");
    server.verify(&request);
}