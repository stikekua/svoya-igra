using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SvoyaIgra.MultimediaProvider.Services;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace SvoyaIgra.MultimediaViewer.ViewModel;

[INotifyPropertyChanged]
public partial class EditorViewModel
{
    private readonly IMultimediaService _multimediaService;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(OpenFolderCommand))]
    [NotifyCanExecuteChangedFor(nameof(CopyToClipboardCommand))]
    private string _selected_multimedia;

    [ObservableProperty]
    private IEnumerable<string> _multimedias = new List<string>();

    public EditorViewModel(IMultimediaService multimediaService)
    {
        _multimediaService = multimediaService;
    }

    [RelayCommand]
    private void CreateMultimedia(object obj)
    {
        var ms = Multimedias.ToList();
        var newMultimedia = _multimediaService.CreateMultimedia();
        ms.Add(newMultimedia);
        Multimedias = ms;        
    }

    [RelayCommand(CanExecute = nameof(CanOpenFolder))]
    private void OpenFolder(object obj)
    {
        var mutimediaCfg = _multimediaService.GetMultimediaConfig(Selected_multimedia);
        var path = mutimediaCfg.FolderPath;
        if (Directory.Exists(path))
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = path,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        }
        else
        {
            //MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
        }
    }
    public bool CanOpenFolder(object obj)
    {
        return !string.IsNullOrWhiteSpace(Selected_multimedia);
    }

    [RelayCommand(CanExecute = nameof(CanOpenFolder))]
    private void CopyToClipboard(object obj)
    {
        Clipboard.SetText(Selected_multimedia);
    }
}