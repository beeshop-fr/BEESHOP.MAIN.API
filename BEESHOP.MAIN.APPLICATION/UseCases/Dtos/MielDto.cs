using BEESHOP.MAIN.DOMAIN.Miels;
using System.Text.Json.Serialization;

namespace BEESHOP.MAIN.APPLICATION.UseCases.Dtos;

public class MielDto : IdentifiableDto
{
    public required string Nom { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public required ETypeMiel TypeMiel { get; set; }
    public required int Prix { get; set; }
    public string Description { get; set; }
    public required int Poids { get; set; }
}
