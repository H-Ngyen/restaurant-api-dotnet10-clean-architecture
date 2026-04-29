using MediatR;

namespace Application.Users.Commands.UnassignUserRole;

public class UnassignUserRoleCommand : IRequest
{
    public required string UserEmail { get; set; }
    public required string UserRole { get; set; }
}