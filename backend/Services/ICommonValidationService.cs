using backend.Dtos;

namespace backend.Services
{
    public interface ICommonValidationService
    {
        Task<T?> FindByIdAsync<T>(int id) where T : class, IEntityWithId;
        Task<ServiceResponse<bool>> NameExistsAsync<T>(string name, int? id = null) where T : class, IEntityWithName;
        Task<ServiceResponse<bool>> NameAndBirthDateExistsAsync<T>(string name, DateOnly birthDate, int? id = null) where T : class, IEntityWithNameAndBirthDate;
    }
}
