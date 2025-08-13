using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Handlers;

public sealed class ModifierMielHandler : IRequestHandler<ModifierMielCommand, MielDto>
{
    private readonly IMielRepository _mielRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ModifierMielHandler> _logger;

    public ModifierMielHandler(
        ILogger<ModifierMielHandler> logger,
        IMielRepository mielRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _mielRepository = mielRepository;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<MielDto> Handle(ModifierMielCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Modification d'un miel avec l'ID {Id}", request.Id);

        var existingMiel = await _mielRepository.RecupererParId(request.Id);

        if (existingMiel == null)
        {
            _logger.LogWarning("Miel with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Miel with ID {request.Id} not found");
        }

        // Mise à jour des propriétés
        existingMiel.Nom = request.Nom;
        existingMiel.TypeMiel = request.Type;
        existingMiel.Prix = request.Prix;
        existingMiel.Description = request.Description;
        existingMiel.Poids = request.Poids;

        // Si une nouvelle image est fournie (chemin récupéré depuis le HttpContext)
        var imagePath = _httpContextAccessor.HttpContext?.Items["ImagePath"] as string;
        if (!string.IsNullOrWhiteSpace(imagePath))
        {
            existingMiel.ImagePath = imagePath;
        }

        await _mielRepository.Modifier(existingMiel);

        _logger.LogInformation("Miel with ID {Id} successfully modified", request.Id);

        return existingMiel.Adapt<MielDto>();
    }
}