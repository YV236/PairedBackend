using PairedBackend.Domain.Enums;

namespace PairedBackend.Domain.Entities;

public class Chat
{
    public Guid Id { get; private set; }

    private readonly List<Message> messages = new();
    public IReadOnlyCollection<Message> Messages => messages;

    private readonly List<ChatParticipant> participants = new();
    public IReadOnlyCollection<ChatParticipant> Participants => participants;

    private Chat() { }

    public Chat(Guid id, IEnumerable<Guid> participantIds)
    {
        Id = id;

        var distinct = participantIds.Distinct().ToList();
        if (!distinct.Any())
            throw new ArgumentException("Chat must have at least one participant");

        participants = distinct.Select(id => new ChatParticipant(id)).ToList();
    }

    public Message SendMessage(Guid senderId, string messageValue, MusicPlatform musicPlatform)
    {
        var participant = GetParticipant(senderId);

        if (participant.IsBlocked)
            throw new Exception("User is blocked in this chat");

        var message = new Message(Id, senderId, messageValue, musicPlatform);

        messages.Add(message);

        return message;
    }
    public void MarkMessageAsRead(Guid messageId, Guid userId)
    {
        var participant = GetParticipant(userId);

        if (participant.IsBlocked)
            throw new Exception("Blocked user cannot read messages");

        var message = messages.FirstOrDefault(x => x.Id == messageId);

        if (message is null)
            throw new Exception("Message not found");

        message.MarkAsRead(userId);
    }

    public void AddUser(Guid userId)
    {
        if (participants.Any(x => x.UserId == userId))
            throw new Exception("User already in chat");

        participants.Add(new ChatParticipant(userId));
    }
    public void BlockUser(Guid userId)
    {
        var participant = GetParticipant(userId);

        if (!participant.IsBlocked)
            participant.Block();
    }
    public void UnblockUser(Guid userId)
    {
        var participant = GetParticipant(userId);

        if (participant.IsBlocked)
            participant.Unblock();
    }

    private ChatParticipant GetParticipant(Guid userId)
    {
        var participant = participants.FirstOrDefault(x => x.UserId == userId);

        if (participant is null)
            throw new Exception("User is not in chat");

        return participant;
    }
}