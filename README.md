# 🧩 MicroInventory – Modular Inventory Management System (Backend Only)

**MicroInventory**, mikroservis mimarisiyle geliştirilmiş, event-driven bir **backend** envanter yönetim sistemidir. Uygulama sadece arka uç (backend) servislerinden oluşur; herhangi bir frontend (web veya mobil arayüz) katmanı bulunmamaktadır.

Sistem; ürünlerin kategoriye göre tanımlanması, personele zimmetlenmesi, stok yönetimi ve kullanıcı yönetimini servisler arası mesajlaşma (Azure Service Bus) ile sağlar.

---

## 🏗️ Kullanılan Teknolojiler

- **.NET 8** – Mikroservis altyapısı
- **PostgreSQL** – Her servisin kendi veritabanı
- **Azure Service Bus** – Servisler arası event tabanlı iletişim
- **GitHub** – Kaynak kod ve versiyon kontrolü
- **CQRS + MediatR** – Komut/sorgu ayrımı (UserService gibi servislerde)
- **JWT Authentication** – UserService içinde kimlik doğrulama (opsiyonel)

---

## 🔧 Servisler ve Görevleri

### 📦 ProductService
- Ürün CRUD işlemleri.
- Dinler:
  - `CategoryCreatedIntegrationEvent`
  - `CategoryUpdatedIntegrationEvent`
  - `CategoryDeletedIntegrationEvent`
- Yayar:
  - `ProductCreatedIntegrationEvent`
  - `ProductUpdatedIntegrationEvent`
  - `ProductDeletedIntegrationEvent`

### 🗂️ CategoryService
- Ürün kategorilerinin yönetimi.
- Yayar:
  - `CategoryCreatedIntegrationEvent`
  - `CategoryUpdatedIntegrationEvent`
  - `CategoryDeletedIntegrationEvent`

### 👤 PersonService
- Ürünleri kullanacak kişilerin yönetimi (login yok).
- Yayar:
  - `PersonCreatedIntegrationEvent`
  - `PersonUpdatedIntegrationEvent`
  - `PersonDeletedIntegrationEvent`

### 🔗 AssignmentService
- Ürünlerin personele zimmetlenmesi veya iade edilmesi.
- Dinler:
  - `ProductCreatedIntegrationEvent`
  - `ProductUpdatedIntegrationEvent`
  - `ProductDeletedIntegrationEvent`
  - `PersonCreatedIntegrationEvent`
  - `PersonUpdatedIntegrationEvent`
  - `PersonDeletedIntegrationEvent`
- Yayar:
  - `AssignmentAddedIntegrationEvent`
  - `AssignmentDeletedIntegrationEvent`

### 🧮 StockService
- Ürün stoklarının takibi.
- Dinler:
  - `AssignmentAddedIntegrationEvent`
  - `AssignmentDeletedIntegrationEvent`
  - `ProductCreatedIntegrationEvent`
  - `ProductUpdatedIntegrationEvent`
  - `ProductDeletedIntegrationEvent`
- Not: Stok seviyesi eşik değerinin altına düşerse otomatik olarak 10 adet artırılır.

### 🔐 UserService
- Sistem yöneticileri için kullanıcı yönetimi.
- JWT tabanlı authentication.
- CQRS ve MediatR kullanır.

---

## 🔄 Servisler Arası Event Akışı

| Gönderen | Event | Dinleyen Servis(ler) | Açıklama |
|----------|-------|----------------------|----------|
| CategoryService | CategoryCreatedIntegrationEvent | ProductService | Yeni kategori bilgisi gönderilir |
| CategoryService | CategoryUpdatedIntegrationEvent | ProductService | Kategori güncellendiğinde bilgi verilir |
| CategoryService | CategoryDeletedIntegrationEvent | ProductService | Kategori silinince ilgili ürünler haberdar edilir |
| ProductService | ProductCreatedIntegrationEvent | AssignmentService, StockService | Yeni ürün oluşturulunca paylaşılır |
| ProductService | ProductUpdatedIntegrationEvent | AssignmentService, StockService | Ürün güncellemesi yayılır |
| ProductService | ProductDeletedIntegrationEvent | AssignmentService, StockService | Ürün silinince diğer servisler haberdar olur |
| PersonService | PersonCreatedIntegrationEvent | AssignmentService | Yeni personel bilgisi paylaşılır |
| PersonService | PersonUpdatedIntegrationEvent | AssignmentService | Personel bilgisi güncellenince yayılır |
| PersonService | PersonDeletedIntegrationEvent | AssignmentService | Personel silinince bilgi verilir |
| AssignmentService | AssignmentAddedIntegrationEvent | StockService | Zimmetleme yapıldığında stok düşürülür |
| AssignmentService | AssignmentDeletedIntegrationEvent | StockService | Zimmet kaldırıldığında stok artırılır |

> 📌 Tüm event adları `IntegrationEvent` ile biter ve servisler arası mesajlaşmayı temsil eder.

---

## 📁 Proje Klasör Yapısı

/services
/ProductService
/CategoryService
/PersonService
/AssignmentService
/StockService
/UserService
/shared
/Contracts
/Events


---

## 🧠 Genel Uygulama Akışı

1. Admin bir kategori oluşturur → `CategoryCreatedIntegrationEvent` → ProductService bu bilgiyi alır.
2. Yeni bir ürün tanımlanır → `ProductCreatedIntegrationEvent` → AssignmentService ve StockService bilgilendirilir.
3. Personel kaydı yapılır → `PersonCreatedIntegrationEvent` → AssignmentService bunu kullanır.
4. Zimmet işlemi yapılır → `AssignmentAddedIntegrationEvent` → StockService stoktan düşer.
5. Zimmet kaldırılır → `AssignmentDeletedIntegrationEvent` → StockService stoku artırır.

---

## 📌 Notlar

- Proje sadece backend tarafını kapsar.
- Frontend (web ya da mobil arayüz) bu yapıya entegre edilmemiştir.
- Geliştirme tamamlanmış, bakım ve ek özellikler ihtiyaç doğdukça planlanacaktır.

---

