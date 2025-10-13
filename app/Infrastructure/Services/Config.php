<?php

declare(strict_types=1);

namespace Finhub\Infrastructure\Services;

class Config
{
    private static ?self $instance = null;
    private array $settings = [];

    private function __construct()
    {
        // Private constructor to enforce singleton pattern
    }

    public static function getInstance(): self
    {
        if (self::$instance === null) {
            self::$instance = new self();
        }
        return self::$instance;
    }

    public function load(string $path): void
    {
        if (!file_exists($path)) {
            throw new \Exception("Configuration file not found: {$path}");
        }

        $lines = file($path, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);
        foreach ($lines as $line) {
            if (str_starts_with(trim($line), '#')) {
                continue;
            }

            list($name, $value) = explode('=', $line, 2);
            $name = trim($name);
            $value = trim($value);

            $this->settings[$name] = $value;
        }
    }

    public function get(string $key, $default = null): ?string
    {
        return $this->settings[$key] ?? $default;
    }
}