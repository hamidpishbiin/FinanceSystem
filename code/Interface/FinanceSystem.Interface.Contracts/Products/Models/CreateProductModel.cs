namespace FinanceSystem.Interface.Contracts.Products.Models
{
    public class CreateProductModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
