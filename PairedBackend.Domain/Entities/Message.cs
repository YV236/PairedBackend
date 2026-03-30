using PairedBackend.Domain.Enums;
using PairedBackend.Domain.ValueObjects;

namespace PairedBackend.Domain.Entities;

public class Message
{
    public Guid Id { get; private set; }
    public MessageValue Value { get; private set; }
    public Guid SenderId { get; private set; }
    public Guid ChatId { get; private set; }
    public DateTime SentDate { get; private set; }

    private readonly List<MessageRead> reads = [];

    public IReadOnlyCollection<MessageRead> Reads => reads;

    private Message() { }

    internal Message(Guid chatId, Guid senderId, string value, MusicPlatform platform)
    {
        Id = Guid.NewGuid();
        ChatId = chatId;
        SenderId = senderId;
        Value = new MessageValue(value, platform);
        SentDate = DateTime.UtcNow;
    }
    internal void MarkAsRead(Guid userId)
    {
        if (reads.Any(x => x.UserId == userId))
            return;

        reads.Add(new MessageRead(Id, userId));
    }
}