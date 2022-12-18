using Cumulative_Project_Najib_Osman.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cumulative_Project_Najib_Osman.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        //GET: /Student/List
        public ActionResult List(string SearchKey = null)
        {
            StudentDataController controller = new StudentDataController();
            IEnumerable<Student> Students = controller.ListStudents(SearchKey);
            return View(Students);
        }

        //GET: /Student/Show/{id} 
        public ActionResult Show(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student SelectedStudent = controller.FindStudent(id);

            return View(SelectedStudent);
        }

        //GET: /Student/DeleteConfirm/{id}
        public ActionResult DeleteConfirm(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student NewStudent = controller.FindStudent(id);

            return View(NewStudent);
        }  

        //POST: /Student/Delete/{id}
        public ActionResult Delete(int id)
        {
            StudentDataController controller = new StudentDataController();
            controller.DeleteStudent(id);
            return RedirectToAction("List");
        }

        //GET: /Student/New
        public ActionResult New()
        {
            return View();
        }

        //GET: /Student/Create
        [HttpPost]
        public ActionResult Create(string StudentFname, string StudentLname, DateTime StudentEnrolDate)
        {
            //Identify that this method is running
            //Identify the inputs provided from the form

            Student NewStudent = new Student();
            NewStudent.StudentFname = StudentFname;
            NewStudent.StudentLname = StudentLname;
            NewStudent.StudentEnrolDate = StudentEnrolDate;

            StudentDataController controller = new StudentDataController();
            controller.AddStudent(NewStudent);

            return RedirectToAction("List");
        }

        /// <summary>
        /// Routes to a dynamically generated "Student Update" Page. Gathers information from the database.
        /// </summary>
        /// <param name="id">Id of the Student</param>
        /// <returns>A dynamic "Update Student" webpage which provides the current infomration of the student and asks the user for new information as part of a form.</returns>
        /// <example>GET: /Student/Update/{id}</example>
        //GET: /Student/Update/{id}
        [HttpGet]
        public ActionResult Update(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student SelectedStudent = controller.FindStudent(id);

            return View(SelectedStudent);
        }

        /// <summary>
        ///  Receives a POST request containing information about an existing teacher in the system, with new values. Conveys this information to the API, and redirects to the "Student Show" page of our updated student.
        /// </summary>
        /// <param name="id">Id of the Teacher to update</param>
        /// <param name="StudentFname">The updated first name of the student</param>
        /// <param name="StudentLname">The updated last name of the student</param>
        /// <param name="StudentEnrolDate">the updated enrol date of the student</param>
        /// <returns>A dynamic webpage which provides the current information of the student.</returns>
        /// <example>POST: /Student/Update/12
        /// FORM DATA/ POST DATA/ REQUEST BODY
        /// {
        /// StudentFname:"Najib",
        /// StudentLname:"Osman",
        /// StudentEnrolDate:"2022-09-10"
        /// }
        /// </example>
        //POST: /Teacher/Update/{id}
        [HttpPost]
        public ActionResult Update(int id, string StudentFname, string StudentLname, DateTime StudentEnrolDate)
        {
            Student StudentInfo = new Student();
            StudentInfo.StudentFname = StudentFname;
            StudentInfo.StudentLname = StudentLname;
            StudentInfo.StudentEnrolDate = StudentEnrolDate;

            StudentDataController controller = new StudentDataController();
            controller.UpdateStudent(id, StudentInfo);

            return RedirectToAction("Show/" + id);
        }

    }
}