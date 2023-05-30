using Mercure.API.Models;

public class SpeciesDto
{
    public int SpeciesId { get; set; }
    public string SpeciesName { get; set; }

    public SpeciesDto(Species species)
    {
        SpeciesId = species.SpeciesId;
        SpeciesName = species.SpeciesName;
    }

    public SpeciesDto(string speciesName)
    {
        SpeciesName = speciesName;
    }
}
