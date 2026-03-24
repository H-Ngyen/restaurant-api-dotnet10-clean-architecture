using MediatR;

namespace Application.Users.Commands.AssignUserRole;

public class AssignUserRoleCommand : IRequest
{
    public required string Email { get; set; }
    public required string Role { get; set; }   
}