## MiniPayment 
  * .Net7 ile oluşturuldu, swagger arayüzlü ve solid prensibleriyle yazılmış bir api.
  *  Onine Architecture mimarisi ile oluşturuldu.
  *  ORM aracım: EF, Database: InMemoryDB
  *  Pay, Cancel, Refund, FilterBy isteklerine hizmet vermektedir.
  *  Ödeme isteklerini karşılarken alınan bankaId sine göre hangi banka servislerine yönlendirmek için Factory Design Pattern Kullanıldı.
## Notlar
  * Ödeme isteklerini bankaId sine göre doğru banka servislerine yönlendirmek için Strategy Design Pattern kullanılabilirdi, ancak Strategy tasarım kalıbını çalıştırabilmek için de ilk 
    etapta Factory tasarım kalıbındaki switch leme mantığına ihtiyaç olacağından dolayı direk Factory Design Pattern kullanıldı.
  * Çalışmanın basitliğini korumak için bir çok tablo, sütün, validasyon ve parametre kullanmasından vazgeçtim.
