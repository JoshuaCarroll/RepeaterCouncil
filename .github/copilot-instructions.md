# RepeaterCouncil Codebase Guide

## Architecture Overview
This is a **multi-tenant ASP.NET Core 9.0 web application** for managing amateur radio repeater coordination. Each tenant represents a different repeater council (e.g., Arkansas Repeater Council).

### Multi-Tenancy Implementation
- **Host-based tenant resolution**: `TenantResolverMiddleware` extracts tenant from domain (e.g., `arkansasrepeatercouncil.org`)
- Tenant data cached for 30 minutes using `IMemoryCache`
- All repeater data is scoped by `TenantId` - always filter queries by current tenant
- If no tenant found, returns 404 with "These are not the droids you're looking for"

### Core Domain Models
- **Repeater**: Main entity with technical specs (frequencies, tones, coordinates, power)
- **Tenant**: Represents repeater councils, owns repeaters and code tables
- **Link**: Represents linked repeater systems (many-to-many via junction table)
- **RepeaterNote**: Timestamped notes attached to repeaters
- **CoordinationRule**: Business rules for frequency coordination

## Key Patterns & Conventions

### Authentication & Authorization
- Uses ASP.NET Core Identity with custom `ApplicationUser`
- Google and Microsoft OAuth providers configured
- Role-based authorization: `SiteAdministrator` and `TenantCoordinator`
- Admin controllers use route prefix: `[Route("Admin/{controller=Home}/{action=Index}/{id?}")]`

### Entity Framework Patterns
- `ApplicationDbContext` extends `IdentityDbContext<ApplicationUser>`
- Uses enum converters for `RepeaterType`, `RepeaterStatus`, `ToneSquelchType`, `LinkType`
- Foreign key relationships use `DeleteBehavior.Restrict` to prevent cascading deletes
- Example: `Link.LinkedRepeater` relationship in `OnModelCreating`

### Validation & Business Logic
- Custom `CallsignAttribute` validates US amateur radio callsigns: `^[AKNW][A-Z]{0,2}[0-9][A-Z]{1,3}$`
- Display attributes on enums for user-friendly names
- Tenant URL validation with regex for domains/subdomains

### External Integrations
- **QRZ.com API**: `QrzAuthService` authenticates callsigns via XML API
- Uses `HttpClient` with DI for external API calls
- SendGrid for email notifications (see `EmailSender`)

## Development Workflow

### Database Operations
```powershell
# Add migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Generate controllers with views
dotnet aspnet-codegenerator controller -name ModelController -m Model -dc ApplicationDbContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries
```

### Project Structure
- `Controllers/`: MVC controllers (admin routes prefixed)
- `Models/`: Domain entities
- `Enums/`: Strongly-typed enumerations
- `Services/`: External API integration (QRZ, SendGrid)
- `Middleware/`: Custom middleware (tenant resolution)
- `Helpers/`: Validation attributes and utilities
- `ViewModels/`: DTOs for views (currently minimal)

## Important Notes

### Tenant Context Access
Always access current tenant via `HttpContext.Items["Tenant"]` in controllers:
```csharp
var tenant = (Tenant)HttpContext.Items["Tenant"];
var repeaters = _context.Repeaters.Where(r => r.TenantId == tenant.Id);
```

### Enum Display Names
Enums use `[Display(Name = "...")]` attributes. Controllers extract display names via reflection for SelectLists.

### Connection Strings
Uses "RepeaterData3" connection string name - check `appsettings.json` configuration.

### Identity Configuration
- Email confirmation disabled: `RequireConfirmedAccount = false`
- Roles seeded via migrations (see `SeedIdentityRoles` migration)

## Common Tasks
- **Adding new models**: Create entity, add DbSet to context, create migration
- **Admin controllers**: Inherit authorization attributes and route prefixes from existing controllers
- **Tenant filtering**: Always scope database queries by current tenant
- **External APIs**: Follow `QrzAuthService` pattern for HTTP client injection and error handling