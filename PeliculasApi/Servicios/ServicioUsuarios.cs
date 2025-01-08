using Microsoft.AspNetCore.Identity;

namespace PeliculasApi.Servicios
{
    public class ServicioUsuarios : IServicioUsuarios
    {
        private readonly IHttpContextAccessor httpContext;
        private readonly UserManager<IdentityUser> userManager;

        public ServicioUsuarios(IHttpContextAccessor httpContext, UserManager<IdentityUser> userManager)
        {
            this.httpContext = httpContext;
            this.userManager = userManager;
        }
        public async Task<string> ObtenerUsuarioId()
        {
            var email = httpContext.HttpContext!.User.Claims.FirstOrDefault(x => x.Type == "email")!.Value;
            var usuario = await userManager.FindByEmailAsync(email);
            return usuario!.Id;
        }
    }
}
