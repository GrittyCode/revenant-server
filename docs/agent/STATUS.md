# STATUS (source of truth)

Last updated: 2026-11-03. Korean view: `docs/STATUS.ko.md` (regenerate on every update).

## Phase
W1 prep (11/03–11/09): environment + repo setup, then design docs. Implementation starts after design (target ~11/10, not strict).

## Done
- Tools: .NET SDK 10.0.100, Rider (update to 2026.x pending), Docker Desktop, gh (scopes: repo, workflow), k6, mysql-client, git-lfs, Unity Hub, Redis Insight, dotnet-counters/trace, csharp-ls 0.28.0
- Solution skeleton: Revenant.Api / Realtime / Shared (UPM pkg `shared/com.revenant.shared`, netstandard2.1 C#9) / Tests (xUnit v3, MTP runner). build + test + format pass
- CI workflow `.github/workflows/ci.yml` (ubuntu-24.04-arm), not run yet
- Claude plugins, all project scope (`.claude/settings.json` + install records; user-scope records removed 11/03): csharp-lsp, hookify, context7, learning-output-style, dotnet, dotnet-aspnetcore, dotnet-test, dotnet-nuget, notion (11/04; knowledge-capture/research-documentation skills; bundles its own Notion MCP, duplicates claude.ai connector)
- Notion (11/04): workspace "GrittyCode Portfolio" → page "Revenant 서버 포트폴리오" → 2 inline DBs: 작업 로그 (제목, 날짜) and 학습·결정 노트 (제목, 날짜, 유형, 주제, 복습). One view each, newest first, no relations. Repo is source of truth
- ADR 0001 tech stack, ADR 0002 art direction (2D top-down pixel + pixelfx; 2.5D optional after Jan; server coords = floor plane x,y)
- Docker resources wiped on 11/03 for user practice; `docker-compose.yml` is to be written by the user (guide §9)
- Repo: `git init` done, **no commits yet**, all files staged. GitHub repo `GrittyCode/revenant-server` (public) not created

## Pending decisions
None.

Resolved 11/03 (2): feature folders (`Features/Gacha/`); no `ConfigureAwait(false)` (removed from `Worker.cs`); no LICENSE (all rights reserved); first commit/repo/PR flow is user practice (agent writes guide). Rules added to CLAUDE.md Code rules.

Resolved 11/03: language = C# only (C++ hybrid rejected: IOCP needs Windows, Windows PC weekends only). Apply to C++ 3y+ postings too, leveraging 2y C++ MFC. Full scope kept (see SCOPE.md) ; dates are reference only, optimize for momentum (see CLAUDE.md Progress principles).

## Next (sequential)
1. [x] Restart Claude Code from a new iTerm2 tab (load plugins + LSP PATH)
2. [x] Resolve pending decisions
3. [x] Repo config: .gitignore fixes (un-ignore `*.meta`, `client/Packages/`; `.env.*`), .gitattributes (Unity YAML, LFS, CRLF scripts), nuget.config (`<clear/>` + source mapping), lock files (`RestoreLockedMode` on GitHub Actions). CI-equivalent check passes. Not staged (user stages in step 4). Later, at Unity setup: register `unityyamlmerge` driver in local git config
4. [ ] First commit → create GitHub repo → push → CI green. **User practice**, guide `docs/learning/02-git-first-push.md`. Repo scrubbed of employer references 11/04 (no commits yet, so no history to rewrite)
5. [ ] Branch protection (PR required, CI required, up-to-date, linear, no force push), labels, milestones, secret scanning, Dependabot

## Next (parallel, after step 4)
| Track | Task | Owner | Branch |
|---|---|---|---|
| A | Conventions (csharp, unity, api, sql; English) + `docs/git-strategy.md` (Korean) | agent session | docs/conventions |
| B | Project skills: /explain, /guide, /adr, /migration, /interview. Refs: `alirezarezvani/claude-skills` engineering/grill-me (MIT, adapt + credit in THIRD_PARTY_NOTICES) for /interview; `lyndonkl/claude` skills/adr-architecture (no license: ideas only, no copying) for /adr | agent session | chore/skills |
| C | hookify rules: block docker delete, unnamed prune, force push, push to main | user + agent | chore/hooks |
| D | Doc skeletons: architecture, runbook, protocol | agent session | docs/skeleton |
| U | Docker hands-on, Unity 6.3 LTS install | user | - |

## Design (11/04–11/09)
- [ ] 1-page GDD + feature list (Must/Should/Could) + perf targets
- [ ] Architecture diagram
- [ ] ERD + rationale
- [ ] API spec + error codes + packet spec
- [ ] GitHub issues

## Practice tracker
| Area | Item | Stage | Guide |
|---|---|---|---|
| Docker | CLI: images, containers, volumes, networks | guide | docs/learning/00-docker-hands-on.md |
| Docker | Write docker-compose.yml | guide | 00 §9 |
| DB | Flyway migrations | guide | 00 §7 |
| Claude Code | Plugin install/scope management (11/03: user→project migration done, guided) | practicing | - |
| Git | First commit → repo create → push → CI | guide | docs/learning/02-git-first-push.md |
| Git | Branch → push → PR → CI → squash merge | - | - |
| DB | Queries, EXPLAIN | - | - |
| Redis | redis-cli data structures | - | - |
| .NET | dotnet CLI | - | - |
| .NET | Rider debugging | - | - |
| CI/CD | Edit GitHub Actions | - | - |
| Deploy | EC2, SSM, compose on server | - | - |
| Perf | k6, dotnet-counters | - | - |
| Unity | Project setup, local UPM package | - | - |

## Active sessions
| Session | Task | Branch/worktree | Started |
|---|---|---|---|
| - | | | |

## Ideas
- Raid practice area: training dummy + server-authoritative live DPS meter (reuses raid contribution logic). Should.
- TLS on/off perf comparison for Realtime (config-driven via SslStream, not recompile). Should, week 7.
- Realtime rule: no DB/HTTP calls inside Room JobQueue; dispatch async and post results back to the queue.
