using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Commands;
using BEESHOP.MAIN.DOMAIN.Stocks;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Handlers;

public sealed class CreerStockHandler : IRequestHandler<CreerStockCommand, StockDto>
{
    private readonly IStockRepository _stockRepository;
    private readonly ILogger<CreerStockCommand> _logger;
    public CreerStockHandler(ILogger<CreerStockCommand> logger, IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
        _logger = logger;
    }

    public async Task<StockDto> Handle(CreerStockCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Création d'un stock pour le miel : {MielId}", request.MielId);

        Stock stock = new(request.MielId,
                          request.Quantite);

        stock = await _stockRepository.Creer(stock);
        return stock.Adapt<StockDto>();
    }
}
