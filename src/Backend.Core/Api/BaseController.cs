using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Core.Api;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseController(IMediator mediator) : ControllerBase
{
    protected readonly IMediator _mediator = mediator;
}