using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercure.API.Models;

public class Species
{
    public Species(string speciesName)
    {
        SpeciesName = speciesName;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SpeciesId { get; set; }
    public string SpeciesName { get; set; }

    public ICollection<AnimalSpecies> AnimalSpecies { get; set; }

}