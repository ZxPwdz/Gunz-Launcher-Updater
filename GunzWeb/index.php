<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gunz Online</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(180deg, #0A1218 0%, #050A10 100%);
            min-height: 100vh;
            color: #fff;
            overflow-x: hidden;
        }

        /* Announcement Bar */
        .announcement-bar {
            background: linear-gradient(90deg, #2196F3, #1976D2);
            padding: 8px 20px;
            text-align: center;
            font-size: 12px;
            font-weight: 600;
            letter-spacing: 0.5px;
        }

        /* Hero Section */
        .hero {
            position: relative;
            height: 180px;
            background: linear-gradient(180deg, #0C1620 0%, #081018 100%);
            display: flex;
            align-items: center;
            justify-content: center;
            overflow: hidden;
            border-bottom: 1px solid #1A2D42;
        }

        .hero::before {
            content: '';
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background: url('assets/images/banner.jpg') center/cover;
            opacity: 0.15;
        }

        .hero-content {
            position: relative;
            text-align: center;
            z-index: 1;
        }

        .hero h1 {
            font-size: 32px;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 6px;
            color: #FFFFFF;
            margin-bottom: 8px;
        }

        .hero h1 span {
            color: #2196F3;
        }

        .hero p {
            font-size: 13px;
            color: #A0AEC0;
            letter-spacing: 2px;
            text-transform: uppercase;
        }

        /* Featured Items Section */
        .section {
            padding: 16px;
        }

        .section-header {
            display: flex;
            justify-content: space-between;
            align-items: center;
            margin-bottom: 12px;
            padding-bottom: 10px;
            border-bottom: 1px solid #1A2D42;
        }

        .section-title {
            font-size: 11px;
            font-weight: 600;
            color: #4A5568;
            text-transform: uppercase;
            letter-spacing: 2px;
        }

        /* Item Cards Grid */
        .items-grid {
            display: grid;
            grid-template-columns: repeat(auto-fill, minmax(130px, 1fr));
            gap: 10px;
        }

        .item-card {
            background: linear-gradient(180deg, #0E1822 0%, #0A1218 100%);
            border-radius: 6px;
            padding: 12px;
            text-align: center;
            transition: all 0.2s ease;
            border: 1px solid #1A2D42;
            cursor: pointer;
        }

        .item-card:hover {
            border-color: #2196F3;
            background: linear-gradient(180deg, #101E2E 0%, #0C1620 100%);
        }

        .item-image {
            width: 50px;
            height: 50px;
            margin: 0 auto 8px;
            background: linear-gradient(180deg, #1A2D42 0%, #132030 100%);
            border-radius: 6px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 20px;
        }

        .item-name {
            font-size: 11px;
            font-weight: 600;
            color: #FFFFFF;
            margin-bottom: 3px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .item-category {
            font-size: 9px;
            color: #4A5568;
            margin-bottom: 6px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }

        .item-price {
            font-size: 11px;
            color: #2196F3;
            font-weight: 600;
        }

        .item-price.cash {
            color: #42A5F5;
        }

        /* Category Icons */
        .cat-melee::before { content: '⚔️'; }
        .cat-rifle::before { content: '🎯'; }
        .cat-smg::before { content: '🔫'; }
        .cat-shotgun::before { content: '💥'; }
        .cat-pistol::before { content: '🔫'; }
        .cat-armor::before { content: '🛡️'; }
        .cat-accessory::before { content: '💎'; }

        /* No items message */
        .no-items {
            text-align: center;
            padding: 30px;
            color: #4A5568;
            font-size: 12px;
        }

        /* Scrollbar */
        ::-webkit-scrollbar {
            width: 4px;
        }

        ::-webkit-scrollbar-track {
            background: #0A1218;
        }

        ::-webkit-scrollbar-thumb {
            background: #1A2D42;
            border-radius: 2px;
        }

        ::-webkit-scrollbar-thumb:hover {
            background: #2A4A6A;
        }

        /* Loading state */
        .loading {
            text-align: center;
            padding: 40px;
            color: #4A5568;
            font-size: 12px;
        }

        .loading::after {
            content: '';
            display: inline-block;
            width: 16px;
            height: 16px;
            border: 2px solid #2196F3;
            border-top-color: transparent;
            border-radius: 50%;
            animation: spin 1s linear infinite;
            margin-left: 8px;
            vertical-align: middle;
        }

        @keyframes spin {
            to { transform: rotate(360deg); }
        }

        /* Stats Bar */
        .stats-bar {
            display: flex;
            justify-content: center;
            gap: 30px;
            padding: 12px;
            background: linear-gradient(180deg, #0E1822 0%, #0A1218 100%);
            border-bottom: 1px solid #1A2D42;
        }

        .stat-item {
            text-align: center;
        }

        .stat-value {
            font-size: 18px;
            font-weight: 700;
            color: #2196F3;
        }

        .stat-label {
            font-size: 9px;
            color: #4A5568;
            text-transform: uppercase;
            letter-spacing: 1px;
            margin-top: 2px;
        }
    </style>
</head>
<body>
    <div id="announcement-bar" class="announcement-bar" style="display: none;"></div>

    <div class="hero">
        <div class="hero-content">
            <h1 id="hero-title">GUNZ <span>ONLINE</span></h1>
            <p id="hero-subtitle">Season 2 Now Live</p>
        </div>
    </div>

    <div class="stats-bar">
        <div class="stat-item">
            <div class="stat-value" id="online-count">--</div>
            <div class="stat-label">Online</div>
        </div>
        <div class="stat-item">
            <div class="stat-value" id="total-players">--</div>
            <div class="stat-label">Players</div>
        </div>
    </div>

    <div class="section">
        <div class="section-header">
            <span class="section-title">Featured Items</span>
        </div>
        <div id="items-container" class="items-grid">
            <div class="loading">Loading items</div>
        </div>
    </div>

    <script>
        var API_BASE = 'api';

        // IE-compatible XMLHttpRequest helper
        function httpGet(url, callback) {
            var xhr = new XMLHttpRequest();
            xhr.onreadystatechange = function() {
                if (xhr.readyState === 4) {
                    if (xhr.status === 200) {
                        try {
                            var data = JSON.parse(xhr.responseText);
                            callback(null, data);
                        } catch (e) {
                            callback(e, null);
                        }
                    } else {
                        callback(new Error('Request failed'), null);
                    }
                }
            };
            xhr.open('GET', url, true);
            xhr.send();
        }

        function loadSettings() {
            httpGet(API_BASE + '/settings.php', function(err, settings) {
                if (err) {
                    console.log('Error loading settings');
                    return;
                }

                if (settings.bannerText) {
                    var title = document.getElementById('hero-title');
                    var parts = settings.bannerText.split(' ');
                    if (parts.length >= 2) {
                        title.innerHTML = parts[0] + ' <span>' + parts.slice(1).join(' ') + '</span>';
                    } else {
                        title.textContent = settings.bannerText;
                    }
                }
                if (settings.bannerSubtext) {
                    document.getElementById('hero-subtitle').textContent = settings.bannerSubtext;
                }
                if (settings.announcementEnabled && settings.announcementText) {
                    var bar = document.getElementById('announcement-bar');
                    bar.textContent = settings.announcementText;
                    bar.style.display = 'block';
                }

                // Update stats
                if (settings.onlineCount !== undefined) {
                    document.getElementById('online-count').textContent = settings.onlineCount;
                }
                if (settings.totalPlayers !== undefined) {
                    document.getElementById('total-players').textContent = settings.totalPlayers;
                }
            });
        }

        function loadFeaturedItems() {
            var container = document.getElementById('items-container');
            httpGet(API_BASE + '/shop.php?featured=true', function(err, items) {
                if (err) {
                    container.innerHTML = '<div class="no-items">Unable to load items</div>';
                    return;
                }

                if (!items || items.length === 0) {
                    container.innerHTML = '<div class="no-items">No featured items available</div>';
                    return;
                }

                var html = '';
                for (var i = 0; i < items.length; i++) {
                    var item = items[i];
                    var categoryClass = 'cat-' + item.category.toLowerCase();
                    var priceClass = item.currency === 'Cash' ? 'cash' : '';
                    html += '<div class="item-card">' +
                        '<div class="item-image ' + categoryClass + '"></div>' +
                        '<div class="item-name">' + item.name + '</div>' +
                        '<div class="item-category">' + item.category + '</div>' +
                        '<div class="item-price ' + priceClass + '">' + item.price.toLocaleString() + ' ' + item.currency + '</div>' +
                        '</div>';
                }
                container.innerHTML = html;
            });
        }

        // Load everything when page is ready
        if (document.readyState === 'loading') {
            document.addEventListener('DOMContentLoaded', function() {
                loadSettings();
                loadFeaturedItems();
            });
        } else {
            loadSettings();
            loadFeaturedItems();
        }

        // Refresh every 30 seconds
        setInterval(function() {
            loadSettings();
            loadFeaturedItems();
        }, 30000);
    </script>
</body>
</html>
