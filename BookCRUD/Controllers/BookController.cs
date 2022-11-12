using BookAPI.Data;
using BookAPI.Models;
using BookCRUD.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace BookCRUD.Controllers
{
    public class BookController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        HttpClient client = new HttpClient();

        // GET: Book
        public IActionResult Index()
        {
            List<BookAPIDb> books = new List<BookAPIDb>();
            client.BaseAddress = new Uri("https://localhost:7152/api/BookAPI");
            var response = client.GetAsync("BookAPI");
            response.Wait();

            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<BookAPIDb>>();
                display.Wait();
                books = display.Result;
            }

            return View(books);
        }

        //GET: Book/Details/5
        public IActionResult Details(int id)
        {
            //BookAPIDb book = new BookAPIDb();
            client.BaseAddress = new Uri("https://localhost:7152/api/");
            var response = client.GetAsync("BookAPI/" + id);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<BookAPIDb>();
                display.Wait();
                var book = display.Result;
                return View(book);
            }
            else
            {
                TempData["error"] = "Book not found";
                return RedirectToAction("Index");
            }
            
        }

        // GET: Book/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookAPIDb obj, IFormFile? file)
        {
            if (file != null)
            {
                obj.Image = SaveImagePath(file);
            }


            client.BaseAddress = new Uri("https://localhost:7152/api/BookAPI");
            var response = client.PutAsJsonAsync<BookAPIDb>("BookAPI", obj);
            response.Wait();

            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                TempData["success"] = "Data is Successfully Added";
                return RedirectToAction("Index");
                
            }

            TempData["error"] = "Some Error Occured Please Re-Enter the Data";
            return View();
        }

        private string SaveImagePath(IFormFile file)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            string fileName = Guid.NewGuid().ToString();
            var uploads = Path.Combine(wwwRootPath, @"Images");
            var extension = Path.GetExtension(file.FileName);
            using (var fileStream = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return @"\Images\" + fileName + extension;
        }


        // GET: Book/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                TempData["error"] = "Data Not Found!";
                return View("Index");
            }
            client.BaseAddress = new Uri("https://localhost:7152/api/");
            var response = client.GetAsync("BookAPI/" + id);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<BookAPIDb>();
                display.Wait();
                var book = display.Result;
                return View(book);
            }
            return NotFound();
        }

        // POST: Book/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookAPIDb obj, IFormFile? file)
        {
            if (file != null)
            {
                obj.Image = SaveImagePath(file);
            }

            client.BaseAddress = new Uri("https://localhost:7152/api/BookAPI");
            var response = client.PutAsJsonAsync<BookAPIDb>("BookAPI", obj);
            response.Wait();

            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                int Id = obj.Id;
                TempData["success"] = "Data is Successfully Updated";
                return RedirectToAction("Details", new { id = Id});

            }

            TempData["error"] = "Some Error Occured Please Re-Enter the Data";
            return View();
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int id)
        {
            client.BaseAddress = new Uri("https://localhost:7152/api/");
            var response = client.GetAsync("BookAPI/" + id);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<BookAPIDb>();
                display.Wait();
                var book = display.Result;
                return View(book);
            }

            return NotFound();
        }

        // POST: Book/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(BookAPIDb obj)
        {
            try
            {
                client.BaseAddress = new Uri("https://localhost:7152/api/");
                var bookResponse = client.GetAsync("BookAPI/" + obj.Id);
                bookResponse.Wait();
                var response = client.DeleteAsync("BookAPI/" + obj.Id);
                response.Wait();
                var test = response.Result;
                var testBook = bookResponse.Result;
                if (test.IsSuccessStatusCode && testBook.IsSuccessStatusCode)
                {
                    var display = testBook.Content.ReadAsAsync<BookAPIDb>();
                    display.Wait();
                    var path = display.Result.Image;

                    bool isDone = DeleteImage(path);
                    if(isDone)
                    {
                        TempData["success"] = "Data is Deleted";
                         return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["error"] = "Image is not Deleted";
                        return RedirectToAction(nameof(Index));
                    }
                }
                TempData["error"] = "Data is not Deleted";
                return View();
            }
            catch
            {
                return View();
            }
        }

        private bool DeleteImage(string path)
        {
            try
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                var file = wwwRootPath + path;
                FileInfo Image = new FileInfo(file);
                if (Image.Exists)
                {
                    Image.Delete();
                    return (true);
                }
                return (false);
            }
            catch
            {
                return (false);
            }
        }
    }
}
