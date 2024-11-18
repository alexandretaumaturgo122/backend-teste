using MediatR;

namespace Backend.Application.Commands.Account.Deposit;

public record DepositCommand(decimal Amount);

public class DepositCommandHandler
{
    
}