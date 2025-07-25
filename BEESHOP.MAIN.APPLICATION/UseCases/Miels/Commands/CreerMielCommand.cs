using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;

public record CreerMielCommand(string Nom,
                               string Type,
                               int Prix,
                               string Description,
                               int Poids) : IRequest<MielDto>;