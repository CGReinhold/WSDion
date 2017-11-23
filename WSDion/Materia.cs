using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSDion
{
    public class Materia
    {
        public string Nome { get; set; }
        public List<Prova> Prova { get; set; }

        public Materia()
        {
            Prova = new List<Prova>();
        }
    }
}