using Backend.Application.Commands.Authentication.CreateUserAccount;
using Backend.Application.Commands.Authentication.Login;
using Backend.Core.Api;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.Api.Controllers.Authentication;

public class AuthenticationController : BaseController
{
    public AuthenticationController(IMediator mediator) : base(mediator) { }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken = default)
    {
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateUserAccountCommand command, CancellationToken cancellationToken = default)
    {

    }
}
