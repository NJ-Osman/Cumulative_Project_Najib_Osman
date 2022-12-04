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
    public class TeacherDataController : ApiController
    {
        private TeacherDbContext Schooldb = new TeacherDbContext();

        //This Controller Will access the Teachers table of our School database.
        /// <summary>
        /// Returns a list of Teachers in the system
        /// Returns A list of Teacher name, salary, and hire date if user clicks on the teacher name
        /// </summary>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>
        /// A list of Students (first names, last names, Teacher Salary)
        /// A list of Teacher name, salary and hiredate
        /// </returns>

        [HttpGet]
        [Route("api/TeacherData/ListTeachers/{SearchKey?}")]
        public IEnumerable<Teacher> ListTeachers(string SearchKey=null)
        {
            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat" +
                "(teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            List<Teacher> Teachers = new List<Teacher>{};

            while (ResultSet.Read())
            {
                int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);

                Teacher newTeacher = new Teacher();
                newTeacher.TeacherId = TeacherId;
                newTeacher.TeacherFname = TeacherFname;
                newTeacher.TeacherLname = TeacherLname;
                newTeacher.TeacherSalary = TeacherSalary;
                newTeacher.TeacherHireDate = TeacherHireDate;
                Teachers.Add(newTeacher);
            }

            Conn.Close();

            return Teachers;
        }
        /// <summary>
        /// show url link needs the teacher id to obtain all the information about the teachers
        /// </summary>
        /// <param name="id"></param>
        /// <example>GET /teacher/Show/3</example>
        /// <returns> A list of all the information about the teachers</returns>
        /// <returns>A list of firstname, lastname, salary, and hiredate</returns>
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher newTeacher = new Teacher();

            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Teachers where teacherid =" + id;

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                int teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                decimal TeacherSalary = Convert.ToDecimal(ResultSet["salary"]);
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                DateTime TeacherHireDate = Convert.ToDateTime(ResultSet["hiredate"]);

                newTeacher.TeacherId = teacherId;
                newTeacher.TeacherSalary = TeacherSalary;
                newTeacher.TeacherFname = TeacherFname;
                newTeacher.TeacherLname = TeacherLname;
                newTeacher.TeacherHireDate = TeacherHireDate;
            }


            return newTeacher;
        }

        /// <summary>
        /// The ability to delete a teacher from the teacher database
        /// </summary>
        /// <param name="id"></param>
        /// <example>GET:Teacher/DeleteConfirm </example>
        /// <example>POST : /api/TeacherData/DeleteTeacher/3</example>

        [HttpPost]
        public void DeleteTeacher(int id)
        {
            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Delete from Teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
            }

            /// <summary>
            /// The ability to add a teacher to the database
            /// Returns the new teacher name salary, and hiredate onto the database
            /// </summary>
            /// <param name="NewTeacher"></param>
            /// <example>GET: /Teacher/New</example>
        [HttpPost]
        public void AddTeacher([FromBody]Teacher NewTeacher)
        {
            MySqlConnection Conn = Schooldb.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "insert into Teachers (teacherfname, teacherlname, salary, hiredate) values (@TeacherFname,@TeacherLname,@TeacherSalary,@TeacherHireDate)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@TeacherSalary", NewTeacher.TeacherSalary);
            cmd.Parameters.AddWithValue("@TeacherHireDate", NewTeacher.TeacherHireDate);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }

        }
    }
