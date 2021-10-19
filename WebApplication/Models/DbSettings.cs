namespace WebApplication.Models
{
    public class DbSettings
    {
        public const string DbSettingsKey = "DbSettings";

        public string ConnectionString { get; set; }
        public string MaxCountElements { get; set; }
    }
}
