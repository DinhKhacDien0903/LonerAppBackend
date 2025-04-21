namespace Loner.Domain.Interfaces;

public interface IPreferenceRepository : IBaseRepository<PreferenceEntity>
{
    Task<PreferenceEntity?> GetByUserId(string userId);
}