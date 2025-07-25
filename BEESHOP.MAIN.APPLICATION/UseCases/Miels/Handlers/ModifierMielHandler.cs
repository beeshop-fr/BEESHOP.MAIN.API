using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;
using BEESHOP.MAIN.DOMAIN.Miels;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Handlers;

public sealed class ModifierMielHandler : IRequestHandler<ModifierMielCommand, MielDto>
{
    private readonly IMielRepository _mielRepository;
    private readonly ILogger<ModifierMielHandler> _logger;

    public ModifierMielHandler(ILogger<ModifierMielHandler> logger, IMielRepository mielRepository)
    {
        _mielRepository = mielRepository;
        _logger = logger;
    }

    public async Task<MielDto> Handle(ModifierMielCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Modification d'un miel avec l'ID {Id}", request.Id);

        // Retrieve the existing miel from the repository
        var existingMiel = await _mielRepository.RecupererParId(request.Id);

        if (existingMiel == null)
        {
            _logger.LogWarning("Miel with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Miel with ID {request.Id} not found");
        }

        if (!Enum.TryParse<ETypeMiel>(request.Type, out var parsedType))
            throw new ArgumentException($"Type de miel invalide : {request.Type}");

        // Update the miel properties
        existingMiel.Nom = request.Nom;
        existingMiel.TypeMiel = parsedType;
        existingMiel.Prix = request.Prix;
        existingMiel.Description = request.Description;
        existingMiel.Poids = request.Poids;

        // Save the updated miel back to the repository
        await _mielRepository.Modifier(existingMiel);

        _logger.LogInformation("Miel with ID {Id} successfully modified", request.Id);
        
        // Return the updated miel as a DTO
        return existingMiel.Adapt<MielDto>();
    }
}
