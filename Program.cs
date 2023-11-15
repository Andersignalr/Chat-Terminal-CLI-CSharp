using System;
using System.Numerics;

namespace UTF16BoxDrawing;

class Program
{
    static void Main()
    {

        Drawing.DrawRectangle(new() { x = 4, y = 2 }, new() { x = 50, y = 10});

        //Drawing.DrawScale();

        //------

        string[] frases =
        {
            "Anderson: olá, esta é a primeira frase",
            "          de um suposto local para exibir",
            "          as mensagens que serão enviadas",
            "                                         ",
            "Atrio: pelos usuários..."
        };

        for(int i = 0; i< frases.Length; i++)
        {
            Console.SetCursorPosition(5, 3+i);

            if (frases[i].Length > 50)
                continue;

            Console.WriteLine(frases[i]);
        }


        Console.SetCursorPosition(0, 20);
        Console.WriteLine("\n\n");

    }

    public static class Drawing
    {
        public static void DrawRectangle(COORD origin, COORD padding)
        {
            string linhaHorizontal = "\u2500";
            string linhaVertical = "\u2502";
            string cantoSuperiorEsquerdo = "\u250C";
            string cantoSuperiorDireito = "\u2510";
            string cantoInferiorEsquerdo = "\u2514";
            string cantoInferiorDireito = "\u2518";

            Console.SetCursorPosition(origin.x, origin.y);

            for (int i = origin.x; i <= origin.x + padding.x + 1; i++)
            {
                if (i == origin.x)
                    Console.Write(cantoSuperiorEsquerdo);
                else if (i == origin.x + padding.x + 1)
                    Console.Write(cantoSuperiorDireito);
                else
                    Console.Write(linhaHorizontal);
            }

            for (int i = origin.y + 1; i <= origin.y + padding.y; i++)
            {
                Console.SetCursorPosition(origin.x, i);
                Console.Write(linhaVertical);
                Console.SetCursorPosition(origin.x + padding.x + 1, i);
                Console.Write(linhaVertical);
            }

            Console.SetCursorPosition(origin.x, (origin.y + padding.y + 1));

            for (int i = origin.x; i <= origin.x + padding.x + 1; i++)
            {
                if (i == origin.x)
                    Console.Write(cantoInferiorEsquerdo);
                else if (i == origin.x + padding.x + 1)
                    Console.Write(cantoInferiorDireito);
                else
                    Console.Write(linhaHorizontal);
            }
        }
        
        public static void DrawScale()
        {
            for (int i = 0; i < Console.WindowWidth-1; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write(i % 10);
            }

            for (int i = 0; i < Console.WindowHeight-1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(i % 10);
            }
        }
    }

    public class COORD
    {
        public int x;
        public int y;
    }
}