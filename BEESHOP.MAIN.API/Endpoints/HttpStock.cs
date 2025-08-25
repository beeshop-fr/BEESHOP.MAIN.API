using BEESHOP.MAIN.API.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Commands;
using BEESHOP.MAIN.APPLICATION.UseCases.Stocks.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BEESHOP.MAIN.API.Endpoints;

public class HttpStock : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/main").RequireCors("AllowFront"); ;

        group.MapPost("/Stock", async (IMediator mediator, [FromBody] CreerStockCommand command) =>
        {
            var created = await mediator.Send(command);
            return Results.Created($"/api/main/Stock/{created.Id}", created);
        });

        group.MapGet("/Stock", async (IMediator mediator) =>
        {
            var query = new RecupererStocksQuery();
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        group.MapGet("/Stock/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var query = new RecupererStockByIdQuery(id);
            var result = await mediator.Send(query);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        });

        group.MapPut("/Stock/{id:guid}", async (IMediator mediator, Guid id, ModifierStockCommand command) =>
        {
            command.Id = id;
            var updated = await mediator.Send(command);
            return Results.Ok(updated);
        });

        group.MapDelete("/Stock/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var command = new SupprimerStockCommand(id);
            await mediator.Send(command);
            return Results.NoContent();
        });
    }
}
