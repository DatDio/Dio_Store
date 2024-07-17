namespace ViewModel.Common
{
    public class PagedResult<T> : PageResultBase
    {
        public List<T> Items { get; set; }

    }
}
