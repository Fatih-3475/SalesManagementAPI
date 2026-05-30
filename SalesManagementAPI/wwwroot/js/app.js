const API_BASE = 'https://localhost:7102';

const PAGE_CONFIG = {
    dashboard: { title: 'Dashboard', btn: null },
    customers: { title: 'Müşteriler', btn: '+ Müşteri Ekle' },
    products: { title: 'Ürünler', btn: '+ Ürün Ekle' },
    orders: { title: 'Siparişler', btn: '+ Sipariş Oluştur' },
    reports: { title: 'Analitikler', btn: null },
};

function showToast(message, type = 'success') {
    const container = document.getElementById('toastContainer');
    const icons = { success: '✅', error: '❌', info: 'ℹ️' };
    const toast = document.createElement('div');
    toast.className = `toast toast-${type}`;
    toast.innerHTML = `<span>${icons[type]}</span><span>${message}</span>`;
    container.appendChild(toast);
    setTimeout(() => toast.remove(), 3500);
}

function parseError(json) {
    if (json.errors && typeof json.errors === 'object') {
        return Object.values(json.errors).flat().join(' | ');
    }
    return json.title ?? 'Bir şeyler ters gitti';
}

async function apiFetch(endpoint) {
    try {
        const res = await fetch(API_BASE + endpoint);
        if (!res.ok) throw new Error(`HTTP ${res.status}`);
        const json = await res.json();
        return json.data ?? json;
    } catch (err) {
        console.error(`[API] ${endpoint} hatası:`, err);
        return null;
    }
}

async function loadCustomers() {
    const tbody = document.getElementById('customers-tbody');
    tbody.innerHTML = `<tr><td colspan="4" class="loading-row">Yükleniyor...</td></tr>`;
    const data = await apiFetch('/customers');
    if (!data || data.length === 0) {
        tbody.innerHTML = `<tr><td colspan="4" class="empty-row">Müşteri bulunamadı</td></tr>`;
        return;
    }
    document.getElementById('stat-customers').textContent = data.length;
    tbody.innerHTML = data.map(c => `
        <tr>
            <td>${c.id}</td>
            <td>${c.name ?? '—'}</td>
            <td>${c.email ?? '—'}</td>
            <td>${c.phone ?? '—'}</td>
        </tr>
    `).join('');
}

function setupCustomerForm() {
    const formCard = document.getElementById('customer-form-card');

    document.getElementById('topbarBtn').addEventListener('click', () => {
        const hash = window.location.hash.replace('#', '') || 'dashboard';
        if (hash === 'customers') {
            formCard.style.display = 'block';
            document.getElementById('customer-name').focus();
        }
    });

    document.getElementById('closeCustomerForm').addEventListener('click', () => {
        formCard.style.display = 'none';
        clearCustomerForm();
    });
    document.getElementById('closeCustomerForm2').addEventListener('click', () => {
        formCard.style.display = 'none';
        clearCustomerForm();
    });

    document.getElementById('saveCustomerBtn').addEventListener('click', saveCustomer);
}

function clearCustomerForm() {
    document.getElementById('customer-name').value = '';
    document.getElementById('customer-email').value = '';
    document.getElementById('customer-phone').value = '';
}

async function saveCustomer() {
    const name = document.getElementById('customer-name').value.trim();
    const email = document.getElementById('customer-email').value.trim();
    const phone = document.getElementById('customer-phone').value.trim();

    if (!name || !email || !phone) {
        showToast('Lütfen tüm alanları doldurun!', 'error');
        return;
    }

    const btn = document.getElementById('saveCustomerBtn');
    btn.textContent = 'Kaydediliyor...';
    btn.disabled = true;

    
    try {
        const res = await fetch(API_BASE + '/customers', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, email, phone })
        });
        const json = await res.json();

        if (!res.ok) {
            const errorMsg = json.errors && typeof json.errors === 'object'
                ? Object.values(json.errors).flat().join(' | ')
                : json.title ?? 'Bir şeyler ters gitti';
            showToast(errorMsg, 'error');
            return;
        }

        if (!json.isSuccess) {
            showToast(json.errors?.[0] ?? 'Bir şeyler ters gitti', 'error');
            return;
        }

        document.getElementById('customer-form-card').style.display = 'none';
        clearCustomerForm();
        showToast('Müşteri başarıyla eklendi!', 'success');
        loadCustomers();
    } catch (err) {
        showToast('Sunucuya bağlanılamadı!', 'error');
        console.error(err);
    } finally {
        btn.textContent = 'Kaydet';
        btn.disabled = false;
    }
}

async function loadProducts() {
    const tbody = document.getElementById('products-tbody');
    tbody.innerHTML = `<tr><td colspan="4" class="loading-row">Yükleniyor...</td></tr>`;
    const data = await apiFetch('/products');
    if (!data || data.length === 0) {
        tbody.innerHTML = `<tr><td colspan="4" class="empty-row">Ürün bulunamadı</td></tr>`;
        return;
    }
    document.getElementById('stat-products').textContent = data.length;
    tbody.innerHTML = data.map(p => `
        <tr>
            <td>${p.id}</td>
            <td>${p.name ?? '—'}</td>
            <td>₺${Number(p.price).toLocaleString('tr-TR')}</td>
            <td>${p.stock}</td>
        </tr>
    `).join('');
}

function setupProductForm() {
    const formCard = document.getElementById('product-form-card');

    document.getElementById('topbarBtn').addEventListener('click', () => {
        const hash = window.location.hash.replace('#', '') || 'dashboard';
        if (hash === 'products') {
            formCard.style.display = 'block';
            document.getElementById('product-name').focus();
        }
    });

    document.getElementById('closeProductForm').addEventListener('click', () => {
        formCard.style.display = 'none';
        clearProductForm();
    });
    document.getElementById('closeProductForm2').addEventListener('click', () => {
        formCard.style.display = 'none';
        clearProductForm();
    });

    document.getElementById('saveProductBtn').addEventListener('click', saveProduct);
}

function clearProductForm() {
    document.getElementById('product-name').value = '';
    document.getElementById('product-price').value = '';
    document.getElementById('product-stock').value = '';
}

async function saveProduct() {
    const name = document.getElementById('product-name').value.trim();
    const price = parseFloat(document.getElementById('product-price').value);
    const stock = parseInt(document.getElementById('product-stock').value);

    if (!name || isNaN(price) || isNaN(stock)) {
        showToast('Lütfen tüm alanları doldurun!', 'error');
        return;
    }
    if (price < 0 || stock < 0) {
        showToast('Fiyat ve stok 0\'dan küçük olamaz!', 'error');
        return;
    }

    const btn = document.getElementById('saveProductBtn');
    btn.textContent = 'Kaydediliyor...';
    btn.disabled = true;

    try {
        const res = await fetch(API_BASE + '/products', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ name, price, stock })
        });
        const json = await res.json();
        if (!res.ok || !json.isSuccess) {
            showToast('Hata: ' + parseError(json), 'error');
            return;
        }
        document.getElementById('product-form-card').style.display = 'none';
        clearProductForm();
        showToast('Ürün başarıyla eklendi!', 'success');
        loadProducts();
    } catch (err) {
        showToast('Sunucuya bağlanılamadı!', 'error');
        console.error(err);
    } finally {
        btn.textContent = 'Kaydet';
        btn.disabled = false;
    }
}

let customersList = [];
let productsList = [];

async function loadOrders(params = {}) {
    const tbody = document.getElementById('orders-tbody');
    tbody.innerHTML = `<tr><td colspan="5" class="loading-row">Yükleniyor...</td></tr>`;

    const query = new URLSearchParams();
    if (params.startDate) query.append('startDate', params.startDate);
    if (params.endDate) query.append('endDate', params.endDate);
    if (params.minAmount) query.append('minAmount', params.minAmount);
    if (params.maxAmount) query.append('maxAmount', params.maxAmount);

    const endpoint = '/orders' + (query.toString() ? '?' + query.toString() : '');
    const data = await apiFetch(endpoint);

    if (!data || data.length === 0) {
        tbody.innerHTML = `<tr><td colspan="5" class="empty-row">Sipariş bulunamadı</td></tr>`;
        document.getElementById('orders-count').textContent = '0 kayıt';
        return;
    }

    document.getElementById('stat-orders').textContent = data.length;
    document.getElementById('orders-count').textContent = data.length + ' kayıt';

    tbody.innerHTML = data.map(o => `
        <tr>
            <td>#${o.id}</td>
            <td>${o.customerName ?? '—'}</td>
            <td>${new Date(o.orderDate).toLocaleDateString('tr-TR')}</td>
            <td>₺${Number(o.totalAmount).toLocaleString('tr-TR')}</td>
            <td>
                <button class="btn-secondary" style="padding:4px 10px;font-size:11px"
                    onclick="showOrderDetail(${o.id})">
                    Detay
                </button>
            </td>
        </tr>
    `).join('');
}

function setupOrderForm() {
    const formCard = document.getElementById('order-form-card');

    document.getElementById('topbarBtn').addEventListener('click', () => {
        const hash = window.location.hash.replace('#', '') || 'dashboard';
        if (hash === 'orders') {
            openOrderForm();
        }
    });

    document.getElementById('closeOrderForm').addEventListener('click', () => {
        formCard.style.display = 'none';
        clearOrderForm();
    });
    document.getElementById('closeOrderForm2').addEventListener('click', () => {
        formCard.style.display = 'none';
        clearOrderForm();
    });

    document.getElementById('addOrderItemBtn').addEventListener('click', addOrderItemRow);
    document.getElementById('saveOrderBtn').addEventListener('click', saveOrder);
}

async function openOrderForm() {
    const formCard = document.getElementById('order-form-card');

    if (customersList.length === 0) {
        customersList = await apiFetch('/customers') ?? [];
    }
    if (productsList.length === 0) {
        productsList = await apiFetch('/products') ?? [];
    }

    const customerSelect = document.getElementById('order-customer');
    customerSelect.innerHTML = '<option value="">Müşteri seçin...</option>';
    customersList.forEach(c => {
        customerSelect.innerHTML += `<option value="${c.id}">${c.name}</option>`;
    });

    clearOrderForm();
    addOrderItemRow();
    formCard.style.display = 'block';
}

function addOrderItemRow() {
    const container = document.getElementById('order-items');
    const row = document.createElement('div');
    row.className = 'order-item-row';
    row.innerHTML = `
        <select class="item-product">
            <option value="">Ürün seçin...</option>
            ${productsList.map(p => `<option value="${p.id}">${p.name} — ₺${Number(p.price).toLocaleString('tr-TR')}</option>`).join('')}
        </select>
        <input type="number" class="item-quantity" placeholder="Adet" min="1" value="1" />
        <button class="btn-remove" onclick="this.parentElement.remove()">✕</button>
    `;
    container.appendChild(row);
}

function clearOrderForm() {
    document.getElementById('order-customer').value = '';
    document.getElementById('order-items').innerHTML = '';
}

async function saveOrder() {
    const customerId = parseInt(document.getElementById('order-customer').value);
    if (!customerId) {
        showToast('Lütfen bir müşteri seçin!', 'error');
        return;
    }

    const rows = document.querySelectorAll('.order-item-row');
    if (rows.length === 0) {
        showToast('En az bir ürün ekleyin!', 'error');
        return;
    }

    const items = [];
    for (const row of rows) {
        const productId = parseInt(row.querySelector('.item-product').value);
        const quantity = parseInt(row.querySelector('.item-quantity').value);
        if (!productId) {
            showToast('Lütfen tüm satırlarda ürün seçin!', 'error');
            return;
        }
        if (!quantity || quantity < 1) {
            showToast('Adet en az 1 olmalıdır!', 'error');
            return;
        }
        items.push({ productId, quantity });
    }

    const btn = document.getElementById('saveOrderBtn');
    btn.textContent = 'Kaydediliyor...';
    btn.disabled = true;

    try {
        const res = await fetch(API_BASE + '/orders', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ customerId, items })
        });
        const json = await res.json();
        if (!res.ok || !json.isSuccess) {
            showToast('Hata: ' + parseError(json), 'error');
            return;
        }
        document.getElementById('order-form-card').style.display = 'none';
        clearOrderForm();
        showToast('Sipariş başarıyla oluşturuldu!', 'success');
        loadOrders();
    } catch (err) {
        showToast('Sunucuya bağlanılamadı!', 'error');
        console.error(err);
    } finally {
        btn.textContent = 'Siparişi Kaydet';
        btn.disabled = false;
    }
}

function setupOrderFilters() {
    document.getElementById('applyFilterBtn').addEventListener('click', () => {
        const startDate = document.getElementById('filter-start').value;
        const endDate = document.getElementById('filter-end').value;
        const minAmount = document.getElementById('filter-min').value;
        const maxAmount = document.getElementById('filter-max').value;
        loadOrders({ startDate, endDate, minAmount, maxAmount });
    });

    document.getElementById('clearFilterBtn').addEventListener('click', () => {
        document.getElementById('filter-start').value = '';
        document.getElementById('filter-end').value = '';
        document.getElementById('filter-min').value = '';
        document.getElementById('filter-max').value = '';
        loadOrders();
    });

    document.getElementById('closeDetailBtn').addEventListener('click', () => {
        document.getElementById('order-detail-overlay').style.display = 'none';
    });
}

async function showOrderDetail(orderId) {
    const overlay = document.getElementById('order-detail-overlay');
    const body = document.getElementById('detail-body');
    const title = document.getElementById('detail-title');

    overlay.style.display = 'flex';
    body.innerHTML = '<p style="color:var(--color-text-tertiary)">Yükleniyor...</p>';

    const data = await apiFetch('/orders');
    const order = data?.find(o => o.id === orderId);

    if (!order) {
        body.innerHTML = '<p style="color:var(--color-danger-text)">Sipariş bulunamadı</p>';
        return;
    }

    title.textContent = `Sipariş #${order.id} Detayı`;
    body.innerHTML = `
        <div style="display:grid;grid-template-columns:1fr 1fr;gap:10px;margin-bottom:14px">
            <div>
                <div style="font-size:11px;color:var(--color-text-tertiary)">Müşteri</div>
                <div style="font-weight:500">${order.customerName ?? '—'}</div>
            </div>
            <div>
                <div style="font-size:11px;color:var(--color-text-tertiary)">Tarih</div>
                <div style="font-weight:500">${new Date(order.orderDate).toLocaleDateString('tr-TR')}</div>
            </div>
            <div>
                <div style="font-size:11px;color:var(--color-text-tertiary)">Toplam Tutar</div>
                <div style="font-weight:600;color:var(--color-primary)">₺${Number(order.totalAmount).toLocaleString('tr-TR')}</div>
            </div>
        </div>
        <table>
            <thead>
                <tr><th>Ürün</th><th>Adet</th><th>Birim Fiyat</th><th>Toplam</th></tr>
            </thead>
            <tbody>
                ${order.items.map(item => `
                    <tr>
                        <td>${item.productName ?? '—'}</td>
                        <td>${item.quantity}</td>
                        <td>₺${Number(item.unitPrice ?? 0).toLocaleString('tr-TR')}</td>
                        <td>₺${Number(item.totalPrice ?? 0).toLocaleString('tr-TR')}</td>
                    </tr>
                `).join('')}
            </tbody>
        </table>
    `;
}

async function loadDashboardOrders() {
    const tbody = document.getElementById('dashboard-orders-tbody');
    const data = await apiFetch('/orders');
    if (!data || data.length === 0) {
        tbody.innerHTML = `<tr><td colspan="4" class="empty-row">Sipariş bulunamadı</td></tr>`;
        return;
    }
    document.getElementById('stat-orders').textContent = data.length;
    document.getElementById('dashboard-orders-count').textContent = data.length + ' kayıt';
    const son5 = data.slice(-5).reverse();
    tbody.innerHTML = son5.map(o => `
        <tr>
            <td>#${o.id}</td>
            <td>${o.customerName ?? '—'}</td>
            <td>${new Date(o.orderDate).toLocaleDateString('tr-TR')}</td>
            <td>₺${Number(o.totalAmount).toLocaleString('tr-TR')}</td>
        </tr>
    `).join('');
}

async function loadReports() {
    const data = await apiFetch('/reports/sales');

    if (!data) {
        document.getElementById('report-detail').innerHTML =
            '<p style="color:var(--color-danger-text)">Rapor yüklenemedi</p>';
        return;
    }

    document.getElementById('report-total').textContent =
        '₺' + Number(data.totalSalesAmount).toLocaleString('tr-TR');
    document.getElementById('report-top-customer').textContent =
        data.topCustomerName ?? '—';
    document.getElementById('report-top-customer-count').textContent =
        data.topCustomerOrderCount + ' sipariş';
    document.getElementById('report-top-product').textContent =
        data.topProductName ?? '—';
    document.getElementById('report-top-product-qty').textContent =
        data.topProductQuantity + ' adet satıldı';
    document.getElementById('stat-revenue').textContent =
        '₺' + Number(data.totalSalesAmount).toLocaleString('tr-TR');

    document.getElementById('report-detail').innerHTML = `
        <div style="display:grid;grid-template-columns:1fr 1fr;gap:20px">
            <div>
                <div style="font-size:12px;font-weight:600;color:var(--color-text-secondary);margin-bottom:10px">MÜŞTERİ</div>
                <div style="display:flex;flex-direction:column;gap:8px">
                    <div style="display:flex;justify-content:space-between;font-size:13px">
                        <span style="color:var(--color-text-secondary)">En İyi Müşteri</span>
                        <span style="font-weight:500">${data.topCustomerName ?? '—'}</span>
                    </div>
                    <div style="display:flex;justify-content:space-between;font-size:13px">
                        <span style="color:var(--color-text-secondary)">Sipariş Sayısı</span>
                        <span style="font-weight:500">${data.topCustomerOrderCount}</span>
                    </div>
                </div>
            </div>
            <div>
                <div style="font-size:12px;font-weight:600;color:var(--color-text-secondary);margin-bottom:10px">ÜRÜN</div>
                <div style="display:flex;flex-direction:column;gap:8px">
                    <div style="display:flex;justify-content:space-between;font-size:13px">
                        <span style="color:var(--color-text-secondary)">En Çok Satan</span>
                        <span style="font-weight:500">${data.topProductName ?? '—'}</span>
                    </div>
                    <div style="display:flex;justify-content:space-between;font-size:13px">
                        <span style="color:var(--color-text-secondary)">Satış Adedi</span>
                        <span style="font-weight:500">${data.topProductQuantity}</span>
                    </div>
                </div>
            </div>
        </div>
        <div style="margin-top:16px;padding-top:16px;border-top:0.5px solid var(--color-border-strong);display:flex;justify-content:space-between;align-items:center">
            <span style="font-size:13px;color:var(--color-text-secondary)">Toplam Satış Tutarı</span>
            <span style="font-size:18px;font-weight:600;color:var(--color-primary)">₺${Number(data.totalSalesAmount).toLocaleString('tr-TR')}</span>
        </div>
    `;
}

function navigateTo(pageId) {
    document.querySelectorAll('.page').forEach(p => p.classList.remove('active'));
    document.querySelectorAll('.nav-item').forEach(el => el.classList.remove('active'));
    const page = document.getElementById('page-' + pageId);
    if (page) page.classList.add('active');
    const navItem = document.querySelector(`.nav-item[data-page="${pageId}"]`);
    if (navItem) navItem.classList.add('active');
    const config = PAGE_CONFIG[pageId];
    if (config) {
        document.getElementById('topbarTitle').textContent = config.title;
        const btn = document.getElementById('topbarBtn');
        if (config.btn) {
            btn.style.display = 'inline-flex';
            document.getElementById('btnLabel').textContent = config.btn;
        } else {
            btn.style.display = 'none';
        }
    }
    if (pageId === 'customers') loadCustomers();
    if (pageId === 'products') loadProducts();
    if (pageId === 'orders') loadOrders();
    if (pageId === 'reports') loadReports();
    if (pageId === 'dashboard') loadDashboardOrders();
    document.querySelector('.sidebar').classList.remove('open');
    window.location.hash = pageId;
}

function handleRoute() {
    const hash = window.location.hash.replace('#', '') || 'dashboard';
    const validPages = Object.keys(PAGE_CONFIG);
    const pageId = validPages.includes(hash) ? hash : 'dashboard';
    navigateTo(pageId);
}

/* ─── Nav Linkleri ─────────────────────────────────────── */
document.querySelectorAll('.nav-item').forEach(item => {
    item.addEventListener('click', (e) => {
        e.preventDefault();
        const pageId = item.dataset.page;
        if (pageId) navigateTo(pageId);
    });
});

/* ─── Mobil Menü ───────────────────────────────────────── */
document.getElementById('menuToggle').addEventListener('click', () => {
    document.querySelector('.sidebar').classList.toggle('open');
});

document.addEventListener('click', (e) => {
    const sidebar = document.querySelector('.sidebar');
    const toggle = document.getElementById('menuToggle');
    if (sidebar.classList.contains('open') &&
        !sidebar.contains(e.target) &&
        !toggle.contains(e.target)) {
        sidebar.classList.remove('open');
    }
});

/* ─── Başlangıç ────────────────────────────────────────── */
window.addEventListener('hashchange', handleRoute);
setupCustomerForm();
setupProductForm();
setupOrderForm();
setupOrderFilters();
loadDashboardOrders();
handleRoute();