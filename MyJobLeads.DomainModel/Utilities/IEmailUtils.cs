using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Utilities
{
    public interface IEmailUtils
    {
        void Send(string toAddress, string subject, string body);
    }
}
