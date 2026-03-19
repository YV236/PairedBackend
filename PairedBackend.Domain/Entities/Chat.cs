using PairedBackend.Domain.Enums;

namespace PairedBackend.Domain.Entities;

public class Chat
{
    public Guid Id { get; private set; }

    private readonly List<Message> messages = new();

    public IReadOnlyCollection<Message> Messages => messages;

    private readonly List<Guid> participants = new();

    public IReadOnlyCollection<Guid> Participants => participants;

    private Chat() { }

    public Chat(Guid id, IEnumerable<Guid> participants)
    {
        Id = id;
        this.participants = participants.ToList();
    }

    public Message SendMessage(Guid senderId, string messageValue, MusicPlatform musicPlatform)
    {
        if (!participants.Contains(senderId))
            throw new Exception("User is not in chat");

        var message = new Message(Id, senderId, messageValue, musicPlatform);

        messages.Add(message);

        return message;
    }
}