using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using APBD5.Models;


namespace APBD5.DAL
{
    public class DbService : IDbService
    {
        public IEnumerable<Student> GetStudents()
        {
            List<Student> students = new List<Student>();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s16710;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                com.CommandText = "SELECT * FROM STUDENT";

                con.Open();

                var reader = com.ExecuteReader();

                while (reader.Read())
                {
                    Student student = new Student();

                    student.FirstName = reader["FirstName"].ToString();
                    student.LastName = reader["LastName"].ToString();
                    student.IndexNumber = reader["IndexNumber"].ToString();

                    students.Add(student);
                }

                con.Close();
            }

            return students;
        }


        public IEnumerable<Enrollment> GetStudentEnrollment(string IdOfStudent)
        {
            List<Enrollment> enrollments = new List<Enrollment>();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s16710;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;

                com.CommandText = $"SELECT * FROM dbo.ENROLLMENT WHERE IdEnrollment = (SELECT IDENROLLMENT FROM dbo.STUDENT WHERE INDEXNUMBER = @studentId)";
               
                com.Parameters.AddWithValue("studentId", IdOfStudent);

                con.Open();

                var reader = com.ExecuteReader();

                while (reader.Read())
                {
                    
                    Console.WriteLine(reader);

                    Enrollment enrollment = new Enrollment();

                    enrollment.Semester = (int)reader["Semester"];
                    enrollment.IdEnrollment = (int)reader["IdEnrollment"];
                    enrollment.IdStudy = (int)reader["IdStudy"];
                    enrollment.StartDate = System.Convert.ToDateTime(reader["StartDate"].ToString());

                    enrollments.Add(enrollment);
                }

                con.Close();
            }

            return enrollments;
        }

        public Study GetStudy(string nameOfStudy)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s16710;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "SELECT * FROM STUDIES WHERE NAME = @studyName";
                com.Parameters.AddWithValue("studyName", nameOfStudy);

                con.Open();

                var reader = com.ExecuteReader();

                Study study = new Study();

                if (!reader.Read())
                {
                    return null;
                }

                study.IdStudy = (int)reader["IdStudy"];
                study.Name = reader["Name"].ToString();

                con.Close();

                return study;
            }
        }

        public Student GetStudent(string indexNumber)
        {
            Student student = new Student();

            try
            {
                using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s17082;Integrated Security=True"))
                using (var com = new SqlCommand())
                {
                    com.Connection = con;

                    com.CommandText = "SELECT * FROM STUDENT WHERE IndexNumber = @indexNumber";
                    com.Parameters.AddWithValue("indexNumber", indexNumber);

                    con.Open();

                    var reader = com.ExecuteReader();

                    if (!reader.Read())
                    {
                        return null;
                    }

                    student.IndexNumber = reader["IndexNumber"].ToString();
                    student.FirstName = reader["FirstName"].ToString();
                    student.LastName = reader["LastName"].ToString();
                    student.BirthDate = (DateTime)reader["BirthDate"];

                    con.Close();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return null;
            }

            return student;
        }

    }
}

