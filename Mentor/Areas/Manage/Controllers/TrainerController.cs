using Mentor.DAL;
using Mentor.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Mentor.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class TrainerController : Controller
    {
        private readonly MentorAppDbContext mentorAppDbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TrainerController(MentorAppDbContext _mentorAppDbContext, IWebHostEnvironment webHostEnvironment)
        {
            mentorAppDbContext = _mentorAppDbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var trainers = mentorAppDbContext.Trainers.ToList();
            return View(trainers);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Trainer trainer)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (trainer.Photo == null)
            {
                ModelState.AddModelError(nameof(trainer.Photo), "You must add a photo for trainer");
                return View();
            }

            if (!trainer.Photo.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError(nameof(trainer.Photo), "File must be an image");
                return View();
            }

            string fileExtension = Path.GetExtension(trainer.Photo.FileName);
            string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team", uniqueFileName);

            using (FileStream stream = new FileStream(uploadPath, FileMode.Create))
            {
                trainer.Photo.CopyTo(stream);
            }

            trainer.TrainerImage = uniqueFileName;

            mentorAppDbContext.Trainers.Add(trainer);
            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index", "Trainer");
        }


        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var trainerToEdit = mentorAppDbContext.Trainers.FirstOrDefault(t => t.Id == id.Value);
            if (trainerToEdit == null)
            {
                return NotFound();
            }

            return View(trainerToEdit);
        }

        [HttpPost]
        public IActionResult Edit(Trainer trainer)
        {
            if (!ModelState.IsValid)
            {
                return View(trainer);
            }

            if (trainer == null)
            {
                return NotFound();
            }

            var existingTrainer = mentorAppDbContext.Trainers.FirstOrDefault(t => t.Id == trainer.Id);
            if (existingTrainer == null)
            {
                return NotFound();
            }

            string previousImage = existingTrainer.TrainerImage;

            if (trainer.Photo != null)
            {
                string extension = Path.GetExtension(trainer.Photo.FileName);
                string imageName = $"{Guid.NewGuid()}{extension}";
                string savePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team", imageName);

                using (var stream = new FileStream(savePath, FileMode.Create))
                {
                    trainer.Photo.CopyTo(stream);
                }

                existingTrainer.TrainerImage = imageName;

                string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team", previousImage);

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }

            existingTrainer.FullName = trainer.FullName;
            existingTrainer.Bio = trainer.Bio;
            existingTrainer.Expertise = trainer.Expertise;

            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index", "Trainer");
        }


        public IActionResult Delete(int id)
        {
            var trainer = mentorAppDbContext.Trainers.FirstOrDefault(t => t.Id == id);
            if (trainer == null)
            {
                return NotFound();
            }

            string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team", trainer.TrainerImage);

            if (!string.IsNullOrEmpty(imagePath) && System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            mentorAppDbContext.Trainers.Remove(trainer);
            mentorAppDbContext.SaveChanges();

            return RedirectToAction("Index", "Trainer");
        }

        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var eTrainer = mentorAppDbContext.Trainers.FirstOrDefault(t=>t.Id == id);
            if (eTrainer is null)
                return NotFound();

            return View(eTrainer);
        }
    }
}
