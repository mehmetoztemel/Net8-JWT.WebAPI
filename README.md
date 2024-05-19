# .NET Core JWT Token Projesi README

Bu proje, .NET Core kullanılarak JWT (JSON Web Token) tabanlı kimlik doğrulama ve yetkilendirme sağlayan bir örnek uygulamadır.

## Kurulum

1. Bu projeyi klonlayın veya indirin.
2. Proje klasörüne gidin.
3. Proje klasöründe terminali açın.
4. Aşağıdaki komutu çalıştırarak projeyi derleyin:

```bash
dotnet build
```

## Kullanım

1. Proje derlendikten sonra, aşağıdaki komutla uygulamayı başlatabilirsiniz:

```bash
dotnet run
```

2. Tarayıcınızda `https://localhost:5001` adresine giderek uygulamaya erişebilirsiniz.

## JWT Token Oluşturma

1. Uygulamaya başladıktan sonra, kullanıcı oluşturun veya mevcut bir kullanıcıyla giriş yapın.
2. Başarılı giriş yaptıktan sonra, bir JWT token alacaksınız.
3. Bu token'ı diğer isteklerde kullanarak yetkilendirme yapabilirsiniz.

## API Endpoint'leri

Bu uygulama aşağıdaki API Endpoint'lerini sağlar:

- **POST /api/auth/register:** Yeni bir kullanıcı kaydı oluşturur.
- **POST /api/auth/login:** Kullanıcı girişi yapar ve JWT token döndürür.

## Katkıda Bulunma

Katkılarınızı memnuniyetle karşılıyoruz! Eğer projeyi geliştirmek isterseniz bir çekme isteği göndermekten çekinmeyin.

## Lisans

Bu proje MIT Lisansı altında lisanslanmıştır. Daha fazla bilgi için [LICENSE](LICENSE) dosyasına bakın.

---

Bu README dosyası, proje kullanıcılarına proje hakkında genel bir anlayış sağlamak ve onları nasıl kullanacaklarını yönlendirmek için tasarlanmıştır. Kendi proje ihtiyaçlarınıza göre özelleştirebilirsiniz.
