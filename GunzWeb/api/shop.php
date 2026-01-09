<?php
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: GET, POST, PUT, DELETE');
header('Access-Control-Allow-Headers: Content-Type');

$dataFile = __DIR__ . '/../data/shop.json';

function getShop() {
    global $dataFile;
    if (file_exists($dataFile)) {
        return json_decode(file_get_contents($dataFile), true);
    }
    return [];
}

function saveShop($shop) {
    global $dataFile;
    file_put_contents($dataFile, json_encode($shop, JSON_PRETTY_PRINT));
}

$method = $_SERVER['REQUEST_METHOD'];

switch ($method) {
    case 'GET':
        $items = getShop();
        // Filter featured only if requested
        if (isset($_GET['featured']) && $_GET['featured'] === 'true') {
            $items = array_filter($items, function($item) {
                return $item['featured'] === true;
            });
            $items = array_values($items);
        }
        // Filter by category if requested
        if (isset($_GET['category'])) {
            $category = $_GET['category'];
            $items = array_filter($items, function($item) use ($category) {
                return strtolower($item['category']) === strtolower($category);
            });
            $items = array_values($items);
        }
        echo json_encode($items);
        break;

    case 'POST':
        $input = json_decode(file_get_contents('php://input'), true);
        $shop = getShop();
        $input['id'] = count($shop) > 0 ? max(array_column($shop, 'id')) + 1 : 1;
        $shop[] = $input;
        saveShop($shop);
        echo json_encode(['success' => true, 'id' => $input['id']]);
        break;

    case 'PUT':
        $input = json_decode(file_get_contents('php://input'), true);
        $shop = getShop();
        foreach ($shop as $key => $item) {
            if ($item['id'] == $input['id']) {
                $shop[$key] = array_merge($item, $input);
                break;
            }
        }
        saveShop($shop);
        echo json_encode(['success' => true]);
        break;

    case 'DELETE':
        $id = isset($_GET['id']) ? intval($_GET['id']) : 0;
        $shop = getShop();
        $shop = array_filter($shop, function($item) use ($id) {
            return $item['id'] != $id;
        });
        saveShop(array_values($shop));
        echo json_encode(['success' => true]);
        break;
}
?>
