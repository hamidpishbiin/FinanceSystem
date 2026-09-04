using FluentAssertions;
using NetArchTest.Rules;
using FinanceSystem.Interface.WriteModel;

namespace FinanceSystem.Architecture.Tests.Unit
{
    public class WriteModelAssemblyTests
    {
        private readonly string because = "WriteModel should not depend on ";

        [Fact]
        public void WriteModel_Services_ShouldNot_DependOn_ReadModel()
        {
            var result = GetWriteModelAssembly()
                .ShouldNot()
                .HaveDependencyOn("FinanceSystem.Interface.ReadModel")
                .GetResult();

            result.IsSuccessful.Should().Be(true, $"{because} ReadModel.");
        }

        [Fact]
        public void WriteModel_Services_ShouldNot_DependOn_Application()
        {
            var result = GetWriteModelAssembly()
                .ShouldNot()
                .NotHaveDependencyOn("FinanceSystem.Application")
                .GetResult();

            result.IsSuccessful.Should().Be(true, $"{because} Application.");
        }

        [Fact]
        public void WriteModel_Services_ShouldNot_DependOn_Domain()
        {
            var result = GetWriteModelAssembly()
                .ShouldNot()
                .NotHaveDependencyOn("FinanceSystem.Domain")
                .GetResult();

            result.IsSuccessful.Should().Be(true, $"{because} Domain.");
        }

        [Fact]
        public void WriteModel_Services_ShouldNot_DependOn_Persistance()
        {
            var result = GetWriteModelAssembly()
                .ShouldNot()
                .HaveDependencyOn("FinanceSystem.Persistance")
                .GetResult();

            result.IsSuccessful.Should().Be(true, $"{because} Persistance.");
        }

        private PredicateList GetWriteModelAssembly()
        {
            return Types
                .InAssembly(typeof(ProductFacadeService).Assembly)
                .That()
                .ResideInNamespace("FinanceSystem.Interface.WriteModel");
        }
    }
}
