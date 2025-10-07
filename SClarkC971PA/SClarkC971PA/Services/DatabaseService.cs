//using CoreAnimation;
using SClarkC971PA.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SClarkC971PA.Services
{
    internal class DatabaseService
    {
        private static SQLiteAsyncConnection _db;
        static async Task Init()
        {
            if (_db != null)
            {
                return;
            }
            var databasePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "WGUTerms.db");

            _db = new SQLiteAsyncConnection(databasePath);

            await _db.CreateTableAsync<Term>();
            await _db.CreateTableAsync<Course>();
            await _db.CreateTableAsync<Instructor>();
            await _db.CreateTableAsync<Assessment>();
            await _db.CreateTableAsync<Note>();
            await _db.CreateTableAsync<User>();
        }
        #region User Methods
        //Get list of users
        public static async Task<IEnumerable<User>> GetUsers()
        {
            await Init();
            var users = await _db.Table<User>().ToListAsync();
            return users;
        }
        #endregion

        #region Term Methods
        //Add term
        public static async Task AddTerm(string termTitle, DateTime termStartDate, DateTime termEndDate)
        {
            await Init();
            var term = new Term()
            {
                TermTitle = termTitle,
                TermStartDate = termStartDate,
                TermEndDate = termEndDate

            };
            await _db.InsertAsync(term);
            var id = term.TermId;
        }

        //Remove term
        public static async Task RemoveTerm(int id)
        {
            await Init();
            await _db.DeleteAsync<Term>(id);
        }
        //Get terms
        public static async Task<IEnumerable<Term>> GetTerms(int userId)
        {
            await Init();
            var terms = await _db.Table<Term>().Where(i => i.AssociatedUserId == userId).ToListAsync();
            return terms;
        }
        public static async Task<Term> LookupTerm(int termId)
        {
            await Init();
            var lookedupTerm =  await _db.Table<Term>()
                .Where(i => i.TermId == termId)
                .FirstOrDefaultAsync();
            return lookedupTerm;
        }
        //Update term
        public static async Task UpdateTerm(int termId, string termTitle, DateTime termStartDate, DateTime termEndDate)
        {
            await Init();
            var termQuery = await _db.Table<Term>()
                .Where(i => i.TermId == termId)
                .FirstOrDefaultAsync();
            if (termQuery != null)
            {
                termQuery.TermTitle = termTitle;
                termQuery.TermStartDate = termStartDate;
                termQuery.TermEndDate = termEndDate;

                await _db.UpdateAsync(termQuery);
            }
        }

        #endregion

        #region Course Methods
        //Add course
        public static async Task AddCourse(Course course)
        {
            await Init();
            await _db.InsertAsync(course);
        }
        //Remove course
        public static async Task RemoveCourse(int courseId)
        {
            await Init();
            await _db.DeleteAsync<Course>(courseId);
        }
        //Get courses associated with a specific term
        public static async Task<IEnumerable<Course>> GetCourses(int associatedTermId)
        {
            await Init();
            var courses = await _db.Table<Course>().Where(i => i.AssociatedTermId == associatedTermId).ToListAsync();
            return courses;
        }
        //Get courses w/o regard to term
        public static async Task<IEnumerable<Course>> GetCourses()
        {
            await Init();
            var courses = await _db.Table<Course>().ToListAsync();
            return courses;
        }
        //Update course
        public static async Task UpdateCourse(Course course)
        {
            await Init();
            var courseQuery = await _db.Table<Course>()
                .Where(i => i.CourseId == course.CourseId)
                .FirstOrDefaultAsync();
            if (courseQuery != null)
            {
                courseQuery.AssociatedTermId = course.AssociatedTermId;
                courseQuery.CourseId = course.CourseId;
                courseQuery.CourseName = course.CourseName;
                courseQuery.CourseStartDate = course.CourseStartDate;
                courseQuery.CourseEndDate = course.CourseEndDate;
                courseQuery.CourseStatus = course.CourseStatus;
                courseQuery.CourseNotify = course.CourseNotify;

                await _db.UpdateAsync(courseQuery);
            }
        }
        #endregion

        #region Instructor Methods
        //Add instructor
        public static async Task AddInstructor(Instructor instructor)
        {
            await Init();
            await _db.InsertAsync(instructor);
            var id = instructor.InstructorId;
        }
        //Remove instructor
        public static async Task RemoveInstructor(int instructorId)
        {
            await Init();
            await _db.DeleteAsync<Instructor>(instructorId);
        }
        //Get instructor associated with a specific course
        public static async Task<Instructor> LookupInstructor(int courseId)
        {
            await Init();
            var lookedupInstructor = await _db.Table<Instructor>()
                .Where(i => i.AssociatedCourseId == courseId)
                .FirstOrDefaultAsync();
            return lookedupInstructor;
        }
        //Update instructor
        public static async Task UpdateInstructor(Instructor instructor)
        {
            await Init();
            var instructorQuery = await _db.Table<Instructor>()
                .Where(i => i.InstructorId == instructor.InstructorId)
                .FirstOrDefaultAsync();
            if (instructorQuery != null)
            {
                instructorQuery.AssociatedCourseId = instructor.AssociatedCourseId;
                instructorQuery.InstructorId = instructor.InstructorId;
                instructorQuery.InstructorName = instructor.InstructorName;
                instructorQuery.InstructorPhone = instructor.InstructorPhone;
                instructorQuery.InstructorEmail = instructor.InstructorEmail;

                await _db.UpdateAsync(instructorQuery);

            }
        }
        #endregion

        #region Assessment Methods
        //Add assessment to course
        public static async Task AddAssessment(Assessment assessment)
        {
            await Init();
            await _db.InsertAsync(assessment);
            var id = assessment.AssessmentId;
        }

        public static async Task<IEnumerable<Assessment>> GetAssessments(int associatedCourseId)
        {
            await Init();
            var assessments = await _db.Table<Assessment>().Where(i => i.AssociatedCourseId == associatedCourseId).ToListAsync();
            return assessments;
        }
        public static async Task<IEnumerable<Assessment>> GetAssessments()
        {
            await Init();
            var assessments = await _db.Table<Assessment>().ToListAsync();
            return assessments;
        }
        public static async Task UpdateAssessment(Assessment assessment)
        {
            await Init();
            var assessmentQuery = await _db.Table<Assessment>()
                .Where(i => i.AssessmentId == assessment.AssessmentId)
                .FirstOrDefaultAsync();
            if (assessmentQuery != null)
            {
                assessmentQuery.AssociatedCourseId = assessment.AssociatedCourseId;
                assessmentQuery.AssessmentId = assessment.AssessmentId;
                assessmentQuery.AssessmentName = assessment.AssessmentName;
                assessmentQuery.AssessmentStartDate = assessment.AssessmentStartDate;
                assessmentQuery.AssessmentEndDate = assessment.AssessmentEndDate;
                assessmentQuery.AssessmentType = assessment.AssessmentType;
                assessmentQuery.AssessmentNotify = assessment.AssessmentNotify;

                await _db.UpdateAsync(assessmentQuery);

            }
        }
        public static async Task<bool> VerifyAssessmentCountMet(int assessmentId, int associatedCourseId, string assessmentType)
        {
            await Init();
            var assessmentCountQuery = await _db.Table<Assessment>()
                .Where(i => i.AssociatedCourseId == associatedCourseId && i.AssessmentType == assessmentType && i.AssessmentId != assessmentId)
                .FirstOrDefaultAsync();
            if (assessmentCountQuery != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static async Task RemoveAssessment(int id)
        {
            await Init();
            await _db.DeleteAsync<Assessment>(id);
        }
        #endregion

        #region Note Methods
        //Add note
        public static async Task AddNote(Note note)
        {
            await Init();
            await _db.InsertAsync(note);
        }
        //Remove note
        public static async Task RemoveNote(int noteId)
        {
            await Init();
            await _db.DeleteAsync<Note>(noteId);
        }
        //Get notes associated with a specific course
        public static async Task<IEnumerable<Note>> GetNotes(int associatedCourseId)
        {
            await Init();
            var notes = await _db.Table<Note>().Where(i => i.AssociatedCourseId == associatedCourseId).ToListAsync();
            return notes;
        }
        //Get notes w/o regard to course
        public static async Task<IEnumerable<Note>> GetNotes()
        {
            await Init();
            var notes = await _db.Table<Note>().ToListAsync();
            return notes;
        }
        //Update note
        public static async Task UpdateNote(Note note)
        {
            await Init();
            var noteQuery = await _db.Table<Note>()
                .Where(i => i.NoteId == note.NoteId)
                .FirstOrDefaultAsync();
            if (noteQuery != null)
            {
                noteQuery.AssociatedCourseId = note.AssociatedCourseId;
                noteQuery.NoteId = note.NoteId;
                noteQuery.NoteTitle = note.NoteTitle;
                noteQuery.NoteBody = note.NoteBody;

                await _db.UpdateAsync(noteQuery);
            }
        }
        #endregion

        #region DemoData
        public static async Task LoadSampleData()
        {
            await Init();
            Term term = new Term()
            {
                TermTitle = "Fall 2030",
                TermStartDate= DateTime.Today.Date,
                TermEndDate= DateTime.Today.Date.AddDays(1)
            };
            await _db.InsertAsync(term);

            Course course = new Course()
            {
              AssociatedTermId = term.TermId,
              CourseName = "Networking",
              CourseStartDate=DateTime.Today.Date,
              CourseEndDate=DateTime.Today.Date.AddDays(1),
              CourseStatus="Active",
              CourseNotify = true,
            };
            await _db.InsertAsync(course);

            Assessment assessment = new Assessment()
            {
                AssociatedCourseId = course.CourseId,
                AssessmentName = "Networking PA",
                AssessmentType = "Performance",
                AssessmentStartDate= DateTime.Today.Date,
                AssessmentEndDate= DateTime.Today.Date.AddDays(1),
                AssessmentNotify = true
            };
            await _db.InsertAsync(assessment);

            Assessment assessment2 = new Assessment()
            {
                AssociatedCourseId = course.CourseId,
                AssessmentName = "Networking OA",
                AssessmentType = "Objective",
                AssessmentStartDate = DateTime.Today.Date,
                AssessmentEndDate = DateTime.Today.Date.AddDays(1),
                AssessmentNotify = true
            };
            await _db.InsertAsync(assessment2);

            Instructor instructor = new Instructor()
            {
                AssociatedCourseId= course.CourseId,
                InstructorName="Anika Patel",
                InstructorPhone="555-123-4567",
                InstructorEmail="anika.patel@strimeuniveristy.edu"
            };
            await _db.InsertAsync(instructor);
        }
        #endregion
    }
}
