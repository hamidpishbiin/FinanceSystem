namespace FinanceSystem.Core
{
    public enum Permissions : long
    {
        AccessProduct = 100,
        CreateProduct = 101,
        ModifyProduct = 102,
        DeleteProduct = 103,

        AccessPayment = 200,
        CreatePayment = 201,
    }
}
