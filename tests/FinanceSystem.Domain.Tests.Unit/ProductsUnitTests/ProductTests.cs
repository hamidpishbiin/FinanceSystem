using FluentAssertions;
using NSubstitute;
using FinanceSystem.Domain.Products.Exceptions;
using Shared.Core.Events;

namespace FinanceSystem.Domain.Tests.Unit.ProductsUnitTests
{
    public class ProductTests 
    {
        private const string Name = "Lubricant H250 z";
        private const string Description = "descripton of Lubricant H250 z";
        private const decimal Price = 1000;
        private const int StockQuantity = 100;
        private static Guid Id = Guid.Parse("6B29FC40-CA47-1067-B31D-00DD010662DA");

        private readonly ProductBuilder _builder;
        private readonly IEventPublisher _eventPublisher;

        public ProductTests()
        {
            _builder = new ProductBuilder();
            _eventPublisher = Substitute.For<IEventPublisher>();
        }


        [Fact]
        public async Task Constructor_should_create_properly_Product()
        {
            var product = await _builder.WithName(Name).WithDescription(Description)
                .WithPrice(Price).WithStockQuantity(StockQuantity).Build();
            product.Name.Should().Be(Name);
            product.Description.Should().Be(Description);
            product.Price.Should().Be(Price);
            product.StockQuantity.Should().Be(StockQuantity);
            product.Id.Should().Be(Id);
        }

        [Fact]
        public async Task Update_should_change_the_Product_to_new_values()
        {
            var additive = await _builder.WithName(Name).Build();
            var expectedName = "Lubricant X2300 green";
            await additive.Update(expectedName,Description,Price,StockQuantity, _eventPublisher);
            additive.Name.Should().Be(expectedName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Constructor_and_Update_should_throw_when_Name_is_not_valid(string name)
        {
            var builder = _builder.WithName(name);
            Func<Task> expected = async () => await builder.Build();
            expected.Should().ThrowAsync<ProductNameRequiredException>();
        }
    }
}