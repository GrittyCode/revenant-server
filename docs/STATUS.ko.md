# 진행 현황

> 이 문서는 `docs/agent/STATUS.md`(영문, 원본)를 한국어로 옮긴 것입니다. Claude가 원본을 갱신할 때 함께 갱신합니다.
> 마지막 갱신: 2026-11-05

## 현재 단계
**W1 준비 (11/3~11/9).** 개발 환경과 리포지토리를 세팅한 뒤 설계 문서를 작성하는 단계입니다. 설계가 끝나면 구현을 시작합니다 (11/10 목표, 꼭 지킬 필요는 없음).

## 완료
- 개발 도구: .NET 10, IDE는 Rider 2025.3.4 (2026.x 업데이트는 선택. 편집·디버깅은 Rider, 실행·빌드·테스트는 터미널 CLI), Docker Desktop, gh, k6, mysql-client, git-lfs, Unity Hub, Redis Insight, dotnet-counters/trace, csharp-ls
- .NET 솔루션 뼈대 (Api / Realtime / Shared / Tests). 빌드, 테스트, 포맷 검사 통과
- CI 워크플로: 11/5 첫 실행 통과 (48초)
- Claude Code 플러그인 7개 (11/5 dotnet-nuget, notion 제거. context7은 인증 필요) (활성화 설정과 설치 기록 모두 Project 범위. 11/3에 User 범위 설치 기록 제거)
- Notion (11/4): "Revenant 서버 포트폴리오" 페이지에 인라인 DB 2개. 작업 로그(제목, 날짜)와 학습 노트(11/5 개편: 순서, 제목, 주제, 복습, 날짜. 왜 썼나와 확인 질문은 페이지 맨 위 콜아웃에 둠. 뷰는 순서대로, 복습할 것). 페이지 명령어·옵션 모음 추가. 레포가 원본
- ADR 0001 기술 스택, ADR 0002 아트 방향
- Docker 리소스는 실습을 위해 비워둠. `docker-compose.yml`은 실습 9단계에서 직접 작성
- 리포지토리 (11/5): 공개 레포 `GrittyCode/revenant-server` 생성, 첫 커밋 `caaa4f6` push. `gh auth setup-git`으로 git 인증 연결

## 결정 대기
없음

11/3 결정 (2): 폴더는 기능별(`Features/Gacha/`), `ConfigureAwait(false)`는 쓰지 않음(`Worker.cs`에서 제거), LICENSE 없음(모든 권리 보유), 첫 커밋·레포 생성·PR 흐름은 가이드를 보며 직접 실습.

11/3 결정: 언어는 C#만 사용 (C++ 혼합안은 IOCP가 Windows 전용이고 Windows PC를 주말에만 쓸 수 있어 제외). C++ MFC 2년 경력으로 C++ 3년 이상 공고에도 지원. 범위는 원래대로 유지합니다. 날짜는 참고용이고, 일정보다 진도가 꾸준히 나가는 것을 우선해서 Must 순서대로 진행합니다 ([SCOPE.ko.md](SCOPE.ko.md)).

## 다음 할 일

### 순서대로
1. [x] Claude Code 재시작 (iTerm2 새 탭에서)
2. [x] 결정 대기 항목 확정
3. [x] 리포지토리 설정 파일 마무리 (.gitignore 버그 2건 수정, .gitattributes, nuget.config, lock 파일). 스테이징은 4단계에서 직접. Unity 세팅할 때 `unityyamlmerge` 드라이버를 로컬 git config에 등록
4. [x] 첫 커밋 → GitHub 리포지토리 생성 → push → CI 통과 (11/5 직접 실습. 가이드는 Notion 학습 노트에 보관)
4a. [ ] **여기서 시작.** `main`에 커밋 안 된 수정 3개(CLAUDE.md, STATUS 두 개)가 있음. **브랜치 → PR → CI → Squash Merge**로 반영: `git switch -c docs/status-first-push` → 커밋 → `git push -u` → `gh pr create` → `gh pr checks --watch` → squash merge → main 동기화. 직접 실습(가이드는 채팅으로 1~4, 5~7 묶음). 끝나면 첫 Notion 합본(Git, 노트 3개)을 만들지 물어봄
4b. [ ] 도구 후보 (11/5 조사. 해당 작업을 시작할 때 Claude가 추가할지 물어봄(CLAUDE.md "Tooling timing"). graphify는 W4~5): Redis MCP(`redis/mcp-redis`, Redis 주차), 읽기 전용 MySQL MCP(SELECT 전용 DB 계정, ERD·쿼리 주차), `grafana/mcp-grafana`(Viewer 토큰, Grafana 단계), `dotnet-diag`(7주차), Unity CLI MCP(공식. 지원 종료된 에디터 내장 MCP의 대체. Unity 세팅 때 CoplayDev unity-mcp와 비교)
5. [ ] main 보호 규칙, 라벨, 마일스톤, 시크릿 스캔, Dependabot

### 4번 이후 병렬로
| 트랙 | 작업 | 담당 |
|---|---|---|
| A | 컨벤션 문서 + git 전략 문서 | Claude 세션 |
| B | 프로젝트 전용 스킬 5종 (참고: grill-me는 MIT라 각색 가능, ADR 스킬은 라이선스가 없어 아이디어만 참고) | Claude 세션 |
| C | hookify 차단 규칙 | 같이 |
| D | 문서 뼈대 (아키텍처, 런북, 프로토콜) | Claude 세션 |
| U | Docker 실습, Unity 6.3 LTS 설치 | **직접** |

### 설계 (11/4~11/9)
- [ ] 1페이지 게임 기획서 + 기능 목록 + 성능 목표
- [ ] 아키텍처 다이어그램
- [ ] ERD + 설계 근거
- [ ] API·에러코드·패킷 명세
- [ ] GitHub 이슈 등록

## 실습 현황
단계: `가이드` → `실습 중` → `익숙함` → `자동화`. 도움 없이 2번 성공하고 역설명을 통과하면 Claude가 자동화할지 물어봅니다.

| 영역 | 항목 | 단계 | 가이드 |
|---|---|---|---|
| Docker | 이미지·컨테이너·볼륨·네트워크 CLI | 가이드 | [00](learning/00-docker-hands-on.md) |
| Docker | docker-compose.yml 작성 | 가이드 | [00 §9](learning/00-docker-hands-on.md) |
| DB | Flyway 마이그레이션 | 가이드 | [00 §7](learning/00-docker-hands-on.md) |
| Claude Code | 플러그인 설치·범위 관리 (11/3: User→Project 이전 완료, 가이드 보며 실행) | 실습 중 | - |
| Git | 첫 커밋 → 레포 생성 → push → CI (11/5 도움 받아 완료. 역설명 일부 보충: -u, add ., CI에서만 restore 실패) | 실습 중 | Notion |
| Git | 브랜치 → push → PR → CI → Squash Merge | - | - |
| Linux | 터미널 기본, 프로세스, 포트·소켓, ulimit, ssh·systemd·journalctl (Docker, 서버 실행, k6, EC2 작업에 끼워서 학습) | 실습 중 | Notion 명령어·옵션 모음 |
| DB | 쿼리, EXPLAIN | - | - |
| Redis | redis-cli 자료구조 | - | - |
| .NET | dotnet CLI | - | - |
| .NET | Rider 디버깅 | - | - |
| CI/CD | GitHub Actions 수정 | - | - |
| 배포 | EC2, SSM, 서버 compose | - | - |
| 성능 | k6, dotnet-counters | - | - |
| Unity | 프로젝트 생성, 로컬 UPM 패키지 | - | - |

## 진행 중인 세션
없음

## 아이디어 메모
- 레이드 연습장: 허수아비 + 서버 판정 기반 실시간 DPS 측정 (Should)
- 실시간 서버 TLS 켜고 끄기에 따른 성능 비교. 설정값으로 전환 (Should, 7주차)
- 실시간 서버 규칙: Room JobQueue 안에서는 DB나 HTTP를 직접 호출하지 않기
