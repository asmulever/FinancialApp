<?php

declare(strict_types=1);

namespace Finhub\Application\Shared;

final class Result
{
    public readonly bool $isSuccess;
    public readonly bool $isFailure;
    private readonly mixed $value;
    private readonly ?string $error;

    private function __construct(bool $isSuccess, mixed $value, ?string $error)
    {
        $this->isSuccess = $isSuccess;
        $this->isFailure = !$isSuccess;
        $this->value = $value;
        $this->error = $error;
    }

    public static function success(mixed $value = null): self
    {
        return new self(true, $value, null);
    }

    public static function failure(string $error): self
    {
        return new self(false, null, $error);
    }

    public function getValue(): mixed
    {
        if ($this->isFailure) {
            throw new \LogicException('Cannot get value from a failed result.');
        }
        return $this->value;
    }

    public function getError(): ?string
    {
        if ($this->isSuccess) {
            throw new \LogicException('Cannot get error from a successful result.');
        }
        return $this->error;
    }
}