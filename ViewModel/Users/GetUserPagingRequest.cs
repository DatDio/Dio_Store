namespace ViewModel.Users
{
    public class GetUserPagingRequest
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        public string Keyword { get; set; }
    }
}
