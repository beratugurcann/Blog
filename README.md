# Blog Platformu

ASP.NET Core MVC ve MySQL kullanılarak geliştirilmiş blog platformu.

## Özellikler

- Blog yazıları oluşturma, düzenleme ve silme
- Yorum ekleme ve yönetimi
- Kullanıcı kayıt ve giriş sistemi
- Her kullanıcının kendi yönetim paneli
- Responsive tasarım
- Form validasyonları

## Teknolojiler

- ASP.NET Core MVC 8.0
- MySQL
- Entity Framework Core
- Cookie Authentication

## Kurulum

### Gereksinimler

- .NET 8.0 SDK
- MySQL Server 8.0+
- Visual Studio veya VS Code

### Kurulum Adımları

1. MySQL veritabanını oluşturun:
```sql
CREATE DATABASE BlogDb;
```

2. `appsettings.json.example` dosyasını kopyalayıp `appsettings.json` olarak kaydedin ve veritabanı bilgilerinizi girin:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=BlogDb;User=root;Password=SIFRENIZ;Port=3306;"
}
```

3. Paketleri yükleyin:
```bash
dotnet restore
```

4. Veritabanını oluşturun:
```bash
dotnet ef database update
```

5. Projeyi çalıştırın:
```bash
dotnet run
```

6. Tarayıcıda `http://localhost:5000` adresine gidin.

## Güvenlik

- `appsettings.json` dosyası `.gitignore`'a eklenmiştir

## Kullanım

### İlk Kullanım

1. Ana sayfadan "Üye Ol" butonuna tıklayın
2. Kullanıcı adı, e-posta ve şifre girin
3. Kayıt olduktan sonra otomatik giriş yapılır

### Blog Yazısı Oluşturma

1. Sağ üstteki "Yönetim Paneli" linkine tıklayın
2. "Yeni Yazı Ekle" butonuna tıklayın
3. Başlık, özet ve içerik alanlarını doldurun
4. "Kaydet" butonuna tıklayın

### Yorum Yapma

1. Herhangi bir blog yazısının detay sayfasına gidin
2. Sayfanın altındaki yorum formunu doldurun
3. "Yorumu Gönder" butonuna tıklayın

## Proje Yapısı

```
Blog/
├── Controllers/          # Controller sınıfları
├── Models/              # Veri modelleri
├── Views/               # Razor görünümleri
├── Data/                # Veritabanı bağlamı
├── Migrations/          # Veritabanı migration'ları
└── wwwroot/             # Statik dosyalar
```

## Deployment

### Render.com

1. GitHub'a projeyi yükleyin
2. Render.com'da yeni Web Service oluşturun
3. Build Command: `dotnet publish -c Release -o ./publish`
4. Start Command: `dotnet Blog.dll`
5. MySQL veritabanı ekleyin
6. Connection string'i environment variable olarak ayarlayın

