namespace FinanceSystem.Application.Contracts.Product.Command
{
    public class ModifyProductCommand : ProductCommand
    {
        public Guid Id { get; set; }
    }
}
