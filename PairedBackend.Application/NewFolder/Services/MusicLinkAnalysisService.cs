using PairedBackend.Domain.Enums;
using System.Text.RegularExpressions;

namespace PairedBackend.Application.NewFolder.Services;

internal class MusicLinkAnalysisService : IMusicLinkAnalysisService
{
    public MusicPlatform FindMusicPlatform(string messageText)
    {
        if (!CheckIfContainsLink(messageText))
        {
            return MusicPlatform.None;
        }

        if (messageText.Contains("spotify.com"))
        {
            return MusicPlatform.Spotify;
        }

        if (messageText.Contains("music.youtube.com"))
        {
            return MusicPlatform.YoutubeMusic;
        }

        if (messageText.Contains("youtube.com"))
        {
            return MusicPlatform.Youtube;
        }

        return MusicPlatform.None;
    }

    private bool CheckIfContainsLink(string messageText)
    {
        var pattern = @"https?:\/\/[^\s]+";

        return Regex.IsMatch(messageText, pattern, RegexOptions.IgnoreCase);
    }
}
