using Grand_Hall.Data;
using Grand_Hall.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class HallController : Controller
{
    private readonly ApplicationDbContext _context;

    public HallController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Index action to list all halls
    public async Task<IActionResult> Index()
    {
        var halls = await _context.Halls.ToListAsync();
        if (halls == null || !halls.Any())
        {
            ViewData["Message"] = "No halls found.";
        }
        return View(halls);
    }

    // GET: AddHall action to display the form
    public IActionResult AddHall()
    {
        ViewBag.Halls = _context.Halls.OrderBy(x => x.HallsName).ToList();
        return View();
    }

    // POST: AddHall action to handle form submission
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddHall(Hall model)
    {
        UploadImage(model);

        if (ModelState.IsValid)
        {
            _context.Halls.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        ViewBag.Halls = _context.Halls.OrderBy(x => x.HallsName).ToList();
        return View(model);
    }

    // GET: EditHall to display the form with the hall's details
    public IActionResult EditHall(int? id)
    {
        if (id == null) return NotFound();

        var hall = _context.Halls.Find(id);
        if (hall == null) return NotFound();

        ViewBag.Halls = _context.Halls.OrderBy(x => x.HallsName).ToList();
        return View("AddHall", hall);
    }

    // POST: EditHall to update the hall
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditHall(int id, Hall model)
    {
        if (id != model.HallsID)
        {
            return NotFound(); // If the IDs do not match, return NotFound
        }

        if (ModelState.IsValid)
        {
            try
            {
                UploadImage(model);  // Upload the image (if any)

                _context.Halls.Update(model);  // Update the hall in the database
                _context.SaveChanges();  // Commit the changes
                return RedirectToAction("Index");  // Redirect to the list of halls
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Halls.Any(h => h.HallsID == id))
                {
                    return NotFound();  // Hall not found during update
                }
                else
                {
                    throw;  // Rethrow the exception if it persists
                }
            }
        }

        ViewBag.Halls = _context.Halls.OrderBy(x => x.HallsName).ToList();
        return View("AddHall", model);  // If invalid, redisplay the form with errors
    }

    // DELETE: Delete a hall
    public IActionResult DeleteHall(int? id)
    {
        var hall = _context.Halls.Find(id);
        if (hall != null)
        {
            _context.Halls.Remove(hall);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    // GET: ShowHall action to display details of a specific hall
    public async Task<IActionResult> ShowHall(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var hall = await _context.Halls.FirstOrDefaultAsync(h => h.HallsID == id);
        if (hall == null)
        {
            return NotFound();
        }

        return View(hall);
    }

    private void UploadImage(Hall model)
    {
        var file = HttpContext.Request.Form.Files;
        if (file.Count > 0)
        {
            string imageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
            var fileStream = new FileStream(Path.Combine("wwwroot", "Images", imageName), FileMode.Create);
            file[0].CopyTo(fileStream);
            model.ImagePath = imageName;
        }
        else
        {
            if (string.IsNullOrEmpty(model.ImagePath))
            {
                model.ImagePath = "Default.jpeg";
            }
        }
    }
}
