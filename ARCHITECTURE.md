# Notatka architektoniczna

## Leczenie anemicznego modelu domeny

W oryginalnym projekcie klasy `Product`, `Category`, `User`, `Cart` i `Order`
były głównie strukturami danych z publicznymi setterami i właściwościami
nawigacyjnymi EF Core. Po refaktoryzacji encje ukrywają stan przez prywatne
settery i udostępniają metody opisujące zachowania biznesowe.

Przykłady:

- `Product.ReserveStock(quantity)` pilnuje dodatniej ilości i stanu magazynu.
- `Cart.AddItem(productId, quantity, availableStock)` waliduje ilość i łączy
  pozycje tego samego produktu.
- `Cart.ChangeQuantity(...)` pilnuje spójności koszyka.
- `Order.Place(...)` tworzy zamówienie tylko z niepustego koszyka.
- `User.ChangeProfile(...)` i `User.ChangePasswordHash(...)` kontrolują zmianę
  danych użytkownika.

## Value Objecty

- `Money` reprezentuje cenę i blokuje wartości ujemne.
- `EmailAddress` normalizuje adres email i waliduje format.
- `ProductSnapshot` zapisuje dane produktu potrzebne zamówieniu w momencie
  zakupu, bez trzymania referencji do agregatu produktu.

## Agregaty

Agregaty są małe i mają wyraźne root entity:

- `Product` - root produktu; zna `CategoryId`, ale nie ma referencji do
  `Category`.
- `Category` - root kategorii; nie zawiera kolekcji produktów.
- `User` - root użytkownika; nie zawiera koszyka ani zamówień jako obiektów.
- `Cart` - root koszyka; ma pozycje `CartItem`, które przechowują `ProductId`.
- `Order` - root zamówienia; ma pozycje `OrderItem`, które przechowują
  `ProductId`, nazwę i cenę z momentu zakupu.

Dzięki temu agregaty komunikują się przez tożsamość (`Id`) i dane wejściowe,
a nie przez graf obiektów EF.

## Porty i adaptery

Porty są zdefiniowane w `Sklep.Application/Ports`:

- `IProductRepository`
- `ICategoryRepository`
- `IUserRepository`
- `ICartRepository`
- `IOrderRepository`
- `IUnitOfWork`
- `IPasswordHasher`
- `ITokenGenerator`

Adaptery są w `Sklep.Infrastructure`:

- repozytoria `Ef*Repository` używają `SklepDbContext`,
- `AspNetPasswordHasher` opakowuje haszowanie haseł,
- `JwtTokenGenerator` generuje token JWT,
- `DatabaseInitializer` tworzy i seeduje bazę SQLite.

## Zależności

Kierunek zależności:

```text
Sklep.Api -> Sklep.Application -> Sklep.Domain
Sklep.Api -> Sklep.Infrastructure -> Sklep.Application
Sklep.Infrastructure -> Sklep.Domain
```

`Sklep.Domain` nie ma zależności od żadnej technologii. `Sklep.Application`
zna domenę i porty, ale nie zna EF, SQLite ani ASP.NET. Detale techniczne są
w adapterach infrastruktury i HTTP.
