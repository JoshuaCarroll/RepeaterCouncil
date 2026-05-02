-- Legacy Database Migration Script
-- Migrates data from legacy RepeaterCouncil database to new multi-tenant EF Core structure
-- Target: ASP.NET Core 9.0 with Identity and multi-tenancy

-- ================================================================================
-- MIGRATION OVERVIEW
-- ================================================================================
-- Legacy DB Structure -> New EF Core Structure
-- Users -> AspNetUsers (ApplicationUser)
-- Repeaters -> Repeaters (with TenantId)
-- Links -> Links (simplified structure)
-- RepeaterChangeLogs -> RepeaterNotes
-- Various code tables -> Enums
-- States table -> Tenant records

-- ================================================================================
-- ASSUMPTIONS AND CONFIGURATION
-- ================================================================================
-- 1. Default Tenant Creation
--    Since legacy DB has no tenant concept but uses 'State' field,
--    we'll create tenants based on unique states in the data
-- 2. User Migration to ASP.NET Identity
--    Legacy Users table -> AspNetUsers with generated GUIDs
-- 3. Enum Mappings
--    Legacy code tables -> Enum values (RepeaterType, RepeaterStatus, LinkType)

-- ================================================================================
-- STEP 1: CREATE TENANT MAPPING
-- ================================================================================
-- Create tenants based on states found in legacy data
-- This maps legacy state-based coordination to new tenant system

DECLARE @TenantMappingTable TABLE (
    LegacyState VARCHAR(10),
    TenantId INT,
    TenantName VARCHAR(255),
    TenantUrl VARCHAR(255)
);

-- Insert tenant mapping (customize these based on your actual requirements)
INSERT INTO @TenantMappingTable (LegacyState, TenantName, TenantUrl) VALUES
('Arkansas', 'Arkansas Repeater Council', 'arkansasrepeatercouncil.org'),
('Alabama', 'Alabama Repeater Council', 'alabama.repeatercouncil.org'),
('Mississippi', 'Mississippi Repeater Council', 'mississippi.repeatercouncil.org');
-- Add more states as needed

-- Create tenants in new database
INSERT INTO [dbo].[Tenants] ([Name], [Url], [AboutUsContent])
SELECT 
    TenantName,
    TenantUrl,
    'Welcome to the ' + TenantName + ' coordination system.'
FROM @TenantMappingTable;

-- Update tenant IDs in mapping table
UPDATE tm
SET tm.TenantId = t.Id
FROM @TenantMappingTable tm
INNER JOIN [dbo].[Tenants] t ON tm.TenantUrl = t.Url;

-- ================================================================================
-- STEP 2: MIGRATE USERS TO ASP.NET IDENTITY
-- ================================================================================
-- Create user ID mapping table for reference during repeater migration
DECLARE @UserMappingTable TABLE (
    LegacyUserId INT,
    NewUserId NVARCHAR(450),
    Callsign VARCHAR(10),
    Email VARCHAR(255)
);

-- Generate new GUIDs for users and insert into AspNetUsers
INSERT INTO [dbo].[AspNetUsers] (
    [Id],
    [UserName], 
    [NormalizedUserName],
    [Email],
    [NormalizedEmail],
    [EmailConfirmed],
    [PasswordHash],
    [SecurityStamp],
    [ConcurrencyStamp],
    [PhoneNumber],
    [PhoneNumberConfirmed],
    [TwoFactorEnabled],
    [LockoutEnabled],
    [AccessFailedCount],
    [Callsign],
    [FullName]
)
OUTPUT 
    INSERTED.[Callsign], -- We'll use this to map back to legacy ID
    INSERTED.[Id],
    INSERTED.[Callsign],
    INSERTED.[Email]
INTO @UserMappingTable (Callsign, NewUserId, Callsign, Email)
SELECT 
    NEWID(), -- Generate new GUID for Id
    u.[Callsign], -- UserName
    UPPER(u.[Callsign]), -- NormalizedUserName
    COALESCE(u.[Email], u.[Callsign] + '@example.com'), -- Email (generate if missing)
    UPPER(COALESCE(u.[Email], u.[Callsign] + '@example.com')), -- NormalizedEmail
    CASE WHEN u.[Email] IS NOT NULL THEN 1 ELSE 0 END, -- EmailConfirmed
    NULL, -- PasswordHash (users will need to reset)
    NEWID(), -- SecurityStamp
    NEWID(), -- ConcurrencyStamp
    u.[PhoneHome], -- PhoneNumber
    CASE WHEN u.[PhoneHome] IS NOT NULL THEN 1 ELSE 0 END, -- PhoneNumberConfirmed
    0, -- TwoFactorEnabled
    1, -- LockoutEnabled
    0, -- AccessFailedCount
    u.[Callsign], -- Custom Callsign field
    u.[FullName] -- Custom FullName field
FROM [LegacyDB].[dbo].[Users] u
WHERE u.[SK] = 0 AND u.[LicenseExpired] = 0; -- Only active users

-- Update mapping table with legacy user IDs
UPDATE um
SET um.LegacyUserId = u.[ID]
FROM @UserMappingTable um
INNER JOIN [LegacyDB].[dbo].[Users] u ON um.Callsign = u.[Callsign];

-- ================================================================================
-- STEP 3: CREATE ENUM MAPPING FUNCTIONS
-- ================================================================================
-- Map legacy code table values to new enum values

-- RepeaterType mapping (legacy Type field -> RepeaterType enum)
DECLARE @RepeaterTypeMapping TABLE (LegacyTypeId TINYINT, NewEnumValue INT);
INSERT INTO @RepeaterTypeMapping VALUES 
(1, 1), -- Repeater
(2, 2), -- Link
(3, 3), -- Control
(4, 4), -- Packet
(5, 5), -- Beacon
(6, 6), -- Amateur TV
(7, 7), -- Remote Base
(8, 8), -- Closed (Private)
(9, 9); -- None

-- RepeaterStatus mapping (legacy Status field -> RepeaterStatus enum)
DECLARE @RepeaterStatusMapping TABLE (LegacyStatusId TINYINT, NewEnumValue INT);
INSERT INTO @RepeaterStatusMapping VALUES 
(1, 1), -- Proposed
(2, 2), -- Under Construction
(3, 3), -- Operational
(4, 4), -- Temporarily Offline
(5, 5), -- Suspected Offline
(6, 6); -- Decoordinated

-- LinkType mapping (legacy LinkTypeID -> LinkType enum)
DECLARE @LinkTypeMapping TABLE (LegacyLinkTypeId INT, NewEnumValue INT);
INSERT INTO @LinkTypeMapping VALUES 
(1, 1), -- Radio/RF
(2, 2), -- Allstar
(3, 3), -- Echolink
(4, 4), -- D-Star
(5, 5), -- DMR
(6, 6), -- P25
(7, 7), -- NXDN
(8, 8), -- Yaesu System Fusion
(9, 9), -- Hamshack Hotline
(10, 10), -- Hams Over IP
(11, 11); -- Broadcastify

-- ================================================================================
-- STEP 4: MIGRATE REPEATERS
-- ================================================================================
-- Create repeater ID mapping table
DECLARE @RepeaterMappingTable TABLE (
    LegacyRepeaterId INT,
    NewRepeaterId INT
);

INSERT INTO [dbo].[Repeaters] (
    [TenantId],
    [Callsign],
    [TrusteeId],
    [Type],
    [Status],
    [City],
    [SiteDescription],
    [Latitude],
    [Longitude],
    [AltitudeMeters],
    [OutputPowerWatts],
    [EffectiveRadiatedPower],
    [AntennaGain],
    [AntennaHeightMeters],
    [TransmitFreq],
    [ReceiveFreq],
    [InputToneType],
    [InputToneValue],
    [OutputToneType],
    [OutputToneValue],
    [AnalogBandwidth],
    [DateCoordinated],
    [DateUpdated],
    [DateDecoordinated]
)
OUTPUT 
    INSERTED.[Id]
INTO @RepeaterMappingTable (NewRepeaterId)
SELECT 
    tm.TenantId, -- Map to tenant based on state
    r.[Callsign],
    um.NewUserId, -- Map to new user ID
    COALESCE(rtm.NewEnumValue, 1), -- RepeaterType (default to Repeater if unmapped)
    COALESCE(rsm.NewEnumValue, 1), -- RepeaterStatus (default to Proposed if unmapped)
    r.[City],
    COALESCE(r.[SiteName], ''), -- SiteDescription
    COALESCE(r.[_Latitude], r.[Location].Lat, 0), -- Latitude
    COALESCE(r.[_Longitude], r.[Location].Long, 0), -- Longitude
    COALESCE(r.[AMSL], 0), -- AltitudeMeters (AMSL = Above Mean Sea Level)
    COALESCE(r.[OutputPower], 0), -- OutputPowerWatts
    COALESCE(r.[ERP], 0), -- EffectiveRadiatedPower
    COALESCE(r.[AntennaGain], 0), -- AntennaGain
    COALESCE(r.[AntennaHeight], 0), -- AntennaHeightMeters
    r.[OutputFrequency], -- TransmitFreq
    COALESCE(r.[InputFrequency], r.[OutputFrequency]), -- ReceiveFreq
    1, -- InputToneType (default to None - adjust based on Analog_InputAccess)
    CASE 
        WHEN r.[Analog_InputAccess] IS NOT NULL AND ISNUMERIC(r.[Analog_InputAccess]) = 1 
        THEN CAST(r.[Analog_InputAccess] AS FLOAT)
        ELSE NULL 
    END, -- InputToneValue
    1, -- OutputToneType (default to None - adjust based on Analog_OutputAccess)
    CASE 
        WHEN r.[Analog_OutputAccess] IS NOT NULL AND ISNUMERIC(r.[Analog_OutputAccess]) = 1 
        THEN CAST(r.[Analog_OutputAccess] AS FLOAT)
        ELSE NULL 
    END, -- OutputToneValue
    COALESCE(CAST(r.[Analog_Width] AS VARCHAR), ''), -- AnalogBandwidth
    COALESCE(r.[DateCoordinated], GETDATE()), -- DateCoordinated
    r.[DateUpdated], -- DateUpdated
    r.[DateDecoordinated] -- DateDecoordinated
FROM [LegacyDB].[dbo].[Repeaters] r
LEFT JOIN @TenantMappingTable tm ON r.[State] = tm.LegacyState
LEFT JOIN @UserMappingTable um ON r.[TrusteeID] = um.LegacyUserId
LEFT JOIN @RepeaterTypeMapping rtm ON r.[Type] = rtm.LegacyTypeId
LEFT JOIN @RepeaterStatusMapping rsm ON r.[Status] = rsm.LegacyStatusId
WHERE tm.TenantId IS NOT NULL; -- Only migrate repeaters with valid tenant mapping

-- Update repeater mapping with legacy IDs
-- This requires a cursor since OUTPUT doesn't preserve row order
DECLARE @LegacyId INT, @NewId INT, @RowNum INT = 1;
DECLARE repeater_cursor CURSOR FOR 
SELECT [ID] FROM [LegacyDB].[dbo].[Repeaters] r
INNER JOIN @TenantMappingTable tm ON r.[State] = tm.LegacyState
ORDER BY [ID];

OPEN repeater_cursor;
FETCH NEXT FROM repeater_cursor INTO @LegacyId;

WHILE @@FETCH_STATUS = 0
BEGIN
    SELECT @NewId = NewRepeaterId FROM (
        SELECT NewRepeaterId, ROW_NUMBER() OVER (ORDER BY NewRepeaterId) as rn 
        FROM @RepeaterMappingTable
    ) t WHERE rn = @RowNum;
    
    UPDATE @RepeaterMappingTable 
    SET LegacyRepeaterId = @LegacyId 
    WHERE NewRepeaterId = @NewId;
    
    SET @RowNum = @RowNum + 1;
    FETCH NEXT FROM repeater_cursor INTO @LegacyId;
END;

CLOSE repeater_cursor;
DEALLOCATE repeater_cursor;

-- ================================================================================
-- STEP 5: MIGRATE LINKS
-- ================================================================================
INSERT INTO [dbo].[Links] (
    [RepeaterId],
    [LinkType],
    [LinkDetails],
    [LinkedRepeaterId]
)
SELECT 
    rm1.NewRepeaterId, -- RepeaterId
    COALESCE(ltm.NewEnumValue, 0), -- LinkType (default to None if unmapped)
    COALESCE(l.[Comment], ''), -- LinkDetails
    rm2.NewRepeaterId -- LinkedRepeaterId
FROM [LegacyDB].[dbo].[Links] l
INNER JOIN @RepeaterMappingTable rm1 ON l.[LinkFromRepeaterID] = rm1.LegacyRepeaterId
INNER JOIN @RepeaterMappingTable rm2 ON l.[LinkToRepeaterID] = rm2.LegacyRepeaterId
LEFT JOIN @LinkTypeMapping ltm ON l.[LinkTypeID] = ltm.LegacyLinkTypeId;

-- ================================================================================
-- STEP 6: MIGRATE REPEATER NOTES (from RepeaterChangeLogs)
-- ================================================================================
INSERT INTO [dbo].[RepeaterNotes] (
    [RepeaterId],
    [UserId],
    [Timestamp],
    [Note]
)
SELECT 
    rm.NewRepeaterId, -- RepeaterId
    um.NewUserId, -- UserId
    rcl.[ChangeDateTime], -- Timestamp
    rcl.[ChangeDescription] -- Note
FROM [LegacyDB].[dbo].[RepeaterChangeLogs] rcl
INNER JOIN @RepeaterMappingTable rm ON rcl.[RepeaterId] = rm.LegacyRepeaterId
LEFT JOIN @UserMappingTable um ON rcl.[UserId] = um.LegacyUserId;

-- ================================================================================
-- STEP 7: DATA VALIDATION QUERIES
-- ================================================================================
-- Run these queries after migration to validate data integrity

-- Validate repeater counts
SELECT 
    'Legacy Repeaters' as Source, 
    COUNT(*) as Count 
FROM [LegacyDB].[dbo].[Repeaters]
UNION ALL
SELECT 
    'Migrated Repeaters' as Source, 
    COUNT(*) as Count 
FROM [dbo].[Repeaters];

-- Validate user counts
SELECT 
    'Legacy Active Users' as Source, 
    COUNT(*) as Count 
FROM [LegacyDB].[dbo].[Users] 
WHERE [SK] = 0 AND [LicenseExpired] = 0
UNION ALL
SELECT 
    'Migrated Users' as Source, 
    COUNT(*) as Count 
FROM [dbo].[AspNetUsers];

-- Validate link counts
SELECT 
    'Legacy Links' as Source, 
    COUNT(*) as Count 
FROM [LegacyDB].[dbo].[Links]
UNION ALL
SELECT 
    'Migrated Links' as Source, 
    COUNT(*) as Count 
FROM [dbo].[Links];

-- Check for orphaned records
SELECT 
    'Repeaters without Tenants' as Issue,
    COUNT(*) as Count
FROM [dbo].[Repeaters] r
LEFT JOIN [dbo].[Tenants] t ON r.TenantId = t.Id
WHERE t.Id IS NULL;

SELECT 
    'Repeaters without Trustees' as Issue,
    COUNT(*) as Count
FROM [dbo].[Repeaters] r
LEFT JOIN [dbo].[AspNetUsers] u ON r.TrusteeId = u.Id
WHERE r.TrusteeId IS NOT NULL AND u.Id IS NULL;

-- ================================================================================
-- STEP 8: POST-MIGRATION CLEANUP
-- ================================================================================
-- Create default coordination rules for each tenant
INSERT INTO [dbo].[CoordinationRules] (
    [TenantId],
    [FrequencyStart],
    [FrequencyEnd],
    [SeparationDistanceMiles],
    [IsActive]
)
SELECT 
    t.Id,
    144.000, -- Default 2m start frequency
    148.000, -- Default 2m end frequency
    50, -- Default 50-mile separation
    1 -- Active
FROM [dbo].[Tenants] t;

-- Set up default roles for migrated users (optional)
-- This would require additional role setup based on legacy permissions

PRINT 'Migration completed successfully!';
PRINT 'Please review validation query results and run post-migration tests.';
PRINT 'Users will need to reset their passwords using the forgot password feature.';