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
            await _db.CreateTableAsync<Objective>();
            await _db.CreateTableAsync<Performance>();
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
        
        //Add user
        public static async Task<int> AddUser(string username, string password)
        {
            await Init();
            var user = new User()
            {
                UserUsername = username,
                UserPassword = password
            };
            await _db.InsertAsync(user);
            var id = user.UserId;
            return id;
        }
        //Authenticate user and return UserId
        public static async Task<int> AuthenticateUser(string username, string password)
        {
            var lookedUpUserId = 0;
            await Init();
            var user = await _db.Table<User>()
                .Where(i => i.UserUsername == username && i.UserPassword == password)
                .FirstOrDefaultAsync();
            if (user == null) {
                return 0;
            }
            return user.UserId;
        }

        //TODO: Delete the Clear User Table functionality
        //public static async Task ClearUserTable()
        public static async Task ClearAllTables()
        {
            await Init();
            //var clearTableCmd = "DELETE FROM User";
            //await _db.ExecuteAsync(clearTableCmd);

            //await _db.DeleteAllAsync<User>();
            await _db.DeleteAllAsync<Term>();
            await _db.DeleteAllAsync<Course>();
            await _db.DeleteAllAsync<Instructor>();
            await _db.DeleteAllAsync<Assessment>();
            await _db.DeleteAllAsync<Note>();
            await _db.DeleteAllAsync<User>();
        }
        //TODO: Delete "Get user count" functionality
        public static async Task<int> GetUserCount()
        {
            await Init();
            int userCount = await _db.ExecuteScalarAsync<int>("Select Count(*) from User");
            return userCount;
        }
        #endregion

        #region Term Methods
        //Add term
        public static async Task AddTerm(string termTitle, DateTime termStartDate, DateTime termEndDate, int currentUserId)
        {
            await Init();
            var term = new Term()
            {
                TermTitle = termTitle,
                TermStartDate = termStartDate,
                TermEndDate = termEndDate,
                AssociatedUserId = currentUserId
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
            if (assessment is Performance performanceAssessment)
            {
                Performance performance = new Performance();
                performance = (Performance)assessment;
                await Init();
                await _db.InsertAsync(performance);
                var id = assessment.AssessmentId;
            }
            else if (assessment.GetType() == typeof(Objective))
            {
                Objective objective = new Objective();
                objective = (Objective)assessment;
                await Init();
                await _db.InsertAsync(objective);
                var id = objective.AssessmentId;
            }
        }

        public static async Task<IEnumerable<Object>> GetAssessments(int associatedCourseId)
        {
            await Init();
            //Lookup each assessment for the associated course
            var performanceAssessments = await _db.Table<Performance>().Where(i => i.AssociatedCourseId == associatedCourseId).ToListAsync();
            var objectiveAssessments = await _db.Table<Objective>().Where(i => i.AssociatedCourseId == associatedCourseId).ToListAsync();

            List<Object> assessments = new List<Object>();
            assessments.AddRange(performanceAssessments);
            assessments.AddRange(objectiveAssessments);
            return assessments;
        }
        public static async Task<IEnumerable<Object>> GetAssessments()
        {
            await Init();
            List<Object> assessments = new List<Object>();
            var performanceAssessments = await _db.Table<Performance>().ToListAsync();
            var objectiveAssessments = await _db.Table<Objective>().ToListAsync();
            assessments.AddRange(performanceAssessments);
            assessments.AddRange(objectiveAssessments);
            return assessments;
        }
        public static async Task UpdateAssessment(Assessment assessment)
        {
            await Init();
            if (assessment.GetType() == typeof(Performance))
            {
                Performance performance = new Performance();
                performance = (Performance)assessment;

                //Check for the assessment in the other table
                var otherTypeQuery = await _db.Table<Objective>()
                .Where(i => i.AssessmentId == performance.AssessmentId)
                .FirstOrDefaultAsync();
                if (otherTypeQuery is not null)
                {
                    RemoveAssessment(assessment.AssessmentId, "Objective");
                }
                var assessmentQuery = await _db.Table<Performance>()
                .Where(i => i.AssessmentId == performance.AssessmentId)
                .FirstOrDefaultAsync();



                //Update and execute the query
                if (assessmentQuery is not null)
                {
                    assessmentQuery.AssociatedCourseId = performance.AssociatedCourseId;
                    assessmentQuery.AssessmentId = performance.AssessmentId;
                    assessmentQuery.AssessmentName = performance.AssessmentName;
                    assessmentQuery.AssessmentStartDate = performance.AssessmentStartDate;
                    assessmentQuery.AssessmentEndDate = performance.AssessmentEndDate;
                    assessmentQuery.PAssessmentFeedback = performance.PAssessmentFeedback;
                    assessmentQuery.AssessmentStatus = performance.AssessmentStatus;
                    assessmentQuery.AssessmentNotify = performance.AssessmentNotify;
                    await _db.UpdateAsync(assessmentQuery);
                }
                else if (assessmentQuery is null)
                {
                   AddAssessment(performance);
                }
            }
            else if (assessment.GetType() == typeof(Objective))
            {
                Objective objective = new Objective();
                objective = (Objective)assessment;

                var otherTypeQuery = await _db.Table<Performance>()
                .Where(i => i.AssessmentId == objective.AssessmentId)
                .FirstOrDefaultAsync();
                if (otherTypeQuery is not null)
                {
                    RemoveAssessment(assessment.AssessmentId, "Performance");
                }
                var assessmentQuery = await _db.Table<Objective>()
                .Where(i => i.AssessmentId == objective.AssessmentId)
                .FirstOrDefaultAsync();
                if (assessmentQuery is not null)
                {
                    assessmentQuery.AssociatedCourseId = objective.AssociatedCourseId;
                    assessmentQuery.AssessmentId = objective.AssessmentId;
                    assessmentQuery.AssessmentName = objective.AssessmentName;
                    assessmentQuery.AssessmentStartDate = objective.AssessmentStartDate;
                    assessmentQuery.AssessmentEndDate = objective.AssessmentEndDate;
                    assessmentQuery.AssessmentStatus = objective.AssessmentStatus;
                    assessmentQuery.OAssessmentScore = objective.OAssessmentScore;
                    assessmentQuery.AssessmentNotify = objective.AssessmentNotify;

                    await _db.UpdateAsync(assessmentQuery);
                }
                else if (assessmentQuery is null)
                {
                    AddAssessment(objective);
                }
            }

        }
        public static async Task<bool> VerifyAssessmentCountMet(int assessmentId, int associatedCourseId, string assessmentType)
        {
            if (assessmentType == "Performance")
            {
                await Init();
                var assessmentCountQuery = await _db.Table<Performance>()
                .Where(i => i.AssociatedCourseId == associatedCourseId && i.AssessmentId != assessmentId)
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
            else if (assessmentType == "Objective")
            {
                await Init();
                var assessmentCountQuery = await _db.Table<Objective>()
                .Where(i => i.AssociatedCourseId == associatedCourseId && i.AssessmentId != assessmentId)
                .FirstOrDefaultAsync();

                if (assessmentCountQuery != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }//Return true by default (limit met)
            else
            {
                return true;
            }
        }
        public static async Task RemoveAssessment(int id, string assessmentType)
        {
            await Init();
            if (assessmentType == "Performance")
            {
                await _db.DeleteAsync<Performance>(id);
            }
            else if (assessmentType == "Objective")
            {
                await _db.DeleteAsync<Objective>(id);
            }

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

            Performance performance = new Performance()
            {
                AssociatedCourseId = course.CourseId,
                AssessmentName = "Networking PA",
                PAssessmentFeedback = "You did great!",
                AssessmentStartDate= DateTime.Today.Date,
                AssessmentEndDate= DateTime.Today.Date.AddDays(1),
                AssessmentNotify = true
            };
            await _db.InsertAsync(performance);

            Objective objective = new Objective()
            {
                AssociatedCourseId = course.CourseId,
                AssessmentName = "Networking OA",
                OAssessmentScore = 100,
                AssessmentStartDate = DateTime.Today.Date,
                AssessmentEndDate = DateTime.Today.Date.AddDays(1),
                AssessmentNotify = true
            };
            await _db.InsertAsync(performance);

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
