using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace UTF16BoxDrawing
{
    public static class Database
    {
        private const string FilePath = "mensagens.json";

        public static void Initialize()
        {
            if (!File.Exists(FilePath))
            {
                // Cria o arquivo e escreve uma lista vazia
                SalvarMensagens(new List<Mensagem>());
            }
        }

        public static int SalvarMensagem(Mensagem mensagem)
        {
            // Lê as mensagens existentes do arquivo
            List<Mensagem> mensagens = CarregarMensagens();

            // Gera um novo ID
            int generatedId = mensagens.Count + 1;

            // Adiciona a nova mensagem à lista
            mensagem.Id = generatedId;
            mensagens.Add(mensagem);

            // Salva a lista atualizada de mensagens no arquivo
            SalvarMensagens(mensagens);

            return generatedId;
        }

        public static List<Mensagem> GetMensagens()
        {
            // Lê as mensagens do arquivo
            return CarregarMensagens();
        }

        private static List<Mensagem> CarregarMensagens()
        {
            try
            {
                // Lê o conteúdo do arquivo
                string json = File.ReadAllText(FilePath);

                // Desserializa o JSON para obter a lista de mensagens
                return JsonSerializer.Deserialize<List<Mensagem>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar mensagens: {ex.Message}");
                return new List<Mensagem>();
            }
        }

        private static void SalvarMensagens(List<Mensagem> mensagens)
        {
            try
            {
                // Serializa a lista de mensagens para JSON
                string json = JsonSerializer.Serialize(mensagens);

                // Salva o JSON no arquivo
                File.WriteAllText(FilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao salvar mensagens: {ex.Message}");
            }
        }
    }
}
