<?php

declare(strict_types=1);

namespace Finhub\Presentation\Controllers;

use Finhub\Application\Services\AuthService;

class AuthController
{
    private AuthService $authService;

    public function __construct()
    {
        $this->authService = new AuthService();
    }

    public function register(): void
    {
        $data = json_decode(file_get_contents('php://input'), true);
        $username = $data['username'] ?? '';
        $email = $data['email'] ?? '';
        $password = $data['password'] ?? '';

        if (empty($username) || empty($email) || empty($password)) {
            http_response_code(400);
            echo json_encode(['error' => 'Missing required fields.']);
            return;
        }

        $result = $this->authService->register($username, $email, $password);

        if ($result->isSuccess) {
            http_response_code(201);
            echo json_encode($result->getValue());
        } else {
            http_response_code(400);
            echo json_encode(['error' => $result->getError()]);
        }
    }

    public function login(): void
    {
        $data = json_decode(file_get_contents('php://input'), true);
        $email = $data['email'] ?? '';
        $password = $data['password'] ?? '';

        if (empty($email) || empty($password)) {
            http_response_code(400);
            echo json_encode(['error' => 'Missing required fields.']);
            return;
        }

        $result = $this->authService->login($email, $password);

        if ($result->isSuccess) {
            http_response_code(200);
            echo json_encode($result->getValue());
        } else {
            http_response_code(401);
            echo json_encode(['error' => $result->getError()]);
        }
    }
}