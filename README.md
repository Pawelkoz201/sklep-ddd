# Sklep - refaktoryzacja DDD / Clean Architecture

To repozytorium jest osobną, przebudowaną wersją projektu `Pawelkoz201/sklep`.
Stary kod nie został zmieniony. Backend został rozdzielony na warstwy zgodnie z
architekturą portów i adapterów, a model domenowy nie zależy od EF Core, SQLite,
kontrolerów ani adnotacji frameworka.

## Struktura

```text
src/
  Sklep.Domain/          # encje, agregaty, value objecty, reguły biznesowe
  Sklep.Application/     # przypadki użycia, DTO, porty repozytoriów/usług
  Sklep.Infrastructure/  # adaptery EF Core, SQLite, JWT, haszowanie haseł
  Sklep.Api/             # adapter HTTP: kontrolery ASP.NET Core
sklep-frontend/          # frontend React przeniesiony z oryginalnego repo
```

## Co zostało zrefaktoryzowane

- Anemiczne modele z `Models` zostały zastąpione zachowaniowymi encjami:
  `Product`, `Category`, `User`, `Cart`, `Order`.
- Dodano niemutowalne obiekty wartości: `Money`, `EmailAddress`,
  `ProductSnapshot`.
- Logika biznesowa koszyka i zamówień została przeniesiona z kontrolerów do
  modelu domenowego i serwisów aplikacyjnych.
- Agregaty komunikują się przez ID i snapshoty, a nie przez referencje
  nawigacyjne EF, np. `CartItem` ma `ProductId`, a `OrderItem` przechowuje
  snapshot produktu z momentu zakupu.
- Porty repozytoriów i usług technicznych są w `Sklep.Application/Ports`.
- Adaptery EF/JWT/haszowania są w `Sklep.Infrastructure`.
- Kontrolery API są cienkie: przyjmują request, wywołują use case i zwracają
  odpowiedź HTTP.

## Uruchomienie backendu

```bash
dotnet build sklep-clean-architecture.sln
dotnet run --project src/Sklep.Api/Sklep.Api.csproj --urls http://localhost:5000
```

Baza SQLite `sklep-clean.db` jest tworzona automatycznie przy starcie API i
seedowana kategoriami oraz produktami.

Swagger:

```text
http://localhost:5000/swagger
```

## Uruchomienie frontendu

```bash
cd sklep-frontend
npm install
npm run dev
```

Frontend korzysta z API pod adresem `http://localhost:5000/api`, tak jak w
oryginalnym projekcie.

## Szybki test API

```bash
curl http://localhost:5000/api/categories
curl http://localhost:5000/api/products/category/1
```
