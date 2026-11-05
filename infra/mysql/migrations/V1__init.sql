-- 초기 마이그레이션 자리. 실제 스키마는 ERD 확정 후(11/6) 작성한다.
CREATE TABLE IF NOT EXISTS schema_bootstrap (
    id         TINYINT     NOT NULL PRIMARY KEY,
    created_at DATETIME(3) NOT NULL DEFAULT CURRENT_TIMESTAMP(3)
) ENGINE = InnoDB;
