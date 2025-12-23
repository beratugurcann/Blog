# Blog Platformu - Proje Raporu

## 1. Projenin Amacı

Bu proje, ASP.NET Core MVC ve MySQL teknolojileri kullanılarak geliştirilmiş bir blog platformudur. Projenin amacı, kullanıcıların blog yazıları oluşturup paylaşabilecekleri, yorum yapabilecekleri ve kendi içeriklerini yönetebilecekleri modern bir web uygulaması geliştirmektir.

## 2. Kullanılan Teknolojiler

### Backend
- **ASP.NET Core MVC 8.0**: Web uygulaması framework'ü
- **Entity Framework Core 8.0**: ORM (Object-Relational Mapping) aracı
- **MySQL**: İlişkisel veritabanı yönetim sistemi
- **Pomelo.EntityFrameworkCore.MySql**: MySQL için Entity Framework sağlayıcısı

### Frontend
- **HTML5**: Sayfa yapısı
- **CSS3**: Stil ve animasyonlar
- **JavaScript**: İstemci tarafı işlevsellik
- **jQuery**: DOM manipülasyonu ve form validasyonu

### Güvenlik
- **Cookie Authentication**: Kullanıcı kimlik doğrulama
- **SHA256**: Şifre hashleme algoritması
- **ValidateAntiForgeryToken**: CSRF koruması

## 3. Veritabanı Şeması

### Tablolar

#### Users (Kullanıcılar)
- `Id` (int, PK, Auto Increment)
- `Username` (varchar(50), Unique, Not Null)
- `Email` (varchar(200), Unique, Not Null)
- `PasswordHash` (varchar(255), Not Null)
- `CreatedDate` (datetime, Not Null)

#### BlogPosts (Blog Yazıları)
- `Id` (int, PK, Auto Increment)
- `Title` (varchar(200), Not Null)
- `Content` (longtext, Not Null)
- `Summary` (varchar(500), Nullable)
- `CreatedDate` (datetime, Not Null)
- `UpdatedDate` (datetime, Nullable)
- `UserId` (int, FK → Users.Id, Cascade Delete)

#### Comments (Yorumlar)
- `Id` (int, PK, Auto Increment)
- `AuthorName` (varchar(100), Not Null)
- `Email` (varchar(200), Not Null)
- `Content` (varchar(1000), Not Null)
- `CreatedDate` (datetime, Not Null)
- `BlogPostId` (int, FK → BlogPosts.Id, Cascade Delete)

### İlişkiler
- Bir kullanıcının birden fazla blog yazısı olabilir (1-N)
- Bir blog yazısının birden fazla yorumu olabilir (1-N)
- Kullanıcı silindiğinde blog yazıları da silinir (Cascade Delete)
- Blog yazısı silindiğinde yorumlar da silinir (Cascade Delete)

## 4. Ekran Görüntüleri

**Not:** Bu bölüme projenin ekran görüntülerini ekleyin:
- Ana sayfa
- Blog detay sayfası
- Yönetim paneli
- Üye olma sayfası
- Giriş sayfası
- Blog yazısı ekleme/düzenleme sayfaları

## 5. Backend Kod Yapısının Açıklaması

### 5.1. Backend Mantığının Nasıl Çalıştığı

Proje MVC (Model-View-Controller) mimarisi kullanılarak geliştirilmiştir:

1. **Model**: Veritabanı tablolarını temsil eden sınıflar (BlogPost, Comment, User)
2. **View**: Kullanıcıya gösterilen HTML sayfaları (Razor View Engine)
3. **Controller**: İş mantığını yöneten sınıflar (HomeController, BlogController, AccountController, DashboardController)

### 5.2. Controller-Model-View İlişkisinin Nasıl Kurulduğu

**Örnek: Blog Yazısı Listeleme**

1. Kullanıcı ana sayfaya gider (`/Home/Index`)
2. `HomeController.Index()` metodu çalışır
3. Controller, `BlogDbContext` üzerinden veritabanından blog yazılarını çeker
4. Veriler `List<BlogPost>` modeli olarak `Views/Home/Index.cshtml` view'ına gönderilir
5. View, Razor syntax kullanarak verileri HTML'e dönüştürür ve kullanıcıya gösterir

**Kod Örneği:**
```csharp
// Controller
public async Task<IActionResult> Index()
{
    var blogPosts = await _context.BlogPosts
        .OrderByDescending(b => b.CreatedDate)
        .ToListAsync();
    return View(blogPosts);
}
```

### 5.3. Silme/Güncelleme İşlemlerinde Hangi Endpoint'lerin Çalıştığı

#### Blog Yazısı Silme
- **GET Request**: `/Dashboard/Delete/{id}` - Silme onay sayfasını gösterir
- **POST Request**: `/Dashboard/Delete` - Silme işlemini gerçekleştirir
- **Endpoint Metodu**: `DashboardController.DeleteConfirmed(int id)`

#### Blog Yazısı Güncelleme
- **GET Request**: `/Dashboard/Edit/{id}` - Düzenleme formunu gösterir
- **POST Request**: `/Dashboard/Edit` - Güncelleme işlemini gerçekleştirir
- **Endpoint Metodu**: `DashboardController.Edit(int id, BlogPost blogPost)`

**Güvenlik Kontrolleri:**
- Her işlem öncesi kullanıcının giriş yapmış olması kontrol edilir (`[Authorize]`)
- Kullanıcının sadece kendi yazılarını düzenleyebilmesi için `UserId` kontrolü yapılır

## 6. Deployment Süreci

### Render.com'a Deployment

1. **GitHub Repository Oluşturma**
   - GitHub'da yeni bir repository oluşturun
   - Projeyi GitHub'a push edin

2. **Render.com'da Web Service Oluşturma**
   - Render.com'a giriş yapın
   - "New Web Service" seçeneğini seçin
   - GitHub repository'nizi bağlayın

3. **Build Ayarları**
   - **Build Command**: `dotnet publish -c Release -o ./publish`
   - **Start Command**: `dotnet Blog.dll`
   - **Environment**: `.NET`

4. **MySQL Veritabanı Ekleme**
   - Render.com'da "New PostgreSQL" veya MySQL servisi oluşturun
   - Veritabanı bağlantı bilgilerini alın

5. **Environment Variables**
   - `ConnectionStrings__DefaultConnection` değişkenini ekleyin
   - Değer: `Server=...;Database=...;User=...;Password=...;Port=3306;`

6. **Deployment**
   - Render.com otomatik olarak projeyi build eder ve deploy eder
   - Canlı URL'i alırsınız

### Notlar
- Production ortamında `appsettings.json` yerine environment variables kullanılmalıdır
- HTTPS otomatik olarak sağlanır
- Veritabanı migration'ları ilk deployment'ta otomatik çalışır

## 7. Sonuç ve Değerlendirme

Bu proje ile ASP.NET Core MVC framework'ü kullanarak tam fonksiyonlu bir web uygulaması geliştirildi. Proje, CRUD işlemleri, kullanıcı yönetimi, güvenlik önlemleri ve modern bir arayüz içermektedir.

### Başarılan Hedefler
- ✅ MVC mimarisi ile temiz kod yapısı
- ✅ MySQL veritabanı entegrasyonu
- ✅ Kullanıcı kimlik doğrulama sistemi
- ✅ CRUD işlemlerinin tamamlanması
- ✅ Responsive tasarım
- ✅ Güvenlik önlemleri

### Gelecek İyileştirmeler
- Şifre hashleme için daha güvenli algoritmalar (bcrypt, Argon2)
- Rate limiting eklenmesi
- Email doğrulama sistemi
- Admin paneli geliştirmeleri
- Arama ve filtreleme özellikleri

---

**Proje Tarihi:** Aralık 2025  
**Geliştirici:** [Adınız]  
**Kurs:** Web Programlama - Final Projesi

