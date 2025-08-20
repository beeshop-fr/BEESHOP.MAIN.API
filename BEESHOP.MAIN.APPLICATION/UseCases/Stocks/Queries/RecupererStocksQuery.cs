using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Queries;

public record RecupererStocksQuery() : IRequest<ListDto<StockDto>>;
