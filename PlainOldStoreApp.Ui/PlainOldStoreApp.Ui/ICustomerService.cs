using PlainOldStoreApp.Ui.Dots;

namespace PlainOldStoreApp.Ui
{
    public interface ICustomerService
    {
        Task<bool> GetIfEmailFound(string email);
        Task<List<Customer>> GetAllCustomersByFullName(string firstName, string lastName);
        Task<Guid> GetCustomerId(string email);
        Task<bool> PostNewCustomer(Customer newCustomer);
    }
}
