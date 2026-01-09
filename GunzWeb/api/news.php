<?php
header('Content-Type: application/json');
header('Access-Control-Allow-Origin: *');
header('Access-Control-Allow-Methods: GET, POST, PUT, DELETE');
header('Access-Control-Allow-Headers: Content-Type');

$dataFile = __DIR__ . '/../data/news.json';

function getNews() {
    global $dataFile;
    if (file_exists($dataFile)) {
        return json_decode(file_get_contents($dataFile), true);
    }
    return [];
}

function saveNews($news) {
    global $dataFile;
    file_put_contents($dataFile, json_encode($news, JSON_PRETTY_PRINT));
}

$method = $_SERVER['REQUEST_METHOD'];

switch ($method) {
    case 'GET':
        echo json_encode(getNews());
        break;

    case 'POST':
        $input = json_decode(file_get_contents('php://input'), true);
        $news = getNews();
        $input['id'] = count($news) > 0 ? max(array_column($news, 'id')) + 1 : 1;
        $input['date'] = date('Y-m-d');
        $news[] = $input;
        saveNews($news);
        echo json_encode(['success' => true, 'id' => $input['id']]);
        break;

    case 'PUT':
        $input = json_decode(file_get_contents('php://input'), true);
        $news = getNews();
        foreach ($news as $key => $item) {
            if ($item['id'] == $input['id']) {
                $news[$key] = array_merge($item, $input);
                break;
            }
        }
        saveNews($news);
        echo json_encode(['success' => true]);
        break;

    case 'DELETE':
        $id = isset($_GET['id']) ? intval($_GET['id']) : 0;
        $news = getNews();
        $news = array_filter($news, function($item) use ($id) {
            return $item['id'] != $id;
        });
        saveNews(array_values($news));
        echo json_encode(['success' => true]);
        break;
}
?>
