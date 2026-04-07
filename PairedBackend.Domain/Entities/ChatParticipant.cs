using PairedBackend.Domain.Shared;

namespace PairedBackend.Domain.Entities;

public class ChatParticipant
{
    public Guid ChatId { get; }
    public Guid UserId { get; }
    public bool IsBlocked { get; private set; }

    private ChatParticipant() { }

    internal ChatParticipant(Guid userId, Guid chatId)
    {
        ChatId = chatId;
        UserId = userId;
    }

    public void Block() => IsBlocked = true;

    public void Unblock() => IsBlocked = false;
}