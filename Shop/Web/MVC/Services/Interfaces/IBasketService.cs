namespace MVC.Services.Interfaces
{
    public interface IBasketService
    {
        public Task<string> BasketLogger(string message);
        public Task<string> BasketLoggerId();
    }
}
