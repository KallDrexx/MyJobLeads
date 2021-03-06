﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions.Jigsaw
{
    public class JigsawContactNotFoundException : JigsawException
    {
        public JigsawContactNotFoundException(string contactId)
            : base(string.Format("The jigsaw contact Id {0} could not be found", contactId))
        {
            ContactId = contactId;
        }

        public JigsawContactNotFoundException(int contactId) : this(contactId.ToString())
        {

        }

        public string ContactId { get; set; }
    }
}
