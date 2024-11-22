using CHRISTIANEXAMENFINAL.Repository;
using CHRISTIANEXAMENFINAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CHRISTIANEXAMENFINAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly EventosRepository _eventosRepository;

            public EventosController(EventosRepository eventosRepository)
    {
        _eventosRepository = eventosRepository ?? throw new ArgumentNullException(nameof(eventosRepository));
    }

        // GET: api/Eventos
        [HttpGet]
        public async Task<ActionResult<List<EventoCorporativo>>> GetEventos()
        {
            var eventos = await _eventosRepository.ListarEventos();
            return Ok(eventos);
        }

        // GET: api/Eventos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<EventoCorporativo>> GetEvento(int id)
        {
            var eventos = await _eventosRepository.ListarEventos();
            var evento = eventos.Find(e => e.EventoID == id);
            if (evento == null)
            {
                return NotFound();
            }

            return Ok(evento);
        }

        // Editar Evento: api/Eventos/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult> PutEvento(int id, [FromBody] EventoCorporativo evento)
        {
            if (id != evento.EventoID)
            {
                return BadRequest("El ID del evento no coincide.");
            }

            var result = await _eventosRepository.EditarEvento(evento);
            if (string.IsNullOrEmpty(result))
            {
                return Ok(evento);
            }
            else
            {
                return BadRequest(result);
            }
        }


        // POST: api/Eventos
        [HttpPost]
        public async Task<ActionResult> PostEvento([FromBody] EventoCorporativo evento)
        {
            var result = await _eventosRepository.GuardarEvento(evento);
            if (string.IsNullOrEmpty(result))
            {
                return Ok();
            }
            else
            {
                return BadRequest(result);
            }
        }

        // DELETE: api/Eventos/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvento(int id)
        {
            var result = await _eventosRepository.EliminarEvento(id);
            if (string.IsNullOrEmpty(result))
            {
                return NoContent();
            }
            else
            {
                return BadRequest(result);
            }
        }

        // GET: api/Eventos/{id}/Asistentes
        [HttpGet("{id}/asistentes")]
        public async Task<ActionResult<List<AsistenteEvento>>> GetAsistentes(int id)
        {
            var asistentes = await _eventosRepository.ListarAsistentes(id);
            return Ok(asistentes);
        }
    }
}