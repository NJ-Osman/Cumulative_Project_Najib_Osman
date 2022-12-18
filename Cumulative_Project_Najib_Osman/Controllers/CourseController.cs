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
            Course SelectedCourse = controller.FindCourse(id);
            return View(SelectedCourse);
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

        /// <summary>
        /// Routes to a dynamically generated "course Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Course</param>
        /// <returns>A dynamic "Update Course" webpage which provides the current infomration of the course and asks the user for new information as part of a form.</returns>
        /// <example>GET: /Course/Update/{id}</example>
        //GET: /Course/Update/{id}
        [HttpGet]
        public ActionResult Update(int id)
        {
            CoursesDataController controller = new CoursesDataController();
            Course SelectedCourse = controller.FindCourse(id);

            return View(SelectedCourse);
        }

        /// <summary>
        ///  Receives a POST request containing information about an existing course in the system, with new values. Conveys this information to the API, and redirects to the "Course Show" page of our updated course.
        /// </summary>
        /// <param name="id">Id of the Teacher to update</param>
        /// <param name="StartDate">The updated start date of the course</param>
        /// <param name="FinishDate">The updated finish date of the course</param>
        /// <param name="CourseName">the updated course name of the course</param>
        /// <returns>A dynamic webpage which provides the current information of the course.</returns>
        /// <example>POST: /Course/Update/12
        /// FORM DATA/ POST DATA/ REQUEST BODY
        /// {
        /// StartDate:"2022-08-31",
        /// FinishDate:"2022-12-06",
        /// CourseName:"Web Programming"
        /// }
        /// </example>
        //POST: /Teacher/Update/{id}
        [HttpPost]
        public ActionResult Update(int id, DateTime StartDate, DateTime FinishDate, string CourseName)
        {
            Course CourseInfo = new Course();
            CourseInfo.StartDate = StartDate;
            CourseInfo.FinishDate = FinishDate;
            CourseInfo.CourseName = CourseName;

            CoursesDataController controller = new CoursesDataController();
            controller.UpdateCourse(id, CourseInfo);

            return RedirectToAction("Show/" + id);
        }

    }
}