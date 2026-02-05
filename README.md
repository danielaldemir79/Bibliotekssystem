# Bibliotekssystem

Ett konsolbaserat bibliotekssystem utvecklat i C# som hanterar böcker, medlemmar och utlåning.

## Om projektet

Detta projekt är en del av kursen i objektorienterad programmering och demonstrerar:

- Klasser och inkapsling med väldefinierade modeller
- Komposition där Library klassen koordinerar BookCatalog, MemberRegistry och LoanManager
- Interface och polymorfism genom ISearchable
- Algoritmer för sökning, sortering och statistikberäkning

- ## Projektstruktur

**Bibliotekssystem** (huvudprojekt)
- Models: Book, Member, Loan
- Services: BookCatalog, MemberRegistry, LoanManager
- Interfaces: ISearchable
- Helpers: ConsoleHelper, InputHelper, MenuHandler
- Library.cs och Program.cs

**Biblioteksystem.Tests** (testprojekt)
- Models: BookTests, MemberTests, LoanTests
- Services: BookCatalogTests, MemberRegistryTests, LoanManagerTests
- LibraryTests

## Funktioner

### Bokhantering
- Lägg till och ta bort böcker
- Sök böcker på titel, författare, ISBN eller årtal
- Sortera böcker efter titel, författare eller utgivningsår
- Visa tillgängliga böcker

### Medlemshantering
- Registrera och ta bort medlemmar
- Sök medlemmar på namn, ID eller e-post
- Visa medlemsinformation och lånehistorik

### Lånehantering
- Låna ut böcker till medlemmar
- Returnera böcker
- Visa aktiva lån och försenade lån
- Identifiera mest aktiva låntagare

### Statistik
- Totalt antal böcker
- Antal tillgängliga/utlånade böcker
- Antal medlemmar
- Antal aktiva och försenade lån


## Installation

### Förutsättningar
- .NET 9 SDK
- Visual Studio 2022 eller VS Code

### Klona och bygg
git clone https://github.com/danielaldemir79/Bibliotekssystem.git cd Bibliotekssystem dotnet build


### Exempelkörning
=== Bibliotekssystem ===
1.	Visa alla böcker
2.	Sök bok
3.	Låna bok
4.	Returnera bok
5.	Visa medlemmar
6.	Statistik
7.	Avsluta
Välj: 2 Sökterm: Tolkien
Sökresultat:
1.	"Sagan om ringen" av J.R.R. Tolkien (1954) - Tillgänglig
2.	"Hobbiten" av J.R.R. Tolkien (1937) - Utlånad


## Testning
Projektet innehåller 127 enhetstester med xUnit som täcker modeller, services, integration samt edge cases och negativa tester.

### Kör tester
Passed! - Failed: 0, Passed: 132, Skipped: 0, Total: 132
![Testresultat](screenshot/Tester.png)


## Teknisk dokumentation

### ISearchable Interface
Alla sökbara klasser implementerar ISearchable för enhetlig sökning med case-insensitive matchning:

### Designbeslut
- Komposition över arv för flexibilitet och lösare koppling mellan komponenter
- IReadOnlyList för att exponera listor som readonly och bevara inkapsling
- Nullable return types där metoder kan returnera null för tydlig hantering
- Negativa lånedagar tillåts i CreateLoan för att möjliggöra testning av försenade lån


## Författare
Daniel Aldemir
GitHub: https://github.com/danielaldemir79


