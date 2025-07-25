using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Miels;
using MediatR;
using System.Text.Json.Serialization;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;

public record ModifierMielCommand(string Nom,
                                  string Type,
                                  int Prix,
                                  string Description,
                                  int Poids) : IRequest<MielDto>
{
    [JsonIgnore] // empêche que Swagger ou l'appelant l'attende dans le body
    public Guid Id { get; set; }
}
