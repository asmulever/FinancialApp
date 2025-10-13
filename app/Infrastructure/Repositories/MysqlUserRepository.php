<?php

declare(strict_types=1);

namespace Finhub\Infrastructure\Repositories;

use Finhub\Domain\Repositories\UserRepository;
use Finhub\Domain\User;
use Finhub\Infrastructure\Services\Database;
use PDO;

class MysqlUserRepository implements UserRepository
{
    private PDO $db;

    public function __construct()
    {
        $this->db = Database::getInstance()->getConnection();
    }

    public function findByEmail(string $email): ?User
    {
        $stmt = $this->db->prepare('SELECT * FROM users WHERE email = :email');
        $stmt->execute(['email' => $email]);
        $user = $stmt->fetch();

        return $user ? $this->mapToUser($user) : null;
    }

    public function findByUsername(string $username): ?User
    {
        $stmt = $this->db->prepare('SELECT * FROM users WHERE username = :username');
        $stmt->execute(['username' => $username]);
        $user = $stmt->fetch();

        return $user ? $this->mapToUser($user) : null;
    }

    public function save(User $user): User
    {
        $stmt = $this->db->prepare(
            'INSERT INTO users (username, email, pass_hash, full_name, role, status)
             VALUES (:username, :email, :pass_hash, :full_name, :role, :status)'
        );

        $stmt->execute([
            'username' => $user->username,
            'email' => $user->email,
            'pass_hash' => $user->pass_hash,
            'full_name' => $user->full_name,
            'role' => $user->role ?? 'user',
            'status' => $user->status ?? 'active',
        ]);

        $id = (int) $this->db->lastInsertId();
        return new User($id, $user->username, $user->email, $user->pass_hash, $user->full_name, $user->role, $user->status);
    }

    private function mapToUser(array $row): User
    {
        return new User(
            id: (int) $row['id'],
            username: $row['username'],
            email: $row['email'],
            pass_hash: $row['pass_hash'],
            full_name: $row['full_name'],
            role: $row['role'],
            status: $row['status']
        );
    }
}