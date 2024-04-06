using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException()
          : base($"resource not found")
        { }

        public NotFoundException(object key)
            : base($"({key}) is not found")
        { }
    }
}
