using ImpoClaude;

Console.WriteLine("Programa Avançado de Imposição de Páginas PDF");
Console.WriteLine("=============================================");

try
{
    // Obter configurações do usuário
    ImpositionSettings settings = UserInterface.GetUserSettings();

    // Executar imposição com base nas configurações
    ImpositionEngine.RunImposition(settings);

    Console.WriteLine("Imposição concluída com sucesso!");
}
catch (Exception ex)
{
    Console.WriteLine($"Erro: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
}

Console.WriteLine("Pressione qualquer tecla para sair...");
Console.ReadKey();
        
