# Git hooks

Versioned git hooks for this repo. They are **not** active until you point git
at this directory (a one-time step per clone).

## Enable

```sh
git config core.hooksPath .githooks
```

On Windows, also mark the hook executable so git's bundled `sh` runs it cleanly:

```sh
git update-index --add --chmod=+x .githooks/pre-commit
```

## What `pre-commit` does

On every commit it:

1. **Scans staged changes for secrets** — added lines are checked against
   patterns for private keys, AWS keys, JWTs, GitHub tokens, connection-string
   passwords, and generic `secret/password/api_key/token = ...` assignments.
   It also refuses to commit `appsettings.json` / `appsettings.Development.json`.
2. **Builds** the solution (`dotnet build AscentListerAPI.sln`).
3. **Runs the tests** (`dotnet test AscentListerAPI.sln --no-build`).

If any step fails, the commit is aborted.

## False positives & bypass

- Append `# pragma: allowlist secret` to a flagged line to whitelist it.
- Skip all checks for a single commit with `git commit --no-verify`.

## Tuning

Secret patterns live in the `PATTERNS` block at the top of `pre-commit`.
