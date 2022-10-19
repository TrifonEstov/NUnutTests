using MyFirstApp;
using Newtonsoft.Json.Linq;
using Project1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SchoolTestProject
{
    public abstract class BaseTests
    {
        protected School School { get; set; }

        protected JArray Array { get; set; }

        [OneTimeSetUp]
        public void SetupScholl()
        {
            School = new School("The School", "Sofia, 1000");
            Array = JsonDataFileReader.GetJArray("Students.json");

            List<Student> students = Array.ToObject<List<Student>>();
            School.AddStudents(students);
        }

        protected static List<JObject> AddStudentId(List<JObject> students)
        {
            List<JObject> studentsListWithIds = new List<JObject>();
            foreach (var studentObject in students)
            {
                Random random = new Random();
                int randomId = random.Next();
                studentObject.Add("Id", randomId);
                studentsListWithIds.Add(studentObject);
            }
            return studentsListWithIds;
        }
    }
}
