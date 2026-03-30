using PairedBackend.Domain.Shared;

namespace PairedBackend.Domain.Entities;

public class Friendship
{
    public Guid UserId { get; private set; }
    public Guid FriendId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Friendship() { }

    internal Friendship(Guid userId, Guid friendId)
    {
        UserId = userId;
        FriendId = friendId;
        CreatedAt = DateTime.UtcNow;
    }

    public static Result<Friendship> Create(Guid userId, Guid friendId)
    {
        if (userId == friendId)
            return Result.Failure<Friendship>(new("Friendship exception","Cannot be friend with yourself"));

        return Result.Success(new Friendship(userId, friendId));
    }
}
