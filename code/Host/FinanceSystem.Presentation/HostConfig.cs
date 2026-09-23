using FinanceSystem.Security;

namespace FinanceSystem.Presentation
{
    public class HostConfig
    {
        public Origins Origins { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public JWTTokenConfig JWTTokenConfig { get; set; }
    }
    public sealed class Origins
    {
        public const string SectionName = "Origins";
        public string AllowOrigins { get; set; } = default!;
    }
    public sealed class ConnectionStrings
    {
        public const string SectionName = "ConnectionStrings";
        public string DefaultConnection { get; set; } = default!;
        public string ReadOnlyConnection { get; set; } = default!;
    }
}