using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;
using BEESHOP.MAIN.DOMAIN.Miels;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Handlers;

public sealed class CreerMielHandler : IRequestHandler<CreerMielCommand, MielDto>
{
    private readonly IMielRepository _mielRepository;
    private readonly ILogger<CreerMielHandler> _logger;

    public CreerMielHandler(ILogger<CreerMielHandler> logger, IMielRepository mielRepository)
    {
        _mielRepository = mielRepository;
        _logger = logger; 
    }

    public async Task<MielDto> Handle(CreerMielCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Création d'un miel", request.Nom);

        if (!Enum.TryParse<ETypeMiel>(request.Type, out var parsedType))
            throw new ArgumentException($"Type de miel invalide : {request.Type}");

        Miel miel = new(request.Nom, parsedType, request.Prix, request.Description, request.Poids);

        miel = await _mielRepository.Creer(miel);

        return miel.Adapt<MielDto>();
    }
}