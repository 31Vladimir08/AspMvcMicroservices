namespace WebApplication.Models.Settings
{
    public class DbSettings
    {
        public const string DbSettingsKey = "DbSettings";

        public string ConnectionString { get; init; }
        public string IdentityConnectionString { get; init; }
        public int MaxCountElements { get; init; }
    }
}
