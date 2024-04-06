using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Domain.Common.Results
{
    public class PaginatedResult<T> : Result
    {
        public PaginatedResult(List<T> data)
        {
            Data = data;
        }

        public List<T> Data { get; set; }

        public PaginatedResult(bool succeeded, IEnumerable<T> data = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10, string sortOn = "", string sortDir = "")
        {
            Data = data.ToList();
            CurrentPage = page;
            Succeeded = succeeded;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            SortOn = sortOn;
            SortDir = sortDir;
        }

        public PaginatedResult()
        {
        }

        public static PaginatedResult<T> Success(IEnumerable<T> data, int count, int page, int pageSize)
        {
            return new PaginatedResult<T>(true, data, null, count, page, pageSize);
        }
        public static Task<PaginatedResult<T>> FailureAsync(List<string> messages)
        {
            return Task.FromResult(new PaginatedResult<T>(false, default, messages));
        }
        public static Task<PaginatedResult<T>> SuccessAsync(IEnumerable<T> data, int count, int page, int pageSize, string sortOn = "", string sortDir = "")
        {
            return Task.FromResult(new PaginatedResult<T>(true, data, null, count, page, pageSize, sortOn, sortDir));
        }
        public int CurrentPage { get; set; }

        public int TotalPages { get; set; }

        public int TotalCount { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        public string SortOn { get; set; }
        public string SortDir { get; set; }

    }
}
