using System;
using System.Drawing;
using System.Numerics;
using static System.Net.Mime.MediaTypeNames;

namespace UTF16BoxDrawing;

class Program
{
    static void Main()
    {
        string textoTeste = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. " +
            "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown " +
            "printer took a galley of type and scrambled it to make a type specimen book. It has survived " +
            "not only five centuries, but also the leap into electronic typesetting, remaining essentially " +
            "unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem " +
             "Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including " +
            "versions of Lorem Ipsum.";

        TextArea textArea = new();

        Drawing.DrawTextArea(textArea);

        Drawing.WriteTextArea(textArea, CortaTexto(textoTeste));

        //------

        Console.SetCursorPosition(0, 20);
        Console.WriteLine("\n\n");

    }

    public static List<string> CortaTexto(string texto)
    {
        List<string> textos = new();
        int max = 48;

        while (texto.Length > 0)
        {
            textos.Add(texto.Substring(0, Math.Min(texto.Length, max)));
            texto = texto.Substring(Math.Min(texto.Length, max));
        }

        return textos;
    }
}

public class TextArea
{
    public int Margin = 1;
    public COORD Size = new() { x = 50, y = 10 };
    public COORD Origin = new() { x = 4, y = 2 };
    public int line = 0;
}

public static class Drawing
{
    public static void DrawTextArea(TextArea textArea)
    {
        string linhaHorizontal = "\u2500";          // ─
        string linhaVertical = "\u2502";            // │
        string cantoSuperiorEsquerdo = "\u250C";    // ┌
        string cantoSuperiorDireito = "\u2510";     // ┐
        string cantoInferiorEsquerdo = "\u2514";    // └
        string cantoInferiorDireito = "\u2518";     // ┘

        Console.SetCursorPosition(textArea.Origin.x, textArea.Origin.y);

        for (int i = textArea.Origin.x; i <= textArea.Origin.x + textArea.Size.x + 1; i++)
        {
            if (i == textArea.Origin.x)
                Console.Write(cantoSuperiorEsquerdo);
            else if (i == textArea.Origin.x + textArea.Size.x + 1)
                Console.Write(cantoSuperiorDireito);
            else
                Console.Write(linhaHorizontal);
        }

        for (int i = textArea.Origin.y + 1; i <= textArea.Origin.y + textArea.Size.y; i++)
        {
            Console.SetCursorPosition(textArea.Origin.x, i);
            Console.Write(linhaVertical);
            Console.SetCursorPosition(textArea.Origin.x + textArea.Size.x + 1, i);
            Console.Write(linhaVertical);
        }

        Console.SetCursorPosition(textArea.Origin.x, (textArea.Origin.y + textArea.Size.y + 1));

        for (int i = textArea.Origin.x; i <= textArea.Origin.x + textArea.Size.x + 1; i++)
        {
            if (i == textArea.Origin.x)
                Console.Write(cantoInferiorEsquerdo);
            else if (i == textArea.Origin.x + textArea.Size.x + 1)
                Console.Write(cantoInferiorDireito);
            else
                Console.Write(linhaHorizontal);
        }
    }

    public static void WriteTextArea(TextArea textArea, List<string> texts)
    {
        int line = 0;
        foreach (var text in texts.AsEnumerable().Reverse())
        {
            Console.SetCursorPosition(textArea.Origin.x + 2, textArea.Origin.y + textArea.Size.y - line);
            Console.WriteLine(text);
            line++;
            if (line == textArea.Size.y)
                break;
        }
        //Console.SetCursorPosition(textArea.Origin.x + 2, textArea.Origin.y + 1 + textArea.line);
        //Console.Write(text);
        //textArea.line++;
    }

    public static void DrawScale()
    {
        for (int i = 0; i < Console.WindowWidth - 1; i++)
        {
            Console.SetCursorPosition(i, 0);
            Console.Write(i % 10);
        }

        for (int i = 0; i < Console.WindowHeight - 1; i++)
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


/* MAYBE A INPUT?
 int messagesCount = 0;
        

        Console.SetCursorPosition(5, 20);
        string entrada = Console.ReadLine() ?? "";


        while (entrada != ".quit")
        {
            Console.SetCursorPosition(5, 3 + messagesCount);
            if (entrada.Length > 0 && entrada.Length < 50)
            {
                Console.SetCursorPosition(5, 3 + messagesCount);
                Console.Write(entrada);
            }

            Console.SetCursorPosition(5, 20);
            Console.Write("                                          ");
            Console.SetCursorPosition(5, 20);
            entrada = Console.ReadLine() ?? "";
            messagesCount++;
        }

 */