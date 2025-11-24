namespace backend.Services
{
    public interface ICommonValidationService
    {
        Task<T?> FindByIdAsync<T>(int id) where T : class, IEntityWithId;
        Task<bool> NameExistsAsync<T>(string name, int? id = null) where T : class, IEntityWithName;
        Task<bool> NameAndBirthDateExistsAsync<T>(string name, DateOnly birth_date, int? id = null) where T : class, IEntityWithNameAndBirthDate;
    }
}
