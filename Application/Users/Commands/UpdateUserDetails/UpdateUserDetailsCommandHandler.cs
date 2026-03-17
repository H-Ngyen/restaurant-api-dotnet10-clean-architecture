using AutoMapper;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.UpdateUserDetails;

public class UpdateUserDetailsCommandHandler(ILogger<UpdateUserDetailsCommandHandler> logger,
    IUserContext userContext,
    IUserStore<User> userStore,
    IMapper mapper) : IRequestHandler<UpdateUserDetailsCommand>
{
    public async Task Handle(UpdateUserDetailsCommand request, CancellationToken cancellationToken)
    {
        var user = userContext.GetCurrentUser();
        
        logger.LogInformation("Updating user: {userId}, with {@Request}", user!.Id, request);
        
        var dbUser = await userStore.FindByIdAsync(user!.Id, cancellationToken) 
            ?? throw new NotFoundException(nameof(User), user!.Id.ToString());
    
        mapper.Map(request, dbUser);
        await userStore.UpdateAsync(dbUser, cancellationToken);
    }
}