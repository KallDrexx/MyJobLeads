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
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;
using MyJobLeads.DomainModel.Enums.FillPerfect;

namespace MyJobLeads.DomainModel.Processes.FillPerfect
{
    public class FillPerfectLicenseProcesses : IProcess<GetFillPerfectLicenseByKeyParams, FillPerfectLicenseViewModel>,
                                               IProcess<ActivateFillPerfectKeyParams, FillPerfectKeyActivationViewModel>
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

            // Generate XML from the license
            var xml = new XDocument(
                        new XElement("FillPerfectLicense",
                            new XElement("LicenseType", license.LicenseType),
                            new XElement("LicensedFor", user.FullName),
                            new XElement("Email", user.Email),
                            new XElement("LicenseForMachine", license.ActivatedComputerId),
                            new XElement("EffectiveDate", license.EffectiveDate.ToLongDateString()),
                            new XElement("ExpirationDate", license.ExpirationDate.ToLongDateString())));

            // Convert from an XDocument to XmlDocument for processing
            var xmlDocument = new XmlDocument();
            using (var reader = xml.CreateReader())
                xmlDocument.Load(reader);
            
            // Sign the xml
            var key = new RSACryptoServiceProvider();
            var signedXml = new SignedXml(xmlDocument);
            signedXml.SigningKey = key;

            var reference = new Reference { Uri = "" };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            signedXml.AddReference(reference);

            signedXml.ComputeSignature();
            var signature = signedXml.GetXml();
            xmlDocument.DocumentElement.AppendChild(xmlDocument.ImportNode(signature, true));
            if (xmlDocument.FirstChild is XmlDeclaration)
                xmlDocument.RemoveChild(xmlDocument.FirstChild);

            // Return the license 
            return new FillPerfectLicenseViewModel 
            { 
                LicenseXml = xmlDocument.InnerXml,
                KeyXml = key.ToXmlString(false),
                RequestDate = DateTime.Now
            };
        }

        /// <summary>
        /// Activates the specified key for the machine identifier
        /// </summary>
        /// <param name="procParams"></param>
        /// <returns></returns>
        public FillPerfectKeyActivationViewModel Execute(ActivateFillPerfectKeyParams procParams)
        {
            var user = _context.Users.Where(x => x.FillPerfectKey == procParams.FillPerfectKey).FirstOrDefault();
            if (user == null)
                return new FillPerfectKeyActivationViewModel { Result = FillPerfectActivationResult.InvalidKey };

            if (string.IsNullOrWhiteSpace(procParams.MachineId))
                return new FillPerfectKeyActivationViewModel { Result = FillPerfectActivationResult.InvalidMachineId };

            // Get the user's FP license
            var license = user.OwnedOrders
                              .SelectMany(x => x.FillPerfectLicenses)
                              .OrderByDescending(x => x.EffectiveDate)
                              .FirstOrDefault();
            if (license == null)
                return new FillPerfectKeyActivationViewModel { Result = FillPerfectActivationResult.NoLicense };

            if (!string.IsNullOrEmpty(license.ActivatedComputerId))
                return new FillPerfectKeyActivationViewModel { Result = FillPerfectActivationResult.KeyAlreadyActivated };

            license.ActivatedComputerId = procParams.MachineId;
            _context.SaveChanges();

            return new FillPerfectKeyActivationViewModel { Result = FillPerfectActivationResult.ActivationSuccessful };
        }
    }
}
