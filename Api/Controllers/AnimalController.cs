using Api.Mapper;
using Api.Models;
using Api.Models.DTO;
using Api.Models.EntityFramework;
using Api.Models.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimalController : Controller
    {
        private readonly IAnimalRepository<Animal, int, string> _repository;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        /// <summary>
        /// Constructeur pour le contrôleur SoftwaresController.
        /// </summary>
        /// <param name="repository">Le DataRepository utilisé pour accéder aux logiciels.</param>
        public AnimalController(IAnimalRepository<Animal, int, string> repository)
        {
            _repository = repository;

            _context = new AppDbContext();
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperProfile());
            });
            _mapper = config.CreateMapper();
        }

        /// <summary>
        /// Récupère tous les logiciels.
        /// </summary>
        /// <returns>Une liste de logiciels sous forme de réponse HTTP 200 OK.</returns>
        /// <response code="200">La liste des logiciels a été récupérée avec succès.</response>
        /// <response code="500">Une erreur interne s'est produite sur le serveur.</response>
        // GET: Softwares
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AnimalDTO>>> GetAnimaux()
        {
            IEnumerable<Animal> animaux = await _repository.GetAllAsync();

            IEnumerable<AnimalDTO> animauxDto = _mapper.Map<IEnumerable<AnimalDTO>>(animaux);
            return Ok(animauxDto);
        }

        /// <summary>
        /// Récupère un software avec son id.
        /// </summary>
        /// <param name="id">L'id du software.</param>
        /// <returns>Un software sous forme de réponse HTTP 200 OK.</returns>
        /// <response code="200">Le software a été récupéré avec succès.</response>
        /// <response code="404">Le software demandé n'existe pas.</response>
        /// <response code="500">Une erreur interne s'est produite sur le serveur.</response>
        // GET: Softwares/GetById/5
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Animal>> GetAnimalById(int id)
        {
            Animal animal = await _repository.GetByIdAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            return Ok(animal);
        }

        /// <summary>
        /// Récupère une liste de softwares avec une référence.
        /// </summary>
        /// <param name="reference">La référence du software.</param>
        /// <returns>Une liste de softwares sous forme de réponse HTTP 200 OK.</returns>
        /// <response code="200">Les softwares ont été récupéré avec succès.</response>
        /// <response code="404">Le software demandé n'existe pas.</response>
        /// <response code="500">Une erreur interne s'est produite sur le serveur.</response>
        // GET: Softwares/GetByReference/5
        [HttpGet]
        [Route("[action]/{name}")]
        [ActionName("GetByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AnimalDTO>> GetAnimalByName(string name)
        {
            Animal animal = await _repository.GetByKeyAsync(name);

            if (animal == null)
            {
                return NotFound();
            }

            AnimalDTO animalDto = _mapper.Map<AnimalDTO>(animal);
            return Ok(animalDto);
        }

        /// <summary>
        /// Modifie un software.
        /// </summary>
        /// <param name="id">L'id du software.</param>
        /// <param name="software">L'objet software.</param>
        /// <returns>Une réponse HTTP 204 NoContent.</returns>
        /// <response code="204">Le software a été modifié avec succès.</response>
        /// <response code="400">L'id donné ne correspond pas à l'id du software.</response>
        /// <response code="404">Le software n'existe pas.</response>
        /// <response code="500">Une erreur interne s'est produite sur le serveur.</response>
        // PUT: Softwares/Put/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutSoftware(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ActionResult<Animal> animalToUpdate = await _repository.GetByIdAsync(id);
            if (animalToUpdate.Value == null)
            {
                return NotFound();
            }

            await _repository.UpdateAsync(animalToUpdate.Value, animal);
            return NoContent();
        }

        /// <summary>
        /// Créer un software.
        /// </summary>
        /// <param name="softwareDto">L'objet software.</param>
        /// <returns>Une réponse HTTP 201 Created.</returns>
        /// <response code="201">Le software a été créé avec succès.</response>
        /// <response code="400">Le format du software est incorrect.</response>
        /// <response code="500">Une erreur interne s'est produite sur le serveur.</response>
        // POST: Softwares/Post
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Animal>> PostSoftware(AnimalDTO animalDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Animal animal = _mapper.Map<Animal>(animalDto);

            await _repository.AddAsync(animal);
            return CreatedAtAction("GetById", new { id = animal.Id }, animal);
        }

        /// <summary>
        /// Supprime un software.
        /// </summary>
        /// <param name="id">L'id du software.</param>
        /// <returns>Une réponse HTTP 204 No Content.</returns>
        /// <response code="204">Le software a été supprimé avec succès.</response>
        /// <response code="404">Le software n'existe pas.</response>
        /// <response code="500">Une erreur interne s'est produite sur le serveur.</response>
        // DELETE: Softwares/Delete/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteSoftware(int id)
        {
            Animal animal = await _repository.GetByIdAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(animal);
            return NoContent();
        }
    }
}
