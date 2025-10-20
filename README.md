# 🚗 Toyota Türkiye - Araç Servis Takip Sistemi

## 🔐 Giriş Bilgileri
- **Kullanıcı Adı:** `admin`
- **Şifre:** `Toyota2024!`

## 🌐 Erişim URL'leri
- **Frontend:** http://localhost:3000
- **API:** http://localhost:5221/api
- **Swagger UI:** http://localhost:5221
- **PostgreSQL:** localhost:5432

## 🚀 Docker ile Çalıştırma

### Temel Komutlar
```bash
# Tüm sistemi ayağa kaldır
docker-compose up -d

# Servisleri kontrol et  
docker-compose ps

# Logları görüntüle
docker-compose logs

# Sistemi durdur
docker-compose down

# Sistemi durdur ve verileri temizle
docker-compose down -v
```

### Environment Ayarları
```yaml
# Development (Swagger aktif)
ASPNETCORE_ENVIRONMENT=Development

# Production (Swagger kapalı) 
ASPNETCORE_ENVIRONMENT=Production
```

## 🛠️ Teknoloji Yığını

### Backend
- **Framework:** .NET 9.0
- **Web API:** ASP.NET Core Web API
- **ORM:** Entity Framework Core 9.0
- **Veritabanı:** PostgreSQL 16
- **CQRS:** MediatR
- **Authentication:** JWT Bearer Token

### Frontend
- **Framework:** React 18.3
- **Language:** TypeScript
- **Build Tool:** Vite
- **State Management:** TanStack Query (React Query)
- **HTTP Client:** Axios
- **CSS Framework:** Tailwind CSS
- **Routing:** React Router

### DevOps
- **Containerization:** Docker & Docker Compose
- **Database Container:** PostgreSQL 16-alpine
- **Web Server:** Nginx (for React app)

## 📋 Toyota Türkiye – Case Raporu

## ⚙️ Gereksinimler

### 🔐 İşlevsel Gereksinimler

#### Login Sistemi
- Kullanıcı sisteme adı ve şifresi ile JWT token tabanlı authentication sistemi ile giriş yapabilecektir
- Kullanıcının sisteme giriş yaptığı token 8 saat sonra uçurulur - token expired yönetimi
- Case için tanımlanan kullanıcı bilgisi defaulttur (admin/Toyota2024!)
- Sisteme Ajax Form Submit Mantığı ile istek atılmaktadır

#### Authorization
- API endpoint'leri JWT token gerektirir kullanıcı sisteme giriş yaptıktan sonra /api ucuna istek atabilir
- Eğer token yoksa kullanıcı otomatik olarak login sayfasına yönlendirilir

#### Servis Kayıt Yönetimi
Kullanıcı sisteme giriş yapar Yeni Servis Girişi butonuna tıklar, popup/modal açılır, ekteki bilgileri girer, Kaydet butonuna tıklar ve sisteme kayıt yapmış olur, kayıt işlemi başarıyla tamamlandığında popup kapanır ve tablo (datatable) güncellenir.

**Form Alanları:**
- ✅ **Araç plaka bilgisi** (Türkiye plaka format valide edilmiştir) - *Zorunlu*
- ✅ **Marka bilgisi** - *Zorunlu*
- ✅ **Model bilgisi** - *Zorunlu*
- ✅ **Kilometre bilgisi** (0+ valide edilmiştir) - *Zorunlu*
- ⚪ **Model yılı** (1900-2026 arası) – *Zorunlu değil*
- ✅ **Servis tarihi** (geçmiş/bugün, gelecek tarihe kayıt yapamaz) - *Zorunlu*
- ⚪ **Garanti durumu** (Var/Yok/Belirsiz) – *Zorunlu Değil*
- ⚪ **Servis yapılan şehir** (81 Türkiye ili ile sınırlı) – *Zorunlu Değil*
- ⚪ **Servis notu** (Maksimum 1000 karakter limiti var) – *Zorunlu değil*

#### Servis Kayıtları Listeleme 
Kullanıcı sisteme giriş yapar, dashboard sayfasına yönlendirilir. Servis Girişleri altında ekteki özelliklere sahip server-side pagination listesini görüntüler.

**Liste Özellikleri:**
- 📊 Tablo formatında listeleme
- 📋 Plaka, Marka & Model, KM, Model Yılı, Servis Tarihi, Garanti durumu, Şehir görüntüleme
- 📅 Tarih format Türkçe (dd.MM.yyyy)
- 📄 Sayfa başına 10 kayıt
- 🔢 Toplam kayıt sayısı gösterimi
- ⬅️➡️ Önceki/Sonraki sayfa navigasyonu
- 📍 Sayfa bilgisi

### 🏗️ İşlevsel Olmayan Gereksinimler

#### Clean Architecture
- **Layered Architecture:** Domain → Application → Infrastructure → WebAPI
- **Dependency Inversion:** Abstract interfaces kullanımı

#### Design Patterns
- **CQRS Pattern** (MediatR)
- **Repository Pattern**
- **Unit of Work Pattern**
- **Mediator Pattern**
- **Dependency Injection**

#### Database Performance
- **Indexing**
- **Pagination**
- **AsNoTracking**

#### API Performance & Security
- **Async/Await**
- **DTO Pattern**
- **Request Validation**
- **Auth Required**
- **CORS Configuration**
- **Request Logging**

#### Data Security
- **Password Security**
- **JWT Security**
- **Input Validation**
- **SQL Injection Prevention**

#### Code Quality
- **SOLID Prensipleri**
- **Clean Code**
- **Error Handling**
- **Logging**

## 🏗️ Sistem Mimarisi

Proje üç katmanlı Docker container yapısında geliştirilmiştir:

```
┌─────────────────────────────────────────┐
│     Container 3: toyota-web             │
│     (React + Nginx)                     │
│     Port: 3000                          │
└──────────────┬──────────────────────────┘
               │ HTTP
               ▼
┌─────────────────────────────────────────┐
│     Container 2: toyota-api             │
│     ┌─────────────────────────────┐     │
│     │ WebAPI Layer                │     │
│     │ Application Layer (CQRS)    │     │
│     │ Infrastructure Layer (EF)   │     │
│     │ Domain Layer (Entities)     │     │
│     └─────────────────────────────┘     │
│     Port: 5221                          │
└──────────────┬──────────────────────────┘
               │ Database Connection
               ▼
┌─────────────────────────────────────────┐
│     Container 1: toyota-postgres        │
│     (PostgreSQL 16)                     │
│     Port: 5432                          │
└─────────────────────────────────────────┘
```

### 📁 Proje Yapısı
```
├── 📁 src/
│   ├── 📁 Domain/           # Entity'ler ve Business Logic
│   ├── 📁 Application/      # CQRS Commands/Queries
│   ├── 📁 Infrastructure/   # Data Access ve External Services
│   └── 📁 WebAPI/          # API Controllers ve Middleware
├── 📁 DatabaseScripts/      # SQL Init Scripts
├── 🐳 Dockerfile.api       # API Container
├── 🐳 Dockerfile.web       # Frontend Container  
├── 🐳 Dockerfile.db        # Database Container
└── 🐳 docker-compose.yml   # Orchestration

