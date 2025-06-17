using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroInventory.Shared.Common.Response
{
    public class ErrorResult:Result
    {
        public ErrorResult() : base(false)
        {

        }

        public ErrorResult(string message, int? statusCodes = 200) : base(false, message, statusCodes)
        {

        }
    }
}
