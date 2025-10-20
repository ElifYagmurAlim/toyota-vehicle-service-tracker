# 🚗 Toyota Türkiye - Araç Servis Takip Sistemi

## 🔐 Giriş Bilgileri
- **Kullanıcı Adı:** `admin`
- **Şifre:** `Toyota2024!`

## 🌐 Erişim URL'leri
- **Frontend:** http://localhost:3000
- **API:** http://localhost:5221/api
- **Swagger UI:** http://localhost:5221
- **PostgreSQL:** localhost:5432

## Veri Sayısı
- **30 Adet araç bilgisi sistemde mevcut**

## 🚀 Docker ile Çalıştırma

### 🆙 Sistem Başlatma
```bash
# 1. Repository'yi klonlayın
git clone https://github.com/ElifYagmurAlim/toyota-vehicle-service-tracker.git
cd toyota-vehicle-service-tracker

# 2. Tüm sistemi ayağa kaldır (ilk çalıştırmada build edilir)
docker-compose up -d

# 3. Container durumlarını kontrol et
docker-compose ps

# 4. Sistem hazır! Browser'da http://localhost:3000 adresine gidin
```

### 📊 Sistem Yönetimi
```bash
# Logları görüntüle (tüm servisler)
docker-compose logs

# Belirli servis loglarını görüntüle
docker-compose logs toyota-api
docker-compose logs toyota-web  
docker-compose logs toyota-postgres

# Gerçek zamanlı log takibi
docker-compose logs -f

# Container durumları ve kaynak kullanımı
docker-compose ps
docker stats
```

### 🔄 Güncellemeler ve Yeniden Build
```bash
# Sadece web frontend'ini yeniden build et
docker-compose build web
docker-compose up -d web

# Sadece API'yi yeniden build et
docker-compose build api
docker-compose up -d api

# Tüm servisleri yeniden build et
docker-compose build
docker-compose up -d
```

### ⚠️ Sorun Giderme
```bash
# Container'ları yeniden başlat
docker-compose restart

# Belirli container'ı yeniden başlat
docker-compose restart toyota-api

# Container içine bağlan (debug için)
docker exec -it toyota-postgres psql -U postgres -d VehicleServiceTrackerDb
docker exec -it toyota-api bash
docker exec -it toyota-web sh
```

### 🛑 Sistemi Durdurma ve Temizlik
```bash
# Sistemi durdur (container'lar durur, veriler korunur)
docker-compose down

# Sistemi durdur ve tüm verileri sil
docker-compose down -v

# Sistemi durdur, volume'ları ve image'ları da sil
docker-compose down -v --rmi all

# Kullanılmayan Docker kaynaklarını temizle
docker system prune -a

# Sadece PostgreSQL verilerini sıfırla
docker-compose down
docker volume rm vehicleservicetracker_postgres_data
docker-compose up -d
```

### 🔧 Environment Ayarları
```yaml
# Development (Swagger aktif, detaylı loglar)
ASPNETCORE_ENVIRONMENT=Development

# Production (Swagger kapalı, optimize loglar) 
ASPNETCORE_ENVIRONMENT=Production
```

### 📡 Port Yapılandırması
- **Frontend (React):** http://localhost:3000
- **API (.NET):** http://localhost:5221
- **Database (PostgreSQL):** localhost:5432
- **Swagger UI:** http://localhost:5221 (Development modunda)

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
- **Database Container:** PostgreSQL 16 (UTF-8 locale support)
- **Web Server:** Nginx (for React app)
- **Auto Seed Data:** PostgreSQL init scripts
- **Multi-stage Builds:** Optimized container images

### 🆕 Son Güncellemeler
- ✅ **Türkçe Karakter Desteği:** PostgreSQL UTF-8 encoding düzeltmeleri
- ✅ **30 Adet Seed Data:** Gerçekçi Toyota araç servisi verileri
- ✅ **Servis Detay Modal:** Interactive servis notu görüntüleme
- ✅ **Docker Optimizasyon:** Health checks ve otomatik seed loading
- ✅ **GitHub Integration:** Tam proje deployment

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
- ✨ **YENİ:** Servis detay görüntüleme modal penceresi

#### 🆕 Servis Detay Görüntüleme
Kullanıcı servis kayıtları tablosunda her satırın sonundaki **"Detay"** butonuna tıklayarak servis notunu ve diğer detayları modal pencerede görüntüleyebilir.

**Modal Özellikleri:**
- 🔍 **Servis Detay Butonu:** Her tablo satırında Toyota kırmızısı renkli "Detay" butonu
- 📝 **Servis Notu Görüntüleme:** Tam servis notunu scrollable alan içinde gösterme
- 📊 **Detaylı Bilgiler:** Servis tarihi, şehir, kilometre, garanti durumu
- ❌ **Kolay Kapatma:** Modal dışına tıklama veya "Kapat" butonu ile kapatma
- 📱 **Responsive:** Mobil uyumlu tasarım
- 🎨 **Toyota Corporate Colors:** Kırmızı renk teması

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

