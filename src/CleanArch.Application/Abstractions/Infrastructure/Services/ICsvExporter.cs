using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Abstractions.Infrastructure.Services
{
    public interface ICsvExporter
    {
        byte[] ExportToCsv(List<object> exports); //or model instead of object
    }
}
