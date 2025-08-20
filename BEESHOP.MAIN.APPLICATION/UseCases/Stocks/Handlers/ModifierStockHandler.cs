using BEESHOP.MAIN.APPLICATION.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Commands;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Handlers;

public sealed class ModifierStockHandler : IRequestHandler<ModifierStockCommand, StockDto>
{
    private readonly IStockRepository _stockRepository;
    private readonly ILogger<ModifierStockHandler> _logger;

    public ModifierStockHandler(ILogger<ModifierStockHandler> logger, IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
        _logger = logger;
    }
    public async Task<StockDto> Handle(ModifierStockCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Modification du stock pour le miel avec l'ID {MielId}", request.MielId);

        // Retrieve the existing stock from the repository
        var existingStock = await _stockRepository.RecupererParId(request.Id);

        if (existingStock == null)
        {
            _logger.LogWarning("Stock with ID {Id} not found", request.Id);
            throw new KeyNotFoundException($"Stock with ID {request.Id} not found");
        }
        // Update the stock quantity
        existingStock.Quantite = request.Quantite;

        // Save the updated stock back to the repository
        await _stockRepository.Modifier(existingStock);
        _logger.LogInformation("Stock for miel with ID {MielId} successfully modified", request.MielId);
        // Return the updated stock as a DTO
        return existingStock.Adapt<StockDto>();

    }
}
