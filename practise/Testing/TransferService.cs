using System;

namespace practise.Testing
{
    public class TransferService
    {
        private readonly IValidWireTransfer _inValidWireTransfer;
        public TransferService(IValidWireTransfer inValidWireTransfer)
        {
            _inValidWireTransfer = inValidWireTransfer;
        }
        public void WireTransfer(Account origin, Account destination, decimal amount)
        {
            var state = _inValidWireTransfer.Validate(origin, destination, amount);
            if(!state.IsSuccessfull)
            {
                throw new ApplicationException(state.ErrorMessage);
            }


            origin.Funds -= amount;
            destination.Funds += amount;
        }
       

    }
}
