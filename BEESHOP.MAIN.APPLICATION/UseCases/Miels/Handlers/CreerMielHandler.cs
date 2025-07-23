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
        _logger.LogInformation("Création d'un miel", request.nom);

        Miel miel = new(request.nom, request.type, request.prix, request.description, request.poids);

        miel = await _mielRepository.Creer(miel);

        return miel.Adapt<MielDto>();
    }
}