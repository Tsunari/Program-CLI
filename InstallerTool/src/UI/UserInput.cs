namespace InstallerTool.UI;

public static class UserInput
{
    private static bool UserInputOn = false;
    public static string GetDownloadPath()
    {
        if (UserInputOn)
        {
            Console.Write("Enter the download folder path (or leave blank for standart): ");
            return Console.ReadLine() ?? "Installers";
        } else
        {
            return "Installers";
        }
    }

}
