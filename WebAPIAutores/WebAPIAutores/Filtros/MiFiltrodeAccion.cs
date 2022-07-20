using Microsoft.AspNetCore.Mvc.Filters;

namespace WebAPIAutores.Filtros
{
    public class MiFiltrodeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltrodeAccion> logger;

        public MiFiltrodeAccion(ILogger<MiFiltrodeAccion> logger) //Constructor que tiene como parametro ILogger que llama a la clase.
        {
            this.logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context) //Método de la interfaz IActionResult-Se ejecuta antes de la Acción del Controlador
        {
            logger.LogInformation("Filtro que se activa Antes de Ejecutar la Acción");
        }
        public void OnActionExecuted(ActionExecutedContext context)//Método de la interfaz IActionResult-Se ejecuta después de la Acción del Controlador
        {
            logger.LogInformation("Filtro que se activa Despues de Ejecutar la Acción");
        }

       
    }
}
