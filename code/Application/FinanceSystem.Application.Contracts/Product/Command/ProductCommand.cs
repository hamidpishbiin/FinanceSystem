using Shared.Core;

namespace FinanceSystem.Application.Contracts.Product.Command
{
    public abstract class ProductCommand : ICommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
