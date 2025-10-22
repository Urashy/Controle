namespace Api.Models.Repository.Interfaces
{
    public interface IWriteableRepository<in TEntity>
    {
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entityToUpdate, TEntity entity);
        Task DeleteAsync(TEntity entity);
    }
}
