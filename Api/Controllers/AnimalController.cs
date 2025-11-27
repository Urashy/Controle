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
        /// Constructeur pour le contrôleur animalController.
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
        // GET: animal
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
        /// Récupère un animal avec son id.
        /// </summary>
        /// <param name="id">L'id du animal.</param>
        /// <returns>Un animal sous forme de réponse HTTP 200 OK.</returns>
        /// <response code="200">Le animal a été récupéré avec succès.</response>
        /// <response code="404">Le animal demandé n'existe pas.</response>
        /// <response code="500">Une erreur interne s'est produite sur le serveur.</response>
        // GET: animal/GetById/5
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
        /// Récupère une liste de animal avec une référence.
        /// </summary>
        /// <param name="reference">La référence du animal.</param>
        /// <returns>Une liste de animal sous forme de réponse HTTP 200 OK.</returns>
        /// <response code="200">Les animal ont été récupéré avec succès.</response>
        /// <response code="404">Le animal demandé n'existe pas.</response>
        /// <response code="500">Une erreur interne s'est produite sur le serveur.</response>
        // GET: animal/GetByReference/5
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
        /// Modifie un animal.
        /// </summary>
        /// <param name="id">L'id du animal.</param>
        /// <param name="animal">L'objet animal.</param>
        /// <returns>Une réponse HTTP 204 NoContent.</returns>
        /// <response code="204">Le animal a été modifié avec succès.</response>
        /// <response code="400">L'id donné ne correspond pas à l'id du animal.</response>
        /// <response code="404">Le animal n'existe pas.</response>
        /// <response code="500">Une erreur interne s'est produite sur le serveur.</response>
        // PUT: animal/Put/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PutAnimal(int id, Animal animal)
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
        /// Créer un animal.
        /// </summary>
        /// <param name="animalDto">L'objet animal.</param>
        /// <returns>Une réponse HTTP 201 Created.</returns>
        /// <response code="201">Le animal a été créé avec succès.</response>
        /// <response code="400">Le format du animal est incorrect.</response>
        /// <response code="500">Une erreur interne s'est produite sur le serveur.</response>
        // POST: animal/Post
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Animal>> PostAnimal(AnimalDTO animalDto)
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
        /// Supprime un animal.
        /// </summary>
        /// <param name="id">L'id du animal.</param>
        /// <returns>Une réponse HTTP 204 No Content.</returns>
        /// <response code="204">Le animal a été supprimé avec succès.</response>
        /// <response code="404">Le animal n'existe pas.</response>
        /// <response code="500">Une erreur interne s'est produite sur le serveur.</response>
        // DELETE: animal/Delete/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAnimal(int id)
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
