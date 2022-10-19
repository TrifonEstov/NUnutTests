using MyFirstApp;
using Newtonsoft.Json.Linq;
using Project1;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace SchoolTestProject
{
    [TestFixture]
    public class StudentsTests : BaseTests
    {
        [Test]
        [Description("Validate the students don't have IDs")]
        public void ValidateNoIDs()
        {
            foreach (var studentJObject in Array)
            {
                JObject jObjectItem = studentJObject.ToObject<JObject>();
                Assert.False(jObjectItem.ContainsKey("Id"), "Each student has such a property!");
            }
        }

        [Test]
        [Description("Validate each students has unique ID property")]
        public void ValidateIdPresence()
        {
            List<JObject> studentsWithOutIDs = Array.ToObject<List<JObject>>();
            List<JObject> studentsListWithIds = AddStudentId(studentsWithOutIDs);


            List<JToken> idList = new List<JToken>();

            foreach (var studentJObject in studentsListWithIds)
            {
                JObject jObjectItem = studentJObject.ToObject<JObject>();
                Assert.True(jObjectItem.ContainsKey("Id"), "Missing property!");
                idList.Add(studentJObject.GetValue("Id"));
            }

            bool isUnique = idList.Distinct().Count() == idList.Count();
            Assert.True(isUnique, "Students'IDs are not unique");
        }

        [Test]
        [Description("Validate presence of Marks property")]
        public void ValidateMarksProperty()
        {
            List<JObject> studentsJObjects = Array.ToObject<List<JObject>>();

            foreach (var studentJObject in studentsJObjects)
            {
                JObject jObjectItem = studentJObject.ToObject<JObject>();
                Assert.True(jObjectItem.ContainsKey("Marks"), "Missing property!");
            }
        }

        [Test]
        [Description("Validate object parsing")]
        public void ValidateObjectParsing()
        {
            List<JObject> studentsJObjects = Array.ToObject<List<JObject>>();

            List<string> expectedNamesList = new List<string>();
            List<int> expectedAgesList = new List<int>();
            List<Dictionary<string, int>> expectedMarksList = new List<Dictionary<string, int>>();

            List<string> actualNamesList = new List<string>();
            List<int> actualAgesList = new List<int>();
            List<Dictionary<string, int>> actualMarksList = new List<Dictionary<string, int>>();

            foreach (var studentJObject in studentsJObjects)
            {
                expectedNamesList.Add(studentJObject["Name"].ToObject<string>());
                expectedAgesList.Add(studentJObject["Age"].ToObject<int>());
                expectedMarksList.Add(studentJObject["Marks"].ToObject<Dictionary<string, int>>());
            }

            foreach (var student in School.StudentsList)
            {
                actualNamesList.Add(student.Name);
                actualAgesList.Add(student.Age);
                actualMarksList.Add(student.Marks);
            }

            CollectionAssert.AreEqual(expectedNamesList, actualNamesList);
            CollectionAssert.AreEqual(expectedAgesList, actualAgesList);
            CollectionAssert.AreEqual(expectedMarksList, actualMarksList);
        }

        [Test]
        [Description("Validate graduated students' count")]
        public void ValidateGraduatedStudentsCount()
        {
            int initialStudentsCount = School.StudentsList.Count;
            List<JObject> studentsJObjectList = Array.ToObject<List<JObject>>();
            List<JObject> studentsJObjectsWithIDs = AddStudentId(studentsJObjectList);

            foreach (var student in studentsJObjectsWithIDs)
            {
                Student currentStudentFromTheList = student.ToObject<Student>();
                currentStudentFromTheList.Age = student.GetValue("Age").ToObject<int>();
                currentStudentFromTheList.Id = student.GetValue("Id").ToObject<int>();
                if (currentStudentFromTheList.Age >= 18)
                {
                    School.RemoveStudent(currentStudentFromTheList.Id);
                }
            }
            Assert.AreNotEqual(School.StudentsList.Count, initialStudentsCount);
        }
    }
}