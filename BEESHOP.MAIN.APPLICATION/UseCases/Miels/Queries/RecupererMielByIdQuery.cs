using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Queries;

public record RecupererMielByIdQuery(Guid Id) : IRequest<MielDto?>;