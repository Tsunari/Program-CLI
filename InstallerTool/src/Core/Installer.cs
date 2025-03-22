using System.Diagnostics;
using System.Net.Http;

namespace InstallerTool.Core;

public static class Installer
{
    public static async Task DownloadAndInstallWinget(Software software)
    {
        Console.WriteLine($"Starting installation of {software.Name} using Winget...");

        // Construct the winget command
        string wingetCommand = $"install --id={software.WingetCommandId} -e";

        Console.WriteLine($"Executing Winget command: winget {wingetCommand}");

        // Start the process
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = "winget",
            Arguments = wingetCommand,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        });

        if (process != null)
        {
            Console.WriteLine($"Winget process started for {software.Name}...");

            // Capture output for debugging or logging
            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();

            process.WaitForExit();

            Console.WriteLine($"Winget process finished for {software.Name}.");

            // Check the exit code
            if (process.ExitCode == 0)
            {
                Console.WriteLine($"{software.Name} was installed successfully using Winget!");
            }
            else
            {
                Console.WriteLine($"Failed to install {software.Name} using Winget. Exit code: {process.ExitCode}");
                if (error.Length > 0) Console.WriteLine($"Error: {error}");
            }
        }
        else
        {
            Console.WriteLine($"Failed to start the Winget installation process for {software.Name}.");
        }
    }

    public static async Task DownloadAndInstallExe(Software software, string downloadPath)
    {
        string fileName = Path.GetFileName(new Uri(software.Url).LocalPath);
        string filePath = Path.Combine(downloadPath, fileName);

        Console.WriteLine($"Starting download of {software.Name}...");
        Directory.CreateDirectory(downloadPath);

        using HttpClient client = new();
        var response = await client.GetAsync(software.Url);

        Console.WriteLine($"Download started for {software.Name}...");
        await using (var fs = new FileStream(filePath, FileMode.Create))
        {
            await response.Content.CopyToAsync(fs);
        } // Ensure the FileStream is disposed before proceeding.
        Console.WriteLine($"Download finished for {software.Name}.");

        Console.WriteLine($"Starting installation of {software.Name}...");
        var process = Process.Start(new ProcessStartInfo
        {
            FileName = filePath,
            Arguments = software.InstallCommand,
            UseShellExecute = false,
            CreateNoWindow = true
        });

        if (process != null)
        {
            Console.WriteLine($"Installation process started for {software.Name}...");
            process.WaitForExit();
            Console.WriteLine($"Installation process finished for {software.Name}.");

            // Check the exit code
            if (process.ExitCode == 0)
            {
                Console.WriteLine($"{software.Name} was installed successfully!");
            }
            else
            {
                Console.WriteLine($"Failed to install {software.Name}. Exit code: {process.ExitCode}");
            }
        }
        else
        {
            Console.WriteLine($"Failed to start the installation process for {software.Name}.");
        }
    }
}
