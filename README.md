# Sales Management API

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet"/>
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp"/>
  <img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white"/>
  <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white"/>
  <img src="https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=for-the-badge&logo=dotnet"/>
</p>

---

## 🇹🇷 Türkçe

### Proje Hakkında

Sales Management API, gerçek dünyada kullanılan bir satış yönetim sisteminin backend altyapısını kapsayan bir RESTful API projesidir. Katmanlı mimari prensiplerine uygun olarak geliştirilmiş olup müşteri, ürün ve sipariş yönetiminin yanı sıra satış raporlama özellikleri de sunmaktadır.

### Özellikler

- Müşteri, ürün ve sipariş CRUD işlemleri
- Çok adımlı sipariş iş kuralları (müşteri kontrolü, stok kontrolü, tutar hesaplama)
- FluentValidation ile veri doğrulama
- AutoMapper ile entity/DTO dönüşümleri
- LINQ tabanlı satış raporlama ve filtreleme
- Docker ve Docker Compose desteği
- PostgreSQL veritabanı

### Mimari

Proje dört katmandan oluşmaktadır:

```
WebAPI → Business → DataAccess → Core
```

- **Core** — Entity sınıfları, DTO'lar, interface'ler, response modelleri
- **DataAccess** — DbContext, Generic Repository, Order Repository, Migration'lar
- **Business** — Servis sınıfları, iş kuralları, AutoMapper profili, FluentValidation
- **WebAPI** — Controller'lar, API endpoint'leri, Program.cs yapılandırması

### API Endpoint'leri

| Method | Endpoint | Açıklama |
|--------|----------|----------|
| GET | /customers | Tüm müşterileri listele |
| POST | /customers | Yeni müşteri ekle |
| GET | /products | Tüm ürünleri listele |
| POST | /products | Yeni ürün ekle |
| GET | /orders | Siparişleri listele (filtreleme destekli) |
| POST | /orders | Yeni sipariş oluştur |
| GET | /reports/sales | Satış raporu getir |

### Kurulum

#### Gereksinimler

- .NET 10 SDK
- PostgreSQL 16
- Docker (opsiyonel)

#### Yerel Kurulum

1. Repoyu klonlayın:
```bash
git clone https://github.com/kullanici-adi/SalesManagementAPI.git
cd SalesManagementAPI
```

2. `appsettings.json` içindeki bağlantı dizesini güncelleyin:
```json
"ConnectionStrings": {
  "PostgreSql": "Host=localhost;Port=5432;Database=SalesManagementDb;Username=postgres;Password=sifreniz"
}
```

3. Migration'ları uygulayın:
```bash
dotnet ef database update --project DataAccess --startup-project SalesManagementAPI
```

4. Uygulamayı başlatın:
```bash
dotnet run --project SalesManagementAPI
```

#### Docker ile Kurulum

```bash
docker-compose up --build
```

API `http://localhost:8080` adresinde çalışacaktır.

### Teknolojiler

| Teknoloji | Kullanım Amacı |
|-----------|----------------|
| ASP.NET Core 10 | Web API framework |
| Entity Framework Core | ORM ve veritabanı yönetimi |
| PostgreSQL | İlişkisel veritabanı |
| AutoMapper | Entity/DTO dönüşümleri |
| FluentValidation | Veri doğrulama |
| Docker & Compose | Container ortamı |

---

## 🇬🇧 English

### About

Sales Management API is a RESTful API project that covers the backend infrastructure of a real-world sales management system. Built following layered architecture principles, it provides customer, product, and order management along with sales reporting features.

### Features

- Customer, product, and order CRUD operations
- Multi-step order business rules (customer validation, stock check, total calculation)
- Data validation with FluentValidation
- Entity/DTO mapping with AutoMapper
- LINQ-based sales reporting and filtering
- Docker and Docker Compose support
- PostgreSQL database

### Architecture

The project consists of four layers:

```
WebAPI → Business → DataAccess → Core
```

- **Core** — Entities, DTOs, interfaces, response models
- **DataAccess** — DbContext, Generic Repository, Order Repository, Migrations
- **Business** — Service classes, business rules, AutoMapper profile, FluentValidation
- **WebAPI** — Controllers, API endpoints, Program.cs configuration

### API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /customers | Get all customers |
| POST | /customers | Create a new customer |
| GET | /products | Get all products |
| POST | /products | Create a new product |
| GET | /orders | Get orders (with filtering support) |
| POST | /orders | Create a new order |
| GET | /reports/sales | Get sales report |

### Getting Started

#### Prerequisites

- .NET 10 SDK
- PostgreSQL 16
- Docker (optional)

#### Local Setup

1. Clone the repository:
```bash
git clone https://github.com/your-username/SalesManagementAPI.git
cd SalesManagementAPI
```

2. Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "PostgreSql": "Host=localhost;Port=5432;Database=SalesManagementDb;Username=postgres;Password=yourpassword"
}
```

3. Apply migrations:
```bash
dotnet ef database update --project DataAccess --startup-project SalesManagementAPI
```

4. Run the application:
```bash
dotnet run --project SalesManagementAPI
```

#### Docker Setup

```bash
docker-compose up --build
```

The API will be available at `http://localhost:8080`.

### Tech Stack

| Technology | Purpose |
|-----------|---------|
| ASP.NET Core 10 | Web API framework |
| Entity Framework Core | ORM and database management |
| PostgreSQL | Relational database |
| AutoMapper | Entity/DTO mapping |
| FluentValidation | Input validation |
| Docker & Compose | Containerization |

---

<p align="center">
  Developed with ❤️ by <a href="https://github.com/kullanici-adi">Fatih Korkmaz</a>
</p>
