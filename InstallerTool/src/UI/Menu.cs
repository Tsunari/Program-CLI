using InstallerTool.Core;
using Spectre.Console;

namespace InstallerTool.UI;

public static class Menu
{
    public static List<Software> DisplaySoftwareList(List<Software> softwareList)
    {
        // Create a panel with a title
        var panel = new Panel("[bold green]Select the software you want to install:[/]")
            .Border(BoxBorder.Rounded)
            .BorderStyle(Style.Parse("blue"))
            .Header("[bold yellow]Software Installation Menu[/]", Justify.Center)
            .Padding(1, 1);

        // Display the panel
        AnsiConsole.Write(panel);

        // Display the multi-selection prompt
        var selected = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[bold green]Select software to install:[/]")
                .PageSize(10)
                .InstructionsText("[grey](Use [blue]space[/] to select, [green]enter[/] to confirm, [red]ctrl + c[/] to exit)[/]")
                .AddChoices(softwareList.Select(s => s.Name).ToArray()));

        // Return the selected software
        return softwareList.Where(s => selected.Contains(s.Name)).ToList();
    }
}