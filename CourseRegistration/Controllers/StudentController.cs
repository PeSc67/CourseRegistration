using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CourseRegistration.Data;
using CourseRegistration.Models;
//using ClosedXML.Excel;
//using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using System.Text;
using System.IO;
using CourseRegistration.Services;
using System.Configuration;
using CourseRegistration.ViewModels;



namespace CourseRegistration.Controllers
{
    public class StudentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvirement;
        private readonly IStudentService _studentService;

        public StudentController(IConfiguration configuration, IWebHostEnvironment webHostEnvirement, IStudentService studentService)
        {
            _configuration = configuration;
            _webHostEnvirement = webHostEnvirement;
            _studentService = studentService;
        }



        public IActionResult Index(string orderBy = "Id")  // orderby = "FirstName", "LastName" eller "City"
        {
            var students = _studentService.GetStudents().AsQueryable();

            Func<Student, dynamic> orderByFn = orderBy switch
            {
                "FirstName" => (Student student) => student.FirstName,
                "LastName" => (Student student) => student.LastName,
                "City" => (Student student) => student.City,
                _ => (Student student) => student.Id
            };

            var orderedStudents = students.OrderBy(orderByFn).ToList();

            StudentIndexVM vm = new StudentIndexVM()
            {
                Students = orderedStudents,
                OrderBy = orderBy
            };

            return View(vm);
        }




        public IActionResult Details(int? id)
        {
            var objList = _studentService.GetStudents();
            var obj = objList.FirstOrDefault(m => m.Id == id);

            return View(obj);
        }



        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student obj)
        {
            if (ModelState.IsValid)
            {
                _studentService.SaveStudent(obj);
                return View("ThankYou");
            }
            return View(obj);
        }





        public IActionResult Edit(int id)
        {
            var student = _studentService.GetStudentById(id);

            return View(student);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student obj)
        {
            if (ModelState.IsValid)
            {
                _studentService.EditStudent(obj);

                return RedirectToAction(nameof(Index));
            }

            return View(obj);
        }


        public IActionResult Delete(int id)
        {
            Student obj = _studentService.GetStudentById(id);

            return View(obj);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _studentService.DeleteStudent(id);

            return RedirectToAction(nameof(Index));
        }


        private bool StudentExists(int id)
        {
            return _studentService.StudentExist(id);
        }




        public ActionResult SendFileToEmailAddress(string orderBy, bool saveFile, string fileFormat, string destinationEmail)
        {
            StringBuilder csvData = new StringBuilder();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            string query = "SELECT * FROM Students";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                // Write column headers
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    csvData.Append(reader.GetName(i) + ",");
                }
                csvData.AppendLine();

                // Write rows
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        csvData.Append(reader[i].ToString() + ",");
                    }
                    csvData.AppendLine();
                }
            }

            string uploadsFolder = Path.Combine(_webHostEnvirement.WebRootPath, "ExportedStudentLists");

            // Ensure the directory exists
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }


            string todaysDateTime = (DateTime.Now).ToString();
            todaysDateTime = todaysDateTime.Replace(":", "-").Replace(" ", "_");

            string fileName = string.Empty;

            if (fileFormat == "CSV")
            {
                fileName = todaysDateTime + ".csv";
                string filePath = Path.Combine(uploadsFolder, fileName);
                System.IO.File.WriteAllText(filePath, csvData.ToString());


                if (!saveFile)  // Delete file from folder
                {
                    DeleteFileFromDisk(fileName);
                }
            }

            FileSentWithEmailVM vm = new FileSentWithEmailVM()
            {
                FileFormat = fileFormat,
                FileName = fileName,
                FileSavedInSystem = saveFile,
                DestinationEmail = destinationEmail,
                OrderBy = orderBy,
            };

            return View("FileSentWithEmail", vm);
        }


        //private ActionResult SendFileToEmailAddress(string orderBy)
        //{
        //    StringBuilder csvData = new StringBuilder();
        //    var connectionString = _configuration.GetConnectionString("DefaultConnection");
        //    string query = "SELECT * FROM Students";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = new SqlCommand(query, connection);
        //        connection.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        // Write column headers
        //        for (int i = 0; i < reader.FieldCount; i++)
        //        {
        //            csvData.Append(reader.GetName(i) + ",");
        //        }
        //        csvData.AppendLine();

        //        // Write rows
        //        while (reader.Read())
        //        {
        //            for (int i = 0; i < reader.FieldCount; i++)
        //            {
        //                csvData.Append(reader[i].ToString() + ",");
        //            }
        //            csvData.AppendLine();
        //        }
        //    }

        //    string uploadsFolder = Path.Combine(_webHostEnvirement.WebRootPath, "ExportedStudentLists");

        //    // Ensure the directory exists
        //    if (!Directory.Exists(uploadsFolder))
        //    {
        //        Directory.CreateDirectory(uploadsFolder);
        //    }

        //    string todaysDateTime = (DateTime.Now).ToString();
        //    todaysDateTime = todaysDateTime.Replace(":", "-").Replace(" ", "_");

        //    string fileName = todaysDateTime + ".csv";

        //    string filePath = Path.Combine(uploadsFolder, fileName);

        //    System.IO.File.WriteAllText(filePath, csvData.ToString());

        //    return View("FileSaved");
        //}


        private void DeleteFileFromDisk(string fileUrl)
        {
            string fileNameFullPath = Path.Combine(_webHostEnvirement.WebRootPath, "ExportedStudentLists", fileUrl);

            if (fileNameFullPath != null || fileNameFullPath != string.Empty)
            {
                if ((System.IO.File.Exists(fileNameFullPath)))
                {
                    System.IO.File.Delete(fileNameFullPath);
                }
            }
        }


    }
}

