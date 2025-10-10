# Legacy Database Migration Guide

This guide covers migrating data from your legacy RepeaterCouncil database to the new multi-tenant ASP.NET Core 9.0 application.

## Overview

The migration process transforms your legacy single-tenant database structure into a multi-tenant system where each repeater council (typically organized by state) becomes a separate tenant.

## Files Included

### Migration Scripts
- `LegacyDataMigration.sql` - Complete SQL migration script
- `LegacyDataMigrationService.cs` - C# service for programmatic migration
- `MigrationController.cs` - Admin web interface for migration
- `appsettings.migration.json` - Sample configuration

### Migration Components
- **Tenant Creation** - Maps legacy states to new tenants
- **User Migration** - Converts legacy users to ASP.NET Identity users  
- **Repeater Migration** - Transfers repeater data with tenant assignments
- **Link Migration** - Preserves repeater linking relationships
- **Notes Migration** - Converts change logs to repeater notes
- **Rule Creation** - Generates default coordination rules

## Pre-Migration Checklist

### 1. Database Backup
```sql
-- Create a full backup of your current database
BACKUP DATABASE [RepeaterCouncil] 
TO DISK = 'C:\Backups\RepeaterCouncil_PreMigration.bak'
```

### 2. Legacy Database Access
Ensure your legacy database is accessible and update the connection string:
```json
{
  "ConnectionStrings": {
    "LegacyRepeaterData": "Server=your-server;Database=legacy_db;Trusted_Connection=true"
  }
}
```

### 3. Review Tenant Mappings
Update the tenant mappings in `MigrationController.cs`:
```csharp
var tenantMappings = new Dictionary<string, string>
{
    { "Arkansas", "arkansasrepeatercouncil.org" },
    { "Alabama", "alabama.repeatercouncil.org" }
    // Add your state/tenant mappings
};
```

## Data Mapping Reference

### Legacy → New Structure

| Legacy Table | New Table/Entity | Notes |
|--------------|------------------|-------|
| Users | AspNetUsers | Identity integration, password reset required |
| Repeaters | Repeaters | Added TenantId, enum conversions |
| Links | Links | Simplified structure, enum LinkType |
| RepeaterChangeLogs | RepeaterNotes | Converted to note format |
| States | Tenants | State coordination → Tenant system |
| RepeaterTypes | RepeaterType (enum) | Code table → Enum |
| RepeaterStatuses | RepeaterStatus (enum) | Code table → Enum |
| LinkTypes | LinkType (enum) | Code table → Enum |

### Field Mappings

#### Repeaters Table
| Legacy Field | New Field | Conversion |
|--------------|-----------|------------|
| Type | Type | Enum mapping via lookup |
| Status | Status | Enum mapping via lookup |
| OutputFrequency | TransmitFreq | Direct copy as double |
| InputFrequency | ReceiveFreq | Default to OutputFreq if null |
| _Latitude | Latitude | Geography → double |
| _Longitude | Longitude | Geography → double |
| AMSL | AltitudeMeters | Direct copy as double |
| OutputPower | OutputPowerWatts | Direct copy |
| ERP | EffectiveRadiatedPower | Direct copy as double |
| AntennaGain | AntennaGain | Direct copy as double |
| AntennaHeight | AntennaHeightMeters | Direct copy as double |
| Analog_InputAccess | InputToneValue | Parse as double if numeric |
| Analog_OutputAccess | OutputToneValue | Parse as double if numeric |
| State | TenantId | Map via tenant lookup |

#### Users Table
| Legacy Field | New Field | Conversion |
|--------------|-----------|------------|
| Callsign | UserName, Callsign | Identity username |
| FullName | FullName | Direct copy |
| Email | Email | Generate if missing |
| PhoneHome | PhoneNumber | Direct copy |
| Password | PasswordHash | Reset required (null) |

## Migration Methods

### Option 1: SQL Script (Recommended for large datasets)
```powershell
# Run the SQL migration script
sqlcmd -S your-server -d RepeaterCouncil -i LegacyDataMigration.sql
```

### Option 2: Web Interface (Recommended for smaller datasets)
1. Start your application
2. Navigate to `/Admin/Migration`
3. Review configuration
4. Click "Start Migration"
5. Monitor progress and results

### Option 3: Programmatic (Custom scenarios)
```csharp
var migrationService = serviceProvider.GetService<LegacyDataMigrationService>();
var tenantMappings = new Dictionary<string, string> { /* your mappings */ };
var result = await migrationService.MigrateDataAsync(tenantMappings);
```

## Post-Migration Tasks

### 1. Data Validation
Run the validation queries included in the SQL script:
```sql
-- Validate record counts
SELECT 'Legacy Repeaters' as Source, COUNT(*) FROM [LegacyDB].[dbo].[Repeaters]
UNION ALL  
SELECT 'Migrated Repeaters', COUNT(*) FROM [dbo].[Repeaters]

-- Check for orphaned records
SELECT COUNT(*) as RepeatersWithoutTenants 
FROM [dbo].[Repeaters] r
LEFT JOIN [dbo].[Tenants] t ON r.TenantId = t.Id
WHERE t.Id IS NULL
```

### 2. User Account Setup
- All migrated users will need to reset their passwords
- Email confirmations may need to be resent
- Admin roles should be assigned manually

### 3. Coordination Rules Review
- Default coordination rules are created for each tenant
- Review and adjust frequency ranges and separation distances
- Add additional rules as needed for your region

### 4. Testing
- Test login functionality for migrated users
- Verify repeater data accuracy
- Check link relationships
- Validate tenant isolation

## Enum Value Mappings

### RepeaterType
| Legacy ID | Legacy Value | New Enum | New Value |
|-----------|--------------|----------|-----------|
| 1 | Repeater | Repeater | 1 |
| 2 | Link | Link | 2 |
| 3 | Control | Control | 3 |
| 4 | Packet | Packet | 4 |
| 5 | Beacon | Beacon | 5 |
| 6 | Amateur TV | AmateurTv | 6 |
| 7 | Remote Base | RemoteBase | 7 |
| 8 | Closed (Private) | ClosedRepeater | 8 |

### RepeaterStatus  
| Legacy ID | Legacy Value | New Enum | New Value |
|-----------|--------------|----------|-----------|
| 1 | Proposed | Proposed | 1 |
| 2 | Under Construction | UnderConstruction | 2 |
| 3 | Operational | Operational | 3 |
| 4 | Temporarily Offline | TemporarilyOffline | 4 |
| 5 | Suspected Offline | SuspectedOffline | 5 |
| 6 | Decoordinated | Decoordinated | 6 |

### LinkType
| Legacy ID | Legacy Value | New Enum | New Value |
|-----------|--------------|----------|-----------|
| 1 | Radio/RF | RadioRf | 1 |
| 2 | Allstar | Allstar | 2 |
| 3 | Echolink | Echolink | 3 |
| 4 | D-Star | DStar | 4 |
| 5 | DMR | DMR | 5 |

## Troubleshooting

### Common Issues

1. **Connection String Errors**
   - Verify legacy database server name and credentials
   - Ensure MultipleActiveResultSets=true is included

2. **Tenant Mapping Issues**
   - Check that all states in legacy data have tenant mappings
   - Verify tenant URLs are unique and valid

3. **User Creation Failures**
   - Review email address formats in legacy data
   - Check for duplicate callsigns

4. **Missing Foreign Keys**
   - Some legacy trustee IDs may not have corresponding users
   - Migration will set TrusteeId to null for missing users

### Migration Rollback
If you need to rollback the migration:
```sql
-- Delete migrated data (be very careful!)
DELETE FROM RepeaterNotes WHERE Id > [last_id_before_migration]
DELETE FROM Links WHERE Id > [last_id_before_migration]  
DELETE FROM Repeaters WHERE Id > [last_id_before_migration]
DELETE FROM AspNetUsers WHERE Id IN (SELECT Id FROM [backup_user_list])
DELETE FROM Tenants WHERE Id > [last_id_before_migration]
```

## Support

For migration issues:
1. Check the application logs for detailed error messages
2. Verify all prerequisites are met
3. Review the validation queries results
4. Check that enum mappings are complete

The migration process preserves all your legacy data while transforming it for the new multi-tenant architecture. Take time to validate the results thoroughly before going live with the new system.