# 00. Docker 직접 다뤄보기 (CLI)

목표는 **아무것도 없는 상태에서** MySQL과 Redis 개발 환경을 직접 만들고, 마지막에는 `docker-compose.yml`까지 직접 작성하는 것입니다.
모든 명령은 리포지토리 루트(`~/Documents/UnityServer`)에서 실행합니다.

> ⚠️ **이 Docker에는 이 프로젝트와 상관없는 이미지, 볼륨, 네트워크도 있습니다.**
> - 이름을 지정하지 않는 `prune` 명령(`docker system prune`, `docker volume prune` 등)은 **사용 금지**입니다. 지금 컨테이너가 쓰고 있지 않은 다른 볼륨까지 삭제 대상에 포함됩니다.
> - 삭제는 항상 `revenant-`나 `practice-`로 시작하는 **이름을 직접 지정해서** 합니다.
> - 시작하기 전에 `docker context show`의 결과가 `desktop-linux`(이 Mac의 로컬 Docker)인지 확인하세요.

---

## 0. 상태 확인 명령 모음 (실습 내내 사용)

| 확인할 것 | 명령 |
|---|---|
| 지금 어느 Docker에 연결됐나 | `docker context show` |
| 실행 중인 컨테이너 | `docker ps` |
| 중지된 컨테이너까지 전부 | `docker ps -a` |
| 원하는 컬럼만 보기 | `docker ps -a --format 'table {{.Names}}\t{{.Status}}\t{{.Ports}}'` |
| 이미지 목록 | `docker images` |
| 볼륨 / 네트워크 목록 | `docker volume ls` / `docker network ls` |
| 이름으로 걸러 보기 | `docker ps -a --filter name=revenant` |
| 상세 정보 (JSON) | `docker inspect <이름>` |
| 상세 정보에서 한 값만 | `docker inspect -f '{{.State.Health.Status}}' revenant-mysql` |
| 로그 (마지막 50줄 + 계속 보기) | `docker logs --tail 50 -f <이름>` |
| CPU·메모리 실시간 | `docker stats` (한 번만: `docker stats --no-stream`) |
| 디스크 사용량 | `docker system df` (자세히: `-v`) |
| 실시간 이벤트 (다른 터미널에 켜 두기) | `docker events --filter name=revenant` |
| 컨테이너 안에서 명령 실행 | `docker exec -it <이름> <명령>` |

**팁:** 터미널 하나에 `docker events`를 띄워 두고 다른 터미널에서 실습하면, 명령 하나가 어떤 이벤트(create, start, die 등)를 일으키는지 눈으로 볼 수 있습니다.

---

## 1. 이미지 받기

```bash
docker pull mysql:8.4
docker pull redis:7.4
docker images
docker image inspect mysql:8.4 -f '{{.Architecture}} / {{.Os}}'
docker history redis:7.4
```

- ❓ 아키텍처가 `arm64`로 나오는 이유는? (힌트: 이 Mac의 CPU, 그리고 배포할 EC2 Graviton)
- ❓ 이미지와 컨테이너는 어떤 관계일까? (Java의 클래스와 객체에 비유해 보세요)

## 2. 네트워크와 볼륨 만들기

```bash
docker network create revenant-net
docker volume create revenant-mysql-data
docker volume create revenant-redis-data
docker network ls
docker volume ls
docker volume inspect revenant-mysql-data
```

- ❓ `Mountpoint` 경로는 Mac의 Finder에서 찾을 수 없습니다. 왜 그럴까? (힌트: Docker Desktop은 Linux VM 안에서 동작합니다)

## 3. MySQL 컨테이너 띄우기

```bash
docker run -d --name revenant-mysql \
  --network revenant-net \
  -v revenant-mysql-data:/var/lib/mysql \
  -p 127.0.0.1:3306:3306 \
  -e MYSQL_ROOT_PASSWORD=revenant-root \
  -e MYSQL_DATABASE=revenant_game \
  -e MYSQL_USER=revenant \
  -e MYSQL_PASSWORD=revenant \
  mysql:8.4 \
  --character-set-server=utf8mb4 --collation-server=utf8mb4_0900_ai_ci --default-time-zone=+00:00

docker logs -f revenant-mysql     # "ready for connections"가 나오면 Ctrl+C
docker ps
```

- [ ] `mysql -h 127.0.0.1 -urevenant -prevenant revenant_game -e "SELECT VERSION()"` 로 Mac에서 접속해 보기
- [ ] Rider의 Database 탭에서 같은 정보로 접속해 보기
- ❓ `-p 127.0.0.1:3306:3306`에서 앞의 `127.0.0.1`을 빼면 어떤 위험이 생길까?
- ❓ `-e`로 넘긴 값은 어디에 쓰일까? 비밀번호를 이렇게 넘기는 건 운영 환경에서 괜찮을까?

## 4. Redis 컨테이너 띄우기 (직접 작성)

3번을 참고해서 아래 조건으로 **명령을 직접 작성**해 보세요.

- 이름 `revenant-redis`, 네트워크 `revenant-net`, 볼륨 `revenant-redis-data`를 `/data`에 연결
- 포트 6379를 localhost에만 열기
- 실행 옵션: `redis-server --appendonly yes`

확인:
```bash
docker exec -it revenant-redis redis-cli PING
docker exec -it revenant-redis redis-cli SET hello revenant
```
- [ ] Redis Insight에서 `127.0.0.1:6379`로 접속해서 `hello` 키 보기

## 5. 컨테이너 생명주기

```bash
docker stop revenant-redis && docker ps -a --filter name=revenant
docker start revenant-redis
docker exec -it revenant-redis redis-cli GET hello

docker rm -f revenant-redis
# 4번에서 작성한 명령으로 다시 띄우기
docker exec -it revenant-redis redis-cli GET hello
```

- ❓ `stop`과 `rm`의 차이는 무엇일까?
- ❓ 컨테이너를 삭제했는데도 `hello` 값이 남아 있는 이유는? 볼륨을 연결하지 않았다면 어떻게 됐을까?

## 6. 컨테이너끼리 통신하기 (네트워크)

```bash
docker run --rm --network revenant-net redis:7.4 redis-cli -h revenant-redis PING   # 성공
docker run --rm redis:7.4 redis-cli -h revenant-redis PING                          # 실패
docker network inspect revenant-net -f '{{range .Containers}}{{.Name}} {{.IPv4Address}}{{"\n"}}{{end}}'
```

- ❓ 같은 네트워크 안에서는 컨테이너 이름으로 접속할 수 있는 이유는? (힌트: Docker 내장 DNS)
- ❓ 나중에 API 서버도 컨테이너로 띄우면, DB 접속 주소는 `localhost`일까 `revenant-mysql`일까?

## 7. 마이그레이션 실행하기 (Flyway)

```bash
docker run --rm --network revenant-net \
  -v "$(pwd)/infra/mysql/migrations:/flyway/sql:ro" \
  flyway/flyway:11 \
  -url="jdbc:mysql://revenant-mysql:3306/revenant_game?allowPublicKeyRetrieval=true&useSSL=false" \
  -user=root -password=revenant-root migrate

mysql -h 127.0.0.1 -urevenant -prevenant revenant_game -e "SHOW TABLES"
```

- ❓ `--rm`을 붙인 이유는?
- ❓ `-v "$(pwd)/...:/flyway/sql:ro"`는 named volume과 무엇이 다를까? (bind mount)

## 8. 정리하기 (이름을 지정해서)

```bash
docker rm -f revenant-mysql revenant-redis
docker volume ls                          # 볼륨은 남아 있음
docker volume rm revenant-mysql-data revenant-redis-data
docker network rm revenant-net
docker ps -a; docker volume ls; docker network ls   # revenant- 외의 리소스는 그대로인지 확인
```

- ❓ 컨테이너를 지워도 볼륨이 남는 건 장점일까 단점일까?

## 9. `docker-compose.yml` 직접 작성하기

3~7번에서 손으로 친 명령을 **파일 하나로** 옮깁니다. 리포지토리 루트에 `docker-compose.yml`을 만드세요.

요구사항:
- 프로젝트 이름 `revenant`
- `mysql`, `redis` 서비스: 3·4번과 같은 설정 + **healthcheck**
  - mysql: `mysqladmin ping`
  - redis: `redis-cli ping`
- `flyway` 서비스: `profiles: ["tools"]`로 평소에는 실행되지 않게 하고, `mysql`이 healthy가 된 뒤에만 실행 (`depends_on.condition`)
- 비밀번호는 파일에 직접 쓰지 말고 `${MYSQL_ROOT_PASSWORD:-기본값}` 형식으로. 실제 값은 `.env`에 둡니다 (`.env`는 이미 `.gitignore`에 있음)
- `.env.example`도 만들어서 커밋하기

검증:
```bash
docker compose config -q && echo OK      # 문법 확인
docker compose up -d --wait
docker compose ps
docker compose run --rm flyway
docker compose logs --tail 20 mysql
docker compose down                       # 볼륨은 유지
```

- ❓ compose가 만든 네트워크와 볼륨 이름 앞에 `revenant_`이 붙는 이유는?
- ❓ `docker compose down`과 `docker compose down -v`의 차이는? 운영 서버에서 `-v`를 쓰면 어떻게 될까?

> 막히면 Claude에게 요청하세요. 처음 작성했던 참고용 compose 파일과 비교해 드립니다.

---

## 역설명 체크 (Claude에게 설명해 보기)

1. 이미지, 컨테이너, 볼륨, 네트워크를 각각 한 문장으로 설명하기
2. `docker compose up -d`를 실행하면 내부에서 무슨 일이 일어나는가
3. 컨테이너를 지워도 데이터가 남는 조건과, 데이터가 사라지는 경우
4. 포트를 `127.0.0.1`에 바인딩한 이유
5. 이름을 지정하지 않는 `prune`이 이 Mac에서 위험한 이유

## 내 답변 / 메모

(실습하면서 직접 작성)
