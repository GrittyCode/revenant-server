# STATUS (source of truth)

Last updated: 2026-11-05. Korean view: `docs/STATUS.ko.md` (regenerate on every update).

## Phase
W1 prep (11/03–11/09): environment + repo setup, then design docs. Implementation starts after design (target ~11/10, not strict).

## Done
- Tools: .NET SDK 10.0.100, Rider 2025.3.4 = IDE (2026.x update optional; workflow = Rider edit/debug + terminal CLI), Docker Desktop, gh (scopes: repo, workflow), k6, mysql-client, git-lfs, Unity Hub, Redis Insight, dotnet-counters/trace, csharp-ls 0.28.0
- Solution skeleton: Revenant.Api / Realtime / Shared (UPM pkg `shared/com.revenant.shared`, netstandard2.1 C#9) / Tests (xUnit v3, MTP runner). build + test + format pass
- CI workflow `.github/workflows/ci.yml` (ubuntu-24.04-arm), first run green 11/05 (run 41943536690, 48s)
- Claude plugins, all project scope (`.claude/settings.json` + install records; user-scope records removed 11/03): csharp-lsp, hookify, context7 (MCP needs auth), learning-output-style, dotnet, dotnet-aspnetcore, dotnet-test (7). Removed 11/05: dotnet-nuget (CPM already done), notion (duplicated the claude.ai Notion connector). Uninstalling a project plugin only writes `false` to settings.local.json; remove the entry from settings.json too
- Notion (11/04): workspace "GrittyCode Portfolio" → page "Revenant 서버 포트폴리오" → 2 inline DBs: 작업 로그 (제목, 날짜) and 학습 노트 (11/05 restructure: 순서, 제목, 주제, 복습, 날짜; 왜 썼나/확인 질문 live in a callout at the top of each page; views 순서대로 + 복습할 것) + page 명령어·옵션 모음. No relations. Repo is source of truth
- ADR 0001 tech stack, ADR 0002 art direction (2D top-down pixel + pixelfx; 2.5D optional after Jan; server coords = floor plane x,y)
- Docker resources wiped on 11/03 for user practice; `docker-compose.yml` is to be written by the user (guide §9)
- Repo (11/05): public `GrittyCode/revenant-server`, root commit `caaa4f6` pushed to main. `gh auth setup-git` configured (git credential helper for github.com)

## Pending decisions
None.

Resolved 11/03 (2): feature folders (`Features/Gacha/`); no `ConfigureAwait(false)` (removed from `Worker.cs`); no LICENSE (all rights reserved); first commit/repo/PR flow is user practice (agent writes guide). Rules added to CLAUDE.md Code rules.

Resolved 11/03: language = C# only (C++ hybrid rejected: IOCP needs Windows, Windows PC weekends only). Apply to C++ 3y+ postings too, leveraging 2y C++ MFC. Full scope kept (see SCOPE.md) ; dates are reference only, optimize for momentum (see CLAUDE.md Progress principles).

## Next (sequential)
1. [x] Restart Claude Code from a new iTerm2 tab (load plugins + LSP PATH)
2. [x] Resolve pending decisions
3. [x] Repo config: .gitignore fixes (un-ignore `*.meta`, `client/Packages/`; `.env.*`), .gitattributes (Unity YAML, LFS, CRLF scripts), nuget.config (`<clear/>` + source mapping), lock files (`RestoreLockedMode` on GitHub Actions). CI-equivalent check passes. Not staged (user stages in step 4). Later, at Unity setup: register `unityyamlmerge` driver in local git config
4. [x] First commit → create GitHub repo → push → CI green (11/05, user practice; guide archived in Notion 학습 노트)
4a. [ ] **Start here.** 3 files are modified but uncommitted on `main` (CLAUDE.md, both STATUS files). Commit them via **branch → PR → CI → squash merge**: `git switch -c docs/status-first-push` → commit → `git push -u` → `gh pr create` → `gh pr checks --watch` → squash merge → sync main. User practice; re-send the guide in chat as batches 1–4 and 5–7. Then ask to make the first Notion digest (Git, 3 notes)
4b. [ ] Tooling candidates (11/05 research; ask when the matching task starts, per CLAUDE.md "Tooling timing"; graphify W4–5 too): Redis MCP (`redis/mcp-redis`, at Redis week), read-only MySQL MCP (SELECT-only DB user, at ERD/query week), `grafana/mcp-grafana` (Viewer token, at Grafana), `dotnet-diag` (week 7), Unity CLI MCP (official replacement for the deprecated in-Editor MCP; compare with CoplayDev unity-mcp at Unity setup)
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
| Git | First commit → repo create → push → CI (11/05 done with help; teach-back partial: -u, add ., CI-only restore failure reviewed) | practicing | Notion |
| Git | Branch → push → PR → CI → squash merge | - | - |
| Linux | Terminal basics, processes, ports/sockets, ulimit, ssh/systemd/journalctl (woven into Docker, server run, k6, EC2 tasks) | practicing | Notion 명령어·옵션 모음 |
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
