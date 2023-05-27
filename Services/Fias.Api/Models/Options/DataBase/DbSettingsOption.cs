using Fias.Api.Enums;

namespace Fias.Api.Models.Options.DataBase
{
    public class DbSettingsOption
    {
        public SupportedDb TypeDb { get; set; }
        public string ConnectionString { get; set; }
    }
}
