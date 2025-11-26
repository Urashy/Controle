using Api.Models.Repository.Interfaces;

namespace Api.Models.Repository
{
    public interface IAnimalRepository<TEntity, TIdentifier, TKey> : IDataRepository<TEntity, TIdentifier, TKey>
    {
    }
}
