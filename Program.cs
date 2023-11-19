using System;
using System.Drawing;
using System.Numerics;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace UTF16BoxDrawing;

class Program
{
    static void Main()
    {
        List<Mensagem> conversa;

        Database.Initialize();

        conversa = Database.GetMensagens();


        TextArea textArea = new();

        GUI.CarregarMensagens(textArea, conversa);

        GUI.ReceberMensagens(textArea, conversa);

        Console.SetCursorPosition(0, 19);
        Console.WriteLine("\n\n");
        Console.ReadLine();

    }

    public static List<string> CortaTexto(TextArea textArea, string texto)
    {
        List<string> textos = new();
        int maxCaracteresLinha = textArea.Size.x - 2;

        while (texto.Length > 0)
        {
            textos.Add(texto.Substring(0, Math.Min(texto.Length, maxCaracteresLinha)));
            texto = texto.Substring(Math.Min(texto.Length, maxCaracteresLinha));
        }

        return textos;
    }
}

public class TextArea
{
    public int Margin = 1;
    public COORD Size = new() { x = 100, y = 13 };
    public COORD Origin = new() { x = 4, y = 2 };
    public int line = 0;
}

public static class GUI
{
    public static void CarregarMensagens(TextArea textArea, List<Mensagem> conversa)
    {
        Drawing.WriteTextArea(textArea, Drawing.ProcessaMensagens(textArea, conversa));
    }
    public static void ReceberMensagens(TextArea textArea, List<Mensagem> conversa)
    {
        string user;
        string entrada;
        string blankSpace;
        int idResult;
        DateTime dateTime = DateTime.Now;

        while (true)
        {
            blankSpace = "";
            Drawing.DrawTextArea(textArea);

            Console.SetCursorPosition(0, 19);

            Console.Write("Nome de usuário: ");
            user = Console.ReadLine() ?? "Anderson";
            user = user.Substring(0, Math.Min(user.Length, textArea.Size.x-3));
            entrada = "";
            while (entrada != ".quit")
            {
                Console.SetCursorPosition(5, 25);
                blankSpace = blankSpace.PadRight(entrada.Length);
                Console.Write(blankSpace);

                Console.SetCursorPosition(5, 25);
                entrada = Console.ReadLine() ?? ".quit";
                if (entrada == ".quit")
                {
                    Console.SetCursorPosition(5, 25);
                    blankSpace = blankSpace.PadRight(entrada.Length);
                    Console.Write(blankSpace);

                    Console.SetCursorPosition(17, 19);
                    blankSpace = new string(' ', user.Length);
                    Console.Write(blankSpace);

                    break;
                }

                if (entrada == "")
                    continue;


                idResult = Database.SalvarMensagem(new Mensagem
                {
                    Content = entrada,
                    RemetentId = user,
                    CreatedAt = dateTime,
                });

                Mensagem mensagem = new()
                {
                    Id = idResult,
                    Content = entrada,
                    RemetentId = user,
                    CreatedAt = dateTime,
                };
                conversa.Add(mensagem);


                Drawing.WriteTextArea(textArea, Drawing.ProcessaMensagens(textArea, conversa));
            }
        }
    }
}

public static class Drawing
{
    public static void DrawTextArea(TextArea textArea)
    {
        /*
        string linhaHorizontal = "\u2500";          // ─
        string linhaVertical = "\u2502";            // │
        string cantoSuperiorEsquerdo = "\u250C";    // ┌
        string cantoSuperiorDireito = "\u2510";     // ┐
        string cantoInferiorEsquerdo = "\u2514";    // └
        string cantoInferiorDireito = "\u2518";     // ┘
        */

        string linhaHorizontal = "-";          // ─
        string linhaVertical = "|";            // │
        string cantoSuperiorEsquerdo = "+";    // ┌
        string cantoSuperiorDireito = "+";     // ┐
        string cantoInferiorEsquerdo = "+";    // └
        string cantoInferiorDireito = "+";     // ┘

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

    public static List<string> ProcessaMensagens(TextArea textArea, List<Mensagem> conversa)
    {
        List<string> textos = new();
        string lastRemetent = "";

        for (int i = 0; i < conversa.Count; i++)
        {
            var message = conversa[i];

            if (message is null || message.Content is null)
                continue;

            int maxCaracteresLinha = textArea.Size.x - 2;
            int paddingMessage = 1;
            int tamanhoMensagem = maxCaracteresLinha - paddingMessage * 2;
            string contentCopy = message.Content;

            if (message.RemetentId is not null)
            {
                if (message.RemetentId != lastRemetent)
                {
                    lastRemetent = message.RemetentId;
                    textos.Add((message.RemetentId + ":").PadRight(tamanhoMensagem + 2));
                }
            }
            else
            {
                lastRemetent = "Unknow";
                textos.Add(("Unknow" + ":").PadRight(tamanhoMensagem + 2));
            }

            while (contentCopy?.Length > 0)
            {
                string resultado = "";

                for (int j = 0; j < paddingMessage; j++)
                    resultado += " ";

                resultado += contentCopy.Substring(0, Math.Min(contentCopy.Length, tamanhoMensagem));

                for (int j = 0; j < paddingMessage; j++)
                    resultado += " ";

                resultado = resultado.PadRight(tamanhoMensagem + 2);

                textos.Add(resultado);
                contentCopy = contentCopy.Substring(Math.Min(contentCopy.Length, tamanhoMensagem));
            }

            var nextMessage = i + 1 < conversa.Count ? conversa[i + 1] : null;

            if (nextMessage != null)
                if (message.RemetentId == nextMessage.RemetentId)
                    continue;

            if (message.CreatedAt is not null)
                textos.Add($"{message.CreatedAt.Value.Hour}:{message.CreatedAt.Value.Minute}".PadLeft(maxCaracteresLinha));
        }
        return textos;
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


/*string textoTeste = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. " +
           "Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown " +
           "printer took a galley of type and scrambled it to make a type specimen book. It has survived " +
           "not only five centuries, but also the leap into electronic typesetting, remaining essentially " +
           "unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem " +
            "Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including " +
           "versions of Lorem Ipsum.";*/