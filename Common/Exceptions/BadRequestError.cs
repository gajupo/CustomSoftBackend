using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class BadRequestError: Error
    {
        public BadRequestError(string message)
            :base(message)
        {
                
        }
    }
}
