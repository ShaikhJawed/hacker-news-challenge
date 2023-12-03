using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Models
{
    public class BaseResponse
    {
        public BaseResponse()
        { 
        
        }
        public BaseResponse(IEnumerable<Story> data)
        {
            this.Succeeded = true;
            this.Message = string.Empty;
            this.Errors = null;
            this.Data = data;
        }
        public IEnumerable<Story> Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}
