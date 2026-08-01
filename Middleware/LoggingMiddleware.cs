using System.Diagnostics;

namespace ProjetoMIragnum.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        //construtor
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        //Metodo obrigatorio
        public async Task InvokeAsync(HttpContext context)
        {

            Stopwatch cronometro = new Stopwatch();
            cronometro.Start();



            await _next(context);

            Console.WriteLine(context.Request.Method);
            Console.WriteLine(context.Request.Path);
            Console.WriteLine(context.Response.StatusCode);
            cronometro.Stop();
            Console.WriteLine(cronometro.Elapsed);
            
            
        }


    }
}
