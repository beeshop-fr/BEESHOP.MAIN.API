using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;

public record ModifierCommande(DateTime dateCommande, string userEmail) : IRequest<CommandeDto>
{
    [JsonIgnore]
    public Guid Id { get; set; }
}