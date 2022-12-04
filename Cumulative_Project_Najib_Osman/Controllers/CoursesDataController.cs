using Cumulative_Project_Najib_Osman.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cumulative_Project_Najib_Osman.Controllers
{
    public class CoursesDataController : ApiController
    {
        private TeacherDbContext Schooldb = new TeacherDbContext();

        //This Controller Will access the Courses table of our School database.
        /// <summary>
        /// Returns a list of Courses in the system
        /// Returns course start date and finish date if user clicks on the course name
        /// </summary>
        /// <example>GET api/CourseData/ListCourses</example>
        /// <returns>
        /// A list of Courses (Course Names)
        /// List of course name, course startdate and finish date
        /// </returns>

        [HttpGet]
        [Route("api/CoursesData/ListCourses/{SearchKey?}")]
        public IEnumerable<Course> ListCourses(string SearchKey = null)
        {
            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Classes where lower(classid) like lower(@key) or lower(classname) like lower(@key) or lower(concat" +
                "(classid, ' ', classname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Course> Courses = new List<Course> { };

            while (ResultSet.Read())
            {
                int CourseId = Convert.ToInt32(ResultSet["classid"]);
                string CourseName = ResultSet["classname"].ToString();
                DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                Course newCourse = new Course();
                newCourse.CourseId = CourseId;
                newCourse.CourseName = CourseName;
                newCourse.StartDate = StartDate;
                newCourse.FinishDate = FinishDate;
                Courses.Add(newCourse);
            }

            Conn.Close();

            return Courses;
        }

        /// <summary>
        /// show url link needs the Course id to obtain all the information about the Courses
        /// </summary>
        /// <param name="id"></param>
        /// <example>GET /Course/Show/3</example>
        /// <returns> A list of all the information about the Course</returns>
        /// <returns>A list of startdate, finishdate, and coursename</returns>
        [HttpGet]
        public Course FindCourse(int id)
        {
            Course newCourse = new Course();

            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Classes where classid =" + id;

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int CourseId = Convert.ToInt32(ResultSet["classid"]);
                string CourseName = ResultSet["classname"].ToString();
                DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                newCourse.CourseId = CourseId;
                newCourse.CourseName = CourseName;
                newCourse.StartDate = StartDate;
                newCourse.FinishDate = FinishDate;
            }


            return newCourse;

        }

        /// <summary>
        /// The ability to delete a Course from the Course database
        /// </summary>
        /// <param name="id"></param>
        /// <example>GET:Course/DeleteConfirm </example>
        /// <example>POST : /api/CourseData/DeleteCourse/3</example>
        public void DeleteCourse(int id)
        {
            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from Classes where classid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();

        }

        /// <summary>
        /// The ability to add a Course to the database
        /// Returns the new Course startdate, finishdate, and coursename
        /// </summary>
        /// <param name="newCourse"></param>
        /// <example>GET: /Course/New</example>
        [HttpPost]
        public void AddCourse([FromBody] Course newCourse)
        {
            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into Classes (startdate, finishdate, classname) values (@StartDate,@FinishDate,@CourseName)";
            cmd.Parameters.AddWithValue("@StartDate", newCourse.StartDate);
            cmd.Parameters.AddWithValue("@FinishDate", newCourse.FinishDate);
            cmd.Parameters.AddWithValue("@CourseName", newCourse.CourseName);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

    }
}
