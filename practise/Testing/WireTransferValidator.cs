using System;

namespace practise.Testing
{
    public class WireTransferValidator : IValidWireTransfer
    {
        public Operationresult Validate(Account origin,Account destination,decimal amount)
        {
            if(amount>origin.Funds)
            {
                throw new ApplicationException("The origin account does not have founds available");
            }
            return new Operationresult(true);
        }
    }
}
