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
        private TeacherDbContext SchoolDb = new TeacherDbContext();

        [HttpGet]
        [Route("api/CoursesData/ListCourses/{SearchKey?}")]
        public IEnumerable<Course> ListCourses(string SearchKey = null)
        {
            MySqlConnection Conn = SchoolDb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Classes where lower(classid) like lower(@key) or lower(classname) like lower(@key) or lower(concat" +
                "(classid, ' ', classname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Course> Courses = new List<Course>{};

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

        [HttpGet]
        public Course FindCourse(int id)
        {
            Course newCourse = new Course();

            MySqlConnection Conn = SchoolDb.AccessDatabase();

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

    }
}
