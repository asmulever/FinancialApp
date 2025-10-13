<?php

declare(strict_types=1);

namespace Finhub\Application\Services;

use Finhub\Application\Shared\Result;
use Finhub\Domain\Repositories\UserRepository;
use Finhub\Domain\User;
use Finhub\Infrastructure\Repositories\MysqlUserRepository;
use Finhub\Infrastructure\Services\Config;
use Finhub\Infrastructure\ThirdParty\Firebase\JWT\JWT;
use Finhub\Infrastructure\ThirdParty\Firebase\JWT\Key;

class AuthService
{
    private UserRepository $userRepository;

    public function __construct()
    {
        $this->userRepository = new MysqlUserRepository();
    }

    public function register(string $username, string $email, string $password): Result
    {
        if ($this->userRepository->findByUsername($username)) {
            return Result::failure('Username already exists.');
        }

        if ($this->userRepository->findByEmail($email)) {
            return Result::failure('Email already exists.');
        }

        $pass_hash = password_hash($password, PASSWORD_ARGON2ID);
        $user = new User(null, $username, $email, $pass_hash, null, 'user', 'active');

        $this->userRepository->save($user);

        return Result::success(['message' => 'User registered successfully.']);
    }

    public function login(string $email, string $password): Result
    {
        $user = $this->userRepository->findByEmail($email);

        if (!$user || !password_verify($password, $user->pass_hash)) {
            return Result::failure('Invalid credentials.');
        }

        $config = Config::getInstance();
        $secretKey = $config->get('JWT_SECRET');
        $issuedAt = time();
        $expire = $issuedAt + 3600; // 1 hour

        $payload = [
            'iat' => $issuedAt,
            'exp' => $expire,
            'data' => [
                'id' => $user->id,
                'email' => $user->email,
                'role' => $user->role,
            ]
        ];

        $token = JWT::encode($payload, $secretKey, 'HS256');

        return Result::success(['token' => $token]);
    }
}