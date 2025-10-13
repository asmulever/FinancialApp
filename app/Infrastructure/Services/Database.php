<?php

declare(strict_types=1);

namespace Finhub\Infrastructure\Services;

use PDO;
use PDOException;

class Database
{
    private static ?self $instance = null;
    private ?PDO $connection = null;

    private function __construct()
    {
        // Private constructor for singleton pattern
    }

    public static function getInstance(): self
    {
        if (self::$instance === null) {
            self::$instance = new self();
        }
        return self::$instance;
    }

    public function getConnection(): PDO
    {
        if ($this->connection === null) {
            $config = Config::getInstance();

            $host = $config->get('DB_HOST');
            $dbName = $config->get('DB_NAME');
            $user = $config->get('DB_USER');
            $pass = $config->get('DB_PASS');
            $charset = 'utf8mb4';

            $dsn = "mysql:host={$host};dbname={$dbName};charset={$charset}";

            $options = [
                PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION,
                PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
                PDO::ATTR_EMULATE_PREPARES   => false,
            ];

            try {
                $this->connection = new PDO($dsn, $user, $pass, $options);
            } catch (PDOException $e) {
                // In a real app, you'd log this error, not expose it
                throw new PDOException($e->getMessage(), (int)$e->getCode());
            }
        }

        return $this->connection;
    }
}