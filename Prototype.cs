// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Spectre.Console;


class Program
{
    static async Task Main()
    {
        AnsiConsole.Markup("[bold green]Welcome to the Installer Tool![/]\n");

        string jsonPath = "software_list.json";
        if (!File.Exists(jsonPath))
        {
            Console.WriteLine("Error: software_list.json not found!");
            return;
        }

        string jsonContent = File.ReadAllText(jsonPath);
        List<Software> softwareList = JsonSerializer.Deserialize<List<Software>>(jsonContent);
        
        Console.WriteLine("Select the software to install:");
        for (int i = 0; i < softwareList.Count; i++)
            Console.WriteLine($"[{i + 1}] {softwareList[i].Name}");

        Console.Write("Enter numbers separated by spaces (e.g., 1 3): ");
        string input = Console.ReadLine();
        var selectedIndexes = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string index in selectedIndexes)
        {
            if (int.TryParse(index, out int i) && i > 0 && i <= softwareList.Count)
            {
                Software software = softwareList[i - 1];
                await DownloadAndInstall(software);
            }
        }

        Console.WriteLine("Installation process completed.");
    }

    static async Task DownloadAndInstall(Software software)
    {
        string fileName = Path.GetFileName(new Uri(software.Url).LocalPath);
        string filePath = Path.Combine("Installers", fileName);

        Console.WriteLine($"Downloading {software.Name}...");
        Directory.CreateDirectory("Installers");

        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(software.Url);
            await using var fs = new FileStream(filePath, FileMode.Create);
            await response.Content.CopyToAsync(fs);
        }

        Console.WriteLine($"Installing {software.Name}...");
        Process.Start(new ProcessStartInfo
        {
            FileName = filePath,
            Arguments = software.InstallCommand,
            UseShellExecute = false,
            CreateNoWindow = true
        })?.WaitForExit();
        
        Console.WriteLine($"{software.Name} installed successfully!");
    }
}

class Software
{
    public string Name { get; set; }
    public string Url { get; set; }
    public string InstallCommand { get; set; }
}

