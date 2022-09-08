using System.Threading.Tasks;

namespace DemocracyBot.Integration.Insult
{
    public interface IEvilInsultService
    {
        Task<string> GetInsult();
    }
}