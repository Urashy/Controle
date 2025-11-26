using Api.Models;
using Api.Models.EntityFramework;
using Api.Models.Repository;
using Microsoft.EntityFrameworkCore;
using System;

namespace Api.Managers
{
    public class AnimalManager : IAnimalRepository<Animal, int, string>
    {
        private readonly AppDbContext? _context;
        private readonly DbSet<Animal> _softwares;

        public AnimalManager(AppDbContext context)
        {
            _context = context;
            _softwares = _context.Animaux;
        }

        public async Task AddAsync(Animal entity)
        {
            await _softwares.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Animal entity)
        {
            _softwares.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Animal>> GetAllAsync()
        {
            return await _softwares.ToListAsync();
        }

        public async Task<Animal> GetByIdAsync(int id)
        {
            return await _softwares.FindAsync(id);
        }

        public async Task<Animal> GetByKeyAsync(string key)
        {
            return await _softwares.FirstOrDefaultAsync(s => s.Name.ToUpper() == key.ToUpper());
        }

        public async Task UpdateAsync(Animal entityToUpdate, Animal entity)
        {
            _softwares.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
