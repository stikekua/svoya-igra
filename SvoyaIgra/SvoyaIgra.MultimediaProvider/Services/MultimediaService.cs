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
        var files = _multimediaStore.ListMultimedia(multimediaId).ToList();
        return new MultimediaConfig
        {
            QuestionFile = files.FirstOrDefault(f=>f.Item1 == MultimediaForEnum.Question).Item2,
            AnswerFile = files.FirstOrDefault(f => f.Item1 == MultimediaForEnum.Answer).Item2
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

    public Task SaveMultimedia(string multimediaId, MultimediaForEnum multimediaFor, string fileName, Stream fileStream)
    {
        return _multimediaStore.SaveMultimedia(multimediaId, multimediaFor, fileName, fileStream);
    }
}