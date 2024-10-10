# .NET Core 8.0 Çok Dilli Uygulama Apisi

## Özellikler

- **JSON tabanlı dil yönetimi**: Projede dil verileri ayrı JSON dosyalarında saklanmakta, her dil için ilgili JSON dosyası yüklenerek kullanıcılara dinamik içerik sağlanmaktadır.
- **Çoklu dil desteği**: Uygulama, farklı dillerde içerik sunmak için HTTP başlıklarındaki Accept-Language alanını kullanır. Desteklenen dil dosyaları Resources klasörüne eklenir, appsettings.json üzerinden tanımları yapılır.
- **Varsayılan dil desteği**: Kullanıcının talep ettiği dil desteklenmiyorsa, varsayılan dil (örn. Türkçe) devreye girer. Varsayılan dil tanımı appsettings.json üzerinden yapılır.
- **Genişletilebilir dil altyapısı**: Yeni bir dil eklemek için Resources klasörüne yeni bir JSON dosyası ekleyip, appsettings.json'da tanımlanması yeterlidir.
- **Performans**: JSON dosyaları hafif ve hızlı bir şekilde yüklenip işlenir, bir kere yüklenen dil dosyası 1 saat boyunca memory cache üzerinde dictionary olarak tutulur ve gelen istekler performanslı şekilde bu veri setinden karşılanır.

## Nasıl Çalışır

- Kullanıcı, Accept-Language başlığına sahip bir HTTP isteği yapar.
- Sunucu, bu başlığa göre JSON dosyalarını yükler ve ilgili dildeki içeriği döner.
- Eğer başlık belirtilmezse veya desteklenmeyen bir dil istenirse, varsayılan dil (örn. Türkçe) kullanılır.
- Dil dosyaları, Resources klasöründe JSON formatında saklanır ve her bir dil için ayrı dosyalar oluşturulur (örneğin, en.json, tr.json).
