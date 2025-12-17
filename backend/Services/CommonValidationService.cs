using backend.Context;
using backend.Dtos;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class CommonValidationService : ICommonValidationService
    {
        private readonly FootballDbContext _context;

        public CommonValidationService(FootballDbContext context) 
        { 
            _context = context; 
        }

        //T: is an object that has no value initatly, however when you call this function T will be a class like Teams or Matches, which has an Id
        //IEntityWithId is needed since it doesn't know whether the given class has an id in. (the same goes with the other functions)
        public async Task<T?> FindByIdAsync<T>(int id) where T : class, IEntityWithId
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<bool> NameExistsAsync<T>(string name, int? id) where T : class, IEntityWithName
        {
            var entity = _context.Set<T>().Where(e => e.Name == name);

            //it's needed when you call this at put methods, since the changed object's name can be equal with the original record's name
            if (id.HasValue)
            {
                entity = entity.Where(e => e.Id != id);
            }

            return await entity.AnyAsync();
        }

        public async Task<ServiceResponse<bool>> NameAndBirthDateExistsAsync<T>(string name, DateOnly birth_date, int? id = null) where T : class, IEntityWithNameAndBirthDate
        {
            if (birth_date > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return new ServiceResponse<bool> { Success = false, Message = "Invalid birth date" };
            }

            var entity = _context.Set<T>().Where(e => e.Name == name && e.Birth_date == birth_date);

            if (id.HasValue)
            {
                entity = entity.Where(e => e.Id != id);
            }

            if (await entity.AnyAsync())
            {
                var entityName = typeof(T).Name;
                return new ServiceResponse<bool> { Success = false, Message = $"{entityName} with this name and birth date already exists." };
            }

            return new ServiceResponse<bool> { Success = true };
        }
    }
}
