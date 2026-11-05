# Revenant — Agent Rules

## Session start
1. Read `docs/agent/STATUS.md` first (source of truth: phase, pending decisions, next tasks, practice tracker, active sessions). Feature scope and priorities: `docs/agent/SCOPE.md`.
2. Before recommending a tool/IDE/library/approach, check STATUS, SCOPE, ADRs for an existing decision and cite it. IDE = Rider (csharp-ls is Claude's LSP only).
3. When work is done, update `docs/agent/STATUS.md`, then regenerate the Korean view `docs/STATUS.ko.md` in the same change.
4. Session end: add one row per day to Notion inline DB "작업 로그" (`collection://9d24a684-6b08-4d1c-abba-681767e8a830`; props 제목, 날짜 only; body 2–3 bullets). Keep Notion minimal: two DBs, one view each, no relations/links/extra props unless the user asks.

## Document language policy
- **Agent-facing docs are English** (this file, `docs/agent/*`, `.claude/**`, conventions under `docs/conventions/`). Keep them terse.
- **User/interviewer-facing docs are Korean**: `docs/STATUS.ko.md`, `docs/learning/*`, `docs/adr/*`, `docs/db/*`, `README.md`, PR descriptions.
- `docs/STATUS.ko.md` / `docs/SCOPE.ko.md` are Korean views of `docs/agent/STATUS.md` / `docs/agent/SCOPE.md`. Never edit only one side.
- Talk to the user in Korean.

## Project
- Game server portfolio. 2026-12-31 is a **soft target, not a deadline**. Collection RPG + 2–4p real-time co-op raid. Shallow gameplay, **server features must be visible** (debug panel, admin tool, Grafana, demo scenarios).
- User knows C# from 1y of Unity client work; server background is Node and Spring/Kotlin. New territory is **server-side .NET** (thread-pool concurrency, async without SynchronizationContext, DI lifetimes, Generic Host). Skip basic syntax; explain via Spring/Kotlin and "in Unity you did X" comparisons.

## Progress principles (optimize for momentum, not dates)
- Never rescope, rush, or pressure because of the calendar. Missing 12/31 is acceptable.
- Follow the milestone order in `docs/agent/SCOPE.md` (Must first, in order).
- Keep STATUS "Next" always concrete: the very next task must be startable in under 5 minutes.
- Small slices: every session should end with something working or merged, even if tiny.
- The user has ~1h on weekdays: the agent does boilerplate/config/docs so the user's time goes to core logic and learning.
- No gold-plating: polish goes to Should/Could. If stuck >20 min, simplify or ask.

## Learning routine (after every feature/PR)
1. Code tour (Spring/Kotlin/Unity comparisons)
2. Teach-back: user explains, agent corrects
3. 5 interview questions (include DB and pattern questions)
4. 15–30 min variation task done by the user
5. Korean note at `docs/learning/NN-topic.md` (source of truth), then a row in Notion inline DB "학습·결정 노트" (`collection://a7f8bfbe-399c-472d-affd-a6829b646f36`; props 제목, 날짜, 유형=학습|결정|트러블슈팅, 주제, 복습=미복습; body = 3–4 bullets + **면접 질문** 1–2)

Guide the user to write core logic (concurrency, transactions, Room JobQueue) themselves; the agent writes boilerplate only.

## Practice → automation
- New tooling/infra tasks (Docker, migrations, Redis, dotnet CLI, debugging, CI edits, AWS deploy, k6, Unity setup, git/PR flow): agent writes a Korean hands-on guide, **the user executes**, agent only assists.
- Stages tracked in `docs/agent/STATUS.md` → Practice tracker: `guide → practicing → comfortable → automated`.
- After 2 unaided successes + teach-back passed, **ask** "자동화할까요?". Automate only after the user approves (or orders it).
- Guide lifecycle: give hands-on guides **in chat** (Korean, step by step); do not create repo files for them. After the user completes it, publish the **full guide as Markdown** to Notion "학습·결정 노트" (유형=학습, 주제=area, 복습=미복습). Body = the whole guide as given (with corrections), plus "실행 기록" (what happened, errors hit, fixes) and the teach-back Q&A. For guides, Notion is the archive.
- Never run a not-yet-automated task for the user. Exceptions: read-only status commands, boilerplate, formatting, doc drafts.

## Parallel sessions
- One session = one task = one branch, each in its own **git worktree** (never edit the same working tree concurrently).
- Register in STATUS "Active sessions" on start; remove on finish.
- Do not hand user-practice items to parallel sessions.
- Do not touch files owned by another active session. Keep edits to shared files (CLAUDE.md, STATUS files, Directory.*.props) minimal.

## Code rules
- Server projects: net10.0, C# 14. Shared: netstandard2.1 + C# 9 (Unity compatible — no newer syntax).
- Package versions only in `Directory.Packages.props`.
- Warnings are errors. Logging via `[LoggerMessage]` source generator.
- Folders: feature folders (`Features/Gacha/`), infra under `Infrastructure/`.
- No `ConfigureAwait(false)` in server code (no SynchronizationContext).
- Before commit: `dotnet format --verify-no-changes`, `dotnet build`, `dotnet test`.
- Record design decisions as `docs/adr/NNNN-title.md` (Korean).

## Core patterns (only these five)
Layered + Unit of Work / Strategy (reward granting) / Actor + Command (Room JobQueue) / State (Room, boss) / Outbox (DB → Redis)

## Storage
MySQL 8.4 + Redis 7.4 only. No PostgreSQL, no MongoDB.

## Git
- GitHub Flow, small PRs (≤300–400 lines, 1–2 days), Squash Merge default. Branch: `type/issue-slug`. Conventional Commits: English type, Korean summary.
- **Never add `Co-Authored-By` or any AI-attribution line** to commits or PR bodies.

## Infra safety
- The agent does **not** create/delete Docker resources (`compose up/down`, `run`, `rm`, `rmi`, `prune`). Give commands + explanation; the user runs them. Read-only commands (`ps`, `inspect`, `context show`) are fine.
- This Mac's Docker also holds **resources that do not belong to this project**. Never suggest or run unnamed `prune` commands; only remove resources named `revenant-*` / `practice-*`. Always check `docker context show` first.
- Never write anything about the user's employer (names, resources, work) into this repo or its Notion pages.
- Anything touching remote servers or cloud: confirm with the user first.
