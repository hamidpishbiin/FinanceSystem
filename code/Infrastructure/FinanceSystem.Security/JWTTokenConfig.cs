namespace FinanceSystem.Security
{
    public class JWTTokenConfig
    {
        public const string SectionName = "JWTTokenConfig";
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
    }
}
