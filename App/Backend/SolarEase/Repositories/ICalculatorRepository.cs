using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface ICalculatorRepository
    {
        Task<List<Calculator>> GetAllAsync();
        Task<Calculator?> GetByIdAsync(int id);
        Task<Calculator?> GetByPersonIdAsync(int personId);
        Task<Calculator> CreateAsync(Calculator calculatorModel);
        Task<Calculator?> DeleteAsync(int id);
        Task<List<Calculator>?> DeleteAllByPersonAsync(int personId);
    }
}
