using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSDion;

namespace CLIDion
{
    class Program
    {
        static int posicaoAtual = 0;
        static bool vendoNotas = false;
        static int materiaSelecionada = -1;

        static void Main(string[] args)
        {
            Console.WriteLine("Carregando notas");
            List<Materia> listaMaterias = DionAccess.BuscarProvas(User.Username, User.Password);
            if (listaMaterias == null)
            {
                Console.WriteLine("Não foi possível carregar as notas");
                Console.ReadKey();
            }
            ImprimeMaterias(listaMaterias);
            while (true)
            {
                if (vendoNotas)
                    ImprimeNotas(listaMaterias[materiaSelecionada].Prova);
                else
                    ImprimeMaterias(listaMaterias);

                ConsoleKeyInfo key = Console.ReadKey();
                if (key.Key == ConsoleKey.DownArrow && posicaoAtual < 4)
                    posicaoAtual++;
                else if (key.Key == ConsoleKey.UpArrow && posicaoAtual > 0)
                    posicaoAtual--;
                if (key.Key == ConsoleKey.Enter)
                {
                    if (vendoNotas)
                    {
                        if (posicaoAtual == listaMaterias[materiaSelecionada].Prova.Count)
                        {
                            materiaSelecionada = -1;
                            vendoNotas = false;
                        }
                        else
                        {
                            posicaoAtual = 0;
                            ImprimeMaterias(listaMaterias);
                        }
                    }
                    else
                    {
                        if (posicaoAtual == listaMaterias.Count)
                            Environment.Exit(1);
                        else
                        {
                            materiaSelecionada = posicaoAtual;
                            posicaoAtual = 0;
                            ImprimeNotas(listaMaterias[materiaSelecionada].Prova);
                        }
                    }
                }
            }
        }

        private static void ImprimeMaterias(List<Materia> lista)
        {
            Console.Clear();
            Console.CursorTop = Console.WindowTop;
            Console.CursorLeft = 0;
            for (int i = 0; i < lista.Count; i++)
                Console.WriteLine($"[{(posicaoAtual == i ? "X" : " ")}] - {lista[i].Nome}");
            Console.WriteLine($"[{(posicaoAtual == lista.Count ? "X" : " ")}] - Sair");
        }

        private static void ImprimeNotas(List<Prova> provas)
        {
            vendoNotas = true;
            Console.Clear();
            Console.CursorTop = Console.WindowTop;
            Console.CursorLeft = 0;
            for (int i = 0; i < provas.Count; i++)
            {
                string data = provas[i].Data;
                string descricao = provas[i].Descricao;
                string abreviacao = provas[i].Abreviacao;
                string nota = provas[i].Valor == -1 ? "" : provas[i].Valor.ToString();
                Console.WriteLine($"[{(posicaoAtual == i ? "X" : " ")}] -\t{data}\t-\t{descricao}\t-\t{abreviacao}\t-\t{nota}");
            }
            Console.WriteLine($"[{(posicaoAtual == provas.Count ? "X" : " ")}] - Sair");
        }
    }
}
