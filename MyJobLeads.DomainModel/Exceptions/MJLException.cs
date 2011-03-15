using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class MJLException : Exception
    {
        public MJLException(string message)
            : base(message)
        {

        }
    }
}
