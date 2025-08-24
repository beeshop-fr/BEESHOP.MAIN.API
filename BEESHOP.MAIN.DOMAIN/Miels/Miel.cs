using BEESHOP.MAIN.DOMAIN.Common;
using System.Text.Json.Serialization;

namespace BEESHOP.MAIN.DOMAIN.Miels;

public class Miel : IdentifiableEntity
{
    [JsonConstructor]
    public Miel(Guid Id, string nom, ETypeMiel type, decimal prix, string description, int poids, string? imagePath) : base(Id)
    {
        Nom = nom;
        TypeMiel = type;
        Prix = prix;
        Description = description;
        Poids = poids;
        ImagePath = imagePath;
    }

    public Miel(string nom, ETypeMiel type, decimal prix, string description, int poids, string? imagePath)
    {
        Nom = nom;
        TypeMiel = type;
        Prix = prix;
        Description = description;
        Poids = poids;
        ImagePath = imagePath;
    }

    public Miel() : base(Guid.Empty) { }

    public string Nom { get; set; }
    public ETypeMiel TypeMiel { get; set; }
    public decimal Prix { get; set; }
    public string Description { get; set; }
    public int Poids { get; set; }
    public string ImagePath { get; set; }
}