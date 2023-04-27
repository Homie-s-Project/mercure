using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Mercure.API.Context;
using Mercure.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mercure.API.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Mercure.API.Controllers;

/// <summary>
/// All the routes for the animals
/// </summary>
[Route("animals")]
public class AnimalsController : ApiNoSecurityController
{

    private readonly MercureContext _context;

    public AnimalsController(MercureContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get the Animal for the given id
    /// </summary>
    /// <param name="animalId"></param>
    /// <returns></returns>
    [HttpGet("{animalId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnimalDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> AnimalGet(string animalId)
    {
        #region Parameters validation
        if (string.IsNullOrEmpty(animalId))
        {
            return BadRequest(new ErrorMessage("Animal Id is required", StatusCodes.Status400BadRequest));
        }

        bool isParsed = int.TryParse(animalId, out int id);
        if (!isParsed)
        {
            return BadRequest(new ErrorMessage("Animal Id is not a number", StatusCodes.Status400BadRequest));
        }
#endregion

        var AnimalSpeciesDb = await _context.AnimalSpecies
            .Include(a => a.Animal)
            .Include(a => a.Species)
            .FirstOrDefaultAsync(a => a.AnimalId == id);

        if (AnimalDb == null)
#endregion

        var AnimalSpeciesDb = await _context.AnimalSpecies
            .Include(a => a.Animal)
            .Include(a => a.Species)
            .FirstOrDefaultAsync(a => a.AnimalId == id);


        if (AnimalSpeciesDb == null || AnimalSpeciesDb.Animal == null)

        if (AnimalSpeciesDb == null || AnimalSpeciesDb.Animal == null)
        {
            return NotFound(new ErrorMessage("Animal not found", StatusCodes.Status404NotFound));
        }

        return Ok(new AnimalDto(AnimalSpeciesDb.Animal, true));
        return Ok(new AnimalDto(AnimalSpeciesDb.Animal, true));
    }

    /// <summary>
    /// Creates a new Animal.
    /// </summary>
    /// <param name="animalName"></param>
    /// <param name="animalBirthDate"></param>
    /// <param name="animalColor"></param>
    /// <param name="animalPrice">The price of the Animal.</param>
    /// <param name="speciesId"></param>
    /// <returns>Returns a AnimalDto object that represents the created Animal.</returns>
    [HttpPost("create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnimalDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> AnimalCreate(
        [FromForm] string animalName,
        [FromForm] string animalBirthDate,
        [FromForm] string animalColor,
        [FromForm] int animalPrice,
        [FromForm] List<int> speciesId
        )
    {
        #region Parameters validation
        if (string.IsNullOrEmpty(animalName) || animalName.Length >= ConstantRules.MaxLengthName)
        {
            return BadRequest(new ErrorMessage("Your animal needs a name", StatusCodes.Status400BadRequest));
        }

        DateTime dtAnimalBirthdate;
        if (!DateTime.TryParse(animalBirthDate, out dtAnimalBirthdate))
        {
            return BadRequest(new ErrorMessage("Wrong birthdate format", StatusCodes.Status400BadRequest));
        }

        if (animalPrice <= 0)
        {
            return BadRequest(new ErrorMessage("Price is required to create an Animal", StatusCodes.Status400BadRequest));
        }

        if (speciesId.Count == 0)
        {
            return BadRequest(new ErrorMessage("You need to specify species", StatusCodes.Status400BadRequest));
        }

        // On vérifie que l'utilisateur soit au moins un vendeur d'animaux
        var userContext = (User)HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        if (!RoleChecker.HasRole(userContext.Role, RoleEnum.AnimalSeller))
        {
            return Unauthorized(new ErrorMessage("You are not authorized, only people higher or equal as animal seller can access", StatusCodes.Status401Unauthorized));
        }
        #endregion
        var animal = new Animal(animalName, dtAnimalBirthdate, animalColor, animalPrice, DateTime.Now, DateTime.Now);
        animal.AnimalSpecies = new List<AnimalSpecies>();

        if (speciesId.Count != 0)
        {
            foreach (var id in speciesId)
            {
                var speciesDb = await _context.Speciess.FirstOrDefaultAsync(c => c.SpeciesId == id);
                if (speciesDb == null)
                {
                    return BadRequest(new ErrorMessage("We cannot find the species with this id: " + id, StatusCodes.Status400BadRequest));
                }

                var animalSpecies = new AnimalSpecies
                {
                    Animal = animal,
                    Species = speciesDb
                };

                animal.AnimalSpecies.Add(animalSpecies);
            }
        }

        await _context.Animals.AddAsync(animal);
        await _context.SaveChangesAsync();

        return Ok(new AnimalDto(animal, true));
        // On vérifie que l'utilisateur soit au moins un vendeur d'animaux
        var userContext = (User)HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        if (!RoleChecker.HasRole(userContext.Role, RoleEnum.AnimalSeller))
        {
            return Unauthorized(new ErrorMessage("You are not authorized, only people higher or equal as animal seller can access", StatusCodes.Status401Unauthorized));
        }
        #endregion
        var animal = new Animal(animalName, dtAnimalBirthdate, animalColor, animalPrice, DateTime.Now, DateTime.Now);
        animal.AnimalSpecies = new List<AnimalSpecies>();

        if (speciesId.Count != 0)
        {
            foreach (var id in speciesId)
            {
                var speciesDb = await _context.Speciess.FirstOrDefaultAsync(c => c.SpeciesId == id);
                if (speciesDb == null)
                {
                    return BadRequest(new ErrorMessage("We cannot find the species with this id: " + id, StatusCodes.Status400BadRequest));
                }

                var animalSpecies = new AnimalSpecies
                {
                    Animal = animal,
                    Species = speciesDb
                };

                animal.AnimalSpecies.Add(animalSpecies);
            }
        }

        await _context.Animals.AddAsync(animal);
        await _context.SaveChangesAsync();

        return Ok(new AnimalDto(animal, true));
    }

    /// <summary>
    /// Updates an existing Animal.
    /// </summary>
    /// <param name="AnimalId">The identifier of the Animal to update.</param>
    /// <param name="AnimalName">The updated name of the Animal.</param>
    /// <param name="AnimalBrandName">The updated name of the Animal's brand.</param>
    /// <param name="AnimalDescription">The updated description of the Animal.</param>
    /// <param name="AnimalPrice">The updated price of the Animal.</param>
    /// <param name="stockId">The updated identifier of the stock.</param>
    /// <param name="categories">The updated categories of the Animal.</param>
    /// <returns>Returns a AnimalDto object that represents the updated Animal.</returns>
    [HttpPut("update/{AnimalId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnimalDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> AnimalUpdate(
        string AnimalId,
        [FromForm] string animalName,
        [FromForm] string animalBirthDate,
        [FromForm] string animalColor,
        [FromForm] int animalPrice,
        [FromForm] List<int> speciesId
        )
    {
        #region Parameters validation
        if (string.IsNullOrEmpty(AnimalId))
        {
            return BadRequest(new ErrorMessage("You need to provide an id for the animal you want to update", StatusCodes.Status400BadRequest));
        }

        if (animalName.Length >= ConstantRules.MaxLengthName)
        {
            return BadRequest(new ErrorMessage("Your animal name is too long, max " + ConstantRules.MaxLengthName + " character", StatusCodes.Status400BadRequest));
        }

        if (animalColor.Length >= ConstantRules.MaxLengthName)
        {
            return BadRequest(new ErrorMessage("Your color is too long, max " + ConstantRules.MaxLengthDescription + " character", StatusCodes.Status400BadRequest));
        }

        if (animalPrice <= 0)
        {
            return BadRequest(new ErrorMessage("Price is required to update an animal", StatusCodes.Status400BadRequest));
        }

        DateTime dtAnimalBirthdate;
        if (!DateTime.TryParse(animalBirthDate, out dtAnimalBirthdate))
        {
            return BadRequest(new ErrorMessage("Wrong birthdate format", StatusCodes.Status400BadRequest));
        }
        
        // On vérifie que l'utilisateur est au moins un vendeur d'animaux
        var userContext = (User)HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        if (!RoleChecker.HasRole(userContext.Role, RoleEnum.AnimalSeller))
        {
            return Unauthorized(new ErrorMessage("You are not authorized, only people higher or equal as animal seller can access", StatusCodes.Status401Unauthorized));
        }

        bool isAnimalIdParsed = int.TryParse(AnimalId, out int AnimalIdParsed);
        if (!isAnimalIdParsed)
        {
            return BadRequest(new ErrorMessage("Your animal id is not an number", StatusCodes.Status400BadRequest));
        }

        var AnimalUpdatedWanted = await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync(_context.Animals, p => p.AnimalId == AnimalIdParsed);
        if (AnimalUpdatedWanted == null)
        {
            return BadRequest(new ErrorMessage("We cannot found the animal with this id: " + AnimalIdParsed, StatusCodes.Status400BadRequest));
        }
        #endregion

        if (speciesId.Count != 0)
        {
            AnimalUpdatedWanted.AnimalSpecies = new List<AnimalSpecies>();

            foreach (var id in speciesId)
            {
                var speciesDb = await _context.Speciess.FirstOrDefaultAsync(c => c.SpeciesId == id);
                if (speciesDb == null)
                {
                    return BadRequest(new ErrorMessage("We cannot find the species with this id: " + id, StatusCodes.Status400BadRequest));
                }

                var animalSpecies = new AnimalSpecies
                {
                    Animal = AnimalUpdatedWanted,
                    Species = speciesDb
                };

                AnimalUpdatedWanted.AnimalSpecies.Add(animalSpecies);
            }
        }

        AnimalUpdatedWanted.AnimalName = animalName;
        AnimalUpdatedWanted.AnimalBirthDate = dtAnimalBirthdate;
        AnimalUpdatedWanted.AnimalColor = animalColor;
        AnimalUpdatedWanted.AnimalPrice = animalPrice;

        _context.Animals.Update(AnimalUpdatedWanted);
        await _context.SaveChangesAsync();

        return Ok(new AnimalDto(AnimalUpdatedWanted, true));
    }

    [HttpDelete("delete/{AnimalId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorMessage))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage))]
    public async Task<IActionResult> AnimalDelete(string AnimalId)
    {
        #region Parameters validation
        if (string.IsNullOrEmpty(AnimalId))
        {
            return BadRequest(new ErrorMessage("You need to provide an id for the animal you want to delete", StatusCodes.Status400BadRequest));
        }

        // On vérifie que l'utilisateur est au moins un vendeur d'animaux
        var userContext = (User)HttpContext.Items["User"];
        if (userContext == null)
        {
            return Unauthorized(new ErrorMessage("User is not authorized", StatusCodes.Status401Unauthorized));
        }
        if (!RoleChecker.HasRole(userContext.Role, RoleEnum.AnimalSeller))
        {
            return Unauthorized(new ErrorMessage("You are not authorized, only people higher or equal as animal seller can access", StatusCodes.Status401Unauthorized));
        }

        bool isAnimalIdParsed = int.TryParse(AnimalId, out int AnimalIdParsed);
        if (!isAnimalIdParsed)
        {
            return BadRequest(new ErrorMessage("Your animal id is not an number", StatusCodes.Status400BadRequest));
        }
        #endregion

        var AnimalToDelete = await _context.Animals.FirstOrDefaultAsync(p => p.AnimalId == AnimalIdParsed);
        if (AnimalToDelete == null)
        {
            return BadRequest(new ErrorMessage("We cannot found the Animal with this id: " + AnimalIdParsed, StatusCodes.Status400BadRequest));
        }

        _context.Animals.Remove(AnimalToDelete);
        await _context.SaveChangesAsync();

        return Ok(new ErrorMessage("Animal deleted", StatusCodes.Status200OK));
    }
}