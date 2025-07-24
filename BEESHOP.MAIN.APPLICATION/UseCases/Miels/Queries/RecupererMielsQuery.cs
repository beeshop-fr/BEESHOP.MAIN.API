using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Queries;

public class RecupererMielsQuery : IRequest<ListDto<MielDto>>;
