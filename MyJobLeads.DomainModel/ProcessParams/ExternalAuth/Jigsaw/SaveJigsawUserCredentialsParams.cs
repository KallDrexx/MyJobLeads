using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.ProcessParams.ExternalAuth.Jigsaw
{
    public class SaveJigsawUserCredentialsParams
    {
        public int RequestingUserId { get; set; }
        public string JigsawUsername { get; set; }
        public string JigsawPassword { get; set; }
    }
}
