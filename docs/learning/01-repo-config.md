# 01. 리포지토리 설정과 초기 결정 (11/3~11/4)

첫 커밋 전에 정한 규칙과 설정 파일입니다. 각 항목마다 **무엇을 했나 → 왜 → 면접 질문** 순서로 정리합니다.

---

## 1. 초기 결정 4건

| 항목 | 결정 | 이유 |
|---|---|---|
| 폴더 구조 | 기능별 (`Features/Gacha/`), 인프라는 `Infrastructure/` | 기능 하나를 한 폴더에서 끝까지 따라 읽을 수 있음. Spring의 package-by-feature와 같은 방식 |
| ConfigureAwait | 서버 코드에서 쓰지 않음 | 아래 §2 참고 |
| LICENSE | 두지 않음 (모든 권리 보유) | 코드는 공개하되 재사용 권리는 주지 않음. 포트폴리오에서 흔한 선택 |
| git/PR 흐름 | 직접 실습 | 첫 커밋 → 레포 생성 → push → PR을 가이드를 보며 직접 실행 |

## 2. ConfigureAwait(false)를 쓰지 않는 이유

- Unity에서는 `await` 뒤에 **메인 스레드로 돌아와야** 해서 `SynchronizationContext`가 중요했습니다.
- ASP.NET Core와 Generic Host(`BackgroundService`)에는 이 컨텍스트가 **아예 없습니다**. `await` 뒤의 코드는 원래부터 스레드 풀의 아무 스레드에서나 이어서 실행돼요.
- 그래서 앱 코드에서 `ConfigureAwait(false)`를 붙여도 동작은 같고 코드만 지저분해집니다. **라이브러리**를 만들 때만 의미가 있어요. 라이브러리는 WPF나 Unity처럼 컨텍스트가 있는 앱에서 불릴 수도 있기 때문입니다.
- Kotlin으로 치면 이미 `Dispatchers.Default`에서 돌고 있는데 `withContext(Dispatchers.Default)`를 또 감싸는 것과 비슷합니다.
- 규칙을 강제하는 장치: 분석기 규칙 CA2007은 "ConfigureAwait를 붙여라"라는 규칙이라 꺼 둡니다. 실제로 꺼져 있었기 때문에 지워도 빌드가 깨지지 않았어요.

**면접 질문**
1. `ConfigureAwait(false)`는 무엇을 하고, ASP.NET Core 앱에서는 왜 필요 없나요?
2. `SynchronizationContext`가 있는 환경에서 `.Result`로 비동기 작업을 기다리면 왜 데드락이 생기나요?
3. Unity에서는 `await` 뒤의 코드가 어느 스레드에서 실행되나요?

## 3. .gitignore: 템플릿이 Unity 파일을 무시하던 버그 2건

`dotnet new gitignore` 템플릿을 그대로 쓰면 Unity 파일과 충돌합니다. `git check-ignore -v`로 실제로 무시되는 것을 확인했어요.

| 템플릿 규칙 | 문제 | 수정 |
|---|---|---|
| `*.meta` | Unity `.meta` 파일이 커밋되지 않음 | `!*.meta` |
| `**/[Pp]ackages/*` (옛 NuGet 폴더용) | `client/Packages/manifest.json`이 커밋되지 않음 | `!client/Packages/**` |

- **.meta가 중요한 이유**: Unity에서 프리팹이 스크립트나 텍스처를 참조할 때는 경로가 아니라 `.meta`에 적힌 **GUID**를 씁니다. `.meta`가 빠지면 clone할 때 GUID가 새로 만들어져서 모든 참조가 Missing이 돼요. `shared/com.revenant.shared` UPM 패키지에도 필요합니다.
- **git은 마지막에 일치한 규칙을 따릅니다**. 그래서 `!` 예외 규칙은 파일 맨 끝에 둬야 해요.
- 디버깅 명령: `git check-ignore -v <경로>`는 몇 번째 줄의 규칙에 걸렸는지 알려 줍니다.
- 그 밖에: `.env.*`는 무시하고 `!.env.example`은 허용(Node 관례와 같음), `*.dmp`/`*.nettrace`/`*.gcdump` 덤프 파일 무시

**면접 질문**
1. Unity 프로젝트에서 `.meta` 파일을 커밋하지 않으면 어떤 일이 생기나요?
2. 이미 커밋된 파일은 `.gitignore`에 추가해도 계속 추적되는 이유는?

## 4. .gitattributes

- `* text=auto eol=lf`: 줄바꿈 문자를 LF로 통일합니다. 주말에 Windows PC로 작업하더라도 diff 전체가 CRLF로 바뀌는 사고를 막아요. `.cmd`/`.bat`/`.ps1`만 CRLF입니다.
- `merge=unityyamlmerge`: 씬이나 프리팹 충돌을 Unity의 Smart Merge로 풀겠다는 표시입니다. **로컬 git config에 드라이버를 등록해야** 동작해요. Unity를 세팅할 때 등록합니다.
- LFS: png, aseprite, wav 같은 바이너리 파일. git은 바이너리 파일을 수정할 때마다 통째로 새로 저장해서 레포가 계속 커집니다. LFS는 레포에 포인터만 남기고 실제 파일은 따로 보관해요.

## 5. nuget.config와 의존성 혼동 공격

```xml
<clear />   <!-- 머신 전역 소스(다른 사설 피드 등)를 상속하지 않는다 -->
<packageSourceMapping> * → nuget.org
```

- `<clear/>`: 머신 전역 NuGet 설정에 다른 사설 피드가 있어도 이 프로젝트는 영향을 받지 않습니다. Gradle의 `repositories { mavenCentral() }`처럼 이 레포는 이 소스만 쓴다고 명시하는 것과 같아요.
- `packageSourceMapping`: 어떤 패키지를 어느 소스에서 받을지 고정합니다. **의존성 혼동(dependency confusion)** 공격, 즉 공개 피드에 내부(private) 패키지와 같은 이름의 악성 패키지를 올리는 공격을 막아요.

**면접 질문**
1. 의존성 혼동 공격이란 무엇이고 어떻게 막나요?

## 6. lock 파일 (packages.lock.json)

- `RestorePackagesWithLockFile`: npm의 `package-lock.json`, Gradle의 dependency locking과 같은 역할입니다.
- CPM(`Directory.Packages.props`)은 **직접** 참조하는 패키지 버전만 고정합니다. lock 파일은 **전이 의존성**과 해시까지 고정해요.
- CI에서만 `RestoreLockedMode`: GitHub Actions는 `GITHUB_ACTIONS=true`를 넣어 줍니다. 로컬은 `npm install`처럼 lock 파일을 갱신하고, CI는 `npm ci`처럼 lock 파일과 다르면 실패합니다.
- `ContinuousIntegrationBuild`: 빌드 결과물에 로컬 경로를 넣지 않습니다(결정적 빌드).
- 실무 팁: 패키지 버전을 올리면 lock 파일 diff도 **같은 커밋에** 포함해야 CI가 통과합니다.

**면접 질문**
1. Central Package Management와 lock 파일은 각각 무엇을 고정하나요?
2. CI에서 locked mode로 restore하는 이유는?
