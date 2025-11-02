namespace Impulse.SharedFramework.ProjectExplorer;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Impulse.SharedFramework.Services;
using Impulse.SharedFramework.Services.Layout;

internal static class ProjectExplorerDefaults
{
    private static readonly Uri DeleteIcon = new("pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Close.png", UriKind.Absolute);
    private static readonly Uri FolderIcon = new("pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Folder.png", UriKind.Absolute);
    private static readonly Uri RenameIcon = new("pack://application:,,,/Impulse.Dashboard;Component/Icons/Export/Open.png", UriKind.Absolute);

    public static IEnumerable<ProjectExplorerContextMenuItem> GetDefaultItems(ProjectExplorerItemBase item, IProjectExplorerService projectExplorerService)
    {
        yield return new ProjectExplorerContextMenuItem
        {
            Title = "Rename",
            Image = RenameIcon,
            Callback = () => projectExplorerService.BeginRenameAsync(item),
        };

        if (item is ProjectExplorerFolder)
        {
            yield return new ProjectExplorerContextMenuItem
            {
                Title = "Add Subfolder",
                Image = FolderIcon,
                Callback = async () =>
                {
                    await projectExplorerService.CreateFolderAsync(item);
                },
            };
        }

        yield return new ProjectExplorerContextMenuItem
        {
            Title = "Delete",
            Image = DeleteIcon,
            Callback = () => projectExplorerService.DeleteItemsAsync(new[] { item }),
        };
    }
}
