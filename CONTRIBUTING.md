# Contributing to TSAPS

Thank you for your interest in improving TSAPS.

## Contribution Scope

We welcome:

- Security reviews
- Formal verification improvements
- Bug fixes
- Test vector contributions
- Reference implementation improvements
- Documentation clarity improvements

We do NOT accept:

- Marketing changes
- Breaking protocol modifications without formal proposal
- Cryptographic algorithm changes without justification

## Protocol Change Policy

TSAPS is a cryptographic protocol.

Any change that modifies:

- STID computation
- RID computation
- Guard logic
- Signature binding
- Idempotency semantics

MUST be submitted as a formal proposal in the `/spec` directory.

Breaking changes require version bump (e.g., v1.1).

## Pull Request Process

1. Fork the repository
2. Create a feature branch
3. Include test coverage
4. Reference relevant test vectors
5. Submit pull request

All cryptographic modifications require review.

## Security Contributions

If your contribution relates to a potential vulnerability,
please follow SECURITY.md instead of opening a public issue.