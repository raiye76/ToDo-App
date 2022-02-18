using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Net.Mail;
using ToDo_App.Infrastructure.Repositories;
using ToDo_App.Models;

namespace ToDo_App.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IConfiguration _configuration;

        public ToDoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: ToDoController
        public ActionResult Index()
        {
            ToDoRepository toDoRepository = new ToDoRepository(_configuration, HttpContext.User);
            IEnumerable<ToDoModel> toDos = toDoRepository.GetToDos();

            return View(toDos);
        }

        // GET: ToDoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ToDoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                StringValues title;
                collection.TryGetValue("Title", out title);

                StringValues description;
                collection.TryGetValue("Description", out description);

                ToDoModel toDoModel = new ToDoModel()
                {
                    Title = title.ToString(),
                    Description = description.ToString()
                };

                ToDoRepository toDoRepository = new ToDoRepository(_configuration, HttpContext.User);
                bool isInsert = toDoRepository.Insert(toDoModel);

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ToDoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ToDoController/Edit/5
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

        // GET: ToDoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ToDoController/Delete/5
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
