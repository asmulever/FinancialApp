<?php

declare(strict_types=1);

namespace Finhub\Presentation\Http\Middleware;

use Finhub\Infrastructure\Services\Config;
use Finhub\Infrastructure\ThirdParty\Firebase\JWT\JWT;
use Finhub\Infrastructure\ThirdParty\Firebase\JWT\Key;
use UnexpectedValueException;

class AuthMiddleware
{
    public static function handle(callable $handler, array $roles = [])
    {
        $headers = getallheaders();
        $authHeader = $headers['Authorization'] ?? null;

        if (!$authHeader) {
            http_response_code(401);
            echo json_encode(['error' => 'Authorization header not found.']);
            return;
        }

        list($type, $token) = explode(' ', $authHeader, 2);

        if (strcasecmp($type, 'Bearer') !== 0) {
            http_response_code(401);
            echo json_encode(['error' => 'Invalid token type.']);
            return;
        }

        try {
            $config = Config::getInstance();
            $secretKey = $config->get('JWT_SECRET');
            $decoded = JWT::decode($token, new Key($secretKey, 'HS256'));

            if (!empty($roles) && !in_array($decoded->data->role, $roles)) {
                 http_response_code(403);
                 echo json_encode(['error' => 'Forbidden']);
                 return;
            }

            // You can pass the decoded payload to the handler if needed
            $handler($decoded->data);

        } catch (UnexpectedValueException $e) {
            http_response_code(401);
            echo json_encode(['error' => 'Invalid token: ' . $e->getMessage()]);
            return;
        }
    }
}