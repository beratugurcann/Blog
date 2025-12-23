// Sayfa yüklendiğinde form doğrulamalarını başlat
document.addEventListener('DOMContentLoaded', function() {
    const forms = document.querySelectorAll('form');
    
    // Tüm formlar için doğrulama ekle
    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            const requiredFields = form.querySelectorAll('[required]');
            let isValid = true;
            
            // Zorunlu alanları kontrol et
            requiredFields.forEach(field => {
                if (!field.value.trim()) {
                    isValid = false;
                    field.style.borderColor = '#ef4444';
                } else {
                    field.style.borderColor = '';
                }
            });
            
            // Geçersiz form gönderimini engelle
            if (!isValid) {
                e.preventDefault();
                alert('Lütfen tüm zorunlu alanları doldurun.');
            }
        });
    });
});

