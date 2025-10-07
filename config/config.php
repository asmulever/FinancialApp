<?php
// Cargar configuración desde .env
$dotenv = parse_ini_file(__DIR__ . '/../.env');

// Variables principales
$openai_key = $dotenv['OPENAI_API_KEY'] ?? getenv('OPENAI_API_KEY');
$db_host = $dotenv['DB_HOST'] ?? 'localhost';
$db_user = $dotenv['DB_USER'] ?? 'root';
$db_pass = $dotenv['DB_PASS'] ?? '';
$db_name = $dotenv['DB_NAME'] ?? 'financial_db';

// Conexión (ejemplo)
$conn = new mysqli($db_host, $db_user, $db_pass, $db_name);
if ($conn->connect_error) {
    die("Error de conexión: " . $conn->connect_error);
}
?>
