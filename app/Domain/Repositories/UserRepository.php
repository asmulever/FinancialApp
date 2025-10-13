<?php

declare(strict_types=1);

namespace Finhub\Domain\Repositories;

use Finhub\Domain\User;

interface UserRepository
{
    public function findByEmail(string $email): ?User;
    public function findByUsername(string $username): ?User;
    public function save(User $user): User;
}