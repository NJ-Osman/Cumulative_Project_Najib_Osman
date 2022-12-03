using Cumulative_Project_Najib_Osman.Models;
using System;
using System.Collections.Generic;
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

    }
}