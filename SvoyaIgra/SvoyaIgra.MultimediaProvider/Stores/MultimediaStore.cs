using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.MultimediaProvider.Stores;

public class MultimediaStore: IMultimediaStore
{
    private readonly IFileProvider _fileProvider;
    private readonly MultimediaStoreOptions _multimediaStoreOptions;

    public MultimediaStore(IFileProvider fileProvider, IOptions<MultimediaStoreOptions> options)
    {
        _fileProvider = fileProvider;
        _multimediaStoreOptions = options.Value;
    }

    public Stream GetMultimedia(string multimediaId, MultimediaForEnum multimediaFor, string fileName)
    {
        try
        {
            var fileInfo = _fileProvider.GetFileInfo(Path.Combine(multimediaId, multimediaFor.ToString(), fileName));            
            return fileInfo.CreateReadStream();
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }
    public string? GetMultimediaPath(string multimediaId, MultimediaForEnum multimediaFor, string fileName)
    {
        try
        {
            var fileInfo = _fileProvider.GetFileInfo(Path.Combine(multimediaId, multimediaFor.ToString(), fileName));
            return fileInfo.PhysicalPath;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    public async Task SaveMultimedia(string multimediaId, MultimediaForEnum multimediaFor, string fileName, Stream fileStream)
    {
        var path = Path.Combine(_multimediaStoreOptions.RootPath, multimediaId, multimediaFor.ToString(), fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        await using var file = new FileStream(path, FileMode.CreateNew);
        await fileStream.CopyToAsync(file);
    }

    public IEnumerable<(MultimediaForEnum, string)> ListMultimedia(string multimediaId)
    {
        var questionContents = _fileProvider.GetDirectoryContents(Path.Combine(multimediaId, MultimediaForEnum.Question.ToString()));
        var questionList = questionContents.Where(c => !c.IsDirectory).Select(c => (MultimediaForEnum.Question, c.Name));

        var answerContents = _fileProvider.GetDirectoryContents(Path.Combine(multimediaId, MultimediaForEnum.Answer.ToString()));
        var answerList = answerContents.Where(c => !c.IsDirectory).Select(c => (MultimediaForEnum.Answer, c.Name));

        return questionList.Union(answerList);
    }

    public string CreateMultimedia()
    {
        var multimediaId = Guid.NewGuid().ToString().ToUpperInvariant();
        var pathQuestion = Path.Combine(_multimediaStoreOptions.RootPath, multimediaId, MultimediaForEnum.Question.ToString());
        Directory.CreateDirectory(pathQuestion);
        var pathAnswer = Path.Combine(_multimediaStoreOptions.RootPath, multimediaId, MultimediaForEnum.Answer.ToString());
        Directory.CreateDirectory(pathAnswer);

        return multimediaId;
    }

    public string? GetFolderPath(string multimediaId)
    {
        var path = Path.Combine(_multimediaStoreOptions.RootPath, multimediaId);
        return path;
    }
}