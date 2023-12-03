using BackEnd.Models;
using BackEnd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Helpers
{
    public class PagingHelper
    {

        public static PagedResponse CreatePagedReponse(IEnumerable<Story> pagedData, int pageNumber, int pageSize, int totalRecords, IUriService uriService, string route)
        {
            var respose = new PagedResponse(pagedData, pageNumber, pageSize);
            var totalPages = ((double)totalRecords / (double)pageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                pageNumber >= 1 && pageNumber < roundedTotalPages
                ? uriService.GetPageUri(pageNumber + 1, pageSize, route)
                : null;
            respose.PreviousPage =
                pageNumber  - 1 >= 1 && pageNumber <= roundedTotalPages
                ? uriService.GetPageUri(pageNumber - 1, pageSize, route)
                : null;
            respose.FirstPage = uriService.GetPageUri(1, pageSize, route);
            respose.LastPage = uriService.GetPageUri(roundedTotalPages, pageSize, route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
    }
}
