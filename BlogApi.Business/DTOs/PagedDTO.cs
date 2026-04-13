namespace BlogApi.Business.DTOs
{
    public class PagedDTO<T>
    {
        
        public IEnumerable<T> Items{get;set;} = new List<T>();
        public int TotalCount {get;set;}
        public int Page {get;set;}
        public int PageSize{get;set;}
        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);
    }
    
}