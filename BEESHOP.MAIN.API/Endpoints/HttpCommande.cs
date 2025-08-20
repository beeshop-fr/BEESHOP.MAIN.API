using BEESHOP.MAIN.API.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;
using BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BEESHOP.MAIN.API.Endpoints;

public class HttpCommande : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/main");

        group.MapPost("/Commandes", async (IMediator mediator, [FromBody] CreerCommande command) =>
        {
            var created = await mediator.Send(command);
            return Results.Created($"/api/main/Commandes/{created.Id}", created);
        });

        group.MapGet("/Commandes", async (IMediator mediator) =>
        {
            var commandes = await mediator.Send(new RecupererCommandeQuery());
            return Results.Ok(commandes);
        });

        group.MapGet("/Commandes/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var commande = await mediator.Send(new RecupererCommandeByIdQuery());
            if (commande is null)
                return Results.NotFound();
            return Results.Ok(commande);
        });

        group.MapPut("/Commandes/{id:guid}", async (IMediator mediator, Guid id, [FromBody] ModifierCommande command) =>
        {
            command.Id = id;
            var updated = await mediator.Send(command);
            if (updated is null)
                return Results.NotFound();
            return Results.Ok(updated);
        });

        group.MapPost("/Checkout", async (IMediator mediator, CheckoutCommande body, CancellationToken ct) =>
        {
            var res = await mediator.Send(body, ct);
            return Results.Ok(res);
        });
    }
}
