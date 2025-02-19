namespace WebApi.Models
{
    public class PageListState
    {
        public string? SearchName { get; set; }
        public bool Descending { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int PageMax { get; set; }
        public SortingKey SortOrder { get; set; }
        public StateChoose Choose { get; set; }
    }
}
