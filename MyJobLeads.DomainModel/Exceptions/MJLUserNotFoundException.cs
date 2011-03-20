using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Exceptions
{
    public class MJLUserNotFoundException : MJLException
    {
        public MJLUserNotFoundException(SearchPropertyType searchProperty, string searchValue)
            : base(string.Concat("No user found with the '", searchProperty.ToString() , "' property with a value of '", searchValue, "'"))
        {
            SearchProperty = searchProperty;
            SearchValue = searchValue;
        }

        public SearchPropertyType SearchProperty { get; set; }
        public string SearchValue { get; set; }

        public enum SearchPropertyType
        {
            Credentials,
            Username,
            Email
        }
    }
}
