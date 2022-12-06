using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.MultimediaProvider.Stores;

public interface IMultimediaStore
{
    Stream GetMultimedia(string multimediaId, MultimediaForEnum multimediaFor, string fileName);
    string? GetMultimediaPath(string multimediaId, MultimediaForEnum multimediaFor, string fileName);

    string CreateMultimedia();

    Task SaveMultimedia(string multimediaId, MultimediaForEnum multimediaFor, string fileName, Stream fileStream);

    IEnumerable<(MultimediaForEnum, string)> ListMultimedia(string multimediaId);

    string? GetFolderPath(string multimediaId);
}

