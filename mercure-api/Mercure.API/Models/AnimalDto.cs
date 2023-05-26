using System;
using System.Collections.Generic;
using System.Linq;

namespace Mercure.API.Models;

public class AnimalDto
{
    public AnimalDto(Animal animal)
    {
        AnimalId = animal.AnimalId;
        AnimalName = animal.AnimalName;
        AnimalBirthDate = animal.AnimalBirthDate;
        AnimalColor = animal.AnimalColor;
        AnimalPrice = animal.AnimalPrice;
        AnimalCreationDate = animal.AnimalCreationDate;
        AnimalLastUpdate = animal.AnimalLastUpdate;
    }

    public AnimalDto(Animal animal, bool loadMore)
    {
        AnimalId = animal.AnimalId;
        AnimalName = animal.AnimalName;
        AnimalBirthDate = animal.AnimalBirthDate;
        AnimalColor = animal.AnimalColor;
        AnimalPrice = animal.AnimalPrice;
        AnimalCreationDate = animal.AnimalCreationDate;
        AnimalLastUpdate = animal.AnimalLastUpdate;

        if (loadMore)
        {
            if (animal.AnimalSpecies != null)
            {
                Species = animal.AnimalSpecies.Select(c => new SpeciesDto(c.Species)).ToList();
            }
        }
    }

    public int AnimalId { get; set; }
    public string AnimalName { get; set; }
    public DateTime AnimalBirthDate { get; set; }
    public string AnimalColor { get; set; }
    public int AnimalPrice { get; set; }
    public DateTime AnimalCreationDate { get; set; }
    public DateTime AnimalLastUpdate { get; set; }
    public virtual ICollection<SpeciesDto> Species { get; set; }
}