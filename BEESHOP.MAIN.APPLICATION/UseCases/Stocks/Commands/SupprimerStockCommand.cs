using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Commands;

public record SupprimerStockCommand(Guid Id) : IRequest;