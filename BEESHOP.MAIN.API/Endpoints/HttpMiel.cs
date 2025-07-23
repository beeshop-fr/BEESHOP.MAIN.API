using BEESHOP.MAIN.API.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BEESHOP.MAIN.API.Endpoints;

public class HttpMiel : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        app.MapGroup("/api/main")
           .MapPost("/Miel", async (IMediator mediator, CreerMielCommand command) =>
           {
               var created = await mediator.Send(command);
               return Results.Created($"/api/main/Miel/{created.Id}", created);
           })
           .WithName("CreateMiel")
           .Produces<MielDto>(StatusCodes.Status201Created);
    }
}
