using MediatR;
using PairedBackend.Application.Services;
using PairedBackend.Domain.Shared;

namespace PairedBackend.Application.Features.User.ChangeData;

internal class ChangeUserProfileDataCommandHandler(IUserService userService) : IRequestHandler<ChangeUserProfileDataCommand, Result>
{
    public async Task<Result> Handle(ChangeUserProfileDataCommand request, CancellationToken cancellationToken)
    {
        return await userService.ChangeUserProfileData(request.UserId, request.FirstName, request.LastName, request.UserName);
    }
}
