namespace ExchangeRatesAgainstTheDollar.Grpc.Models.Settings
{
    public class WebCurrensySettings
    {
        public WebCurrensySettings()
        {
            PatchToCurrensyElements = new List<CurrensySettings>();
        }
        public string Url { get; init; }
        public IReadOnlyList<CurrensySettings> PatchToCurrensyElements { get; init; }
    }
}
