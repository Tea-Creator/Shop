using Basket.Domain.Models;

using System.Threading.Tasks;

namespace Basket.Application.Repositories
{
    public interface IBasketRepository
    {
        Task<BasketCart> Get(string username);

        Task<BasketCart> Update(BasketCart cart);

        Task<bool> Delete(string username);
    }
}
