using Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common
{
    public class Error
    {
        public string Message { get; set; } = string.Empty;
        public ErrorType ErrorCode { get; set; }
    }
}
