using Backend.Core.CQRS;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Backend.Application.Commands.Authentication.CreateUserAccount;

public record CreateUserAccountCommand : Command
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CreateUserAccountCommandHandler : CommandHandler<CreateUserAccountCommand>
{
    private readonly UserManager<IdentityUser> userManager;

    public override async Task<EmptyResponse> Handle(CreateUserAccountCommand request, CancellationToken cancellationToken)
    {
        await CreateUserAsync(request);

        return EmptyResponse.Instance;
    }

    private async Task<IdentityUser> CreateUserAsync(CreateUserAccountCommand request)
    {
        IdentityUser identityUser = new()
        {
            Email = request.Email,
            UserName = request.Email,
            EmailConfirmed = true,
        };

        var result = await userManager.CreateAsync(identityUser, request.Password);

        await userManager.AddToRoleAsync(identityUser, "Driver");

        return identityUser;
    }
}