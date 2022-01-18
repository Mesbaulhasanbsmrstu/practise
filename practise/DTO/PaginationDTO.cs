namespace practise.DTO
{
    public class PaginationDTO
    {
        public int Page { get; set; } 
        public int recordsPerPage { get; set; }
        private readonly int maxRecordsPerPage = 50;
        public PaginationDTO(int Page,int recordsPerPage)
        {
            this.Page = Page;
            this.recordsPerPage=(recordsPerPage>maxRecordsPerPage)?maxRecordsPerPage:recordsPerPage;
        }

    }
}
