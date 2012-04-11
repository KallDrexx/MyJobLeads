using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyJobLeads.DomainModel.Entities.EF;
using MyJobLeads.DomainModel.Data;
using MyJobLeads.DomainModel.ViewModels.FillPerfect;
using MyJobLeads.DomainModel.ProcessParams.FillPerfect;

namespace MyJobLeads.Areas.Licensing.Controllers
{
    public class FillPerfectController : MyJobLeadsBaseController
    {
        protected IProcess<GetFillPerfectLicenseByKeyParams, FillPerfectLicenseViewModel> _getFpLicenseProc;
        protected IProcess<ActivateFillPerfectKeyParams, FillPerfectKeyActivationViewModel> _activateFpProc;

        public FillPerfectController(MyJobLeadsDbContext context, 
                                        IProcess<GetFillPerfectLicenseByKeyParams, FillPerfectLicenseViewModel> getFpLicenseProc,
                                        IProcess<ActivateFillPerfectKeyParams, FillPerfectKeyActivationViewModel> activateFpProc)
        {
            _context = context;
            _getFpLicenseProc = getFpLicenseProc;
            _activateFpProc = activateFpProc;
        }


        public JsonResult Activate(string key, string machineId)
        {
            var result = new FillPerfectKeyActivationViewModel();
            Guid parsedKey;

            if (Guid.TryParse(key, out parsedKey))
            {
                result = _activateFpProc.Execute(new ActivateFillPerfectKeyParams
                {
                    FillPerfectKey = parsedKey,
                    MachineId = machineId
                });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLicense(string key)
        {
            var result = new FillPerfectLicenseViewModel { Error = DomainModel.Enums.FillPerfectLicenseError.InvalidKey };
            Guid parsedKey;

            if (Guid.TryParse(key, out parsedKey))
            {
                result = _getFpLicenseProc.Execute(new GetFillPerfectLicenseByKeyParams
                {
                    FillPerfectKey = parsedKey
                });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
