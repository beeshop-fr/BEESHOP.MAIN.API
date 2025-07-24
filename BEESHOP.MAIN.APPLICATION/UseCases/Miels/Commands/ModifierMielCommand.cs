using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Miels;
using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;

public record ModifierMielCommand(Guid Id,
                                  string Nom,
                                  ETypeMiel Type,
                                  int Prix,
                                  string Description,
                                  int Poids) : IRequest<MielDto>
{
}
