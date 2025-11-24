using backend.Dtos;
using backend.Dtos.Match;

namespace backend.Services.MatchValidation
{
    public interface IMatchValidationService
    {
        Task<ServiceResponse<MatchValidationData>> ValidateMatchCreationAsync(PostMatchDto matchDto);
        Task<ServiceResponse<MatchValidationData>> ValidateMatchUpdateAsync(PutMatchDto matchDto);
    }
}
