using BEESHOP.MAIN.APPLICATION.UseCases.Dtos;
using BEESHOP.MAIN.DOMAIN.Miels;
using MediatR;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Miels.Commands;

public record CreerMielCommand(string nom,
                                     ETypeMiel type,
                                     int prix,
                                     string description,
                                     int poids) : IRequest<MielDto>;