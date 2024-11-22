using CHRISTIANEXAMENFINAL.Models;
using CHRISTIANEXAMENFINAL.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CHRISTIANEXAMENFINAL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsistenteController : ControllerBase
    {
        private readonly EventosRepository _eventosRepository;

        // Constructor para inyectar el repositorio
        public AsistenteController(EventosRepository eventosRepository)
        {
            _eventosRepository = eventosRepository ?? throw new ArgumentNullException(nameof(eventosRepository));
        }

        // GET: api/Asistente
        [HttpGet]
        public async Task<ActionResult<List<AsistenteEvento>>> GetAsistente()
        {
            try
            {
                var asistentes = await _eventosRepository.ListarAsistentes();
                if (asistentes == null || asistentes.Count == 0)
                {
                    return NotFound("No se encontraron asistentes.");
                }
                return Ok(asistentes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al obtener asistentes: {ex.Message}");
            }
        }



    }

    }
