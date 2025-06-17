using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementBack.Config
{
    public class PaginationParams
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        [FromQuery]
        public int PageNumber { get; set; } = 1;

        [FromQuery]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = Math.Min(value, MaxPageSize);
        }        
        [FromQuery]
        public string EmailFilter { get; set; } = string.Empty;
        [FromQuery]
        public string NameFilter { get; set; } = string.Empty;        
    }
}
