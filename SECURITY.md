# Security Policy

TSAPS (Temporal-Atomic Secure API Protocol) is a formal cryptographic transaction protocol. Given its role in financial integrity, we maintain a strict security posture.

## Supported Versions

Please ensure you are developing against the version archived on Zenodo to maintain deterministic security guarantees.

| Version | Status | DOI / Reference |
| :--- | :--- | :--- |
| **v1.0-draft** | :white_check_mark: Active | [10.5281/zenodo.18821754](https://doi.org/10.5281/zenodo.18821754) |
| < v1.0 | :x: Unsupported | N/A |

## Reporting a Vulnerability

**DO NOT open a public GitHub issue for security vulnerabilities.** Public disclosure before a patch is available puts financial implementations at risk.

If you discover a security issue, please email:
**techken.in@gmail.com**

**Please include the following in your report:**
* **Technical Description:** A detailed explanation of the vulnerability (e.g., Signature Bypass, TLS Binding weakness).
* **Proof-of-Concept (PoC):** Code or steps required to reproduce the issue.
* **Impact Assessment:** How this affects the "Guard" or "Idempotency" logic.
* **Affected Environment:** Hardware or library versions used during discovery.

We aim to acknowledge receipt of your report within **5 business days**.

## Responsible Disclosure

We request a **90-day responsible disclosure window**. This allows us to update the formal model on Zenodo and notify known implementers before the vulnerability is made public.

## Scope

We strictly track vulnerabilities related to the protocol's core logic:
* **Cryptographic Flaws:** Signature/Guard bypass or Ed25519 implementation errors.
* **Protocol Logic:** Replay vulnerabilities or Idempotency bypass.
* **Channel Binding:** Weaknesses in TLS 1.3 session binding.
* **Model Inconsistency:** Deviations from the published TASAP v1.0 specification.

*Note: Infrastructure or local deployment misconfigurations are considered out of scope.*