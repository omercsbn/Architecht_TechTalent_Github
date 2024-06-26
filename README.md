# Architecht TechTalent Github

## Finansal Yönetim Web Uygulaması

Bireysel Finans Yönetim Sistemi, kişilerin finans işlemlerini kolaylıkla yönetebilmeleri için tasarlanmış bir full-stack web uygulamasıdır. Sistem, kişisel hesapların yönetimi, masraf kayıtlarının tutulması, para transfer işlemleri ve dış servislerle etkileşim gibi temel finansal işlevleri içerir.

### Kullanılan Teknolojiler

- **Backend:** C#, .NET/.NET Core
- **Frontend:** React, Next.js, Material-UI
- **Veritabanı:** Microsoft SQL Server
- **API:** ASP.NET Core (C#), Entity Framework Core
- **Diğer:** Git, Github

### Özellikler

- Hesap Yönetimi: Hesap oluşturma, güncelleme, silme ve listeleme.
- Masraf Takibi: Harcama kayıtlarının tutulması ve yönetimi.
- Para Transferi: Hesaplar arası para transfer işlemleri.
- Altın Hesapları: Altın hesapları oluşturma, güncelleme ve silme.
- Fatura Yönetimi: Fatura oluşturma, güncelleme ve listeleme.
- Döviz Kurları: Güncel döviz kurlarının çekilmesi ve listelenmesi.
- Firma Kayıtları: Firma kayıtlarının tutulması ve filtrelenmesi.
- İşlem Yönetimi: Finansal işlemlerin oluşturulması, güncellenmesi ve silinmesi.

Bu uygulama, kullanıcıların finansal verilerini kolayca yönetmelerine olanak sağlar ve kullanıcı dostu bir arayüz sunar.


## API Kullanımı

### Account Management

#### Hesapları getir

```http
  GET /api/account
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `api_key` | `string` | **Gerekli**. API anahtarınız. |

#### IBAN ile Hesap getir

```http
  GET /api/account/iban/${iban}
```

| Parametre | Tip     | Açıklama                       |
| :-------- | :------- | :-------------------------------- |
| `iban`      | `string` | **Gerekli**. Çağrılacak hesabın IBAN numarası |

#### Hesabı sil

```http
  DELETE /api/account/${accountID}
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `accountID` | `int` | **Gerekli**. Silinecek hesabın ID'si |

#### Hesap adını güncelle

```http
  PUT /api/account/${accountID}/update-name
```

| Parametre | Tip     | Açıklama                       |
| :-------- | :------- | :-------------------------------- |
| `accountID`      | `int` | **Gerekli**. Güncellenecek hesabın ID'si|
| `name`      | `string` | **Gerekli**.  Yeni hesap adı |

#### Hesap oluştur

```http
  POST /api/account
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `type` | `string` | **Gerekli**. Hesap türü (Deposit, Gold, Checking) |
| `userID` | `Guid` | **Gerekli**. Kullanıcı ID'si |

#### Kullanıcıya ait tüm hesapları getir

```http
  GET /api/account/user/${userID}
```

| Parametre | Tip     | Açıklama                       |
| :-------- | :------- | :-------------------------------- |
| `userID`      | `Guid` | **Gerekli**. Kullanıcı ID'si|

#### Hesap oluştur

```http
  POST /api/account
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `type` | `string` | **Gerekli**. Hesap türü (Deposit, Gold, Checking) |
| `userID` | `Guid` | **Gerekli**. Kullanıcı ID'si |

#### Toplam hesap sayısını getir

```http
  GET /api/account/user/${userID}
```

| Parametre | Tip     | Açıklama                       |
| :-------- | :------- | :-------------------------------- |
| `api_key`      | `string` | **Gerekli**. API anahtarınız.|


### Gold Management

#### Tüm altın kayıtlarını getir

```http
  GET /api/altin
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `api_key` | `string` | **Gerekli**. API anahtarınız. |

#### Altın kaydını getir

```http
  GET /api/altin/${altin_id}
```

| Parametre | Tip     | Açıklama                       |
| :-------- | :------- | :-------------------------------- |
| `altin_id`      | `int` | **Gerekli**. Çağrılacak altın kaydının ID'si |


### Currency Rate Management

#### Tüm döviz kurlarını getir

```http
  GET /api/currencyrate
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `api_key` | `string` | **Gerekli**. API anahtarınız. |


### Firm Management

#### Tüm firmaları getir

```http
  GET /api/firms
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `api_key` | `string` | **Gerekli**. API anahtarınız. |

#### Firmaları filtrele

```http
  POST /api/firms/filter
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `filterModel` | `FilterModel` | **Gerekli**. Filtreleme parametreleri |

### Invoice Management

#### Tüm faturaları getir

```http
  GET /api/invoices
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `userID` | `Guid` | **Gerekli**. Kullanıcı ID'si |

#### Fatura oluştur

```http
  POST /api/invoices
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `invoice` | `Invoice` | **Gerekli**. Fatura bilgileri |

#### Faturayı ödenmiş olarak işaretle

```http
  PUT /api/invoices/${id}/mark-paid
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `id` | `int` | **Gerekli**. Güncellenecek fatura ID'si |

#### Faturayı getir

```http
  GET /api/invoices/${id}
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `id` | `int` | **Gerekli**. Çağrılacak fatura ID'si |

#### Toplam fatura sayısını getir

```http
  GET /api/invoices/total-count
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `api_key` | `string` | **Gerekli**. API anahtarınız. |

#### Kullanıcıya ait fatura sayısını getir

```http
  GET /api/invoices/count
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `userID` | `Guid` | **Gerekli**. Kullanıcı ID'si |

### Transactions Management

#### İşlemleri getir

```http
  GET /api/transactions
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `userID` | `Guid` | **Gerekli**. Kullanıcı ID'si |

#### İşlemi getir

```http
  GET /api/transactions/${id}
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `id` | `int` | **Gerekli**. Çağrılacak işlem ID'si |

#### İşlemleri filtrele

```http
  GET /api/transactions/filter
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `field` | `string` | **Gerekli**. Filtreleme kriteri |
| `value` | `string` | **Gerekli**. Filtreleme kriterinin değeri |

#### İşlem oluştur

```http
  POST /api/transactions
```

| Parametre | Tip     | Açıklama                |
| :-------- | :------- | :------------------------- |
| `transaction` | `Transaction` | **Gerekli**. İşlem bilgileri |


