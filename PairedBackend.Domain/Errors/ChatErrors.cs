using PairedBackend.Domain.Shared;

namespace PairedBackend.Domain.Errors;

public static class ChatErrors
{
    public static readonly Error UserAlreadyInChat = new("Chat.UserAlreadyInChat", "User is already a participant of this chat.");
    public static readonly Error UserNotFound = new("Chat.UserNotFound", "User is not in this chat.");
    public static readonly Error UserBlocked = new("Chat.UserBlocked", "Action cannot be performed because the user is blocked.");
    public static readonly Error MessageNotFound = new("Chat.MessageNotFound", "The specified message was not found in this chat.");
}
