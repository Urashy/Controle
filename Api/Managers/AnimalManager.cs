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
        private readonly DbSet<Animal> _animaux;

        public AnimalManager(AppDbContext context)
        {
            _context = context;
            _animaux = _context.Animaux;
        }

        public async Task AddAsync(Animal entity)
        {
            await _animaux.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Animal entity)
        {
            _animaux.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Animal>> GetAllAsync()
        {
            return await _animaux.ToListAsync();
        }

        public async Task<Animal> GetByIdAsync(int id)
        {
            return await _animaux.FindAsync(id);
        }

        public async Task<Animal> GetByKeyAsync(string key)
        {
            return await _animaux.FirstOrDefaultAsync(s => s.Name.ToUpper().Contains(key.ToUpper()));
        }

        public async Task UpdateAsync(Animal entityToUpdate, Animal entity)
        {
            _animaux.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
