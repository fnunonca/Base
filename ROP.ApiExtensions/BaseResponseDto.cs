﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROP.ApiExtensions
{
    public class BaseResponseDto<T>
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Response { get; set; }
    }
}
