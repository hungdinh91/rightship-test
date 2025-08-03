using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Domain.Shared.Common;

public class ErrorObject
{
    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}
