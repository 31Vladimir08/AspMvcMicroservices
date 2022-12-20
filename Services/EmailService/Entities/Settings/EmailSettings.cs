namespace EmailService.Entities.Settings
{
    public class EmailSettings
    {
        public string Address { get; init; }
        public string Host { get; init; }
        public string UserName { get; init; }
        public string Password { get; init; }
        public int Port { get; init; }
    }
}
