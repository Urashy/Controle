using Front.Models;

namespace Front.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AnimalService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<AnimalDTO>> GetAllAnimalsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.GetFromJsonAsync<List<AnimalDTO>>("api/Animal");
                return response ?? new List<AnimalDTO>();
            }
            catch
            {
                return new List<AnimalDTO>();
            }
        }

        public async Task<AnimalDTO?> GetAnimalByIdAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                return await client.GetFromJsonAsync<AnimalDTO>($"api/Animal/GetById/{id}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<AnimalDTO?> GetAnimalByNameAsync(string name)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                return await client.GetFromJsonAsync<AnimalDTO>($"api/Animal/GetByName/{name}");
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CreateAnimalAsync(AnimalDTO animal)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.PostAsJsonAsync("api/Animal", animal);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAnimalAsync(int id, AnimalDTO animal)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");

                var animalToUpdate = new AnimalDTO
                {
                    Id = id,
                    Name = animal.Name,
                    Species = animal.Species,
                    Poids = animal.Poids,
                    Taille = animal.Taille
                };

                var response = await client.PutAsJsonAsync($"api/Animal/{id}", animalToUpdate);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur UpdateAnimal: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAnimalAsync(int id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiClient");
                var response = await client.DeleteAsync($"api/Animal/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
