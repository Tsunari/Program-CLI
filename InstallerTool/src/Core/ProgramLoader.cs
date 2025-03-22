using System.Text.Json;

namespace InstallerTool.Core;

public static class ProgramLoader
{
    public static List<Software> LoadSoftwareList(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Program file not found: {path}");

        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<Software>>(json) ?? new List<Software>();
    }
}
