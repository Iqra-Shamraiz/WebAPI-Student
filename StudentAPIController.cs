using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiCore.Models;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly CodeFirstAppContext context;

        public StudentAPIController(CodeFirstAppContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            // Retrieve all students from the database
            var students = await context.Students.ToListAsync();

            // Return the students or a 404 if no students were found
            if (students == null || !students.Any())
            {
                return NotFound(); // Returns a 404 Not Found
            }
            return Ok(students); // Returns a 200 OK with the list of students
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            // Retrieve the student with the specified ID from the database
            var student = await context.Students.FindAsync(id);

            // Check if the student was found
            if (student == null)
            {
                return NotFound(); // Returns a 404 Not Found if the student does not exist
            }

            return Ok(student); // Returns a 200 OK with the student details
        }
        // POST: api/StudentAPI
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            // Check if the provided student data is valid
            if (student == null)
            {
                return BadRequest("Student data is null."); // Returns a 400 Bad Request if the student data is null
            }

            // Add the new student to the database
            context.Students.Add(student);
            await context.SaveChangesAsync();

            // Return a 201 Created response along with the created student's details
            return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student);
        }
        // PUT: api/StudentAPI/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(int id, Student student)
        {
            // Check if the provided student data is valid
            if (id != student.Id)
            {
                return BadRequest("Student ID mismatch."); // Returns a 400 Bad Request if the ID in the URL doesn't match the student ID
            }

            // Check if the student exists
            var existingStudent = await context.Students.FindAsync(id);
            if (existingStudent == null)
            {
                return NotFound(); // Returns a 404 Not Found if the student does not exist
            }

            // Update the student details
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.DateOfBirth = student.DateOfBirth;
            existingStudent.Email = student.Email;
            existingStudent.PhoneNumber = student.PhoneNumber;
            existingStudent.Address = student.Address;
            existingStudent.Grade = student.Grade;

            // Save changes to the database
            await context.SaveChangesAsync();

            return NoContent(); // Returns a 204 No Content response indicating the update was successful
        }
        // DELETE: api/StudentAPI/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            // Find the student by ID
            var student = await context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(); // Returns a 404 Not Found if the student does not exist
            }

            // Remove the student from the context
            context.Students.Remove(student);

            // Save changes to the database
            await context.SaveChangesAsync();

            return NoContent(); // Returns a 204 No Content response indicating the deletion was successful
        }

    }
}
