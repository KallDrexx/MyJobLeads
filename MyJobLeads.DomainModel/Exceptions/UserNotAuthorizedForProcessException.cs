using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class UserNotAuthorizedForProcessException : MJLException
    {
        public UserNotAuthorizedForProcessException(int userId, Type processParams, Type viewModel)
            : base(string.Format("User Id {0} is not authorized for the IProcess<{1}, {2}>", userId, processParams.Name, viewModel.Name))
        {
            UserId = userId;
            ProcessParamType = processParams;
            ViewModelType = viewModel;
        }

        public int UserId { get; set; }
        public Type ProcessParamType { get; set; }
        public Type ViewModelType { get; set; }
    }
}
