using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(int pageNumber, int pageSize, string route);
    }
}
