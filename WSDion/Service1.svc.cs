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
            try
            {
                List<Materia> listaMaterias = new List<Materia>();
                List<string> onClicks = new List<string>();

                string driverPath = @"C:\Users\Cleyson Reinhold\Documents\visual studio 2015\Projects\AcessaDION\packages\Selenium.WebDriver.ChromeDriver.2.33.0\driver\win32";
                ChromeDriverService service = ChromeDriverService.CreateDefaultService(driverPath);
                service.HideCommandPromptWindow = true;
                

                var options = new ChromeOptions();
                options.AddArgument("--window-position=-32000,-32000");

                

                using (var driver = new ChromeDriver(service, options))
                {
                    driver.Navigate().GoToUrl("https://www.furb.br/dion/aluno/diario.xhtml");

                    var inputUsuario = driver.FindElementsByName("j_username")[0];
                    var inputSenha = driver.FindElementsByName("j_password")[0];
                    var botao = driver.FindElementsByClassName("btn-primary")[0];

                    inputUsuario.SendKeys(usuario);
                    inputSenha.SendKeys(senha);
                    botao.Click();

                    var divMaterias = driver.FindElementsByClassName("icon-book");
                    foreach (var materias in divMaterias)
                    {
                        var pai = materias.FindElement(By.XPath(".."));
                        string nomeMateria = pai.GetAttribute("innerText");
                        nomeMateria = nomeMateria.Split('-')[0].Trim();
                        Materia materia = new Materia();
                        materia.Nome = nomeMateria;
                        listaMaterias.Add(materia);
                        var onclick = pai.GetAttribute("onclick");
                        onClicks.Add(onclick);
                    }

                    for (int i = 0; i < onClicks.Count; i++)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript(onClicks[i]);
                        var tabela = driver.FindElementById("formDiarioAluno:apDiarioAluno:j_idt61_data");
                        var linhas = tabela.FindElements(By.XPath(".//tr"));
                        foreach (var linha in linhas)
                        {
                            var td = linha.FindElements(By.XPath(".//td"));
                            if (td.Count >= 4)
                            {
                                Prova prova = new Prova();
                                //prova.Materia = listaMaterias[i];
                                //prova.Data = DateTime.Parse(td[0].GetAttribute("innerText"));
                                prova.Data = td[0].GetAttribute("innerText").Trim();
                                prova.Descricao = td[1].GetAttribute("innerText").Trim();
                                prova.Abreviacao = td[2].GetAttribute("innerText").Trim();
                                string stringNota = td[3].GetAttribute("innerText").Trim();
                                float valorNota = -1;
                                try
                                {
                                    valorNota = float.Parse(stringNota, CultureInfo.InvariantCulture);
                                }
                                catch (Exception) { }
                                if (valorNota != -1)
                                    prova.Valor = valorNota;
                                //notas.Add(nota);
                                listaMaterias[i].Prova.Add(prova);
                            }
                        }
                    }

                    var json = new JavaScriptSerializer().Serialize(listaMaterias);
                    return json;
                }
            }
            catch (Exception e)
            {
                return "";
            }
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
