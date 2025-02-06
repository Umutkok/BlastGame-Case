Good Job Games
 Development Summer Internship Case Study


Bu projede, Unity2D kullanarak Collapse / Blast mekaniğini taklit ettim. Oyuncu,yan yana 2 den fazla şeker varsa bunları patlatabilir ve yeni şekerlerin düşmesini sağlar.Temel amaç, grid tabanlı bir sistem oluşturarak algoritmalar ve oyun mekaniği üzerine çalışmaktı. Ayrıca bir tıkanıklık durumunda devreye girecek SmartShuffle mekaniği ile birlikte yan yana gelen şekerlerin spritlarının değişmesi istenildi.
Bunları yaparken GC(garbage collection), CPU, GPU ve Memory optimizasyonuna dikkat ettim. Update fonksiyonunu proje genelinde sadece MS değerini test etmek için kullanıyoruz bu saye GC değerimiz ciddi ölçüde az. Ikonları değiştirmek veya Shuffle işlemi için kullandığım BFS algoritmaları sadece gridde null olan item varsa devreye sokuyoruz. Sebebi eğer oyuncu bir şekeri patlatırsa BFS i tetiklemesi için. 
Proje geliştirilirken herhangi bir plug in eklenmedi. Animasyonlar basit tutulacağı için DoTwin yerine daha optimize olan Coroutine kullanılarak harekelendirildi.
GC Allocation, Update fonksiyonlarının kaldırılmasıyla frame başına 0.5 MB'tan 0.02 MB'a düşürüldü.
MS değeri ise mobil cihazlarda v-sync kapalı iken 1-2 arasındadır. Son olarak Memory Useage 8x8 grid için 248mb.


 Oyun için geliştirilen mekaniklerin kısa anlatımı:

SmartShuffle 
Time Complexity ile Space Complexity: O(n^2) Nairen aktif edildiği ve tüm grid in iki kez taranması gerektiği için.
Shuffle yapması için griddeki toplam cell sayısı ile gridde ki komşusu olmayan item sayısı eşit olmalı. Bu sayede grid üzerinde hamle kalmadığını anlamış oluyoruz ve shuffle fonksiyonunu çalıştırıyoruz.
Daha sonra gridde ki tüm itemler Dictionary altında renklerine göre listeleniyor ve en fazla elemanı  olan 3 liste belirlediğimiz orana göre azaltılıp Grid in rastgele noktalarından başlatılarak BFS algoritması ile komşulara dağıtılıyor. Kalan liste ve elemanları boş kalan hücrelere yerleştiriliyor.

Singleton Pattern ile Kaynak Yönetimi
ItemFactory, MatchingManager, SpriteHolder gibi sınıflarda Singleton kullanılarak gereksiz instance oluşumu engellendi.

MatchingManager ve IconManager ile Farklı Sprite Oluşturucu
Time Complexity ile Space Complexity: O(n) 
Matching Manager komşu itemlerin bulunduğu cell lerin komşularını bulmamız için BFS algoritmasını kullanır Bu class sayesinde SmartShuffle, FallManager, IconManager gibi mekanikler çalışırken gridin durumu hakkında bilgi sahibi olmamızı sağlar IconManager ise matching manager dan aldığı groupSize bilgisi ile item condition larını ayarlar.

FallManager
Stack (Boş hücreleri saklama) ile Time Complexity: O(n)
Oyuncu grid üzerinde hamle yapar ise While döngüsü ile bir alt hücreye null check yapar eğer null ise bulunduğu item i bir alt hücreye taşır.



NewItem
Time Complexity ile Space Complexity: O(n) 
Sütundaki boşluk sayısını en üst satırdan başlayarak hesaplar ve eksik item sayısı kadar yeni item yaratır. Bu itemler Coroutine ile basit bir animasyon kullanılarak harekelendirildi.


Item ve Null Check Mekanizmaları
Item oyundaki şekerlerimizin ana unsuru diğer script ler ile beraber çalışarak itemlerin spritelarının doğru bir şekilde oluşturuyor. Tek bir prefab ile tüm şekerleri kontrol edebiliyoruz.

Eğer bulunduğu cell input alırsa kendisini destroy edip diğer mekanikleri: fallManager, NewItem, IconManager ı tetikliyor. Bu sayede oyunda eğer herhangi bir item yok olursa grid mekaniklerini devreye sokmuş oluyoruz.
Cell kullanıcadan inputu alırken null ve boolen kontrolü yapıyor. Busayede glitch oluşumu engellendi. fallManager ve MatchingManager'da null kontrolleri (if (cell.item == null)) eklendi. Ayrı olarak Shuffle işlemi sırasında boolen ile kontrol sağlıyoruz.


Grid Yapısı ve Cell 
Cell ve Grid yapısı oldukça basit tutuldu Grid leveData boyunca oluşturulup cell prefablarini yerleştiriyoruz. Cell ler sadece item bilgisi tutar kalan tüm işlemler bu yapının dışında çalışır.
Bu aşamada amaç mekanikleri en etkili bir şekilde geliştirmek olduğu için Aşağıda linkini paylaştığım projeden esinlenmede bulundum. Kod yapısı büyük oranda değişse de bunu söyleme amacım herhangi bir şekilde intihal yaratmamak. Bu yüzden  kaynak belirtmek istedim. github.com/berkaayildiz/toonblast-clone.git

LevelSO ile Level oluşumu
Level lar scriptable objeler ile istenilen büyüklükte seçilerek oluşturuluyor. Aynı zaman belli itemleri istenilen noktalarda LevelData sayesinde önceden belirleyebiliriz. Bu Class lar sadece mekanikleri test etmek için kullanıldığından detaylandırılmadı.

Factory Pattern ile Item Üretimi
Factory design pattern ile ItemType a göre farklı item çeşitleri oluşturulur. ItemFactory'de Dictionary kullanılarak yeni item türleri eklemek kolaylaştırıldı.


Değerlendirdiğiniz için Teşekkürler.
Umut KÖK

