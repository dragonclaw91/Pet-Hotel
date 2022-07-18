using System.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetsController(ApplicationContext context)
        {
            _context = context;
        }

        // This is just a stub for GET / to prevent any weird frontend errors that 
        // occur when the route is missing in this controller
        [HttpGet]
        public List<Pet> GetpetInventories()
        {
            return _context.Pets
              .Include(pet => pet.petOwner)
              .OrderBy(pet => pet.name).ToList();
        }

        // GET a pet by id
        [HttpGet("{id}")] // GET /api/pet/10
        public Pet getpetById(int id)
        {
            return _context.Pets
                .Include(pet => pet.petOwnerid)
                .SingleOrDefault(pet => pet.id == id);
        }
        // DELETE a pet By id
        [HttpDelete("{id}")] // DELETE /api/pet/10
        public IActionResult deletepetById(int id)
        {
            Pet pet = _context.Pets.Find(id);
            // Return a 404 not found if pet id is invalid
            if (pet == null)
            {
                return NotFound(); // 404 NOT FOUND
            }

            _context.Pets.Remove(pet);
            _context.SaveChanges();
            return NoContent(); // 204 NO CONTENT
        }
        // add a pet
        [HttpPost]
        public IActionResult addpet([FromBody] Pet pet)
        {
            _context.Pets.Add(pet);
            _context.SaveChanges();
            Pet newPet = _context.Pets.Include(pet => pet.petOwner).SingleOrDefault(p => p.id == pet.id);
            return CreatedAtAction(nameof(getpetById), new { id = newPet.id }, newPet);
        }



        // PUT (for checkin)
        [HttpPut("{id}/checkin")] // TODO: /{id}/sell
        public IActionResult CheckIn(int id)
        {
            // find the pet
            Pet pet = _context.Pets
            .SingleOrDefault(p => p.id == id);
            // return 404 if not found
            if (pet == null)
            {
                return NotFound();
            }
            pet.checkedInAt = DateTime.UtcNow;
            _context.Update(pet);
            _context.SaveChanges();
            return Ok(pet);
        }

        [HttpPut("{id}/checkout")] // TODO: /{id}/checkout
        public IActionResult CheckOut(int id)
        {
            // find the pet
            Pet pet = _context.Pets
            .SingleOrDefault(p => p.id == id);
            // return 404 if not found
            if (pet == null)
            {
                return NotFound();
            }
            pet.checkedInAt = null;
            _context.Update(pet);
            _context.SaveChanges();
            return Ok(pet);
        }

   

                // PUT (for )
        
        [HttpPut("{id}")] // TODO: /{id}/
        public IActionResult Put(int id, [FromBody] Pet pet)
        {
            if (id != pet.id) return BadRequest();

            if (!_context.Pets.Any(b => b.id == id)) return NotFound();

            _context.Update(pet);

            Console.WriteLine(pet);

            _context.SaveChanges();

            return Ok(_context.Pets.Include(p => p.petOwner).SingleOrDefault(p => p.id == id));
        }

    }
}
