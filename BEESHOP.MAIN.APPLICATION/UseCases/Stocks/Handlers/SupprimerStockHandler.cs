using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Handlers;

public sealed class SupprimerStockHandler : IRequestHandler<SupprimerStockCommand>
{
    private readonly IStockRepository _stockRepository;
    private readonly ILogger<SupprimerStockHandler> _logger;
    public SupprimerStockHandler(ILogger<SupprimerStockHandler> logger, IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
        _logger = logger;
    }
    public async Task Handle(SupprimerStockCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Suppression du stock avec l'ID {Id}", request.Id);

        // Retrieve the existing stock from the repository
        var existingStock = await _stockRepository.RecupererParId(request.Id);

        if (existingStock == null)
        {
            _logger.LogWarning("Stock with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Stock with ID {request.Id} not found");
        }
        // Delete the stock
        await _stockRepository.Supprimer(existingStock.Id);
        _logger.LogInformation("Stock with ID {Id} successfully deleted", request.Id);
    }
}
