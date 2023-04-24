using Exo.WebApi.Models;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Exo.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetoController : ControllerBase
    {
        private readonly ProjetoRepository _projetoRepository;
        public ProjetoController(ProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }
        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(_projetoRepository.Listar());
        }
        [HttpGet("{id}")]
        public IActionResult BuscaPorId(int id)
        {
            Projeto projeto = _projetoRepository.BuscaPorId(id);
            if(projeto == null)
            {
                return NotFound("Não encontrado");
            }
            return Ok(projeto);
        }
        [HttpPost]
        public IActionResult Cadastrar (Projeto projeto)
        {
            _projetoRepository.Cadastrar(projeto);
            return StatusCode(204);
        }
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Projeto projeto)
        {
          _projetoRepository.Atualizar(id, projeto);
          return StatusCode(204);  
        }
        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                _projetoRepository.Deletar(id);
                return
                StatusCode(204);
            }
            catch (Exception )
            {
                return BadRequest();
                
            }
        }
        
    }
}