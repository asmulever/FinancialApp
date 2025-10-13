<?php

declare(strict_types=1);

namespace Finhub\Presentation\Http;

class Router
{
    private array $routes = [];

    public function add(string $method, string $path, array $handler): void
    {
        $this->routes[] = [
            'method' => strtoupper($method),
            'path' => $path,
            'handler' => $handler
        ];
    }

    public function dispatch(): void
    {
        $method = $_SERVER['REQUEST_METHOD'];
        $path = parse_url($_SERVER['REQUEST_URI'], PHP_URL_PATH);

        foreach ($this->routes as $route) {
            if ($route['method'] === $method && $route['path'] === $path) {
                $controllerClass = $route['handler'][0];
                $controllerMethod = $route['handler'][1];

                $controller = new $controllerClass();
                $controller->$controllerMethod();
                return;
            }
        }

        // Handle 404 Not Found
        http_response_code(404);
        echo json_encode(['error' => 'Not Found']);
    }
}