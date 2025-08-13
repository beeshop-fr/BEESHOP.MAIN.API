using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Queries;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Handlers;

public class RecupererStocksHandler : IRequestHandler<RecupererStocksQuery, ListDto<StockDto>>
{
    private readonly IStockRepository _stockRepository;
    private readonly ILogger<RecupererStocksHandler> _logger;

    public RecupererStocksHandler(IStockRepository stockRepository, ILogger<RecupererStocksHandler> logger)
    {
        _stockRepository = stockRepository;
        _logger = logger;
    }
    public async Task<ListDto<StockDto>> Handle(RecupererStocksQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling RecupererStocksQuery");

        var stocks = await _stockRepository.RecupererStocks();

        return stocks.Adapt<ListDto<StockDto>>();
    }
}
