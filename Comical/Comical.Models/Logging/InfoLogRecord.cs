﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comical.Models.Logging
{
    public class InfoLogRecord : LogRecord
    {
        public InfoLogRecord(string message) => this.Message = message;
    }
}