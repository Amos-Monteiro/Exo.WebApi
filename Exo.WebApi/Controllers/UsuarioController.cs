using Exo.WebApi.Models;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Exo.WebApi.Controllers
{
    [Produces("application/jason")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;
        private IEnumerable<Claim> claims;

        public UsuariosController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(_usuarioRepository.Listar());
        } 
        // // [HttpPost]
        // // public IActionResult Cadastrar(Usuario usuario)
        // // {
        // //     _usuarioRepository.Cadastrar(usuario);
        // //     return StatusCode(201);
        // // }
        public IActionResult Post(Usuario usuario)
        {
            Usuario usuarioBuscado = _usuarioRepository.Login(usuario.Email, usuario.Senha);
            if(usuarioBuscado == null)
            {
                return NotFound("E-mail ou senha inválidos");
            }
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken
            (
                issuer: "exoapi.webapi", // Emisor do token
                audience: "exoapi.webapi", //Destinatário do token.
                claims: claims, //dados fornecidos acima.
                expires: DateTime.Now.AddMinutes(30), // tempo para expirar.
                signingCredentials: creds //Credenciais do token.
            );
            return Ok(
                new {token = new JwtSecurityTokenHandler().WriteToken(token) }

            );

        }
        [HttpGet("{id}")]
        public IActionResult BuscaPorId(int id)
        {
            Usuario usuario = _usuarioRepository.BuscaPorId(id);
            if(usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Usuario usuario)
        {
            _usuarioRepository.Atualizar(id, usuario);
            return StatusCode(201);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                _usuarioRepository.Deletar(id);
                return StatusCode(201);

            }
            catch (Exception )
            {
                
                return BadRequest();
            }
        }

        
    }
    
}