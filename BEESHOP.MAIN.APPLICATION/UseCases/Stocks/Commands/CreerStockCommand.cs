using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Commands;

public record CreerStockCommand(Guid MielId,
                               int Quantite) : IRequest<StockDto>;