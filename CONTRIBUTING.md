# Contributing to TSAPS

Thank you for your interest in improving the **Temporal-Atomic Secure API Protocol (TSAPS)**. As a high-stakes financial security protocol, we maintain a rigorous standard for all contributions to ensure deterministic integrity.

## Contribution Scope

We welcome technical contributions that strengthen the protocol's security and implementation:
- **Security Reviews:** Peer reviews of the cryptographic primitives and logic.
- **Formal Verification:** Improvements to the mathematical models and security proofs.
- **Bug Fixes:** Corrections to the reference implementation.
- **Test Vectors:** New scenarios to validate the **Guard** and **Idempotency** logic.
- **Documentation:** Enhancing the clarity of the v1.0-draft specification.

**We do NOT accept:**
- Marketing-heavy changes or non-technical modifications.
- Breaking protocol changes without an accompanying formal security analysis.
- Cryptographic algorithm changes (e.g., moving away from **Ed25519**) without a robust mathematical justification.

## Protocol Change Policy

TSAPS is a formal cryptographic protocol. Any change modifying the core identity or verification flow must align with the official specification archived on **Zenodo** (DOI: [10.5281/zenodo.18821754](https://doi.org/10.5281/zenodo.18821754)).

Specifically, changes to the following **MUST** be submitted as a formal proposal:
- **STID/RID Computation:** Any modification to session or request identification.
- **Guard Logic:** Changes to the Stateless Pre-Verification flow.
- **Signature Binding:** Adjustments to how signatures are bound to the TLS channel.
- **Idempotency Semantics:** Modifications to how the protocol handles duplicate financial transactions.

*Note: Breaking changes will require a version bump (e.g., v1.1) and a new Zenodo archival entry.*

## Pull Request Process

1. **Fork the Repository:** Create a dedicated feature branch for your work.
2. **Test Coverage:** Include unit tests and reference relevant test vectors.
3. **Alignment:** Ensure code matches the logic defined in the **v1.0-draft PDF**.
4. **Licensing:** By contributing, you agree that your work will be licensed under the **Business Source License (BSL) 1.1**.
5. **Review:** All cryptographic modifications require a mandatory security audit by the maintainers.

## Security Contributions

If your contribution relates to a potential vulnerability, **DO NOT open a public issue.** Please follow the private reporting process outlined in our **[SECURITY.md](./SECURITY.md)** to protect financial implementers.