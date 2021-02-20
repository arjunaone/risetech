# risetech
Rise tech assessment project.

Merhaba. Projenin çalıştırılabilmesi için bilgisayarınızda apache kafka ve postgresql yüklü olmalıdır.
Tek solution iki web api projesi içermektedir. O yüzden solution multiple startup için ayarlanmalıdır.
.net core 5 kullanılmıştır.

KAFKA

Kafka'nın çalışması için Java Development Kit gereklidir. Öncelikle Oracle'dan JDK'yı indirip kurunuz.

Kafka'nın ana klasörüne giderek command prompt açınız. Windows explorer'da o klasördeyken adres çubuğuna "cmd" yazarak açabilirsiniz.
Ardından kafka ile birlikte gelen ve kafkanın çalışmasını sağlayan zookeeper servisini başlatmak için aşağıdaki komutu command prompt'a giriniz.

.\bin\windows\zookeeper-server-start.bat .\config\zookeeper.properties

Hemen ardından kafkayı da başlatmak için bir command prompt daha açarak aşağıdaki komutu giriniz.

.\bin\windows\kafka-server-start.bat .\config\server.properties

Proje kafka içerisinde iki topic'den yararlanmaktadır.
Bu topic'leri kurmak için bir komut istemi daha açarak aşağıdaki komutları giriniz.

.\bin\windows\kafka-topics.bat --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic ContactTopic

.\bin\windows\kafka-topics.bat --create --zookeeper localhost:2181 --replication-factor 1 --partitions 1 --topic RequestTopic

Artık kafka çalışmaya hazır.

Postgresql

Proje içerisinde iki ayrı servis (web api) için de DbContext sınıf dosyaları bulunmaktadır. Postgresql bağlantı string'leri bu dosyaların içindedir.
Ancak "Password=" kısımlarının karşılıkları boştur. Siz kendi sisteminizde "postgres" kullanıcısı için hangi şifreyi kullanıyorsanız yazınız.
Tabi ki "postgres" kullanıcısını kullanmak zorunda değilsiniz. Dilerseniz string içindeki "User Id=" karşılığını da değiştirerek farklı kullanıcı ile veritabanına bağlanabilirsiniz.

Normalde farklı mikroservisler farklı veritabanlarında çalışabilirken, bu projede iki servis de sadelik amacıyla aynı veritabanını kullanmaktadır.

Bağlantı string'lerinizi hazırladıktan sonra Package Manager Console'u açınız ve her iki servis için de "update-database" komutunu çalıştırınız.
Migration dosyaları proje içindedir.

Artık projeyi çalıştırabilirsiniz.
Proje swagger içerdiğinden açılacak iki ayrı web tarayıcısında api komutlarını kolayca görebilir ve verebilirsiniz.

PersonApi: https://localhost:44363/

Kişi ekleme: /api/Person/add
Json: { "name": "Onur", "surname": "Dökmetaş", "firm": "Rise Tech" }

Kişiye iletişim detayı ekleme /api/Person/contact/add/{userId}
Json: { "type": "tel", "content": "5359852382" }
ÖNEMLİ! type kısmına konum için "location", email için "email", telefon için "tel" yazınız.

Tüm kişileri listeleme /api/Person/list

Kişi detaylarını getirme /api/Person/detail/{userId}

Kişi silme (kişinin iletişim bilgilerini de silmektedir) /api/Person/delete/{userId}

Kişiden iletişim bilgisi silme /api/Person/contact/delete/{contactId}

ReportApi: https://localhost:44312/

Yeni rapor talebi oluşturma /api/Report/request

Raporları listeleme /api/Report/list

Rapor detayını getirme /api/Report/detail/{reportId}
