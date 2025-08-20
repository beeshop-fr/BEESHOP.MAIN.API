using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Queries;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Handlers;

public class RecupererStockByIdHandler : IRequestHandler<RecupererStockByIdQuery, StockDto?>
{
    private readonly IStockRepository _stockRepository;
    private readonly ILogger<RecupererStockByIdHandler> _logger;
    public RecupererStockByIdHandler(IStockRepository stockRepository, ILogger<RecupererStockByIdHandler> logger)
    {
        _stockRepository = stockRepository;
        _logger = logger;
    }
    public async Task<StockDto?> Handle(RecupererStockByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling RecupererStockByIdQuery for ID: {Id}", request.Id);

        // Appel au repository pour récupérer le stock par ID
        var stock = await _stockRepository.RecupererParId(request.Id);
        if (stock == null)
        {
            _logger.LogWarning("Stock with ID {Id} not found", request.Id);
            return null;
        }
        _logger.LogInformation("Stock with ID {Id} retrieved successfully", request.Id);
        return stock.Adapt<StockDto>();
    }
}