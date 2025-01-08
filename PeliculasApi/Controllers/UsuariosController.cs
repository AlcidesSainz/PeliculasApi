using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PeliculasApi.DTOs.Response;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PeliculasApi.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "esadmin")]

    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public UsuariosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("registrar")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionResponseDTO>> Registrar(CredencialesUsuarioResponseDTO credencialesUsuarioResponseDTO)
        {
            var usuario = new IdentityUser
            {
                Email = credencialesUsuarioResponseDTO.Email,
                UserName = credencialesUsuarioResponseDTO.Email
            };
            var resultado = await userManager.CreateAsync(usuario, credencialesUsuarioResponseDTO.Password);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(usuario);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }

        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<RespuestaAutenticacionResponseDTO>> Login(CredencialesUsuarioResponseDTO credencialesUsuarioResponseDTO)
        {
            var usuario = await userManager.FindByEmailAsync(credencialesUsuarioResponseDTO.Email);

            if (usuario is null)
            {
                var errores = ConstruirLoginIncorrecto();
                return BadRequest(errores);
            }
            var resultado = await signInManager.CheckPasswordSignInAsync(usuario, credencialesUsuarioResponseDTO.Password, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await ConstruirToken(usuario);
            }
            else
            {
                var errores = ConstruirLoginIncorrecto();
                return BadRequest(errores);
            }
        }

        [HttpOptions("HacerAdmin")]
        public async Task<IActionResult> HacerAdmin(EditarClaimResponseDTO editarClaimResponseDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimResponseDTO.Email);
            if (usuario is null)
            {
                return NotFound();
            }
            await userManager.AddClaimAsync(usuario, new Claim("esadmin", "true"));
            return NoContent();
        }

        [HttpOptions("RemoverAdmin")]
        public async Task<IActionResult> RemoverAdmin(EditarClaimResponseDTO editarClaimResponseDTO)
        {
            var usuario = await userManager.FindByEmailAsync(editarClaimResponseDTO.Email);
            if (usuario is null)
            {
                return NotFound();
            }
            await userManager.RemoveClaimAsync(usuario, new Claim("esadmin", "true"));
            return NoContent();
        }

        private IEnumerable<IdentityError> ConstruirLoginIncorrecto()
        {
            var identityError = new IdentityError() { Description = "Login Incorrecto" };
            var errores = new List<IdentityError>();
            errores.Add(identityError);
            return errores;
        }

        private async Task<RespuestaAutenticacionResponseDTO> ConstruirToken(IdentityUser identityUser)
        {
            var claims = new List<Claim>
            {
                new Claim("email", identityUser.Email!),
                new Claim("lo que yo quiera", "cualquier valor")
            };
            var claimsDB = await userManager.GetClaimsAsync(identityUser);
            claims.AddRange(claimsDB);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]!));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddYears(1);

            var tokenSeguridad = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiracion,
                signingCredentials: creds
            );
            var token = new JwtSecurityTokenHandler().WriteToken(tokenSeguridad);

            return new RespuestaAutenticacionResponseDTO
            {
                Token = token,
                Expiracion = expiracion
            };
        }
    }
}
