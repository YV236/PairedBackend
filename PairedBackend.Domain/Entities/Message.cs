using PairedBackend.Domain.Enums;
using PairedBackend.Domain.ValueObjects;

public class Message
{
    public Guid Id { get; private set; }
    public MessageValue Value { get; private set; }
    public Guid SenderId { get; private set; }
    public Guid ChatId { get; private set; }
    public DateTime SentDate { get; private set; }

    private Message() { }

    internal Message(Guid chatId, Guid senderId, string value, MusicPlatform platform)
    {
        Id = Guid.NewGuid();
        ChatId = chatId;
        SenderId = senderId;
        Value = new MessageValue(value, platform);
        SentDate = DateTime.UtcNow;
    }
}