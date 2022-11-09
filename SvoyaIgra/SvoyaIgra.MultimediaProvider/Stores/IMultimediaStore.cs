using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.MultimediaProvider.Stores;

internal interface IMultimediaStore
{
    Stream GetMultimedia(string multimediaId, MultimediaForEnum multimediaFor, string fileName);
    Task SaveMultimedia(string multimediaId, MultimediaForEnum multimediaFor, string fileName, Stream fileStream);

    IEnumerable<(MultimediaForEnum, string)> ListMultimedia(string multimediaId);
}

