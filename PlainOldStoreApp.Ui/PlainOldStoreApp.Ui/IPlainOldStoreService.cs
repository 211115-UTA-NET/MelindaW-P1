using PlainOldStoreApp.Ui.Dots;

namespace PlainOldStoreApp.Ui
{
    internal interface IPlainOldStoreService
    {
        Task<bool> GetIfEmailFound(string email);
        Task<List<Customer>> GetAllCustomersByFullName(string firstName, string lastName);
        Task<Guid> GetCustomerId(string email);
    }
}
