using Mercure.API.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AnimalSpecies
{
    public AnimalSpecies(AnimalSpecies animalSpecies)
    {
        AnimalId = animalSpecies.AnimalId;
        Animal = animalSpecies.Animal;
        SpeciesId = animalSpecies.SpeciesId;
        Species = animalSpecies.Species;
    }

    public AnimalSpecies()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AnimalSpeciesId { get; set; }

    [ForeignKey("Animal")] public int AnimalId { get; set; }
    public virtual Animal Animal { get; set; }

    [ForeignKey("Species")] public int SpeciesId { get; set; }
    public virtual Species Species { get; set; }
}