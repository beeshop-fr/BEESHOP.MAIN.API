using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Queries;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Handlers;

public class RecupererMielsByIdHandler : IRequestHandler<RecupererMielByIdQuery, MielDto?>
{
    private readonly IMielRepository _mielRepository;
    private readonly ILogger<RecupererMielsByIdHandler> _logger;
    public RecupererMielsByIdHandler(IMielRepository mielRepository, ILogger<RecupererMielsByIdHandler> logger)
    {
        _mielRepository = mielRepository;
        _logger = logger;
    }

    public async Task<MielDto?> Handle(RecupererMielByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling RecupererMielByIdQuery for ID: {Id}", request.Id);

        // Appel au repository pour récupérer le miel par ID
        var miel = _mielRepository.RecupererParId(request.Id);

        if (miel == null)
        {
            _logger.LogWarning("Miel with ID {Id} not found", request.Id);
            return null;
        }

        _logger.LogInformation("Miel with ID {Id} retrieved successfully", request.Id);

        return miel.Adapt<MielDto>();

    }
}
