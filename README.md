# BoodschappenApp (GroceryApp) - Sprint 4

Een applicatie voor het beheren van boodschappenlijsten met analytics functionaliteit.

## Functionaliteiten

### Eerdere Sprints
- **UC1-UC3:** Tonen van boodschappenlijsten, boodschappenlijstitems en producten
- **UC4:** Kleur van boodschappenlijst aanpassen
- **UC5:** Producten toevoegen aan boodschappenlijst
- **UC6:** Inlogfunctionaliteit voor gebruikers
- **UC7:** Delen boodschappenlijst
- **UC8:** Zoeken producten
- **UC9:** Registratiescherm

### Sprint 4 (Week 5) - Nieuw
- **UC10:** Product aantal aanpassen in boodschappenlijst (plus/min knoppen)
- **UC11:** Meest verkochte producten tonen
- **UC13:** Aantal klanten per product tonen (alleen voor Admin rol)

## Project Structuur

```
├── Grocery.App/           # UI Layer (Views, ViewModels)
├── Grocery.Core/          # Business Logic Layer (Services, Models, Interfaces)
├── Grocery.Core.Data/     # Data Access Layer (Repositories, Database)
├── TestCore/              # Unit Tests (NUnit)
└── .github/workflows/     # CI/CD Pipeline
```

## Development Workflow (GitFlow)

Deze applicatie gebruikt **GitFlow** als branching strategie voor ontwikkeling.

### Branch Structuur

- **`main`** - Productie-klare releases
- **`development`** - Ontwikkel branch
- **`feature/*`** - Nieuwe functionaliteiten (bijv. `feature/UC10-aanpassen-productaantal`)
- **`hotfix/*`** - Kritieke fixes voor productie
- **`release/*`** - Release voorbereiding branches

### Commit Conventies

- **feat:** nieuwe functionaliteit (`feat(UC13): voeg admin rol check toe`)
- **fix:** bug fix (`fix(UC10): voorkom negatieve aantallen`)
- **docs:** documentatie wijzigingen (`docs: update README voor sprint 4`)

## Test Pipeline

- **Automatische tests** bij elke Pull Request naar `main` of `development`
- **GitHub Actions workflow** in `.github/workflows/maui_unit_tests.yaml`
- Test resultaten beschikbaar als artifact in GitHub Actions

### Sprint 4 Tests

```
TestCore/UC13_BoughtProductsServiceTests.cs
├── TC13-U01: Admin rol verificatie
├── TC13-U02: Reguliere gebruiker rol verificatie  
├── TC13-U03: Eén admin in systeem check
├── TC13-U04: Default rol is None
├── TC13-U05: GetBoughtProducts met geldig productId
├── TC13-U06: GetBoughtProducts met ongeldig productId
└── TC13-U07: GetBoughtProducts met meerdere klanten
```

## Technische Stack

- **Framework:** .NET 8 + .NET MAUI
- **UI Pattern:** MVVM (Model-View-ViewModel)
- **Testing:** NUnit + GitHub Actions
- **Dependencies:**
  - CommunityToolkit.Mvvm (8.4.0) - MVVM helpers
  - MAUI.Controls (8.0.100) - Cross-platform UI

## Versioning

Deze sprint: **v1.3.0** (Minor release - nieuwe functionaliteit)

- **Major:** Breaking changes (bijv. database migratie) → v2.0.0
- **Minor:** Nieuwe functionaliteit (bijv. UC10, UC11, UC13) → v1.1.0
- **Patch:** Bug fixes (bijv. hotfix voor crash) → v1.0.1

## Licentie

MIT License
