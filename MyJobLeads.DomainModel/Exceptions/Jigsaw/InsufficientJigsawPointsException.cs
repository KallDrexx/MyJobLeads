using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions.Jigsaw
{
    public class InsufficientJigsawPointsException : JigsawException
    {
        public InsufficientJigsawPointsException(int userId) 
            : base(string.Format("User {0} does not have enough points in their jigsaw account to perform the requested action", userId))
        {
            UserId = userId;
        }

        public int UserId { get; set; }
    }
}
