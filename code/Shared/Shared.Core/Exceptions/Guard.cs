namespace Shared.Core.Exceptions
{
    public static class Guard<TE> where TE : BusinessException, new()
    {
        public static void AgainstNullOrEmpty(string value)
        {
            if (string.IsNullOrEmpty(value))
                ThrowException();
            if (value != null && value.Trim() == "")
                ThrowException();
        }

        public static void AgainstNull<T>(T? value) where T : class
        {
            if (value == null)
                ThrowException();
        }

        public static void AgainstNull(DateTime value)
        {
            if (value == default(DateTime))
                ThrowException();
        }

        public static void AgainstNull(string value)
        {
            if (string.IsNullOrEmpty(value))
                ThrowException();
        }

        public static void AgainstNotNull<T>(T value) where T : class?
        {
            if (value != null)
                ThrowException();
        }

        public static void IsTrue(bool value)
        {
            if (value)
                ThrowException();
        }

        public static void IsFalse(bool value)
        {
            if (!value)
                ThrowException();
        }

        public static void SmallerThan<T>(List<T> value, decimal target)
        {
            if (value?.Count < target)
                ThrowException();
        }

        public static void BiggerThan<T>(List<T> value, decimal target)
        {
            if (value?.Count > target)
                ThrowException();
        }

        public static void BiggerThan(decimal value, decimal target)
        {
            if (value > target)
                ThrowException();
        }
        public static void SmallerThan(decimal value, decimal target)
        {
            if (value < target)
                ThrowException();
        }

        //public static void NotInRange(decimal value, decimal min, decimal max)
        //{
        //    if (value < min || value > max)
        //        ThrowException();
        //}
        public static void NotInRange(double value, double min, double max)
        {
            if (value < min || value > max)
                ThrowException();
        }

        //public static void InRange(decimal value, decimal min, decimal max)
        //{
        //    if (value > min || value < max)
        //        ThrowException();
        //}

        public static void InRange(double value, double min, double max)
        {
            if (value > min || value < max)
                ThrowException();
        }

        private static void ThrowException()
        {
            throw new TE();
        }
    }
}
