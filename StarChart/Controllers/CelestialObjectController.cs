using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route( "" )]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController( ApplicationDbContext context )
        {
            _context = context;
        }


        [HttpGet( "{id:int}", Name = "GetById" )]
        public IActionResult GetById( int id )
        {

            var res = _context.CelestialObjects.Find(id);

            if ( res is null )
            {
                return NotFound();

            }

            var satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == id);

            res.Satellites = satellites == null ? null : satellites.ToList();

            return Ok( res );
        }

        [HttpGet( "{name}", Name = "GetByName" )]
        public IActionResult GetByName( string name )
        {
            var objects = _context.CelestialObjects.Where(o => o.Name == name);

            if ( !objects.Any() )
            {
                return NotFound();
            }

            foreach ( var obj in objects )
            {
                var satellites = _context.CelestialObjects.Where(o => o.OrbitedObjectId == obj.Id).ToList();
                obj.Satellites = satellites;
            }

            return Ok( objects );

        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var objects = _context.CelestialObjects.ToList();

            foreach ( CelestialObject celestial in objects)
            {
                var satellites = _context.CelestialObjects.Where(c => c.OrbitedObjectId == celestial.Id).ToList();
                celestial.Satellites = satellites;
            }

            return Ok(objects);
        }

    }
}
