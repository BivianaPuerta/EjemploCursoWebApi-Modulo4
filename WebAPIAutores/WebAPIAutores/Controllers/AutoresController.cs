using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPIAutores.Entitys;
using WebAPIAutores.Filtros;
using WebAPIAutores.Servicios;

namespace WebAPIAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicio servicio;
        private readonly ServicioTransient servicioTransient;
        private readonly ServicioScoped servicioScoped;
        private readonly ServicioSingleton servicioSingleton;
        private readonly ILogger<AutoresController> logger;

        public AutoresController(ApplicationDbContext context, IServicio servicio, ServicioTransient servicioTransient, 
            ServicioScoped servicioScoped, ServicioSingleton servicioSingleton, ILogger<AutoresController> logger)
        {
            this.context = context;
            this.servicio = servicio;
            this.servicioTransient = servicioTransient;
            this.servicioScoped = servicioScoped;
            this.servicioSingleton = servicioSingleton;
            this.logger = logger;
        }

        [HttpGet("GUID")]
        //[ResponseCache(Duration = 10)]
        [ServiceFilter(typeof(MiFiltrodeAccion))]
        public ActionResult ObtenerGuids()
        {
            return Ok(new
            {
                AutoresControllerTransient = servicioTransient.Guid,
                AutoresControllerScoped = servicioScoped.Guid,
                AutoresControllerSingleton = servicioSingleton.Guid,
                ServicioA_Transient = servicio.ObtenerTransient(),
                ServicioA_Scoped = servicio.ObtenerScoped(),
                servicio_Singleton = servicio.ObtenerSingleton()
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<Autor>>> Get()
        {
            //throw new NotImplementedException();
            //logger.LogInformation("Estamos obteniendo los autores");
            //logger.LogWarning("Esto es un mensaje de prueba de error");
            servicio.RealizarTareas();
            return await context.Autores.Include(x => x.Libros).ToListAsync();

        }

        [HttpGet("primero")]
        public async Task<ActionResult<Autor>> PrimerAutor([FromHeader] int miValor)
        {
            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Autor>> Get(int id)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Id == id);

            if (autor == null)
            {
                return NotFound();
            }
            else
            {
                return autor;
            }
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Autor>> Get([FromRoute] string name)
        {
            var autor = await context.Autores.FirstOrDefaultAsync(x => x.Name.Contains(name));

            if (autor == null)
            {
                return NotFound();
            }
            else
            {
                return autor;
            }
        }


        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            var existeAutorConElMismoNombre = await context.Autores.AnyAsync(x => x.Name == autor.Name);

            if (existeAutorConElMismoNombre)
            {
                return BadRequest($"Ya existe un autor con el nombre {autor.Name}");
            }

            context.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if(autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la URL");
            }
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Update(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor() { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}