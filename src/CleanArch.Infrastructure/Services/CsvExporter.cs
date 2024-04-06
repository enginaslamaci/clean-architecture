using CleanArch.Application.Abstractions.Infrastructure.Services;
using CsvHelper;
using System.Globalization;

namespace CleanArch.Infrastructure.Services
{
    public class CsvExporter : ICsvExporter
    {
        public byte[] ExportToCsv(List<object> exports)
        {
            using var memoryStream = new MemoryStream();

            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
                csvWriter.WriteRecords(exports);
            }

            return memoryStream.ToArray();
        }
    }
}
