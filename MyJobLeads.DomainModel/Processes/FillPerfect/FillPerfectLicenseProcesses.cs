using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using MyJobLeads.DomainModel.ViewModels.FillPerfect;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ProcessParams.FillPerfect;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Enums;

namespace MyJobLeads.DomainModel.Processes.FillPerfect
{
    public class FillPerfectLicenseProcesses : IProcess<GetFillPerfectLicenseByKeyParams, FillPerfectLicenseViewModel>
    {
        public MyJobLeadsDbContext _context;

        public FillPerfectLicenseProcesses(MyJobLeadsDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves the FillPerfect license tied to a specific key
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public FillPerfectLicenseViewModel Execute(GetFillPerfectLicenseByKeyParams procParams)
        {
            // Get the user the key belongs to
            var user = _context.Users
                               .Where(x => x.FillPerfectKey == procParams.FillPerfectKey)
                               .FirstOrDefault();

            if (user == null)
                return new FillPerfectLicenseViewModel { Error = FillPerfectLicenseError.InvalidKey };

            // Get the latest FillPerfect license for the user
            var license = user.OwnedOrders
                              .SelectMany(x => x.FillPerfectLicenses)
                              .OrderByDescending(x => x.EffectiveDate)
                              .FirstOrDefault();
            if (license == null)
                return new FillPerfectLicenseViewModel { Error = FillPerfectLicenseError.NoLicense };

            if (string.IsNullOrEmpty(license.ActivatedComputerId))
                return new FillPerfectLicenseViewModel { Error = FillPerfectLicenseError.KeyNotActivated };

            return new FillPerfectLicenseViewModel();
        }
    }
}
