# Güvenlik Notları

## Önemli Güvenlik Uyarıları

### 1. Veritabanı Şifreleri
- `appsettings.json` ve `appsettings.Development.json` dosyaları `.gitignore`'a eklenmiştir
- Bu dosyalar GitHub'a commit edilmemelidir
- Canlı ortamda şifreleri environment variables olarak kullanın

### 2. Şifre Hashleme
- Kullanıcı şifreleri SHA256 ile hashlenmektedir
- Canlı ortamda daha güvenli bir hash algoritması (bcrypt, Argon2) kullanılması önerilir

### 3. SQL Injection Koruması
- Entity Framework Core parametreli sorgular kullanılmaktadır
- Raw SQL sorguları kullanılmamaktadır

### 4. XSS Koruması
- ASP.NET Core otomatik HTML encoding yapmaktadır
- Kullanıcı girdileri güvenli şekilde encode edilmektedir

### 5. CSRF Koruması
- Tüm formlarda `ValidateAntiForgeryToken` kullanılmaktadır
- Cookie-based authentication kullanılmaktadır

### 6. Deployment Önerileri
- Production ortamında HTTPS kullanın
- Connection string'leri environment variables olarak saklayın
- Güvenlik başlıklarını (HSTS, CSP) yapılandırın
- Düzenli güvenlik güncellemeleri yapın

## Güvenlik Kontrol Listesi

- [x] SQL Injection koruması
- [x] XSS koruması
- [x] CSRF koruması
- [x] Şifre hashleme
- [x] Authentication ve Authorization
- [x] Sensitive dosyalar .gitignore'da
- [ ] HTTPS zorunluluğu (production)
- [ ] Rate limiting (önerilir)
- [ ] Input validation (tüm formlar)
- [ ] Error handling (hassas bilgi sızıntısı önleme)

