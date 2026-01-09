<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Gunz Online - Admin Panel</title>
    <style>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #1a1a2e 0%, #16213e 100%);
            min-height: 100vh;
            color: #fff;
        }

        .container {
            max-width: 1400px;
            margin: 0 auto;
            padding: 20px;
        }

        header {
            background: rgba(0, 0, 0, 0.3);
            padding: 20px;
            border-radius: 10px;
            margin-bottom: 30px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        header h1 {
            color: #FF8C00;
            font-size: 24px;
        }

        .tabs {
            display: flex;
            gap: 10px;
            margin-bottom: 20px;
        }

        .tab-btn {
            padding: 12px 24px;
            background: rgba(255, 255, 255, 0.1);
            border: none;
            border-radius: 8px;
            color: #fff;
            cursor: pointer;
            font-size: 14px;
            transition: all 0.3s;
        }

        .tab-btn:hover, .tab-btn.active {
            background: #FF8C00;
        }

        .tab-content {
            display: none;
        }

        .tab-content.active {
            display: block;
        }

        .panel {
            background: rgba(0, 0, 0, 0.3);
            border-radius: 10px;
            padding: 20px;
            margin-bottom: 20px;
        }

        .panel h2 {
            color: #FF8C00;
            margin-bottom: 20px;
            font-size: 18px;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .form-group label {
            display: block;
            margin-bottom: 5px;
            color: #aaa;
            font-size: 13px;
        }

        .form-group input, .form-group textarea, .form-group select {
            width: 100%;
            padding: 10px;
            background: #2a2a3e;
            border: 1px solid rgba(255, 255, 255, 0.2);
            border-radius: 6px;
            color: #fff;
            font-size: 14px;
        }

        .form-group select {
            background: #2a2a3e;
            cursor: pointer;
        }

        .form-group select option {
            background: #2a2a3e;
            color: #fff;
            padding: 10px;
        }

        .form-group input:focus, .form-group textarea:focus, .form-group select:focus {
            outline: none;
            border-color: #FF8C00;
        }

        .form-group textarea {
            resize: vertical;
            min-height: 80px;
        }

        .form-row {
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
            gap: 15px;
        }

        .btn {
            padding: 10px 20px;
            border: none;
            border-radius: 6px;
            cursor: pointer;
            font-size: 14px;
            transition: all 0.3s;
        }

        .btn-primary {
            background: #FF8C00;
            color: #fff;
        }

        .btn-primary:hover {
            background: #e67e00;
        }

        .btn-danger {
            background: #dc3545;
            color: #fff;
        }

        .btn-danger:hover {
            background: #c82333;
        }

        .btn-sm {
            padding: 6px 12px;
            font-size: 12px;
        }

        .item-list {
            margin-top: 20px;
        }

        .item-card {
            background: rgba(255, 255, 255, 0.05);
            border-radius: 8px;
            padding: 15px;
            margin-bottom: 10px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }

        .item-card:hover {
            background: rgba(255, 255, 255, 0.1);
        }

        .item-info h3 {
            font-size: 16px;
            margin-bottom: 5px;
        }

        .item-info p {
            font-size: 12px;
            color: #888;
        }

        .item-category {
            display: inline-block;
            padding: 3px 8px;
            background: #FF8C00;
            border-radius: 4px;
            font-size: 10px;
            margin-right: 10px;
        }

        .item-actions {
            display: flex;
            gap: 8px;
        }

        .checkbox-group {
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .checkbox-group input[type="checkbox"] {
            width: auto;
        }

        .alert {
            padding: 12px 20px;
            border-radius: 6px;
            margin-bottom: 20px;
            display: none;
        }

        .alert-success {
            background: rgba(40, 167, 69, 0.2);
            border: 1px solid #28a745;
            color: #28a745;
        }

        .alert-error {
            background: rgba(220, 53, 69, 0.2);
            border: 1px solid #dc3545;
            color: #dc3545;
        }

        .announcement-preview {
            background: linear-gradient(90deg, #FF8C00, #FF6B00);
            padding: 10px 20px;
            border-radius: 6px;
            margin-top: 10px;
            text-align: center;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <div class="container">
        <header>
            <h1>Gunz Online Admin Panel</h1>
            <a href="../index.php" target="_blank" class="btn btn-primary">View Launcher Page</a>
        </header>

        <div id="alert" class="alert"></div>

        <div class="tabs">
            <button class="tab-btn active" onclick="showTab('news')">News & Announcements</button>
            <button class="tab-btn" onclick="showTab('shop')">Shop Items</button>
            <button class="tab-btn" onclick="showTab('settings')">Settings</button>
        </div>

        <!-- News Tab -->
        <div id="news-tab" class="tab-content active">
            <div class="panel">
                <h2>Add New Announcement</h2>
                <form id="news-form">
                    <div class="form-row">
                        <div class="form-group">
                            <label>Category</label>
                            <select id="news-category">
                                <option value="UPDATE">UPDATE</option>
                                <option value="EVENT">EVENT</option>
                                <option value="MAINTENANCE">MAINTENANCE</option>
                                <option value="NEWS">NEWS</option>
                            </select>
                        </div>
                        <div class="form-group">
                            <label>Title</label>
                            <input type="text" id="news-title" placeholder="Enter title" required>
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Summary</label>
                        <textarea id="news-summary" placeholder="Enter summary"></textarea>
                    </div>
                    <button type="submit" class="btn btn-primary">Add News</button>
                </form>
            </div>

            <div class="panel">
                <h2>Current News</h2>
                <div id="news-list" class="item-list"></div>
            </div>
        </div>

        <!-- Shop Tab -->
        <div id="shop-tab" class="tab-content">
            <div class="panel">
                <h2>Add New Shop Item</h2>
                <form id="shop-form">
                    <div class="form-row">
                        <div class="form-group">
                            <label>Item Name</label>
                            <input type="text" id="item-name" placeholder="Enter item name" required>
                        </div>
                        <div class="form-group">
                            <label>Category</label>
                            <select id="item-category">
                                <option value="Melee">Melee</option>
                                <option value="Rifle">Rifle</option>
                                <option value="SMG">SMG</option>
                                <option value="Shotgun">Shotgun</option>
                                <option value="Pistol">Pistol</option>
                                <option value="Armor">Armor</option>
                                <option value="Accessory">Accessory</option>
                            </select>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group">
                            <label>Price</label>
                            <input type="number" id="item-price" placeholder="Enter price" required>
                        </div>
                        <div class="form-group">
                            <label>Currency</label>
                            <select id="item-currency">
                                <option value="Gold">Gold</option>
                                <option value="Cash">Cash</option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Description</label>
                        <textarea id="item-description" placeholder="Enter description"></textarea>
                    </div>
                    <div class="form-group">
                        <label>Image Filename</label>
                        <input type="text" id="item-image" placeholder="e.g., weapon.png">
                    </div>
                    <div class="form-group checkbox-group">
                        <input type="checkbox" id="item-featured">
                        <label for="item-featured">Featured Item (shows on launcher)</label>
                    </div>
                    <button type="submit" class="btn btn-primary">Add Item</button>
                </form>
            </div>

            <div class="panel">
                <h2>Current Shop Items</h2>
                <div id="shop-list" class="item-list"></div>
            </div>
        </div>

        <!-- Settings Tab -->
        <div id="settings-tab" class="tab-content">
            <div class="panel">
                <h2>Launcher Settings</h2>
                <form id="settings-form">
                    <div class="form-row">
                        <div class="form-group">
                            <label>Banner Text</label>
                            <input type="text" id="banner-text" placeholder="Welcome to Gunz Online">
                        </div>
                        <div class="form-group">
                            <label>Banner Subtext</label>
                            <input type="text" id="banner-subtext" placeholder="Season 2 Now Live!">
                        </div>
                    </div>
                    <div class="form-group checkbox-group">
                        <input type="checkbox" id="announcement-enabled">
                        <label for="announcement-enabled">Enable Announcement Bar</label>
                    </div>
                    <div class="form-group">
                        <label>Announcement Text</label>
                        <input type="text" id="announcement-text" placeholder="Enter announcement">
                    </div>
                    <div id="announcement-preview" class="announcement-preview" style="display:none;"></div>
                    <br>
                    <button type="submit" class="btn btn-primary">Save Settings</button>
                </form>
            </div>
        </div>
    </div>

    <script>
        const API_BASE = '../api';

        function showTab(tab) {
            document.querySelectorAll('.tab-content').forEach(t => t.classList.remove('active'));
            document.querySelectorAll('.tab-btn').forEach(b => b.classList.remove('active'));
            document.getElementById(tab + '-tab').classList.add('active');
            event.target.classList.add('active');
        }

        function showAlert(message, type) {
            const alert = document.getElementById('alert');
            alert.textContent = message;
            alert.className = 'alert alert-' + type;
            alert.style.display = 'block';
            setTimeout(() => alert.style.display = 'none', 3000);
        }

        // News Functions
        async function loadNews() {
            const response = await fetch(API_BASE + '/news.php');
            const news = await response.json();
            const list = document.getElementById('news-list');
            list.innerHTML = news.map(item => `
                <div class="item-card">
                    <div class="item-info">
                        <h3><span class="item-category">${item.category}</span>${item.title}</h3>
                        <p>${item.summary}</p>
                        <p style="margin-top:5px;color:#666;">${item.date}</p>
                    </div>
                    <div class="item-actions">
                        <button class="btn btn-danger btn-sm" onclick="deleteNews(${item.id})">Delete</button>
                    </div>
                </div>
            `).join('');
        }

        document.getElementById('news-form').addEventListener('submit', async (e) => {
            e.preventDefault();
            const data = {
                category: document.getElementById('news-category').value,
                title: document.getElementById('news-title').value,
                summary: document.getElementById('news-summary').value
            };
            await fetch(API_BASE + '/news.php', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });
            showAlert('News added successfully!', 'success');
            document.getElementById('news-form').reset();
            loadNews();
        });

        async function deleteNews(id) {
            if (confirm('Are you sure you want to delete this news item?')) {
                await fetch(API_BASE + '/news.php?id=' + id, { method: 'DELETE' });
                showAlert('News deleted!', 'success');
                loadNews();
            }
        }

        // Shop Functions
        async function loadShop() {
            const response = await fetch(API_BASE + '/shop.php');
            const items = await response.json();
            const list = document.getElementById('shop-list');
            list.innerHTML = items.map(item => `
                <div class="item-card">
                    <div class="item-info">
                        <h3><span class="item-category">${item.category}</span>${item.name} ${item.featured ? '⭐' : ''}</h3>
                        <p>${item.description}</p>
                        <p style="margin-top:5px;color:#FF8C00;">${item.price} ${item.currency}</p>
                    </div>
                    <div class="item-actions">
                        <button class="btn btn-primary btn-sm" onclick="toggleFeatured(${item.id}, ${!item.featured})">${item.featured ? 'Unfeature' : 'Feature'}</button>
                        <button class="btn btn-danger btn-sm" onclick="deleteShopItem(${item.id})">Delete</button>
                    </div>
                </div>
            `).join('');
        }

        document.getElementById('shop-form').addEventListener('submit', async (e) => {
            e.preventDefault();
            const data = {
                name: document.getElementById('item-name').value,
                category: document.getElementById('item-category').value,
                price: parseInt(document.getElementById('item-price').value),
                currency: document.getElementById('item-currency').value,
                description: document.getElementById('item-description').value,
                image: document.getElementById('item-image').value,
                featured: document.getElementById('item-featured').checked
            };
            await fetch(API_BASE + '/shop.php', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });
            showAlert('Item added successfully!', 'success');
            document.getElementById('shop-form').reset();
            loadShop();
        });

        async function toggleFeatured(id, featured) {
            await fetch(API_BASE + '/shop.php', {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ id, featured })
            });
            loadShop();
        }

        async function deleteShopItem(id) {
            if (confirm('Are you sure you want to delete this item?')) {
                await fetch(API_BASE + '/shop.php?id=' + id, { method: 'DELETE' });
                showAlert('Item deleted!', 'success');
                loadShop();
            }
        }

        // Settings Functions
        async function loadSettings() {
            const response = await fetch(API_BASE + '/settings.php');
            const settings = await response.json();
            document.getElementById('banner-text').value = settings.bannerText || '';
            document.getElementById('banner-subtext').value = settings.bannerSubtext || '';
            document.getElementById('announcement-enabled').checked = settings.announcementEnabled || false;
            document.getElementById('announcement-text').value = settings.announcementText || '';
            updateAnnouncementPreview();
        }

        function updateAnnouncementPreview() {
            const enabled = document.getElementById('announcement-enabled').checked;
            const text = document.getElementById('announcement-text').value;
            const preview = document.getElementById('announcement-preview');
            if (enabled && text) {
                preview.textContent = text;
                preview.style.display = 'block';
            } else {
                preview.style.display = 'none';
            }
        }

        document.getElementById('announcement-enabled').addEventListener('change', updateAnnouncementPreview);
        document.getElementById('announcement-text').addEventListener('input', updateAnnouncementPreview);

        document.getElementById('settings-form').addEventListener('submit', async (e) => {
            e.preventDefault();
            const data = {
                bannerText: document.getElementById('banner-text').value,
                bannerSubtext: document.getElementById('banner-subtext').value,
                announcementEnabled: document.getElementById('announcement-enabled').checked,
                announcementText: document.getElementById('announcement-text').value
            };
            await fetch(API_BASE + '/settings.php', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(data)
            });
            showAlert('Settings saved!', 'success');
        });

        // Initial load
        loadNews();
        loadShop();
        loadSettings();
    </script>
</body>
</html>
