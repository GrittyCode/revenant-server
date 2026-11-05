# Revenant Server

수집형 RPG **Revenant**의 게임 서버 포트폴리오입니다.
게임성은 최소화하고, **라이브 서비스 게임 서버가 마주치는 상황**(동시성 · 멱등성 · 정합성 · 실시간 동기화 · 운영)을 해결하는 것에 집중합니다.

> 🚧 진행 중 (2026.11 ~ 2026.12)

## 구성

| 경로 | 설명 |
|---|---|
| `src/Revenant.Api` | ASP.NET Core 게임 API 서버 (계정, 재화, 가챠, 우편, 상점, 랭킹) |
| `src/Revenant.Realtime` | C# 비동기 TCP 실시간 서버 (협동 레이드) |
| `shared/com.revenant.shared` | Unity와 서버가 공유하는 패킷/DTO (로컬 UPM 패키지) |
| `client/` | Unity 6.3 LTS 클라이언트 |
| `infra/` | DB 마이그레이션(Flyway), 배포 설정 |
| `docs/` | ERD, ADR(설계 결정 기록), 학습 노트 |

## 기술 스택

- **Server**: C# 14, .NET 10, ASP.NET Core, Dapper, MySqlConnector, StackExchange.Redis, MemoryPack
- **Storage**: MySQL 8.4, Redis 7.4
- **Client**: Unity 6.3 LTS, UniTask, Addressables
- **Infra**: Docker, GitHub Actions, AWS EC2 (Graviton), Flyway
- **Test**: xUnit v3, Testcontainers, k6

## 로컬 실행

> `docker-compose.yml`은 Docker 실습([docs/learning/00-docker-hands-on.md](docs/learning/00-docker-hands-on.md))에서 직접 작성합니다.

```bash
docker compose up -d --wait          # MySQL, Redis
docker compose run --rm flyway       # 스키마 마이그레이션
dotnet run --project src/Revenant.Api
dotnet test
```
