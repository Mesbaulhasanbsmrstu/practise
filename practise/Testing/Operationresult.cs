namespace practise.Testing
{
    public class Operationresult
    {
        public Operationresult(bool isSuccessFull,string errorMessage=null)
        {
            IsSuccessfull= isSuccessFull;
            ErrorMessage= errorMessage;
        }

        public bool IsSuccessfull { get; set; }
        public string ErrorMessage { get;set; }
    }
}
