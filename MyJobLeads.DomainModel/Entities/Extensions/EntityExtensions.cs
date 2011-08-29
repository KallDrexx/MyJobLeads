using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyJobLeads.DomainModel.Entities.Extensions
{
    public static class EntityExtensions
    {
        public static string LocationString(this Company company)
        {
            // Form company location string
            string location = string.Empty;

            bool citySpecified = !string.IsNullOrWhiteSpace(company.City);
            bool stateSpecified = !string.IsNullOrWhiteSpace(company.State);
            bool zipSpecified = !string.IsNullOrWhiteSpace(company.Zip);

            if (citySpecified && stateSpecified) { location = string.Concat(company.City, ", ", company.State, " "); }
            else if (citySpecified) { location = company.City + " "; }
            else if (stateSpecified) { location = company.State + " "; }

            if (zipSpecified) { location += company.Zip; }

            return location;
        }
    }
}
