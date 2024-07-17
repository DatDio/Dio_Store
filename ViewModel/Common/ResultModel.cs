namespace ViewModel.Common
{
    public class ResultModel<T>
    {
        public bool IsSuccessed { get; set; }

        public string Message { get; set; }

        public T ResultObject { get; set; }
    }
}
