using SvoyaIgra.MultimediaProvider.Entities;
using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.MultimediaProvider.Services;

public interface IMultimediaService
{
    MultimediaConfig GetMultimediaConfig(string multimediaId);
    (Stream stream, MediaType mediaType) GetMultimedia(string multimediaId, MultimediaForEnum multimediaFor, string fileName);
    Task SaveMultimedia(string multimediaId, MultimediaForEnum multimediaFor, string fileName, Stream fileStream);
}