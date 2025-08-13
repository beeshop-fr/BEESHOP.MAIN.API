using BEESHOP.MAIN.API.Abstractions;
using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;
using BEESHOP.MAIN.APPLICATION.UseCases.Miels.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BEESHOP.MAIN.API.Endpoints;

public class HttpMiel : IEndpointDefinition
{
    public void RegisterEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/main");

        group.MapPost("/Miel", async (HttpRequest request, IMediator mediator, [FromForm] CreerMielCommand command) =>
        {
            string? imagePath = null;
            if (command.Image is not null)
            {
                var ext = Path.GetExtension(command.Image.FileName);
                if (!new[] { ".jpg", ".jpeg", ".png" }.Contains(ext.ToLower()))
                    return Results.BadRequest("Format d'image non supporté");

                var filename = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine("wwwroot/images", filename);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using var stream = File.Create(filePath);
                await command.Image.CopyToAsync(stream);

                imagePath = $"/images/{filename}";
            }

            request.HttpContext.Items["ImagePath"] = imagePath;

            var created = await mediator.Send(command);
            return Results.Created($"/api/main/Miel/{created.Id}", created);
        })
        .DisableAntiforgery()
        .WithName("CreateMiel")
        .Accepts<CreerMielCommand>("multipart/form-data")
        .Produces<MielDto>(StatusCodes.Status201Created);

        group.MapPut("/Miel/{id:guid}", async (HttpRequest request, IMediator mediator, Guid id, [FromForm] ModifierMielCommand command) =>
        {
            command.Id = id;

            if (command.Image is not null)
            {
                var ext = Path.GetExtension(command.Image.FileName);
                var formatsAutorisés = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                if (!formatsAutorisés.Contains(ext.ToLower()))
                    return Results.BadRequest("Format d'image non supporté");

                var filename = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine("wwwroot/images", filename);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using var stream = File.Create(filePath);
                await command.Image.CopyToAsync(stream);

                request.HttpContext.Items["ImagePath"] = $"/images/{filename}";
            }

            var updated = await mediator.Send(command);
            return Results.Ok(updated);
        })
        .DisableAntiforgery()
        .Accepts<ModifierMielCommand>("multipart/form-data")
        .Produces<MielDto>(StatusCodes.Status200OK)
        .WithName("ModifierMiel");

        group.MapGet("/Miel/{id:guid}", async (IMediator mediator, Guid id) =>
        {
            var query = new RecupererMielByIdQuery(id);
            var result = await mediator.Send(query);
            return result is not null ? Results.Ok(result) : Results.NotFound();
        })
        .WithName("GetMielById")
        .Produces<MielDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/Miel", async (IMediator mediator) =>
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