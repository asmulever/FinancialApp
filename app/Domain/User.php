<?php

declare(strict_types=1);

namespace Finhub\Domain;

class User
{
    public function __construct(
        public readonly ?int $id,
        public readonly string $username,
        public readonly string $email,
        public readonly string $pass_hash,
        public readonly ?string $full_name,
        public readonly ?string $role,
        public readonly ?string $status
    ) {
    }
}