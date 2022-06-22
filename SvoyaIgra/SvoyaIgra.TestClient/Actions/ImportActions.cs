using SvoyaIgra.ImportCSV.Services;

namespace SvoyaIgra.TestClient.Actions;

public class ImportActions:IImportActions
{
    private readonly IImportService _importService;

    public ImportActions(IImportService importService)
    {
        _importService = importService;
    }

    public void PerformImportAction()
    {

        var fileName = ShowFilesAsync().Result;
        if (fileName == "") return;
        
        ImportFile(fileName).Wait();
    }

    private async Task<string> ShowFilesAsync()
    {
        Ui.Clear();
        Ui.Write("Import");
        Ui.Write("Files found:");
        var files = (await _importService.GetFilesInImportFolder()).ToArray();
        for (int i = 0; i < files.Count(); i++)
        {
            Ui.Write(FormatFile(i+1, files[i]));
        }
        Ui.Write(" 0. <- BACK");
        Ui.Write();
        var selected = Ui.Choice(files.Length);
        if (selected == 0) return "";
        return files[selected-1];
    }

    private string FormatFile(int n, string name)
    {
        return $" {n}. {name,-20} ";
    }

    private async Task ImportFile(string fileName)
    {
        var topics = await _importService.ParseCsvFile(fileName);

        //TODO push to DB via services
    }
}