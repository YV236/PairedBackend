using PairedBackend.Domain.Enums;
using PairedBackend.Domain.Errors;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Domain.Entities;

public class Chat
{
    public Guid Id { get; private set; }

    private readonly List<Message> messages = [];
    public IReadOnlyCollection<Message> Messages => messages;

    private readonly List<ChatParticipant> participants = [];
    public IReadOnlyCollection<ChatParticipant> Participants => participants;

    private Chat() { }

    public Chat(Guid id, IEnumerable<Guid> participantIds)
    {
        Id = id;

        var distinct = participantIds.Distinct().ToList();
        if (!distinct.Any())
            throw new ArgumentException("Chat must have at least one participant");

        participants = distinct.Select(id => new ChatParticipant(id, Id)).ToList();
    }

    public Result<Message> SendMessage(Guid senderId, string messageValue, MusicPlatform musicPlatform)
    {
        var participantResult = GetParticipant(senderId);

        if (participantResult.IsFailure)
            return Result.Failure<Message>(participantResult.Error);

        if (participantResult.Value.IsBlocked)
            return Result.Failure<Message>(ChatErrors.UserBlocked);

        var message = new Message(Id, senderId, messageValue, musicPlatform);
        messages.Add(message);

        return message;
    }
    public Result MarkMessageAsRead(Guid messageId, Guid userId)
    {
        var result = GetParticipant(userId);
        if (result.IsFailure)
            return Result.Failure<bool>(result.Error);

        if (result.Value.IsBlocked) 
            return Result.Failure(ChatErrors.UserBlocked);

        var message = messages.FirstOrDefault(x => x.Id == messageId);
        if (message is null) return Result.Failure(ChatErrors.MessageNotFound);

        message.MarkAsRead(userId);
        return Result.Success();
    }

    public Result AddUser(Guid userId)
    {
        if (participants.Any(x => x.UserId == userId)) 
            return Result.Failure(ChatErrors.UserAlreadyInChat);

        participants.Add(new ChatParticipant(userId, Id));
        return Result.Success();
    }
    public Result BlockUser(Guid userId)
    {
        var result = GetParticipant(userId);

        if (result.IsFailure) 
            return Result.Failure(result.Error);

        if (!result.Value.IsBlocked)
        {
            result.Value.Block();
            return Result.Success();
        }

        return Result.Success();
    }
    public Result UnblockUser(Guid userId)
    {
        var result = GetParticipant(userId);

        if (result.IsFailure) 
            return Result.Failure(result.Error);

        if (result.Value.IsBlocked)
        {
            result.Value.Unblock();
            return Result.Success();
        }

        return Result.Success();
    }

    private Result<ChatParticipant> GetParticipant(Guid userId)
    {
        var participant = participants.FirstOrDefault(x => x.UserId == userId);

        if (participant is null)
            return Result.Failure<ChatParticipant>(ChatErrors.UserNotFound);

        return participant;
    }
}