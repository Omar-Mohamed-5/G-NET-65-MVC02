using G_NET_65_MVC02.DAL.Data.DBContexts;
using G_NET_65_MVC02.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Data.DataSeeding
{
    public static class GymDataSeeding
    {
        public static async Task SeedAsync(GymDBContext dBContext, string SeedFilePath, ILogger Logger, CancellationToken ct = default)
        {
            try
            {
                if(!await dBContext.Plans.AnyAsync(ct))
                {
                    var Plans = LoadDataFromJsonFile<Plan>("plans.json", SeedFilePath);
                    if(Plans.Count > 0)
                    {
                        dBContext.Plans.AddRange(Plans);
                        Logger.LogInformation("Seeded {Count} Plans", Plans.Count);
                    }
                }
                if (dBContext.ChangeTracker.HasChanges())
                    await dBContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Gym Data Seeding Faileed");
                throw;
            }
        }


        private static List<T> LoadDataFromJsonFile<T>(string FileName, string FolderPath)
        {
            var FilePath = Path.Combine(FolderPath, FileName);
            if (!File.Exists(FilePath))
                throw new FileNotFoundException($"Seed Data File Not Found: {FilePath}");
            var Date = File.ReadAllText(FilePath);
            var Options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            Options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<T>>(Date, Options) ?? [];
        }
    }
}