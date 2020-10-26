using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace CodingChallegeTKB.Models
{
    [Serializable]
    [XmlRoot("Debtor")]
    public class DebtorsImputMetaData
    {
        [XmlElement("Number")]
        public string Number { get; set; }

        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Telephone")]
        public string Telephone { get; set; }

        [XmlElement("Mobile")]
        public string Mobile { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }
    }

    [MetadataType(typeof(DebtorsImputMetaData))]
    public partial class Debtor
    {

    }
}