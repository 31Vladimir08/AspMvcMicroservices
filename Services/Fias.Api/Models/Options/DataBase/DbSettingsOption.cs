using Fias.Api.Enums;

namespace Fias.Api.Models.Options.DataBase
{
    public class DbSettingsOption
    {
        public SupportedDb TypeDb { get; init; }
        public string ConnectionString { get; init; }
        public int Buffer { get; init; }
    }
}
