using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;
using BEESHOP.MAIN.DOMAIN.Miels;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Handlers;

public sealed class CreerMielHandler : IRequestHandler<CreerMielCommand, MielDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMielRepository _mielRepository;
    private readonly ILogger<CreerMielHandler> _logger;

    public CreerMielHandler(ILogger<CreerMielHandler> logger, IMielRepository mielRepository, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _mielRepository = mielRepository;
        _logger = logger; 
    }

    public async Task<MielDto> Handle(CreerMielCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Création d'un miel : {Nom}", request.Nom);

        string? imagePath = _httpContextAccessor.HttpContext?.Items["ImagePath"] as string;

        if (request.Image is null && string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext?.Items["ImagePath"] as string))
            throw new ArgumentNullException(nameof(request.Image), "Une image est requise pour créer un miel.");

        Miel miel = new(request.Nom,
                        request.Type,
                        request.Prix,
                        request.Description ?? "",
                        request.Poids,
                        imagePath: imagePath
                        );

        if (_httpContextAccessor.HttpContext?.Items["ImagePath"] is string path)
            miel.ImagePath = path;

        miel = await _mielRepository.Creer(miel);

        return miel.Adapt<MielDto>();
    }
}