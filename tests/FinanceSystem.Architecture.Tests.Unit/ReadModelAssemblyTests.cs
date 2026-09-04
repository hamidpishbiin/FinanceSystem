using FluentAssertions;
using NetArchTest.Rules;
using FinanceSystem.Interface.ReadModel;

namespace FinanceSystem.Architecture.Tests.Unit
{
    public class ReadModelAssemblyTests
    {
        private readonly string because = "ReadModel should not depend on ";

        [Fact]
        public void ReadModel_Services_ShouldNot_DependOn_WriteModel()
        {
            var result = GetReadModelAssembly()
                .ShouldNot()
                .HaveDependencyOn("FinanceSystem.Interface.WriteModel")
                .GetResult();

            result.IsSuccessful.Should().Be(true, $"{because} WriteModel.");
        }

        [Fact]
        public void ReadModel_Services_ShouldNot_DependOn_Application()
        {
            var result = GetReadModelAssembly()
                .ShouldNot()
                .HaveDependencyOn("FinanceSystem.Application")
                .GetResult();

            result.IsSuccessful.Should().Be(true, $"{because} Application.");
        }

        [Fact]
        public void ReadModel_Services_ShouldNot_DependOn_Persistance()
        {
            var result = GetReadModelAssembly()
                .ShouldNot()
                .HaveDependencyOn("FinanceSystem.Persistance")
                .GetResult();

            result.IsSuccessful.Should().Be(true, $"{because} Persistance.");
        }

        private PredicateList GetReadModelAssembly()
        {
            return Types
                .InAssembly(typeof(ProductFacadeQuery).Assembly)
                .That()
                .ResideInNamespace("FinanceSystem.Interface.ReadModel");
        }
    }
}
