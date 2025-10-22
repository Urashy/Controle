namespace Api.Models.Repository.Interfaces
{
    public interface IDataRepository<TEntity, in TIdentifier, in TKey>
     : ISearchableRepository<TEntity, TKey>,
         IReadableRepository<TEntity, TIdentifier>,
         IWriteableRepository<TEntity>;
}
