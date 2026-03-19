using PairedBackend.Domain.Enums;

namespace PairedBackend.Domain.ValueObjects;

public class MessageValue
{
    public string Value { get; }

    public bool ContainsLink { get; }

    public MusicPlatform MusicPlatform { get; }

    private MessageValue() { }

    internal MessageValue(string value, MusicPlatform platform)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Message cannot be empty");

        Value = value;
        MusicPlatform = platform;
        ContainsLink = platform != MusicPlatform.None;
    }
}
