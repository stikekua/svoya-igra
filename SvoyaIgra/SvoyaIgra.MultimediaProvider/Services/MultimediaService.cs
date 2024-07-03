using SvoyaIgra.MultimediaProvider.Entities;
using SvoyaIgra.MultimediaProvider.Helpers;
using SvoyaIgra.MultimediaProvider.Stores;
using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.MultimediaProvider.Services;

public class MultimediaService : IMultimediaService
{
    private readonly IMultimediaStore _multimediaStore;
    public MultimediaService(IMultimediaStore multimediaStore)
    {
        _multimediaStore = multimediaStore;
    }

    public MultimediaConfig GetMultimediaConfig(string multimediaId)
    {
        var folderPath = _multimediaStore.GetFolderPath(multimediaId) ?? "";
        var files = _multimediaStore.ListMultimedia(multimediaId).ToList();
        return new MultimediaConfig
        {
            FolderPath = folderPath,
            QuestionFiles = files.Where(f=>f.Item1 == MultimediaForEnum.Question).Select(x=>x.Item2),
            AnswerFiles = files.Where(f => f.Item1 == MultimediaForEnum.Answer).Select(x => x.Item2),
        };
    }

    public (Stream stream, MediaType mediaType) GetMultimedia(string multimediaId, MultimediaForEnum multimediaFor, string fileName)
    {
        var provider = new FileExtensionMediaTypeProvider();
        if (!provider.TryGetMediaType(fileName, out var mediaType))
        {
            throw new Exception($"Unknown media type for multimedia {multimediaId}/{multimediaFor}/{fileName}");
        }
        var stream = _multimediaStore.GetMultimedia(multimediaId, multimediaFor, fileName);
        return (stream, mediaType);
    }

    public (string? path, MediaType mediaType) GetMultimediaPath(string multimediaId, MultimediaForEnum multimediaFor, string fileName)
    {
        var provider = new FileExtensionMediaTypeProvider();
        if (!provider.TryGetMediaType(fileName, out var mediaType))
        {
            throw new Exception($"Unknown media type for multimedia {multimediaId}/{multimediaFor}/{fileName}");
        }
        var path = _multimediaStore.GetMultimediaPath(multimediaId, multimediaFor, fileName);
        if (path == null)
        {
            return (path, MediaType.None);
        }

        return (path, mediaType);
    }

    public Task SaveMultimedia(string multimediaId, MultimediaForEnum multimediaFor, string fileName, Stream fileStream)
    {
        return _multimediaStore.SaveMultimedia(multimediaId, multimediaFor, fileName, fileStream);
    }

    public string CreateMultimedia()
    {
        return _multimediaStore.CreateMultimedia();
    }

    public string[] GetMultimedias()
    {
        return _multimediaStore.GetMultimedias();
    }
}