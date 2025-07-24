using BEESHOP.MAIN.API.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Queries;
using MediatR;

namespace BEESHOP.MAIN.API.Endpoints;

public class HttpMiel : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/main");

        group.MapPost("/Miel", async (IMediator mediator, CreerMielCommand command) =>
        {
            var created = await mediator.Send(command);
            return Results.Created($"/api/main/Miel/{created.Id}", created);
        })
        .WithName("CreateMiel")
        .Produces<MielDto>(StatusCodes.Status201Created);

        group.MapPut("/Miel/{id:guid}", async (IMediator mediator, Guid id, ModifierMielCommand command) =>
        {
            if (id != command.Id)
            {
                return Results.BadRequest("ID mismatch");
            }
            var updated = await mediator.Send(command);
            return Results.Ok(updated);
        });

        group.MapGet("/Miel/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var query = new RecupererMielByIdQuery(id);
            var result = await mediator.Send(query);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetMielById")
        .Produces<MielDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/Miels", async (IMediator mediator) =>
        {
            var query = new RecupererMielsQuery();
            var result = await mediator.Send(query);
            return Results.Ok(result);
        });

        group.MapDelete("/Miel/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var command = new SupprimerMielCommand(id);
            await mediator.Send(command);
            return Results.NoContent();
        });
    }
}