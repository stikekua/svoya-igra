using SvoyaIgra.Shared.Entities;

namespace SvoyaIgra.MultimediaProvider.Helpers;

public class FileExtensionMediaTypeProvider
{
    private static string[] imageExtensions = {
        ".PNG", ".JPG", ".JPEG", ".BMP", ".GIF", //etc
    };
    private static string[] audioExtensions = {
        ".WAV", ".MID", ".MIDI", ".WMA", ".MP3", ".OGG", ".RMA", //etc
    };
    private static string[] videoExtensions = {
        ".AVI", ".MP4", ".DIVX", ".WMV", //etc
    };

    public bool TryGetMediaType(string subpath, out MediaType mediaType)
    {
        if (imageExtensions.Contains(Path.GetExtension(subpath), StringComparer.OrdinalIgnoreCase))
        {
            mediaType = MediaType.Image;
            return true;
        }
        if (audioExtensions.Contains(Path.GetExtension(subpath), StringComparer.OrdinalIgnoreCase))
        {
            mediaType = MediaType.Audio;
            return true;
        }
        if (videoExtensions.Contains(Path.GetExtension(subpath), StringComparer.OrdinalIgnoreCase))
        {
            mediaType = MediaType.Video;
            return true;
        }

        mediaType = MediaType.None;
        return false;
    }
}