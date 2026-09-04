using FluentAssertions.Execution;
using NSubstitute.Core;
using NSubstitute.Core.Arguments;

namespace FinanceSystem.Application.Tests.Unit
{
    public static class Inquire
    {
        private static readonly ArgumentFormatter DefaultArgumentFormatter = new ArgumentFormatter();
        public static T That<T>(Action<T> action)
        {
            return ArgumentMatcher.Enqueue<T>(new AssertionMatcher<T>(action));
        }
        private class AssertionMatcher<T> : IArgumentMatcher<T>, IDescribeNonMatches
        {
            private readonly Action<T> assertion;
            private string allFailures = "";
            private bool lastResult = false;
            private bool hasRunBefore = false;
            public AssertionMatcher(Action<T> assertion)
            {
                this.assertion = assertion;
            }

            public bool IsSatisfiedBy(T argument)
            {
                if (hasRunBefore) return lastResult;

                using (var scope = new AssertionScope())
                {
                    try
                    {
                        assertion(argument);
                    }
                    catch (Exception exception)
                    {
                        var f = scope.Discard();
                        allFailures = f.Any() ? AggregateFailures(f) : exception.Message;

                        return SetResult(false);
                    }

                    var failures = scope.Discard().ToList();

                    if (failures.Count == 0) return SetResult(true);

                    allFailures = AggregateFailures(failures);

                    return SetResult(false);
                }
            }

            private bool SetResult(bool result)
            {
                this.hasRunBefore = true;
                this.lastResult = result;
                return result;
            }

            private string AggregateFailures(IEnumerable<string> discard)
                => discard.Aggregate(allFailures, (a, b) => a + "\n" + b);

            public string DescribeFor(object argument)
            {
                return DefaultArgumentFormatter.Format(allFailures, false);
            }
        }
    }
}
