using CHRISTIANEXAMENFINAL.Repository;
using CHRISTIANEXAMENFINAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CHRISTIANEXAMENFINAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TiposController : ControllerBase
    {
        private readonly EventosRepository _eventosRepository;

        public TiposController(EventosRepository eventosRepository)
        {
            _eventosRepository = eventosRepository ?? throw new ArgumentNullException(nameof(eventosRepository));
        }

        // Método para obtener los tipos de evento
        [HttpGet]
        public async Task<ActionResult<List<TipoEvento>>> GetTipos()
        {
            try
            {
                var tipos = await _eventosRepository.ListarTiposEvento();
                if (tipos == null || tipos.Count == 0)
                {
                    return NotFound("No se encontraron tipos de evento.");
                }
                return Ok(tipos);  // Devuelve los tipos de evento como JSON
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener tipos de evento: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> PutTipos(int id, [FromBody] TipoEvento tipos)
        {
            if (id != tipos.TipoEventoID)
            {
                return BadRequest("El ID del evento no coincide.");
            }

            var result = await _eventosRepository.EditarTipos(tipos);
            if (string.IsNullOrEmpty(result))
            {
                return Ok(tipos);
            }
            else
            {
                return BadRequest(result);
            }
        }






    }
}