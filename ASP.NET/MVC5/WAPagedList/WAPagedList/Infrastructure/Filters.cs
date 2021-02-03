using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace WAPagedList.Infrastructure
{
    public class FilterKlient
    {
        public SortOrder SortOrder { get; set; }
        public string SortColumn { get; set; }
        public int? Page { get; set; }
        public string Nazwa { get; set; }
    }
}