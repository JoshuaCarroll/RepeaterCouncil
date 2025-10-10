using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RepeaterCouncil.Web.Data;
using RepeaterCouncil.Web.Enums;
using RepeaterCouncil.Web.Models;
using System.Data;
using Microsoft.Data.SqlClient;

namespace RepeaterCouncil.Web.Services
{
    /// <summary>
    /// Service to handle migration from legacy RepeaterCouncil database to new multi-tenant structure
    /// </summary>
    public class LegacyDataMigrationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<LegacyDataMigrationService> _logger;
        private readonly string _legacyConnectionString;

        // Legacy data structures for mapping
        public class LegacyRepeater
        {
            public int ID { get; set; }
            public byte Type { get; set; }
            public string Callsign { get; set; } = string.Empty;
            public int? TrusteeID { get; set; }
            public byte Status { get; set; }
            public string? City { get; set; }
            public string? SiteName { get; set; }
            public decimal OutputFrequency { get; set; }
            public decimal? InputFrequency { get; set; }
            public decimal? _Latitude { get; set; }
            public decimal? _Longitude { get; set; }
            public decimal? AMSL { get; set; }
            public int? OutputPower { get; set; }
            public decimal? ERP { get; set; }
            public decimal? AntennaGain { get; set; }
            public decimal? AntennaHeight { get; set; }
            public string? Analog_InputAccess { get; set; }
            public string? Analog_OutputAccess { get; set; }
            public decimal? Analog_Width { get; set; }
            public DateTime? DateCoordinated { get; set; }
            public DateTime? DateUpdated { get; set; }
            public DateTime? DateDecoordinated { get; set; }
            public string? State { get; set; }
            public string? CoordinatorComments { get; set; }
            public string? Notes { get; set; }
        }

        public class LegacyUser
        {
            public int ID { get; set; }
            public string Callsign { get; set; } = string.Empty;
            public string? FullName { get; set; }
            public string? Email { get; set; }
            public string? PhoneHome { get; set; }
            public bool SK { get; set; }
            public bool LicenseExpired { get; set; }
        }

        public class LegacyLink
        {
            public int ID { get; set; }
            public int LinkFromRepeaterID { get; set; }
            public int LinkToRepeaterID { get; set; }
            public int? LinkTypeID { get; set; }
            public string? Comment { get; set; }
        }

        public class LegacyRepeaterChangeLog
        {
            public int ID { get; set; }
            public int RepeaterId { get; set; }
            public int UserId { get; set; }
            public DateTime ChangeDateTime { get; set; }
            public string ChangeDescription { get; set; } = string.Empty;
        }

        public LegacyDataMigrationService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<LegacyDataMigrationService> logger,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
            _legacyConnectionString = configuration.GetConnectionString("LegacyRepeaterData")
                ?? throw new ArgumentException("LegacyRepeaterData connection string not found");
        }

        /// <summary>
        /// Performs complete migration from legacy database
        /// </summary>
        /// <param name="tenantMappings">Dictionary mapping legacy state abbreviations to tenant URLs</param>
        public async Task<MigrationResult> MigrateDataAsync(Dictionary<string, string> tenantMappings)
        {
            var result = new MigrationResult();

            try
            {
                _logger.LogInformation("Starting legacy data migration");

                // Step 1: Create tenants
                result.TenantsCreated = await CreateTenantsAsync(tenantMappings);

                // Step 2: Migrate users
                var userMapping = await MigrateUsersAsync();
                result.UsersCreated = userMapping.Count;

                // Step 3: Migrate repeaters
                var repeaterMapping = await MigrateRepeatersAsync(tenantMappings, userMapping);
                result.RepeatersCreated = repeaterMapping.Count;

                // Step 4: Migrate links
                result.LinksCreated = await MigrateLinksAsync(repeaterMapping);

                // Step 5: Migrate repeater notes
                result.NotesCreated = await MigrateRepeaterNotesAsync(repeaterMapping, userMapping);

                // Step 6: Create default coordination rules
                await CreateDefaultCoordinationRulesAsync();

                result.Success = true;
                _logger.LogInformation("Legacy data migration completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during legacy data migration");
                result.Success = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        private async Task<int> CreateTenantsAsync(Dictionary<string, string> tenantMappings)
        {
            var created = 0;
            var updated = 0;

            foreach (var mapping in tenantMappings)
            {
                var existingTenant = await _context.Tenants
                    .FirstOrDefaultAsync(t => t.Url == mapping.Value);

                if (existingTenant == null)
                {
                    var stateName = GetFullStateName(mapping.Key);
                    var tenant = new Tenant
                    {
                        Name = $"{stateName} Repeater Council",
                        Url = mapping.Value,
                        AboutUsContent = $"Welcome to the {stateName} Repeater Council coordination system."
                    };

                    _context.Tenants.Add(tenant);
                    created++;
                    _logger.LogInformation($"Created tenant for {stateName} ({mapping.Key})");
                }
                else
                {
                    // Update existing tenant if needed
                    var stateName = GetFullStateName(mapping.Key);
                    var expectedName = $"{stateName} Repeater Council";
                    if (existingTenant.Name != expectedName)
                    {
                        existingTenant.Name = expectedName;
                        existingTenant.AboutUsContent = $"Welcome to the {stateName} Repeater Council coordination system.";
                        updated++;
                        _logger.LogInformation($"Updated tenant for {stateName} ({mapping.Key})");
                    }
                }
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation($"Tenant creation completed: {created} created, {updated} updated");
            return created;
        }

        private string GetFullStateName(string stateAbbreviation)
        {
            var stateNames = new Dictionary<string, string>
            {
                { "AL", "Alabama" }, { "AK", "Alaska" }, { "AZ", "Arizona" }, { "AR", "Arkansas" },
                { "CA", "California" }, { "CO", "Colorado" }, { "CT", "Connecticut" }, { "DE", "Delaware" },
                { "FL", "Florida" }, { "GA", "Georgia" }, { "HI", "Hawaii" }, { "ID", "Idaho" },
                { "IL", "Illinois" }, { "IN", "Indiana" }, { "IA", "Iowa" }, { "KS", "Kansas" },
                { "KY", "Kentucky" }, { "LA", "Louisiana" }, { "ME", "Maine" }, { "MD", "Maryland" },
                { "MA", "Massachusetts" }, { "MI", "Michigan" }, { "MN", "Minnesota" }, { "MS", "Mississippi" },
                { "MO", "Missouri" }, { "MT", "Montana" }, { "NE", "Nebraska" }, { "NV", "Nevada" },
                { "NH", "New Hampshire" }, { "NJ", "New Jersey" }, { "NM", "New Mexico" }, { "NY", "New York" },
                { "NC", "North Carolina" }, { "ND", "North Dakota" }, { "OH", "Ohio" }, { "OK", "Oklahoma" },
                { "OR", "Oregon" }, { "PA", "Pennsylvania" }, { "RI", "Rhode Island" }, { "SC", "South Carolina" },
                { "SD", "South Dakota" }, { "TN", "Tennessee" }, { "TX", "Texas" }, { "UT", "Utah" },
                { "VT", "Vermont" }, { "VA", "Virginia" }, { "WA", "Washington" }, { "WV", "West Virginia" },
                { "WI", "Wisconsin" }, { "WY", "Wyoming" }, { "DC", "District of Columbia" }
            };

            return stateNames.ContainsKey(stateAbbreviation)
                ? stateNames[stateAbbreviation]
                : stateAbbreviation;
        }

        private async Task<Dictionary<int, string>> MigrateUsersAsync()
        {
            var userMapping = new Dictionary<int, string>();
            var created = 0;
            var updated = 0;
            var skipped = 0;

            using var connection = new SqlConnection(_legacyConnectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(@"
                SELECT ID, Callsign, FullName, Email, PhoneHome, SK, LicenseExpired 
                FROM Users 
                WHERE SK = 0 AND LicenseExpired = 0", connection);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var legacyUser = new LegacyUser
                {
                    ID = reader.GetInt32("ID"),
                    Callsign = reader.GetString("Callsign"),
                    FullName = reader.IsDBNull("FullName") ? null : reader.GetString("FullName"),
                    Email = reader.IsDBNull("Email") ? null : reader.GetString("Email"),
                    PhoneHome = reader.IsDBNull("PhoneHome") ? null : reader.GetString("PhoneHome")
                };

                // Check if user already exists
                var existingUser = await _userManager.FindByNameAsync(legacyUser.Callsign);
                if (existingUser != null)
                {
                    // Update existing user with latest information from legacy system
                    bool needsUpdate = false;

                    if (existingUser.FullName != (legacyUser.FullName ?? legacyUser.Callsign))
                    {
                        existingUser.FullName = legacyUser.FullName ?? legacyUser.Callsign;
                        needsUpdate = true;
                    }

                    if (!string.IsNullOrEmpty(legacyUser.Email) && existingUser.Email != legacyUser.Email)
                    {
                        existingUser.Email = legacyUser.Email;
                        existingUser.EmailConfirmed = true; // Assume legacy emails are valid
                        needsUpdate = true;
                    }

                    if (existingUser.PhoneNumber != legacyUser.PhoneHome)
                    {
                        existingUser.PhoneNumber = legacyUser.PhoneHome;
                        needsUpdate = true;
                    }

                    if (needsUpdate)
                    {
                        await _userManager.UpdateAsync(existingUser);
                        updated++;
                        _logger.LogInformation($"Updated user: {existingUser.Callsign}");
                    }
                    else
                    {
                        skipped++;
                    }

                    userMapping[legacyUser.ID] = existingUser.Id;
                    continue;
                }

                // Create new user
                var newUser = new ApplicationUser
                {
                    UserName = legacyUser.Callsign,
                    Email = legacyUser.Email ?? $"{legacyUser.Callsign}@example.com",
                    Callsign = legacyUser.Callsign,
                    FullName = legacyUser.FullName ?? legacyUser.Callsign,
                    PhoneNumber = legacyUser.PhoneHome,
                    EmailConfirmed = !string.IsNullOrEmpty(legacyUser.Email)
                };

                var result = await _userManager.CreateAsync(newUser);
                if (result.Succeeded)
                {
                    userMapping[legacyUser.ID] = newUser.Id;
                    created++;
                    _logger.LogInformation($"Created user: {newUser.Callsign}");
                }
                else
                {
                    _logger.LogError($"Failed to create user {legacyUser.Callsign}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }

            _logger.LogInformation($"User migration completed: {created} created, {updated} updated, {skipped} skipped");
            return userMapping;
        }

        private async Task<Dictionary<int, int>> MigrateRepeatersAsync(
            Dictionary<string, string> tenantMappings,
            Dictionary<int, string> userMapping)
        {
            var repeaterMapping = new Dictionary<int, int>();

            // Get tenant mappings
            var tenants = await _context.Tenants.ToListAsync();
            var tenantLookup = new Dictionary<string, int>();

            foreach (var tenant in tenants)
            {
                var state = tenantMappings.FirstOrDefault(tm => tm.Value == tenant.Url).Key;
                if (!string.IsNullOrEmpty(state))
                {
                    tenantLookup[state] = tenant.Id;
                }
            }

            using var connection = new SqlConnection(_legacyConnectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(@"
                SELECT ID, Type, Callsign, TrusteeID, Status, City, SiteName, 
                       OutputFrequency, InputFrequency, _Latitude, _Longitude, AMSL,
                       OutputPower, ERP, AntennaGain, AntennaHeight, 
                       Analog_InputAccess, Analog_OutputAccess, Analog_Width,
                       DateCoordinated, DateUpdated, DateDecoordinated, State,
                       CoordinatorComments, Notes
                FROM Repeaters", connection);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var legacyRepeater = new LegacyRepeater
                {
                    ID = reader.GetInt32("ID"),
                    Type = reader.GetByte("Type"),
                    Callsign = reader.GetString("Callsign"),
                    TrusteeID = reader.IsDBNull("TrusteeID") ? null : reader.GetInt32("TrusteeID"),
                    Status = reader.GetByte("Status"),
                    City = reader.IsDBNull("City") ? null : reader.GetString("City"),
                    SiteName = reader.IsDBNull("SiteName") ? null : reader.GetString("SiteName"),
                    OutputFrequency = reader.GetDecimal("OutputFrequency"),
                    InputFrequency = reader.IsDBNull("InputFrequency") ? null : reader.GetDecimal("InputFrequency"),
                    _Latitude = reader.IsDBNull("_Latitude") ? null : reader.GetDecimal("_Latitude"),
                    _Longitude = reader.IsDBNull("_Longitude") ? null : reader.GetDecimal("_Longitude"),
                    AMSL = reader.IsDBNull("AMSL") ? null : reader.GetDecimal("AMSL"),
                    OutputPower = reader.IsDBNull("OutputPower") ? null : reader.GetInt32("OutputPower"),
                    ERP = reader.IsDBNull("ERP") ? null : reader.GetDecimal("ERP"),
                    AntennaGain = reader.IsDBNull("AntennaGain") ? null : reader.GetDecimal("AntennaGain"),
                    AntennaHeight = reader.IsDBNull("AntennaHeight") ? null : reader.GetDecimal("AntennaHeight"),
                    Analog_InputAccess = reader.IsDBNull("Analog_InputAccess") ? null : reader.GetString("Analog_InputAccess"),
                    Analog_OutputAccess = reader.IsDBNull("Analog_OutputAccess") ? null : reader.GetString("Analog_OutputAccess"),
                    Analog_Width = reader.IsDBNull("Analog_Width") ? null : reader.GetDecimal("Analog_Width"),
                    DateCoordinated = reader.IsDBNull("DateCoordinated") ? null : reader.GetDateTime("DateCoordinated"),
                    DateUpdated = reader.IsDBNull("DateUpdated") ? null : reader.GetDateTime("DateUpdated"),
                    DateDecoordinated = reader.IsDBNull("DateDecoordinated") ? null : reader.GetDateTime("DateDecoordinated"),
                    State = reader.IsDBNull("State") ? null : reader.GetString("State")
                };

                // Skip if we don't have a tenant mapping for this state
                if (string.IsNullOrEmpty(legacyRepeater.State) || !tenantLookup.ContainsKey(legacyRepeater.State))
                {
                    _logger.LogWarning($"Skipping repeater {legacyRepeater.Callsign} - no tenant mapping for state {legacyRepeater.State}");
                    continue;
                }

                var tenantId = tenantLookup[legacyRepeater.State];

                // Check if repeater already exists (by callsign and tenant)
                var existingRepeater = await _context.Repeaters
                    .FirstOrDefaultAsync(r => r.Callsign == legacyRepeater.Callsign && r.TenantId == tenantId);

                if (existingRepeater != null)
                {
                    // Update existing repeater with latest legacy data
                    UpdateRepeaterFromLegacy(existingRepeater, legacyRepeater, userMapping);
                    repeaterMapping[legacyRepeater.ID] = existingRepeater.Id;
                    _logger.LogInformation($"Updated existing repeater: {legacyRepeater.Callsign}");
                    continue;
                }

                // Create new repeater
                var newRepeater = new Repeater
                {
                    TenantId = tenantId,
                    Callsign = legacyRepeater.Callsign,
                    TrusteeId = legacyRepeater.TrusteeID.HasValue && userMapping.ContainsKey(legacyRepeater.TrusteeID.Value)
                        ? userMapping[legacyRepeater.TrusteeID.Value] : null,
                    Type = MapRepeaterType(legacyRepeater.Type),
                    Status = MapRepeaterStatus(legacyRepeater.Status),
                    City = legacyRepeater.City ?? string.Empty,
                    SiteDescription = legacyRepeater.SiteName ?? string.Empty,
                    Latitude = (double)(legacyRepeater._Latitude ?? 0),
                    Longitude = (double)(legacyRepeater._Longitude ?? 0),
                    AltitudeMeters = (double)(legacyRepeater.AMSL ?? 0),
                    OutputPowerWatts = legacyRepeater.OutputPower ?? 0,
                    EffectiveRadiatedPower = (double)(legacyRepeater.ERP ?? 0),
                    AntennaGain = (double)(legacyRepeater.AntennaGain ?? 0),
                    AntennaHeightMeters = (double)(legacyRepeater.AntennaHeight ?? 0),
                    TransmitFreq = (double)legacyRepeater.OutputFrequency,
                    ReceiveFreq = (double)(legacyRepeater.InputFrequency ?? legacyRepeater.OutputFrequency),
                    InputToneType = ToneSquelchType.None, // Default - could be enhanced to parse legacy tone data
                    OutputToneType = ToneSquelchType.None, // Default - could be enhanced to parse legacy tone data
                    InputToneValue = ParseToneValue(legacyRepeater.Analog_InputAccess),
                    OutputToneValue = ParseToneValue(legacyRepeater.Analog_OutputAccess),
                    AnalogBandwidth = legacyRepeater.Analog_Width?.ToString() ?? string.Empty,
                    DateCoordinated = legacyRepeater.DateCoordinated ?? DateTime.UtcNow,
                    DateUpdated = legacyRepeater.DateUpdated,
                    DateDecoordinated = legacyRepeater.DateDecoordinated
                };

                _context.Repeaters.Add(newRepeater);
                await _context.SaveChangesAsync();

                repeaterMapping[legacyRepeater.ID] = newRepeater.Id;
                _logger.LogInformation($"Migrated repeater: {newRepeater.Callsign}");
            }

            return repeaterMapping;
        }

        private async Task<int> MigrateLinksAsync(Dictionary<int, int> repeaterMapping)
        {
            var created = 0;

            using var connection = new SqlConnection(_legacyConnectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(@"
                SELECT ID, LinkFromRepeaterID, LinkToRepeaterID, LinkTypeID, Comment 
                FROM Links", connection);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var legacyLink = new LegacyLink
                {
                    ID = reader.GetInt32("ID"),
                    LinkFromRepeaterID = reader.GetInt32("LinkFromRepeaterID"),
                    LinkToRepeaterID = reader.GetInt32("LinkToRepeaterID"),
                    LinkTypeID = reader.IsDBNull("LinkTypeID") ? null : reader.GetInt32("LinkTypeID"),
                    Comment = reader.IsDBNull("Comment") ? null : reader.GetString("Comment")
                };

                // Skip if repeaters aren't mapped
                if (!repeaterMapping.ContainsKey(legacyLink.LinkFromRepeaterID) ||
                    !repeaterMapping.ContainsKey(legacyLink.LinkToRepeaterID))
                {
                    continue;
                }

                var repeaterId = repeaterMapping[legacyLink.LinkFromRepeaterID];
                var linkedRepeaterId = repeaterMapping[legacyLink.LinkToRepeaterID];

                // Check if link already exists
                var existingLink = await _context.Links
                    .FirstOrDefaultAsync(l => l.RepeaterId == repeaterId && l.LinkedRepeaterId == linkedRepeaterId);

                if (existingLink != null)
                {
                    // Update existing link
                    existingLink.LinkType = MapLinkType(legacyLink.LinkTypeID);
                    existingLink.LinkDetails = legacyLink.Comment ?? string.Empty;
                    _logger.LogInformation($"Updated existing link between repeaters {repeaterId} and {linkedRepeaterId}");
                    continue;
                }

                // Create new link
                var newLink = new Link
                {
                    RepeaterId = repeaterId,
                    LinkedRepeaterId = linkedRepeaterId,
                    LinkType = MapLinkType(legacyLink.LinkTypeID),
                    LinkDetails = legacyLink.Comment ?? string.Empty
                };

                _context.Links.Add(newLink);
                created++;
            }

            await _context.SaveChangesAsync();
            return created;
        }

        private async Task<int> MigrateRepeaterNotesAsync(
            Dictionary<int, int> repeaterMapping,
            Dictionary<int, string> userMapping)
        {
            var created = 0;

            using var connection = new SqlConnection(_legacyConnectionString);
            await connection.OpenAsync();

            var command = new SqlCommand(@"
                SELECT ID, RepeaterId, UserId, ChangeDateTime, ChangeDescription 
                FROM RepeaterChangeLogs", connection);

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var legacyLog = new LegacyRepeaterChangeLog
                {
                    ID = reader.GetInt32("ID"),
                    RepeaterId = reader.GetInt32("RepeaterId"),
                    UserId = reader.GetInt32("UserId"),
                    ChangeDateTime = reader.GetDateTime("ChangeDateTime"),
                    ChangeDescription = reader.GetString("ChangeDescription")
                };

                // Skip if repeater or user isn't mapped
                if (!repeaterMapping.ContainsKey(legacyLog.RepeaterId) ||
                    !userMapping.ContainsKey(legacyLog.UserId))
                {
                    continue;
                }

                var repeaterId = repeaterMapping[legacyLog.RepeaterId];
                var userId = userMapping[legacyLog.UserId];

                // Check if note already exists (same repeater, user, timestamp, and content)
                var existingNote = await _context.RepeaterNotes
                    .FirstOrDefaultAsync(n => n.RepeaterId == repeaterId
                                           && n.UserId == userId
                                           && n.CreatedAt == legacyLog.ChangeDateTime
                                           && n.Note == legacyLog.ChangeDescription);

                if (existingNote != null)
                {
                    // Note already exists, skip it
                    continue;
                }

                // Create new note
                var newNote = new RepeaterNote
                {
                    RepeaterId = repeaterId,
                    UserId = userId,
                    CreatedAt = legacyLog.ChangeDateTime,
                    Note = legacyLog.ChangeDescription
                };

                _context.RepeaterNotes.Add(newNote);
                created++;
            }

            await _context.SaveChangesAsync();
            return created;
        }

        private async Task CreateDefaultCoordinationRulesAsync()
        {
            var tenants = await _context.Tenants.ToListAsync();

            foreach (var tenant in tenants)
            {
                // Check if rules already exist
                var existingRules = await _context.CoordinationRules
                    .AnyAsync(cr => cr.TenantId == tenant.Id);

                if (!existingRules)
                {
                    // Create default 2m rules
                    var rule2m = new CoordinationRule
                    {
                        TenantId = tenant.Id,
                        FrequencyStart = 144.000,
                        FrequencyEnd = 148.000,
                        SpacingMHz = 0.6, // Default 600kHz spacing for 2m
                        SeparationMiles = 50
                    };

                    // Create default 70cm rules  
                    var rule70cm = new CoordinationRule
                    {
                        TenantId = tenant.Id,
                        FrequencyStart = 420.000,
                        FrequencyEnd = 450.000,
                        SpacingMHz = 5.0, // Default 5MHz spacing for 70cm
                        SeparationMiles = 30
                    };

                    _context.CoordinationRules.AddRange(rule2m, rule70cm);
                }
            }

            await _context.SaveChangesAsync();
        }

        // Helper methods for enum mapping
        private RepeaterType MapRepeaterType(byte legacyType)
        {
            return legacyType switch
            {
                1 => RepeaterType.Repeater,
                2 => RepeaterType.Link,
                3 => RepeaterType.Control,
                4 => RepeaterType.Packet,
                5 => RepeaterType.Beacon,
                6 => RepeaterType.AmateurTv,
                7 => RepeaterType.RemoteBase,
                8 => RepeaterType.ClosedRepeater,
                9 => RepeaterType.None,
                _ => RepeaterType.Other
            };
        }

        private RepeaterStatus MapRepeaterStatus(byte legacyStatus)
        {
            return legacyStatus switch
            {
                1 => RepeaterStatus.Proposed,
                2 => RepeaterStatus.UnderConstruction,
                3 => RepeaterStatus.Operational,
                4 => RepeaterStatus.TemporarilyOffline,
                5 => RepeaterStatus.SuspectedOffline,
                6 => RepeaterStatus.Decoordinated,
                _ => RepeaterStatus.Proposed
            };
        }

        private LinkType MapLinkType(int? legacyLinkType)
        {
            if (!legacyLinkType.HasValue) return LinkType.None;

            return legacyLinkType.Value switch
            {
                1 => LinkType.RadioRf,
                2 => LinkType.Allstar,
                3 => LinkType.Echolink,
                4 => LinkType.DStar,
                5 => LinkType.DMR,
                6 => LinkType.P25,
                7 => LinkType.NXDN,
                8 => LinkType.YaesuSystemFusion,
                9 => LinkType.HamshackHotline,
                10 => LinkType.HamsOverIp,
                11 => LinkType.Broadcastify,
                _ => LinkType.Other
            };
        }

        private void UpdateRepeaterFromLegacy(Repeater existingRepeater, LegacyRepeater legacyRepeater, Dictionary<int, string> userMapping)
        {
            // Update all fields with latest data from legacy system
            existingRepeater.TrusteeId = legacyRepeater.TrusteeID.HasValue && userMapping.ContainsKey(legacyRepeater.TrusteeID.Value)
                ? userMapping[legacyRepeater.TrusteeID.Value] : null;
            existingRepeater.Type = MapRepeaterType(legacyRepeater.Type);
            existingRepeater.Status = MapRepeaterStatus(legacyRepeater.Status);
            existingRepeater.City = legacyRepeater.City ?? string.Empty;
            existingRepeater.SiteDescription = legacyRepeater.SiteName ?? string.Empty;
            existingRepeater.Latitude = (double)(legacyRepeater._Latitude ?? 0);
            existingRepeater.Longitude = (double)(legacyRepeater._Longitude ?? 0);
            existingRepeater.AltitudeMeters = (double)(legacyRepeater.AMSL ?? 0);
            existingRepeater.OutputPowerWatts = legacyRepeater.OutputPower ?? 0;
            existingRepeater.EffectiveRadiatedPower = (double)(legacyRepeater.ERP ?? 0);
            existingRepeater.AntennaGain = (double)(legacyRepeater.AntennaGain ?? 0);
            existingRepeater.AntennaHeightMeters = (double)(legacyRepeater.AntennaHeight ?? 0);
            existingRepeater.TransmitFreq = (double)legacyRepeater.OutputFrequency;
            existingRepeater.ReceiveFreq = (double)(legacyRepeater.InputFrequency ?? legacyRepeater.OutputFrequency);
            existingRepeater.InputToneType = ToneSquelchType.None; // Default - could be enhanced
            existingRepeater.OutputToneType = ToneSquelchType.None; // Default - could be enhanced
            existingRepeater.InputToneValue = ParseToneValue(legacyRepeater.Analog_InputAccess);
            existingRepeater.OutputToneValue = ParseToneValue(legacyRepeater.Analog_OutputAccess);
            existingRepeater.AnalogBandwidth = legacyRepeater.Analog_Width?.ToString() ?? string.Empty;
            existingRepeater.DateCoordinated = legacyRepeater.DateCoordinated ?? existingRepeater.DateCoordinated;
            existingRepeater.DateUpdated = DateTime.UtcNow; // Mark as updated now
            existingRepeater.DateDecoordinated = legacyRepeater.DateDecoordinated;
        }

        private double? ParseToneValue(string? toneString)
        {
            if (string.IsNullOrEmpty(toneString)) return null;

            if (double.TryParse(toneString, out double value))
            {
                return value;
            }

            return null;
        }
    }

    public class MigrationResult
    {
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int TenantsCreated { get; set; }
        public int UsersCreated { get; set; }
        public int RepeatersCreated { get; set; }
        public int LinksCreated { get; set; }
        public int NotesCreated { get; set; }

        public override string ToString()
        {
            if (!Success)
                return $"Migration failed: {ErrorMessage}";

            return $"Migration completed successfully:\n" +
                   $"- {TenantsCreated} tenants created\n" +
                   $"- {UsersCreated} users migrated\n" +
                   $"- {RepeatersCreated} repeaters migrated\n" +
                   $"- {LinksCreated} links migrated\n" +
                   $"- {NotesCreated} notes migrated";
        }
    }
}