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

    public async Task<IEnumerable<TopicDto>?> ParseCsvFile(string fileName)
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        };
        var topics = new List<TopicDto>();

        using (var reader = new StreamReader(Path.Combine(_appPath, _importFolder, fileName)))
        using (var csvReader = new CsvReader(reader, configuration))
        {
            try
            {
                var records = csvReader.GetRecords<CsvQuestion>().ToList();
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
            catch (CsvHelperException e)
            {
                //TODO rewrite to log
                Console.WriteLine($"{e.Message} " + (e.InnerException == null ? string.Empty : e.InnerException.Message));
                Console.WriteLine($"Row: {csvReader.Context.Parser.Row}; RawLine: {csvReader.Context.Parser.RawRecord}");
                if (csvReader.Context.Reader.CurrentIndex >= 0 &&
                    csvReader.Context.Reader.CurrentIndex < csvReader.Context.Reader.HeaderRecord.Length)
                {
                    Console.WriteLine($"Column: {csvReader.Context.Reader.CurrentIndex}; ColumnName: {csvReader.Context.Reader.HeaderRecord[csvReader.Context.Reader.CurrentIndex]}");
                }
                return null;
            }
        }
        return topics;
    }
}