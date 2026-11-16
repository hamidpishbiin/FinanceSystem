namespace FinanceSystem.Saman;

public abstract class SamanLogModel
{
    public long Id { get; private set; }
    public DateTime CreatedAt { get; set; }
    public string? ErrorType { get; private set; }
    public string? ErrorMessage { get; private set; }

    public void RecordError(Exception exception)
    {
        ErrorType = exception.GetType().Name;
        ErrorMessage = exception.Message;
    }
}
