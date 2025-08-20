using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;

public record SupprimerMielCommand(Guid Id) : IRequest;