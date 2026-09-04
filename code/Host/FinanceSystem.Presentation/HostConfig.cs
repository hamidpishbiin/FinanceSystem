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
        public string AllowOrigins { get; set; }
    }
    public sealed class ConnectionStrings
    {
        public const string SectionName = "ConnectionStrings";
        public string DefaultConnection { get; set; }
        public string ReadOnlyConnection { get; set; }
    }
}