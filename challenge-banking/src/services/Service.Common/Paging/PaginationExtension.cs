using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Service.Common.Paging
{
    public static class PaginationExtension
    {
        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

            response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationHeader));
            // CORS
            response.Headers.Add("access-control-expose-headers", "X-Pagination");
        }

        public static void AddPagination(this HttpResponse response, string paginationHeader)
        {
            response.Headers.Add("X-Pagination", paginationHeader);
            // CORS
            response.Headers.Add("access-control-expose-headers", "X-Pagination");
        }
    }
}
