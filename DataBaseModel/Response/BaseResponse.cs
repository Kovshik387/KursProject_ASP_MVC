using DataBaseModel.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseModel.Response
{
    public class BaseResponse<Tdata> : IBaseResponse<Tdata>
    {
        public string? Description { get; set; }

        public StatusCode StatusCode { get; set; }

        public Tdata? Data { get; set; }
    }

    public interface IBaseResponse<Tdata>
    {
        StatusCode StatusCode { get; }
        Tdata Data { get; }
    }
}
