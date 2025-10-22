namespace Api.Models.Repository.Interfaces
{
    public interface ISearchableRepository<TEntity, in TKey>
    {
        Task<TEntity?> GetByKeyAsync(TKey key);
    }
}
