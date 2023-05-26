using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercure.API.Models;

public class Animal
{
    public Animal(string animalName, DateTime animalBirthDate, string animalColor, int animalPrice, DateTime animalCreationDate, DateTime animalLastUpdate)
    {
        AnimalName = animalName;
        AnimalBirthDate = animalBirthDate;
        AnimalColor = animalColor;
        AnimalPrice = animalPrice;
        AnimalCreationDate = animalCreationDate;
        AnimalLastUpdate = animalLastUpdate;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AnimalId { get; set; }
    public string AnimalName { get; set; }
    public DateTime AnimalBirthDate { get; set; }
    public string AnimalColor { get; set; }
    public int AnimalPrice { get; set; }
    public DateTime AnimalCreationDate { get; set; }
    public DateTime AnimalLastUpdate { get; set; }
    public ICollection<AnimalSpecies> AnimalSpecies { get; set; }
}