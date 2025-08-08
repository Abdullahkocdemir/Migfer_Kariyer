using Microsoft.AspNetCore.Mvc;
using Migfer_Kariyer.Data;
using Migfer_Kariyer.Entities;

namespace Migfer_Kariyer.Controllers
{
    public class AdminAboutTextController : Controller
    {
        private readonly ApplicationContext _context;

        public AdminAboutTextController(ApplicationContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var values = _context.AboutTexts.ToList();
            return View(values);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(AboutText aboutText)
        {
            if (ModelState.IsValid)
            {
                _context.AboutTexts.Add(aboutText);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aboutText);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var values = _context.AboutTexts.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult Edit(AboutText aboutText)
        {
            if (ModelState.IsValid)
            {
                var values = _context.AboutTexts.Find(aboutText.Id);
                if (values != null)
                {
                    values.Başlık = aboutText.Başlık;
                    values.Açıklama = aboutText.Açıklama;
                    values.IsActive = aboutText.IsActive;
                    _context.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(aboutText);
        }
        public IActionResult Detail(int id)
        {
            var values = _context.AboutTexts.Find(id);
            return View(values);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var values = _context.AboutTexts.Find(id);
            if (values != null)
            {
                _context.AboutTexts.Remove(values);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
