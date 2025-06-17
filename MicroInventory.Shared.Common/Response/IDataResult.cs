using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroInventory.Shared.Common.Response
{
    public interface IDataResult<T>:IResult
    {
        T Data { get; set; }
        long TotalCount { get; set; }
        int Page { get; set; }
        int PageSize { get; set; }
        int TotalPage { get; set; }
    }
}
