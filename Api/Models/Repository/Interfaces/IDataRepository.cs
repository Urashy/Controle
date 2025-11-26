namespace Api.Models.Repository.Interfaces
{
    public interface IDataRepository<TEntity, TIdentifier, TKey> : IReadableRepository<TEntity, TIdentifier>, IWriteableRepository<TEntity>, ISearchableRepository<TEntity, TKey>
    {
    }
}
