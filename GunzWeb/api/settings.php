<?php
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: GET, POST');
header('Access-Control-Allow-Headers: Content-Type');

$dataFile = __DIR__ . '/../data/settings.json';

function getSettings() {
    global $dataFile;
    if (file_exists($dataFile)) {
        return json_decode(file_get_contents($dataFile), true);
    }
    return [];
}

function saveSettings($settings) {
    global $dataFile;
    file_put_contents($dataFile, json_encode($settings, JSON_PRETTY_PRINT));
}

$method = $_SERVER['REQUEST_METHOD'];

switch ($method) {
    case 'GET':
        echo json_encode(getSettings());
        break;

    case 'POST':
        $input = json_decode(file_get_contents('php://input'), true);
        $settings = getSettings();
        $settings = array_merge($settings, $input);
        saveSettings($settings);
        echo json_encode(['success' => true]);
        break;
}
?>
