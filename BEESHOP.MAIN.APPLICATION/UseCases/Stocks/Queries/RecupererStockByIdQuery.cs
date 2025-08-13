using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Queries;

public record RecupererStockByIdQuery(Guid Id) : IRequest<StockDto?>;
