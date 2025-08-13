using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using MediatR;
using System.Text.Json.Serialization;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Commands;

public record ModifierStockCommand(Guid MielId,
                                  int Quantite) : IRequest<StockDto>
{
[JsonIgnore] // empêche que Swagger ou l'appelant l'attende dans le body
public Guid Id { get; set; }
}