using System.IO;
using Newtonsoft.Json;
using Storage.Database.Entities.Products;
using Storage.Database.Enums;
using Storage.Models.Settings;

namespace Storage.Helpers
{
    public static class DimensionHelper
    {
        public static bool CheckDanger(this Product product)
        {
            var dangerZones = JsonConvert.DeserializeObject<AppSettings>(File.ReadAllText("appsettings.json"))!;

            return product.DimensionType switch
            {
                DimensionType.Литры => product.Amount <= dangerZones.DangerZones.Resources.Amount,
                DimensionType.Метры => product.Amount <= dangerZones.DangerZones.Resources.Length,
                DimensionType.Штуки => product.Amount <= dangerZones.DangerZones.Resources.Piece,
                _ => false
            };
        }
    }
}