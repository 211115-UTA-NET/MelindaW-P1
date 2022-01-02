using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlainOldStoreApp.DataStorage;

namespace PlainOldStoreApi.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlainOldStoreAppController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;
        public PlainOldStoreAppController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        // GET: Get by email

        // GET: Get by email

        // GET: Get by firstName, lastName

        // POST: Add new customer



        // GET: PlainOldStoreAppController
        public ActionResult Index()
        {
            return View();
        }

        // GET: PlainOldStoreAppController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PlainOldStoreAppController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PlainOldStoreAppController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlainOldStoreAppController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PlainOldStoreAppController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlainOldStoreAppController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PlainOldStoreAppController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
