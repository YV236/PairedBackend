namespace PairedBackend.Domain.Entities;

public class MessageRead
{
    public Guid MessageId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime ReadAt { get; private set; }

    private MessageRead() { }

    internal MessageRead (Guid messageId, Guid userId)
    {
        MessageId = messageId;
        UserId = userId;
        ReadAt = DateTime.UtcNow;
    }
}
