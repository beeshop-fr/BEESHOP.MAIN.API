using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;

public record ModifierCommandeMiel(int quantite) : IRequest<CommandeMielDto>
{
    [JsonIgnore] 
    public Guid Id { get; set; }
}
