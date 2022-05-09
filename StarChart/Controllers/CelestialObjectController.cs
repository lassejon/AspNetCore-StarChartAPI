using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null) return NotFound();

            celestialObject.Satellites = GetOrbittingCelestialObjects(id);

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(o => o.Name.Equals(name));
            if (!celestialObjects.Any()) return NotFound();

            foreach (var celestialObject in celestialObjects) 
            {
                celestialObject.Satellites = GetOrbittingCelestialObjects(celestialObject.Id);
            }

            return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects;

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = GetOrbittingCelestialObjects(celestialObject.Id);
            }

            return Ok(celestialObjects);
        }

        private List<CelestialObject> GetOrbittingCelestialObjects(int id)
        {
            return _context.CelestialObjects
                .Where(c => c.OrbitedObjectId == id).ToList();
        }
    }
}
