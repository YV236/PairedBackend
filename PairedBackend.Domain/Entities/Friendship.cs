namespace PairedBackend.Domain.Entities;

public class Friendship
{
    public Guid UserId { get; private set; }
    public Guid FriendId { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private Friendship() { }

    public Friendship(Guid userId, Guid friendId)
    {
        if (userId == friendId)
            throw new ArgumentException("Cannot be friend with yourself");

        UserId = userId;
        FriendId = friendId;
        CreatedAt = DateTime.UtcNow;
    }
}
