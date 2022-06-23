using SvoyaIgra.Dal.Dto;

namespace SvoyaIgra.ImportCSV.Services;

public interface IImportService
{
    public Task<IEnumerable<string>> GetFilesInImportFolder();
    public Task<IEnumerable<TopicDto>?> ParseCsvFile(string fileName);

}