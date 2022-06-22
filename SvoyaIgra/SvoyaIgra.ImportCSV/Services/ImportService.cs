using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using SvoyaIgra.Dal.Bo;
using SvoyaIgra.Dal.Dto;
using SvoyaIgra.ImportCSV.Dto;

namespace SvoyaIgra.ImportCSV.Services;

public class ImportService : IImportService
{
    readonly string _appPath = Directory.GetCurrentDirectory();
    readonly string _importFolder = "Import";

    public async Task<IEnumerable<string>> GetFilesInImportFolder()
    {
        DirectoryInfo dir = new DirectoryInfo(Path.Combine(_appPath, _importFolder));
        var files = dir.GetFiles();
        return files.Select(f=>f.Name);
    }

    public async Task<IEnumerable<TopicDto>> ParseCsvFile(string fileName)
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };
        var topics = new List<TopicDto>();

        using (var reader = new StreamReader(Path.Combine(_appPath, _importFolder, fileName)))
        using (var csv = new CsvReader(reader, configuration))
        {
            var records = csv.GetRecords<CsvQuestion>();
            var gropedRecords = records.GroupBy(x => x.Topic);
            foreach (var record in gropedRecords)
            {
                topics.Add(new TopicDto
                {
                    Name = record.Key,
                    Difficulty = (TopicDifficulty)record.FirstOrDefault()!.Round,
                    Questions = record.Select(question => new QuestionDto
                    {
                        Type = Enum.Parse<QuestionType>(question.QuestionType),
                        Difficulty = (QuestionDifficulty)question.QuestionDifficulty,
                        Text = question.Question,
                        Answer = question.Answer
                    })
            });
            }
        }

        return topics;
    }
}