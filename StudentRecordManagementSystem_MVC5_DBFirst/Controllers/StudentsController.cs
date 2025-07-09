using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using StudentRecordManagementSystem_MVC5_DBFirst.Models;

namespace StudentRecordManagementSystem_MVC5_DBFirst.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentDBEntities db = new StudentDBEntities();

        // GET: Students
        public ActionResult Index(string search)
        {
            var students = db.Students.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                students = students.Where(s =>
                    s.FullName.Contains(search) ||
                    s.Email.Contains(search) ||
                    s.Phone.Contains(search) ||
                    s.Gender.Contains(search) ||
                    s.Address.Contains(search)
                );
            }

            return View(students.ToList());
        }


        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student, HttpPostedFileBase PhotoFile)
        {
            if (ModelState.IsValid)
            {
                // Save profile photo
                student.Photo = SavePhoto(PhotoFile, student.Gender);

                db.Students.Add(student);
                db.SaveChanges();

                TempData["Success"] = "✅ Student added successfully!";
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int id)
        {
            var student = db.Students.Find(id);
            if (student == null)
            {
                TempData["Error"] = $"⚠️ Student with ID {id} not found.";
                return RedirectToAction("Index");
            }

            return View(student);
        }


        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student, HttpPostedFileBase PhotoFile)
        {
            if (ModelState.IsValid)
            {
                var existingStudent = db.Students.Find(student.StudentId);
                if (existingStudent == null)
                {
                    TempData["Error"] = "Student not found.";
                    return RedirectToAction("Index");
                }

                existingStudent.FullName = student.FullName;
                existingStudent.Email = student.Email;
                existingStudent.Phone = student.Phone;
                existingStudent.Gender = student.Gender;
                existingStudent.EnrollmentDate = student.EnrollmentDate;
                existingStudent.Address = student.Address;

                if (PhotoFile != null && PhotoFile.ContentLength > 0)
                {
                    existingStudent.Photo = SavePhoto(PhotoFile, student.Gender);
                }

                db.SaveChanges();

                TempData["Success"] = "✅ Student updated successfully!";
                return RedirectToAction("Index");
            }

            return View(student);
        }




        // GET: Students/Delete/5
        public ActionResult Delete(int id)
        {
            var student = db.Students.Find(id);
            if (student == null)
                return HttpNotFound();

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var student = db.Students.Find(id);
            if (student == null)
            {
                TempData["Error"] = "⚠️ Student not found.";
                return RedirectToAction("Index");
            }

            db.Students.Remove(student);
            db.SaveChanges();

            TempData["Success"] = "✅ Student deleted successfully!";
            return RedirectToAction("Index");
        }

        // Helper method to save photo
        private string SavePhoto(HttpPostedFileBase PhotoFile, string gender)
        {
            if (PhotoFile != null && PhotoFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(PhotoFile.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/images/avatars/"), fileName);
                PhotoFile.SaveAs(path);
                return "/Content/images/avatars/" + fileName;
            }

            // Default avatar
            return gender == "Female"
                ? "/Content/images/avatars/default_female.png"
                : "/Content/images/avatars/default_male.png";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
