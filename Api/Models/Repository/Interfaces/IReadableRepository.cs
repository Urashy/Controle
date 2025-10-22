namespace Api.Models.Repository.Interfaces
{
    public interface IReadableRepository<TEntity, in TIdentifier>
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(TIdentifier id);
    }
}
