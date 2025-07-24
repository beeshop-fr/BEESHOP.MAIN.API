using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Handlers;

public sealed class SupprimerMielHandler : IRequestHandler<SupprimerMielCommand>
{

    private readonly IMielRepository _mielRepository;
    private readonly ILogger<SupprimerMielHandler> _logger;

    public SupprimerMielHandler(ILogger<SupprimerMielHandler> logger, IMielRepository mielRepository)
    {
        _mielRepository = mielRepository;
        _logger = logger;
    }

    public async Task Handle(SupprimerMielCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Suppression d'un miel avec l'ID {Id}", request.Id);
        
        // Retrieve the existing miel from the repository
        var existingMiel = await _mielRepository.RecupererParId(request.Id);
        
        if (existingMiel == null)
        {
            _logger.LogWarning("Miel with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Miel with ID {request.Id} not found");
        }

        // Delete the miel from the repository
        await _mielRepository.Supprimer(existingMiel.Id);
        
         _logger.LogInformation("Miel with ID {Id} successfully deleted", request.Id);

    }
}
