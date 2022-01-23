using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using practise.Testing;
using System;

namespace Practise.Test
{
    [TestClass]
    public class TransferServiceTest
    {
        [TestMethod]
        public void WireTransferInSufficientFundsThrowsError()
        {
           Account origin = new Account() { Funds=0};
           Account destination = new Account() { Funds = 0 };
            decimal transferAmount = 5m;
            string errorMessage = "Custome Error Message";
            var mockValidatorWireTransfer = new Mock<IValidWireTransfer>();
            mockValidatorWireTransfer.Setup(x => x.Validate(origin, destination, transferAmount)).Returns(new Operationresult(false, errorMessage));

            var service = new TransferService(mockValidatorWireTransfer.Object);
            Exception expectedException = null;
            //testing
            try
            {
                service.WireTransfer(origin,destination,transferAmount);
            }catch (Exception ex)
            {
                expectedException = ex;
            }

            //verification

            if(expectedException==null)
            {
                Assert.Fail("An Exception was expected");
            }

            Assert.IsTrue(expectedException is ApplicationException);

            Assert.AreEqual("The origin account does not have founds available",expectedException.Message);
        }

        [TestMethod]
        public void WireTransferCorrectlyEditFunds()
        {
            Account origin = new Account() { Funds = 10 };
            Account destination = new Account() { Funds = 5 };
            decimal transferAmount = 7m;
            var mockValidatorWireTransfer = new Mock<IValidWireTransfer>();
            mockValidatorWireTransfer.Setup(x => x.Validate(origin, destination, transferAmount)).Returns(new Operationresult(true));

            var service = new TransferService(mockValidatorWireTransfer.Object);
            //testing
            service.WireTransfer(origin,destination,transferAmount);

            //verification
            Assert.AreEqual(3, origin.Funds);
            Assert.AreEqual(12,destination.Funds);
        }
    }
}
