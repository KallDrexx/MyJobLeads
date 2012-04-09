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

            // Generate XML from the license
            var xml = new XDocument(
                        new XElement("FillPerfectLicense",
                            new XElement("LicenseType", license.LicenseType),
                            new XElement("LicensedFor", user.FullName),
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
                LicenseXml = xmlDocument.InnerXml
            };
        }
    }
}
