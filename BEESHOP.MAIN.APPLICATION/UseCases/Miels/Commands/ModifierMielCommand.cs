using BEESHOP.MAIN.DOMAIN.Miels;
using Microsoft.AspNetCore.Mvc;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;

using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using MediatR;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;

public class ModifierMielCommand : IRequest<MielDto>
{
    [FromForm]
    public string Nom { get; set; }

    [FromForm]
    public ETypeMiel Type { get; set; }

    [FromForm]
    public int Prix { get; set; }

    [FromForm]
    public string Description { get; set; }

    [FromForm]
    public int Poids { get; set; }

    [FromForm]
    public IFormFile? Image { get; set; }

    [JsonIgnore]
    [FromForm]
    public Guid Id { get; set; }
}
