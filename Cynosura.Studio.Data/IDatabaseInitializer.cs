using System.Threading.Tasks;

namespace Cynosura.Studio.Data
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }
}
