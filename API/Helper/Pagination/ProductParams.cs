namespace API.Helper.Pagination
{
    public class ProductParams:PaginationParms
    {
        public string OrderBy { get; set; }
        public string SearctTerm { get; set; }
        public string Types { get; set; }
        public string Brands { get; set; }
    }
}
