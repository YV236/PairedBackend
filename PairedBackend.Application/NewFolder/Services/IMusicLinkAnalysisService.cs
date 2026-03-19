using PairedBackend.Domain.Enums;

namespace PairedBackend.Application.NewFolder.Services;

internal interface IMusicLinkAnalysisService
{
    public MusicPlatform FindMusicPlatform(string messageText);
}
