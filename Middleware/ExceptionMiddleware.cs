using System.Diagnostics;
namespace ProjetoMIragnum.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {

                await _next(context);
                
            }

            catch (Exception)
            {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var erro = new
                {
                    Mensagem = "Ocorreu um erro"
                };

                await context.Response.WriteAsJsonAsync(erro);
                

            }
        } 
    }

}