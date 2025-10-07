-- ==========================================================
-- FinancialApp / FinHubDB - Schema Definition
-- Version: 1.0
-- Compatible with MySQL 8.0+
-- ==========================================================

CREATE DATABASE IF NOT EXISTS if0_39913066_finhubdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
USE if0_39913066_finhubdb;

-- =========================
-- USERS
-- =========================
CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(120) NOT NULL UNIQUE,
    pass_hash VARCHAR(255) NOT NULL,
    full_name VARCHAR(120),
    country VARCHAR(80),
    time_zone VARCHAR(60),
    role ENUM('superadmin','admin','user') DEFAULT 'user',
    status ENUM('active','disabled') DEFAULT 'active',
    validated_by INT DEFAULT NULL,
    force_password_change TINYINT(1) DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (validated_by) REFERENCES users(id) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================
-- TICKERS
-- =========================
CREATE TABLE tickers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    symbol VARCHAR(20) NOT NULL UNIQUE,
    market VARCHAR(50),
    name VARCHAR(120),
    sector VARCHAR(80),
    country VARCHAR(80),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================
-- PORTFOLIOS
-- =========================
CREATE TABLE portfolios (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    name VARCHAR(100) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================
-- PORTFOLIO ITEMS
-- =========================
CREATE TABLE portfolio_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    portfolio_id INT NOT NULL,
    ticker_id INT NOT NULL,
    added_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (portfolio_id) REFERENCES portfolios(id) ON DELETE CASCADE,
    FOREIGN KEY (ticker_id) REFERENCES tickers(id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================
-- PRICES
-- =========================
CREATE TABLE prices (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    ticker_id INT NOT NULL,
    ts DATETIME NOT NULL,
    price DECIMAL(18,6) NOT NULL,
    volume BIGINT DEFAULT NULL,
    fx_usdars DECIMAL(18,6) DEFAULT NULL,
    sma20 DECIMAL(18,6) DEFAULT NULL,
    sma50 DECIMAL(18,6) DEFAULT NULL,
    vol30d DECIMAL(18,6) DEFAULT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ticker_id) REFERENCES tickers(id) ON DELETE CASCADE,
    INDEX (ticker_id, ts)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================
-- NEWS
-- =========================
CREATE TABLE news (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    ticker_id INT DEFAULT NULL,
    ts DATETIME NOT NULL,
    source VARCHAR(120),
    title VARCHAR(255),
    url VARCHAR(255),
    sentiment ENUM('positive','neutral','negative') DEFAULT 'neutral',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ticker_id) REFERENCES tickers(id) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================
-- PREDICTIONS
-- =========================
CREATE TABLE predictions (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    ticker_id INT NOT NULL,
    ts DATETIME NOT NULL,
    action ENUM('buy','hold','sell') DEFAULT 'hold',
    target_price DECIMAL(18,6) DEFAULT NULL,
    conf_low DECIMAL(18,6) DEFAULT NULL,
    conf_high DECIMAL(18,6) DEFAULT NULL,
    rationale TEXT,
    model_meta JSON,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ticker_id) REFERENCES tickers(id) ON DELETE CASCADE,
    INDEX (ticker_id, ts)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================
-- JOBS LOG
-- =========================
CREATE TABLE jobs_log (
    id BIGINT AUTO_INCREMENT PRIMARY KEY,
    job VARCHAR(120) NOT NULL,
    started_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    finished_at DATETIME DEFAULT NULL,
    status ENUM('success','error','running') DEFAULT 'running',
    msg TEXT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================
-- SETTINGS (optional global parameters)
-- =========================
CREATE TABLE settings (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL,
    value TEXT,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- =========================
-- DEFAULT ADMIN USER
-- =========================
INSERT INTO users (username, email, pass_hash, full_name, country, time_zone, role, status, force_password_change)
VALUES 
('admin', 'admin@financialapp.local',
 '$argon2id$v=19$m=65536,t=4,p=1$U29tZVNhbHRCYXNl$uEKfZyF5o0MJw8iZ1fKAVS3cY4Z+7W8fG6FrD4xU7c8',
 'System Administrator', 'Argentina', 'America/Argentina/Buenos_Aires', 'superadmin', 'active', 1);

-- ==========================================================
-- END OF SCHEMA
-- ==========================================================
