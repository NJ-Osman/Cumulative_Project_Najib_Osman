using Cumulative_Project_Najib_Osman.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace Cumulative_Project_Najib_Osman.Controllers
{
    public class StudentDataController : ApiController
    {
        private TeacherDbContext Schooldb = new TeacherDbContext();

        //This Controller Will access the Students table of our School database.
        /// <summary>
        /// Returns a list of Students in the system
        /// Returns a list of Students name and their enrol date if the user clicks on the student name
        /// </summary>
        /// <example>GET api/StudentData/ListStudents</example>
        /// <returns>
        /// A list of Students (first names, last names, student numbers)
        /// A list of the student name, and student enrol date
        /// </returns>

        [HttpGet]
        [Route("api/StudentData/ListStudents/{SearchKey?}")]
        public IEnumerable<Student> ListStudents(string SearchKey = null)
        {
            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Students where lower(studentfname) like lower(@key) or lower(studentlname) like lower(@key) or lower(concat" +
                 "(studentfname, ' ', studentlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Student> Students = new List<Student>{};

            while (ResultSet.Read())
            {
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                DateTime StudentEnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                Student NewStudent = new Student();
                NewStudent.StudentId = StudentId;
                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentEnrolDate = StudentEnrolDate;

                Students.Add(NewStudent);
            }

            Conn.Close();

            return Students;

            /// <summary>
            /// show url link needs the Student id to obtain all the information about the Students
            /// </summary>
            /// <param name="id"></param>
            /// <example>GET /Student/Show/3</example>
            /// <returns> A list of all the information about the Stundet</returns>
            /// <returns>A list of firstname, lastname, and enroldate</returns>
        }

        [HttpGet]
        public Student FindStudent(int id)
        {
            Student newStudent = new Student();

            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Students where studentid =" + id;

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                DateTime StudentEnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);

                newStudent.StudentId = StudentId;
                newStudent.StudentFname = StudentFname;
                newStudent.StudentLname = StudentLname;
                newStudent.StudentEnrolDate = StudentEnrolDate;
            }


            return newStudent;
        }

        /// <summary>
        /// The ability to delete a Student from the Student database
        /// </summary>
        /// <param name="id"></param>
        /// <example>GET:Student/DeleteConfirm </example>
        /// <example>POST : /api/StudentData/DeleteStudent/3</example>

        [HttpPost]
        public void DeleteStudent(int id)
        {
            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from Students where studentid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// The ability to add a Student to the database
        /// Returns the new teacher firstname, lastname, and their enroldate onto the database
        /// </summary>
        /// <param name="NewStudent"></param>
        /// <example>GET: /Student/New</example>
        [HttpPost]
        public void AddStudent([FromBody] Student NewStudent)
        {
            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into Students (studentfname, studentlname, enroldate) values (@StudentFname,@StudentLname,@StudentEnrolDate)";
            cmd.Parameters.AddWithValue("@StudentFname", NewStudent.StudentFname);
            cmd.Parameters.AddWithValue("@StudentLname", NewStudent.StudentLname);
            cmd.Parameters.AddWithValue("@StudentEnrolDate", NewStudent.StudentEnrolDate);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        /// <summary>
        /// Updates an Student in the system
        /// <param name="StudentId">The id of the student in the system</param>
        /// <param name="UpdatedStudent">post content, student body including name and enrol date</param>
        /// </summary>
        /// <example>
        /// api/studentdata/updatestudent/509
        ///  POST CONTENT/ FORM BODY / REQUEST BODY
        ///  {studentfirstname:"", studentlastname: "", studentenroldate: ""}
        /// </example>

        public void UpdateStudent(int id, [FromBody] Student StudentInfo)
        {
            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "update Student set studentfname=@StudentFname, studentlname=@StudentLname, enroldate=@StudentEnrolDate where studentid=@StudentId";
            cmd.Parameters.AddWithValue("@StudentFname", StudentInfo.StudentFname);
            cmd.Parameters.AddWithValue("@StudentLname", StudentInfo.StudentLname);
            cmd.Parameters.AddWithValue("@StudentEnrolDate", StudentInfo.StudentEnrolDate);
            cmd.Parameters.AddWithValue("@StudentId", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

    }
}
