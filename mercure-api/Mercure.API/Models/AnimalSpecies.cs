using Mercure.API.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class AnimalSpecies
{
    public AnimalSpecies(Animal animal, Species species)
    {
        AnimalId = animal.AnimalId;
        SpeciesId = species.SpeciesId;
    }
    
    public AnimalSpecies(int animalId, int speciesId)
    {
        AnimalId = animalId;
        SpeciesId = speciesId;
    }
    
    public AnimalSpecies()
    {
    }

    public AnimalSpecies(int animalId, Species randomSpecies)
    {
        AnimalId = animalId;
        SpeciesId = randomSpecies.SpeciesId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AnimalSpeciesId { get; set; }

    [ForeignKey("Animal")] public int AnimalId { get; set; }
    public virtual Animal Animal { get; set; }

    [ForeignKey("Species")] public int SpeciesId { get; set; }
    public virtual Species Species { get; set; }
}