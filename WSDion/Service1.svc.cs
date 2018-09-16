using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.IO;
using System.Net;
using System.Xml;
using System.Globalization;
using System.Web.Script.Serialization;

namespace WSDion
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        public string BuscarProvas(string usuario, string senha)
        {
            var listaMaterias = DionAccess.BuscarProvas(usuario, senha);
            var json = new JavaScriptSerializer().Serialize(listaMaterias);
            return json;
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
