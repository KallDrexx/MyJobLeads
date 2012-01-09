using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions.Jigsaw
{
    public class JigsawException : MJLException
    {
        public JigsawException(string message) : base(message) { }
    }
}
