﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Abstractions.Infrastructure.Services
{
    public interface IDateTimeService
    {
        DateTime NowUtc { get; }
    }
}
