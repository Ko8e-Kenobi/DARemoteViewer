﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DARemoteViewer.Domain.Services
{
    public interface IQuery<ReturnT, InputT>
    {
        public ReturnT Execute(InputT command);
    }
}
