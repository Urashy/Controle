using Microsoft.VisualStudio.TestTools.UnitTesting;
using Api.Controllers;
using Api.Models;
using Api.Models.EntityFramework;
using Api.Managers;
using Api.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers.Tests
{
    [TestClass()]
    public class AnimalControllerTests
    {
        private AppDbContext _context;
        private AnimalController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestAnimalDb_" + System.Guid.NewGuid())
                .Options;

            _context = new AppDbContext(options);

            // Ajout de données de test
            SeedDatabase();

            // Création du manager et du contrôleur
            var manager = new AnimalManager(_context);
            _controller = new AnimalController(manager);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedDatabase()
        {
            var animals = new List<Animal>
            {
                new Animal { Id = 1, Name = "Lion", Species = "Panthera leo", Poids = 190, Taille = 120 },
                new Animal { Id = 2, Name = "Éléphant", Species = "Loxodonta africana", Poids = 6000, Taille = 320 },
                new Animal { Id = 3, Name = "Girafe", Species = "Giraffa camelopardalis", Poids = 1200, Taille = 550 }
            };

            _context.Animaux.AddRange(animals);
            _context.SaveChanges();
        }

        [TestMethod()]
        public async Task GetAnimaux_ReturnsAllAnimals()
        {
            // Act
            var result = await _controller.GetAnimaux();

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var animals = okResult.Value as IEnumerable<AnimalDTO>;
            Assert.IsNotNull(animals);
            Assert.AreEqual(3, animals.Count());
        }

        [TestMethod()]
        public async Task GetAnimalById_ExistingId_ReturnsAnimal()
        {
            // Arrange
            int existingId = 1;

            // Act
            var result = await _controller.GetAnimalById(existingId);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var animal = okResult.Value as Animal;
            Assert.IsNotNull(animal);
            Assert.AreEqual(existingId, animal.Id);
            Assert.AreEqual("Lion", animal.Name);
        }

        [TestMethod()]
        public async Task GetAnimalById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingId = 999;

            // Act
            var result = await _controller.GetAnimalById(nonExistingId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public async Task GetAnimalByName_ExistingName_ReturnsAnimal()
        {
            // Arrange
            string existingName = "Lion";

            // Act
            var result = await _controller.GetAnimalByName(existingName);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var animalDto = okResult.Value as AnimalDTO;
            Assert.IsNotNull(animalDto);
            Assert.AreEqual("Lion", animalDto.Name);
        }

        [TestMethod()]
        public async Task GetAnimalByName_PartialName_ReturnsAnimal()
        {
            // Arrange
            string partialName = "éléph";

            // Act
            var result = await _controller.GetAnimalByName(partialName);

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var animalDto = okResult.Value as AnimalDTO;
            Assert.IsNotNull(animalDto);
            Assert.AreEqual("Éléphant", animalDto.Name);
        }

        [TestMethod()]
        public async Task GetAnimalByName_NonExistingName_ReturnsNotFound()
        {
            // Arrange
            string nonExistingName = "Licorne";

            // Act
            var result = await _controller.GetAnimalByName(nonExistingName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public async Task PostAnimal_ValidAnimal_ReturnsCreatedResult()
        {
            // Arrange
            var newAnimalDto = new AnimalDTO
            {
                Name = "Tigre",
                Species = "Panthera tigris",
                Poids = 220,
                Taille = 110
            };

            // Act
            var result = await _controller.PostAnimal(newAnimalDto);

            // Assert
            Assert.IsNotNull(result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            var createdAnimal = createdResult.Value as Animal;
            Assert.IsNotNull(createdAnimal);
            Assert.AreEqual("Tigre", createdAnimal.Name);

            // Vérifier que l'animal a bien été ajouté à la base de données
            var allAnimals = await _context.Animaux.ToListAsync();
            Assert.AreEqual(4, allAnimals.Count);
        }

        [TestMethod()]
        public async Task PutAnimal_ValidUpdate_ReturnsNoContent()
        {
            // Arrange
            int existingId = 1;
            var updatedAnimal = new Animal
            {
                Id = existingId,
                Name = "Lion Mis à Jour",
                Species = "Panthera leo",
                Poids = 200,
                Taille = 125
            };

            // Act
            var result = await _controller.PutAnimal(existingId, updatedAnimal);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            // Vérifier que les modifications ont été appliquées
            var animal = await _context.Animaux.FindAsync(existingId);
            Assert.IsNotNull(animal);
            Assert.AreEqual("Lion Mis à Jour", animal.Name);
            Assert.AreEqual(200, animal.Poids);
        }

        [TestMethod()]
        public async Task PutAnimal_MismatchedId_ReturnsBadRequest()
        {
            // Arrange
            int urlId = 1;
            var updatedAnimal = new Animal
            {
                Id = 2, // ID différent de celui dans l'URL
                Name = "Test",
                Species = "Test",
                Poids = 100,
                Taille = 100
            };

            // Act
            var result = await _controller.PutAnimal(urlId, updatedAnimal);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod()]
        public async Task PutAnimal_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingId = 999;
            var updatedAnimal = new Animal
            {
                Id = nonExistingId,
                Name = "Test",
                Species = "Test",
                Poids = 100,
                Taille = 100
            };

            // Act
            var result = await _controller.PutAnimal(nonExistingId, updatedAnimal);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public async Task DeleteAnimal_ExistingId_ReturnsNoContent()
        {
            // Arrange
            int existingId = 1;
            int initialCount = await _context.Animaux.CountAsync();

            // Act
            var result = await _controller.DeleteAnimal(existingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            // Vérifier que l'animal a bien été supprimé
            int finalCount = await _context.Animaux.CountAsync();
            Assert.AreEqual(initialCount - 1, finalCount);

            var deletedAnimal = await _context.Animaux.FindAsync(existingId);
            Assert.IsNull(deletedAnimal);
        }

        [TestMethod()]
        public async Task DeleteAnimal_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            int nonExistingId = 999;

            // Act
            var result = await _controller.DeleteAnimal(nonExistingId);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod()]
        public async Task GetAnimaux_EmptyDatabase_ReturnsEmptyList()
        {
            // Arrange - Vider la base de données
            _context.Animaux.RemoveRange(_context.Animaux);
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetAnimaux();

            // Assert
            Assert.IsNotNull(result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var animals = okResult.Value as IEnumerable<AnimalDTO>;
            Assert.IsNotNull(animals);
            Assert.AreEqual(0, animals.Count());
        }
    }
}