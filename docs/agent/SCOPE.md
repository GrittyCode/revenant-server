# SCOPE (source of truth)

Korean view with full rationale: `docs/SCOPE.ko.md` (keep in sync). User chose full scope on 2026-11-03. Dates are reference only (12/31 is a soft target); optimize for steady progress in Must order.

Competency tags: ARCH architecture, NET networking, CONC concurrency, DB schema/tx, CONS consistency/idempotency, PERF performance, LIVE live-ops, SEC security, QA tests/CI, OPS infra/devops, CLI client/Unity.

## Platform
| ID | Feature | Pri | Tags |
|---|---|---|---|
| P1 | Servers: Api / Realtime (TCP) / Chat (Redis Pub/Sub) / Shared (UPM, Unity) | Must | ARCH |
| P2 | Layered + Unit of Work over Dapper | Must | ARCH DB |
| P3 | Shared packets/DTOs compiled by server and Unity | Must | ARCH CLI |
| P4 | Error-code Result pattern, common response envelope | Must | ARCH |
| P5 | Options + ValidateOnStart, user-secrets, GitHub Secrets | Must | SEC OPS |
| P6 | LoggerMessage logging, trace id, game log vs system log | Must | LIVE PERF |

## API (economy)
| ID | Feature | Pri | Tags |
|---|---|---|---|
| A1 | Guest login, JWT, Redis session, kick previous session | Must | SEC ARCH |
| A2 | Currency (paid/free split) + append-only ledger, conditional UPDATE | Must | DB CONS |
| A3 | Gacha: server prob table, pity, disclosure API, draw log, 100k-sim distribution test | Must | CONS DB |
| A4 | Idempotency key (unique + stored result) for gacha/shop | Must | CONS DB NET |
| A5 | Inventory (stack vs instance tables) + enhance with FOR UPDATE | Must | DB CONC |
| A6 | Mail: expiry, no double-claim, covering index (user_id, expired_at) + EXPLAIN | Must | DB PERF |
| A7 | Shop with idempotency, limited stock (DB lock vs Redis) | Must | CONS CONC |
| A8 | Stage result verification (client-run combat) | Must | SEC ARCH |
| A9 | Reward granting via Strategy (IRewardGranter) | Must | ARCH |
| A10 | Stamina lazy calc, daily reset, TimeProvider | Should | ARCH QA |
| A11 | World boss: Redis Lua HP, single last-hit, contribution ZSET | Should | CONC PERF |
| A12 | Season ranking reward batch | Could | LIVE DB |
| A13 | Guild: capacity race, optimistic lock | Could | CONC DB |
| A14 | Attendance | Could | - |
| A15 | Payment mock: receipt validator interface, double-grant guard, payment ledger | Could | CONS ARCH |

## Realtime (co-op raid)
| ID | Feature | Pri | Tags |
|---|---|---|---|
| R1 | TCP server: Socket + Pipelines, session, send queue, framing, MemoryPack; IOCP-like design | Must | NET |
| R2 | Heartbeat/timeout | Must | NET |
| R3 | Entry token issued by Api, verified by Realtime | Must | SEC ARCH |
| R4 | Matchmaking via Redis queue | Must | ARCH CONC |
| R5 | Room JobQueue (Channel<T>, Actor+Command); no DB/HTTP inside room | Must | CONC ARCH |
| R6 | Room/boss state machines; per-state packet gating | Must | ARCH SEC |
| R7 | Server-authoritative combat | Must | NET SEC |
| R8 | Reconnect with state resync | Must | NET LIVE |
| R9 | Result report + Outbox (DB commit → Redis) | Must | CONS ARCH |
| R10 | Graceful shutdown (drain rooms) | Must | LIVE OPS |
| R11 | Practice dummy + live DPS meter | Should | NET CLI |
| R12 | Chat server (Redis Pub/Sub) | Should | ARCH NET |
| R13 | TLS on/off via config (SslStream) perf comparison | Should | NET SEC PERF |

## Live-ops
| ID | Feature | Pri | Tags |
|---|---|---|---|
| L1 | Admin tool: user lookup, grant/revoke with reason log, mail, ban | Must | LIVE |
| L2 | Maintenance mode + client version check/force update | Must | LIVE CLI |
| L3 | Admin → client real-time push (Redis Pub/Sub) | Must | LIVE ARCH |
| L4 | Bulk mail to 100k via batch/queue | Should | PERF LIVE |
| L5 | Master data hot reload (Decorator cache swap) | Should | LIVE ARCH |
| L6 | Lossless migration demo (expand → dual-write → backfill → switch) | Should | DB LIVE |
| L7 | Runbook | Should | LIVE OPS |
| L8 | Failure scenarios: Redis down, Realtime crash, DB slow | Should | LIVE ARCH |

## Client (Unity 6.3, 2D pixel)
| ID | Feature | Pri | Tags |
|---|---|---|---|
| C1 | Core loop under 5 min: login → gacha → team → stage → reward → enhance → raid | Must | CLI |
| C2 | Debug panel: latency, request id, error code, "send 5x" button | Must | CLI CONS |
| C3 | pixelfx effects | Should | CLI |
| C4 | Addressables remote (S3 + CloudFront) | Could | OPS CLI |
| C5 | 2.5D client swap, no server change | Could (Jan+) | ARCH |

## Quality / perf / infra
| ID | Feature | Pri | Tags |
|---|---|---|---|
| Q1 | Testcontainers integration tests (concurrency, idempotency, consistency) | Must | QA CONC |
| Q2 | CI: format, build, test on arm64 | Must | QA OPS |
| Q3 | CD: GHCR → EC2 Graviton, OIDC, health-check rollback | Must | OPS SEC |
| Q4 | Load test once: k6 + C# bots; CCU, RPS, p99 | Must | PERF |
| Q5 | 2–3 bottleneck fixes with before/after numbers | Should | PERF |
| Q6 | Prometheus + Grafana | Should | OPS PERF |
| Q7 | Unity CI (GameCI) | Could | OPS |

## Milestone order (dates are reference only)
W1 11/03–09 env, repo, design, P1–P6 skeleton · W2 A1 A2 A5 migrations · W3 A3 A4 A6 A7 A9 Q1 · W4 A8 L1–L3 Q3 → v0.1.0, start applying · W5–6 R1–R10 C1 C2 → v0.2.0 · W7 Q4 Q5 + Should → v0.3.0 · W8 README, video, blog → v1.0.0
