using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Queries;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Handlers;

public class RecupererMielsHandler : IRequestHandler<RecupererMielsQuery, ListDto<MielDto>>
{
    private readonly IMielRepository _mielRepository;
    private readonly ILogger<RecupererMielsHandler> _logger;

    public RecupererMielsHandler(IMielRepository mielRepository, ILogger<RecupererMielsHandler> logger)
    {
        _mielRepository = mielRepository;
        _logger = logger;
    }

    public async Task<ListDto<MielDto>> Handle(RecupererMielsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling RecupererMielsQuery");

        var miels = await _mielRepository.RecupererMiels();

        return miels.Adapt<ListDto<MielDto>>();
    }
}
