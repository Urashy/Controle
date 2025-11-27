using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Bunit;
using Front.Components.Pages;
using Front.Services;
using Front.ViewModels;
using Front.Models;

namespace FrontTests.Pages
{
    [TestClass]
    public class AnimauxPageTests : Bunit.TestContext
    {
        private Mock<IAnimalService>? _mockAnimalService;
        private List<AnimalDTO>? _testAnimals;

        [TestInitialize]
        public void Setup()
        {
            _testAnimals = new List<AnimalDTO>
            {
                new AnimalDTO { Id = 1, Name = "Lion", Species = "Panthera leo", Poids = 190, Taille = 120 },
                new AnimalDTO { Id = 2, Name = "Éléphant", Species = "Loxodonta africana", Poids = 6000, Taille = 320 }
            };

            _mockAnimalService = new Mock<IAnimalService>();
            _mockAnimalService
                .Setup(s => s.GetAllAnimalsAsync())
                .ReturnsAsync(_testAnimals);

            Services.AddScoped<IAnimalService>(_ => _mockAnimalService.Object);
            Services.AddScoped<AnimalViewModel>();
        }

        [TestMethod]
        public void AnimauxPage_Affiche_le_titre()
        {
            // Act
            var cut = RenderComponent<AnimauxPage>();

            // Assert
            var title = cut.Find("h1");
            Assert.AreEqual("Gestion des Animaux", title.TextContent);
        }

        [TestMethod]
        public void AnimauxPage_A_une_barre_de_recherche()
        {
            // Act
            var cut = RenderComponent<AnimauxPage>();

            // Assert
            var searchInput = cut.Find("input[placeholder='Rechercher par nom...']");
            Assert.IsNotNull(searchInput);
        }

        [TestCleanup]
        public void Cleanup()
        {
            Dispose();
        }
    }
}