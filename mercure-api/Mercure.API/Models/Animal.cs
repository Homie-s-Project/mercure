using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercure.API.Models;

/// <summary>
/// Animal
/// </summary>
public class Animal
{
    public Animal(DateTime animalBirthDate, string animalColor, int animalPrice, DateTime animalCreationDate, DateTime animalLastUpdate)
    {
        AnimalBirthDate = animalBirthDate;
        AnimalColor = animalColor;
        AnimalPrice = animalPrice;
        AnimalCreationDate = animalCreationDate;
        AnimalLastUpdate = animalLastUpdate;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AnimalId { get; set; }
    public DateTime AnimalBirthDate { get; set; }
    public string AnimalColor { get; set; }
    public int AnimalPrice { get; set; }
    public DateTime AnimalCreationDate { get; set; }
    public DateTime AnimalLastUpdate { get; set; }
    
    [ForeignKey("Species")] public int SpeciesId { get; set; }
    public virtual Species Species { get; set; }
}