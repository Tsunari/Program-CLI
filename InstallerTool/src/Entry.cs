using InstallerTool.Core;
using InstallerTool.UI;

class Entry
{
    private static bool WingetOn = true;
    static async Task Main()
    {
        string configPath = "software_list.json";
        var softwareList = ProgramLoader.LoadSoftwareList(configPath);
        var selectedSoftware = Menu.DisplaySoftwareList(softwareList);
        string downloadPath = UserInput.GetDownloadPath();

        foreach (var software in selectedSoftware)
            if (WingetOn && !string.IsNullOrEmpty(software.WingetCommandId))
                await Installer.DownloadAndInstallWinget(software);
            else await Installer.DownloadAndInstallExe(software, downloadPath);
    }
}
