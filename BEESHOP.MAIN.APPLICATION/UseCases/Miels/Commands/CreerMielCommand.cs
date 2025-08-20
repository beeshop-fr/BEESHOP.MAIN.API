using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Miels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;

public record CreerMielCommand(
    [property: FromForm] string Nom,
    [property: FromForm] ETypeMiel Type,
    [property: FromForm] int Prix,
    [property: FromForm] string? Description,
    [property: FromForm] int Poids,
    [property: FromForm] IFormFile? Image
) : IRequest<MielDto>;