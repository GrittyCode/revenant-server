# ADR 0001: 기술 스택 선정

- 상태: 승인
- 날짜: 2026-11-03

## 배경
국내 게임 서버 공고 20건을 조사한 결과, 모바일 라이브 서비스 회사는 C#/.NET 또는 Node 기반 HTTP API 서버 + MySQL + Redis가 주류였다.
C++ IOCP 기반 공고는 대부분 5년 이상 + MMORPG 라이브 경험을 요구했다.

## 결정
- 언어/런타임: C# / .NET 10 (LTS). .NET 8은 2026-11 지원 종료.
- API 서버: ASP.NET Core + Dapper (SQL을 직접 통제, 국내 게임사 관행)
- 실시간 서버: Socket + System.IO.Pipelines 직접 구현 (네트워크 기본기 증명)
- 저장소: MySQL 8.4 LTS + Redis 7.4
  - PostgreSQL: 기술적 문제는 없으나 대상 공고에서 0건 → 제외
  - MongoDB: 우대사항 수준 → 제외. NoSQL 요구는 Redis 활용으로 대응
- CI/CD: GitHub Actions (arm64 러너) → GHCR → EC2 Graviton

## 결과
- Unity 클라이언트와 C# 코드 공유 가능 (shared 패키지)
- Spring 경험(DI, 미들웨어, 트랜잭션)을 ASP.NET Core로 그대로 이전 가능
