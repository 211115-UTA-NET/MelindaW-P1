using PlainOldStoreApp.Ui.Dots;

namespace PlainOldStoreApp.Ui
{
    internal interface IPlainOldStoreService
    {
        Task<List<Customer>> GetAllCustomersByFullName(string firstName, string lastName);
    }
}
