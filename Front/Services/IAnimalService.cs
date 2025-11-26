using Front.Models;

namespace Front.Services
{
    public interface IAnimalService
    {
        Task<List<AnimalDTO>> GetAllAnimalsAsync();
        Task<AnimalDTO?> GetAnimalByIdAsync(int id);
        Task<AnimalDTO?> GetAnimalByNameAsync(string name);
        Task<bool> CreateAnimalAsync(AnimalDTO animal);
        Task<bool> UpdateAnimalAsync(int id, AnimalDTO animal);
        Task<bool> DeleteAnimalAsync(int id);
    }
}
