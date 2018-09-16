using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WSDion
{
    public class DionAccess
    {
        public static List<Materia> BuscarProvas(string usuario, string senha)
        {
            try
            {
                List<Materia> listaMaterias = new List<Materia>();
                List<string> onClicks = new List<string>();

                string driverPath = Environment.CurrentDirectory + @"\..\..\..\packages\Selenium.WebDriver.ChromeDriver.2.42.0.1\driver\win32";
                ChromeDriverService service = ChromeDriverService.CreateDefaultService(driverPath);
                service.HideCommandPromptWindow = true;


                var options = new ChromeOptions();
                options.AddArgument("--window-position=-32000,-32000");



                using (var driver = new ChromeDriver(service, options))
                {
                    driver.Navigate().GoToUrl("https://www.furb.br/dion/aluno/diario.xhtml");

                    var inputUsuario = driver.FindElementsByName("j_username")[0];
                    var inputSenha = driver.FindElementsByName("j_password")[0];
                    
                    inputUsuario.SendKeys(usuario);
                    inputSenha.SendKeys(senha);
                    driver.FindElement(By.XPath("//button[text() = 'Confirmar']")).Click();

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
                        var tabela = driver.FindElementById("formDiarioAluno:apDiarioAluno:j_idt49_data");
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

                    return listaMaterias;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}