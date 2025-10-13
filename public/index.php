<?php
// Finhub - Public Entry Point

declare(strict_types=1);

// Set common headers
header("Content-Type: application/json; charset=UTF-8");
header("Access-Control-Allow-Origin: *"); // For development only
header("Access-Control-Allow-Methods: GET, POST, PUT, DELETE, OPTIONS");
header("Access-Control-Allow-Headers: Content-Type, Authorization");

// Handle preflight requests for CORS
if ($_SERVER['REQUEST_METHOD'] === 'OPTIONS') {
    exit(0);
}

// Include the autoloader
require_once __DIR__ . '/../app/autoloader.php';

use Finhub\Infrastructure\Services\Config;
use Finhub\Presentation\Http\Router;

// Load configuration
try {
    $config = Config::getInstance();
    $config->load(__DIR__ . '/../.env');
} catch (\Exception $e) {
    http_response_code(500);
    echo json_encode(['error' => 'Failed to load configuration.']);
    exit;
}

// Initialize the router
$router = new Router();

// --- Define API Routes ---
use Finhub\Presentation\Controllers\AuthController;

$router->add('POST', '/auth/register', [AuthController::class, 'register']);
$router->add('POST', '/auth/login', [AuthController::class, 'login']);


// --- Dispatch the request ---
$router->dispatch();