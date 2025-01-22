using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PeliculasApi.DTOs.Response;
using PeliculasApi.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PeliculasApi.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "esadmin")]

    public class UsuariosController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext dbContext;
        private readonly IMapper mapper;


        public UsuariosController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration, ApplicationDbContext dbContext, IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        [HttpGet("ListadoUsuarios")]
        public async Task<ActionResult<List<UsuarioResponseDTO>>> ListadoUsuarios([FromQuery] PaginacionResponseDTO paginacionResponseDTO)
        {
            var queryable = dbContext.Users.AsQueryable();
            await HttpContext.InsertarParametrosPaginacionEnCabecera(queryable);
            var usuarios = await queryable.ProjectTo<UsuarioResponseDTO>(mapper.ConfigurationProvider)
                .OrderBy(x => x.Email).Paginar(paginacionResponseDTO).ToListAsync();
            return usuarios;
        }

        [HttpGet("obtenerUsuarios")]
        public async Task<IActionResult> ObtenerUsuariosClaim([FromQuery] PaginacionResponseDTO paginacion)
        {
            var usuarios = userManager.Users.Skip((paginacion.Pagina - 1) * paginacion.RecordsPorPagina)
                .Take(paginacion.RecordsPorPagina)
                .ToList();

            var usuariosDto = new List<UsuarioResponseDTO>();

            foreach(var usuario in usuarios)
            {
                var claims = await userManager.GetClaimsAsync(usuario);
                var esAdmin = claims.Any(c => c.Type == "esadmin" && c.Value == "true");

                usuariosDto.Add(new UsuarioResponseDTO
                {
                    Email = usuario.Email,
                    EsAdmin = esAdmin
                });
            }
            var totalRegistros = userManager.Users.Count();

            Response.Headers.Add("cantidad-total-registros", totalRegistros.ToString());
            return Ok(usuariosDto);
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

        [HttpPost("hacerAdmin")]
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

        [HttpPost("removerAdmin")]
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
