using System;

namespace WebAPIAutores.Servicios
{
    public class EscribirEnArchivo : IHostedService
    {
        private readonly IHostEnvironment environment;
        private readonly string nombreArchivo = "Archivo1.txt"; //Nombre del Archivo donde se implementa los métodos
        private Timer timer;

        public EscribirEnArchivo(IHostEnvironment environment)//IHostEnvironment nos permite acceder a nuestro entorno de trabajo
        {
            this.environment = environment;
        }
        public Task StartAsync(CancellationToken cancellationToken) //Método de Inicio de IHostService
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            Escribir("Proceso Iniciado");
            return Task.CompletedTask; //retorna Tarea completada
        }

        public Task StopAsync(CancellationToken cancellationToken)//Método de Apagado de IHostService
        {
            timer.Dispose();
            Escribir("Proceso Finalizado");
            return Task.CompletedTask;
        }

        private void DoWork(object state)//Método que escribe en el archivo durante la ejecución
        {
            Escribir("Proceso en ejecución" + DateTime.Now.ToString("dd/MM/yy hh:mm:ss"));
        }

        private void Escribir(string mensaje)//Método auxiliar para hacer el mensaje
        {
            var ruta = $@"{environment.ContentRootPath}\wwwroot\{nombreArchivo}";
            using (StreamWriter writer = new StreamWriter(ruta, append: true))
            {
                writer.WriteLine(mensaje);
            }
        }
    }
}
