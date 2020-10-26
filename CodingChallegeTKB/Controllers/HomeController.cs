using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using CodingChallegeTKB.Models;
using System.Xml.Schema;
using System.Xml;
using System.IO;
using System.Data.Entity;

namespace CodingChallegeTKB.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase xmlFile)
        {
            if(xmlFile.ContentType.Equals("application/xml") || xmlFile.ContentType.Equals("text/xml"))
            {
                try
                {
                    //Save the file on the server
                    var xmlPath = Server.MapPath("~/Content/" + xmlFile.FileName);
                    xmlFile.SaveAs(xmlPath);

                    //Load the saved file
                    XDocument xDoc = XDocument.Load(xmlPath);

                    //Validate the xml file againts the xsd schema
                    string xsd = @"<?xml version='1.0' encoding='utf-8'?>
                    <xsd:schema xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xs='http://www.w3.org/2001/XMLSchema' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                       <xs:simpleType name='string'>
                          <xs:restriction base='xs:string'>
                             <xs:maxLength value='255' />
                          </xs:restriction>
                       </xs:simpleType>

                       <xs:element name='ArrayOfDebtor'>
                          <xs:complexType>
                             <xs:sequence>
                                <!-- Debiteur -->
                                <xs:element minOccurs='0' maxOccurs='unbounded' name='Debtor'>
                                   <xs:complexType>
                                      <xs:sequence>
                                         <!-- Debiteurnummer; verplicht; string; maximum lengte 255 -->
                                         <xs:element name='Number' type='string' />

                                         <!-- Debiteurnaam; verplicht; string; maximum lengte 255 -->
                                         <xs:element name='Name' type='string' />

                                         <!-- Telefoonnummer debiteur; optioneel; string; maximum lengte 255 -->
                                         <xs:element name='Telephone' type='string' minOccurs='0' />

                                         <!-- Mobiel telefoonnummer debiteur; optioneel; string; maximum lengte 255 -->
                                         <xs:element name='Mobile' type='string' minOccurs='0' />

                                         <!-- E-mailadres debiteur; optioneel; string; maximum lengte 255 -->
                                         <xs:element name='Email' type='string' minOccurs='0' />
                                      </xs:sequence>
                                   </xs:complexType>
                                </xs:element>
                             </xs:sequence>
                          </xs:complexType>

                          <!-- Voor de gehele import mogen er niet meerdere debiteuren met hetzelfde nummer voorkomen. -->
                          <xs:key name='DebtorNumber'>
                             <xs:selector xpath='Debtor' />
                             <xs:field xpath='Number' />
                          </xs:key>
                       </xs:element>
                    </xsd:schema>
                    ";

                    XmlSchemaSet schema = new XmlSchemaSet();
                    schema.Add("", XmlReader.Create(new StringReader(xsd)));

                    bool errors = false;
                    string errorsMessage = string.Empty;
                    xDoc.Validate(schema, (o, e) =>
                    {
                        errorsMessage = e.Message;
                        errors = true;
                    }, true);

                    if(errors==true)
                    {
                        ViewBag.Message = errorsMessage;
                        return View("Index");
                    }


                    //Make a list with the loaded file
                    List<Debtor> debtorList = xDoc.Descendants("Debtor").Select(debtor => new Debtor
                    {
                        Number = debtor.Element("Number") != null ? debtor.Element("Number").Value : "",
                        Name = debtor.Element("Name") != null ? debtor.Element("Name").Value : "",
                        Telephone = debtor.Element("Telephone") != null ? debtor.Element("Telephone").Value : "",
                        Mobile = debtor.Element("Mobile") != null ? debtor.Element("Mobile").Value : "",
                        Email = debtor.Element("Email") != null ? debtor.Element("Email").Value : ""
                    }).ToList();


                    //Updating or creating the new records on the DB
                    using (masterEntities context = new masterEntities())
                    {
                        List<Debtor> dbDebtorList = context.Debtors.ToList();

                        foreach (Debtor item in dbDebtorList)
                        {
                            if (!debtorList.Contains(item))
                                item.IsClosed = true;
                        }

                        foreach (Debtor item in debtorList)
                        {
                            Debtor dbDebtor = context.Debtors.Find(item.Number);

                            if (dbDebtor != null)
                            {
                                dbDebtor.IsClosed = false;
                                context.Entry(dbDebtor).CurrentValues.SetValues(item);
                                context.Entry(dbDebtor).State = EntityState.Modified;
                            }
                            else
                            {
                                context.Debtors.Add(item);
                            }
                        }

                        context.SaveChanges();
                    }

                    ViewBag.Message = "The file was imported";
                    return View("Index");
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
            }
            else
            {
                ViewBag.Message = "Can't import because is not the right file type, only XML files are allowed";
                return View("Index");
            }
        }
    }
}