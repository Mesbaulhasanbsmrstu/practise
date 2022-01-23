namespace practise.Testing
{
    public interface IValidWireTransfer
    {
        Operationresult Validate(Account origin, Account destination, decimal amount);
    }
}
