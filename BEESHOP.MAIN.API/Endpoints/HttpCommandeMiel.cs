using BEESHOP.MAIN.API.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Commands;
using BEESHOP.MAIN.APPLICATION.UseCases.Commandes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class HttpCommandeMiel : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/main").RequireCors("AllowFront"); ;

        group.MapPost("/CommandesMiel", async (IMediator mediator, [FromBody] CreerCommandeMiel command) =>
        {
            var created = await mediator.Send(command);
            return Results.Created($"/api/main/ComCommandesMielmandes/{created.Id}", created);
        });

        group.MapGet("/CommandesMiel", async (IMediator mediator) =>
        {
            var commandes = await mediator.Send(new RecupererCommandeMielQuery());
            return Results.Ok(commandes);
        });

        group.MapGet("/CommandesMiel/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var commande = await mediator.Send(new RecupererCommandeMielByIdQuery());
            if (commande is null)
                return Results.NotFound();
            return Results.Ok(commande);
        });

        group.MapPut("/CommandesMiel/{id:guid}", async (IMediator mediator, Guid id, [FromBody] ModifierCommandeMiel command) =>
        {
            command.Id = id;
            var updated = await mediator.Send(command);
            if (updated is null)
                return Results.NotFound();
            return Results.Ok(updated);
        });
    }
}