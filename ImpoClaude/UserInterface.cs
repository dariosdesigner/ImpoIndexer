using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpoClaude
{
    public static class UserInterface
    {
            public static ImpositionSettings GetUserSettings()
        {
            ImpositionSettings settings = new ImpositionSettings();

            Console.WriteLine("1. Perfect-Bound (Encadernação perfeita)");
            Console.WriteLine("2. Cut-Stack (Corte e empilhamento)");
            Console.Write("Escolha o método de imposição (1 ou 2): ");
            settings.ImpositionMethod = int.Parse(Console.ReadLine());

            Console.Write("Digite o caminho do arquivo PDF de entrada (deixe vazio para gerar um booklet numerado): ");
            settings.InputPath = Console.ReadLine();

            // Se não informar o input, pergunta quantas páginas terá o booklet
            if (string.IsNullOrWhiteSpace(settings.InputPath))
            {
                Console.Write("Input não informado. Digite o número de páginas para o booklet: ");
                settings.TotalPages = int.Parse(Console.ReadLine());
            }

            Console.Write("Digite o caminho do arquivo PDF de saída: ");
            settings.OutputPath = Console.ReadLine();

            Console.Write("Impressão frente e verso (S/N)? ");
            settings.DoubleSided = Console.ReadLine().Trim().ToUpper() == "S";

            Console.Write("Quantas páginas por lado da folha (2, 4, 6, 8, 9, 16)? ");
            settings.PagesPerSide = int.Parse(Console.ReadLine());

            Console.Write("Distância entre páginas (fresa) em mm: ");
            settings.GapBetweenPages = float.Parse(Console.ReadLine()) * 2.83465f; // Converter de mm para pontos (1 mm = 2.83465 pontos)

            return settings;
        }   
    }
}
