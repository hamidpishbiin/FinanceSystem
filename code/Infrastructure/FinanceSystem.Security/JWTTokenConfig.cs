namespace FinanceSystem.Security
{
    public class JWTTokenConfig
    {
        public const string SectionName = "JWTTokenConfig";
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}
