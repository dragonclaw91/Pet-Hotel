using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // /api/PetOwner
    public class PetOwnersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetOwnersController(ApplicationContext context) {
            _context = context;
        }

        // GET all PetPetOwners
        [HttpGet] // GET /api/PetOwner
        public List<PetOwner> getPetPetOwners() {
            return _context.PetOwners.ToList();
        }

        // GET a PetOwner by id

        [HttpGet("{id}")] // GET /api/PetOwner/10
        public PetOwner getPetOwnerById(int id) {
            return _context.PetOwners
                .Include(PetOwner => PetOwner.pets)
                .SingleOrDefault(PetOwner => PetOwner.id == id);
        }

        [HttpPost] // POST /api/PetOwner
        public IActionResult addPetOwner([FromBody] PetOwner PetOwner) { // how to access the http body?
            _context.PetOwners.Add(PetOwner); // SQL INSERT
            _context.SaveChanges(); // COMMIT TO DB
            // this line is so that we can make a 201 CREATED response, including
            // a reference to the actual PetOwner and URL to get the PetOwner we just made by id
            return CreatedAtAction(nameof(getPetOwnerById), new { id = PetOwner.id}, PetOwner);
        }

        [HttpDelete("{id}")] // DELETE /api/PetOwner/10
        public IActionResult deletePetOwnerById(int id) {
            PetOwner PetOwner = _context.PetOwners.Find(id);
            // Return a 404 not found if PetOwner id is invalid
            if (PetOwner == null) {
                return NotFound(); // 404 NOT FOUND
            }

            _context.PetOwners.Remove(PetOwner);
            _context.SaveChanges();
            return NoContent(); // 204 NO CONTENT
        }

        [HttpPut("{id}")] // find pet owner of a pet and update by id
        public IActionResult updatePetOwnerById(int id, [FromBody] PetOwner petOwner) {
            PetOwner foundOwner = _context.PetOwners.Find(id);
            // Return a 404 not found if PetOwner id is invalid
            if (foundOwner == null) {
                return NotFound(); // 404 NOT FOUND
            }
            foundOwner.name = petOwner.name;
            foundOwner.emailAddress = petOwner.emailAddress;

            _context.PetOwners.Update(foundOwner);
            _context.SaveChanges();
            return Ok(petOwner); // 204 NO CONTENT
        }

    }
}
