using Cumulative_Project_Najib_Osman.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Cumulative_Project_Najib_Osman.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }

        // GET: Course/List
        public ActionResult List(string SearchKey = null)
        {
            CoursesDataController controller = new CoursesDataController();
            IEnumerable<Course> Courses = controller.ListCourses(SearchKey);
            return View(Courses);
        }

        // GET: Course/Show
        public ActionResult Show(int id)
        {
            CoursesDataController controller = new CoursesDataController();
            Course NewCourse = controller.FindCourse(id);
            return View(NewCourse);
        }

        //GET: /Course/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            CoursesDataController controller = new CoursesDataController();
            Course newCourse = controller.FindCourse(id);

            return View(newCourse);
        }

        //POST: /Course/Delete/{id}
        public ActionResult Delete(int id)
        {
            CoursesDataController controller = new CoursesDataController();
            controller.DeleteCourse(id);
            return RedirectToAction("List");

        }

        //GET: /Course/New
        public ActionResult New()
        {
            return View();
        }

        //POST: /Course/Create
        public ActionResult Create(DateTime StartDate, DateTime FinishDate, string CourseName)
        {
            //Identify that this method is running
            //Identify the inputs provided from the form 
            Debug.WriteLine("I have accessed the Create Method!");
            Debug.WriteLine(StartDate);
            Debug.WriteLine(FinishDate);
            Debug.WriteLine(CourseName);

            Course newCourse = new Course();
            newCourse.CourseName = CourseName;
            newCourse.StartDate = StartDate;
            newCourse.FinishDate = FinishDate;

            CoursesDataController controller = new CoursesDataController();
            controller.AddCourse(newCourse);

            return RedirectToAction("List");
        }

    }
}