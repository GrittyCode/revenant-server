# 02. 첫 커밋 → GitHub 레포 생성 → push → CI 확인

목표는 지금 로컬에만 있는 레포를 **공개 GitHub 레포** `GrittyCode/revenant-server`로 올리고, CI가 초록색으로 통과하는 것까지 확인하는 것입니다.
모든 명령은 레포 루트(`~/Documents/UnityServer`)에서 직접 실행합니다. 예상 시간은 20~30분입니다.

> 이번만 `main`에 직접 push합니다. 아직 main 보호 규칙이 없기 때문입니다. 보호 규칙은 다음 단계에서 걸고, 그 뒤로는 모든 변경을 **브랜치 → PR → CI → Squash Merge**로 넣습니다.

---

## 1. 상태 확인 (읽기 전용)

| 확인할 것 | 명령 | 기대 결과 |
|---|---|---|
| 커밋 작성자 | `git config user.name` / `git config user.email` | 개인 이름과 **개인 메일** |
| GitHub 로그인 | `gh auth status` | `GrittyCode` 계정, scopes에 `repo`, `workflow` |
| LFS 설치 | `git lfs version` | 버전이 출력됨 |
| 현재 브랜치 | `git branch --show-current` | `main` |

공개 레포에서는 커밋 작성자 메일이 **누구에게나 보입니다**. 메일을 숨기고 싶으면 GitHub Settings → Emails에서 `ID+GrittyCode@users.noreply.github.com` 주소를 확인하고, 이 레포에서만 `git config user.email <noreply 주소>`로 바꾸세요.

## 2. 커밋 전 검사 (CLAUDE.md 규칙)

```bash
dotnet format Revenant.slnx --verify-no-changes
dotnet build Revenant.slnx
dotnet test --solution Revenant.slnx
```

셋 다 통과해야 다음 단계로 넘어갑니다. CI도 같은 순서로 검사합니다(`.github/workflows/ci.yml`).

## 3. 스테이징

```bash
git add -A
git status
git diff --cached --stat
```

`git add -A`는 새 파일, 수정, 삭제를 전부 스테이징합니다. 이전에 스테이징해 둔 오래된 버전도 지금 파일 내용으로 덮어써요.

**목록에서 확인할 것**
- [ ] `packages.lock.json` 4개와 `nuget.config`가 있다 (빠지면 CI의 locked mode restore가 실패함)
- [ ] `bin/`, `obj/`, `.env`, `.idea/`, `.DS_Store`가 **없다**
- [ ] `.claude/settings.local.json`이 **없다** (개인 설정)

의심스러운 파일이 있으면 이 명령으로 원인을 확인합니다.
```bash
git check-ignore -v <경로>     # 무시되는 이유(몇 번째 줄 규칙인지)
git restore --staged <경로>    # 스테이징만 취소 (파일은 그대로)
```

**공개 전 마지막 점검**: 올라가면 안 되는 단어가 스테이징된 내용에 있는지 검색합니다.
```bash
git grep --cached -niE '<회사 이름>|<회사 메일 도메인>|password=|secret='
```
아무것도 나오지 않아야 합니다. `docs/learning/00`의 `MYSQL_PASSWORD=revenant`처럼 로컬 개발용 기본값은 괜찮아요.

## 4. 첫 커밋

```bash
git commit -m "chore: 프로젝트 초기 세팅"
git log --stat -1
```

- 메시지 형식은 Conventional Commits입니다. 타입(`chore`, `feat`, `fix`, `docs`…)은 영어로, 요약은 한국어로 씁니다.
- `chore`는 기능이나 버그 수정이 아닌 설정, 빌드 작업에 씁니다.
- AI가 함께 작성했다는 표시(`Co-Authored-By` 등)는 넣지 않습니다.

## 5. GitHub 레포 생성

```bash
gh repo create GrittyCode/revenant-server \
  --public \
  --source=. \
  --remote=origin \
  --description "C#/.NET 10 게임 서버 포트폴리오: 수집형 RPG + 실시간 협동 레이드"
git remote -v
```

- `--source=.`: 지금 폴더를 이 레포의 로컬 사본으로 연결합니다.
- `--remote=origin`: 원격 이름을 `origin`으로 등록합니다. `git remote -v`에 URL 두 줄(fetch, push)이 나오면 성공이에요.
- 이 단계에서는 아직 아무것도 올라가지 않습니다. push는 다음 단계에서 따로 합니다(`--push` 옵션을 일부러 쓰지 않음).

## 6. push

```bash
git push -u origin main
```

- `-u`(`--set-upstream`): 로컬 `main`이 `origin/main`을 추적하게 만듭니다. 이후에는 `git push`, `git pull`만 써도 돼요.
- 확인: `git status`에 `Your branch is up to date with 'origin/main'`이 나오면 됩니다.

## 7. CI 확인

```bash
gh run list --limit 3      # 방금 push로 시작된 실행
gh run watch               # 진행 상황을 실시간으로 (끝날 때까지 대기)
gh repo view --web         # 브라우저로 레포 열기 → Actions 탭
```

초록색 ✓가 뜨면 4단계 완료입니다.

### 실패했을 때

| 실패한 스텝 | 흔한 원인 | 확인 방법 |
|---|---|---|
| Restore | lock 파일이 커밋에 없음, 또는 로컬과 버전이 다름 | `git ls-files '*packages.lock.json'` |
| Format check | 포맷이 안 맞는 파일 | 로컬에서 `dotnet format Revenant.slnx` 후 다시 커밋 |
| Build | 경고가 오류로 처리됨 (TreatWarningsAsErrors) | `gh run view --log-failed` |
| Test | 테스트 실패 | `gh run view --log-failed` |

고친 뒤에는 `git commit` → `git push`로 다시 올리면 CI가 새로 돕니다. 이미 push한 커밋을 `--amend`나 force push로 고치지는 않습니다.

---

## 역설명 (끝나고 Claude에게 설명해 보기)
1. `git add -A`와 `git add .`의 차이는?
2. `gh repo create`에서 `--source`와 `--remote`는 각각 무엇을 했나?
3. `git push -u`의 `-u`가 없으면 다음 push 때 무엇이 달라지나?
4. CI의 Restore가 로컬에서는 성공하는데 GitHub에서만 실패한다면 무엇부터 의심하나?
