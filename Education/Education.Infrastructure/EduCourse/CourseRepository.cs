using Education.Application.Data.Context;
using Education.Domain;
using Education.Domain.Aggregation;
using Education.Domain.EduCourses;
using FurtherEducation.Common.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Education.Application.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ICourseContext _context;

        public CourseRepository(ICourseContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //CREATE
        public async Task<bool> Create(EduCourse course)
        {
            if (course.Students == null)
            {
                course.Students = new List<Guid>();
            }
            await _context.Courses.InsertOneAsync(course);

            //  _context.Courses.Indexes.CreateOne(new CreateIndexModel<Course>(Builders<Course>.IndexKeys.Text("$**")));
            var nameIndex = new CreateIndexModel<EduCourse>(Builders<EduCourse>.IndexKeys.Text(x => x.Name));

            _context.Courses.Indexes.CreateOne(nameIndex);

            return await Task.FromResult(true);
        }

        //READ
        public async Task<EduCourseDetailed> FindDetailedAsync(ObjectId id)
        {
            var filter = Builders<EduCourse>.Filter.Eq(x => x.Id, id);
            var q = _context.Courses.Aggregate().Match(filter)
                .Lookup(_context.Modules, c => c.Id, m => m.CourseId, (EduCourseDetailed eo) => eo.Modules)
                .Lookup(_context.Announcements, c => c.Id, m => m.CourseId, (EduCourseDetailed eo) => eo.Announcements)
                .As<EduCourseDetailed>();

            return await q.FirstOrDefaultAsync();
            //LINQ
            //var query = from p in _context.Courses.AsQueryable().Where(x=>x.Id == id)
            //            join m in _context.Modules.AsQueryable() on p.Id equals m.CourseId into moduleCol
            //            join a in _context.Announcements.AsQueryable() on p.Id equals a.CourseId into announceCol
            //            select new CourseDetailed()
            //            {
            //                ContentImage = p.ContentImage,
            //                Description = p.Description,
            //                Hours = p.Hours,
            //                Id = p.Id,
            //                Name = p.Name,
            //                Professor = p.Professor,
            //                Speciality = p.Speciality,
            //                Students = p.Students,
            //                TotalMembers = p.TotalMembers,
            //                Modules = moduleCol,
            //                Announcements = announceCol
            //            };
        }

        public async Task<PagedList<EduCourse>> FindAsync(int pageNumber = 0, int pageSize = 10)
        {
            var query = _context.Courses.Find(x => true);
            var total = await query.CountDocumentsAsync();

            var result = await query.Skip(pageNumber * pageSize).Limit(pageSize).ToListAsync();

            return new PagedList<EduCourse>(result, pageNumber, pageSize, total);
        }

        public async Task<PagedList<EduCourse>> FindAsync(string search = "", int pageNumber = 0, int pageSize = 10)
        {
            if (string.IsNullOrEmpty(search))
            {
                return await FindAsync(pageNumber, pageSize);
            }
            var query = _context.Courses.Find(Builders<EduCourse>.Filter.Text(search));
            var total = await query.CountDocumentsAsync();

            var result = await query.Skip(pageNumber * pageSize).Limit(pageSize).ToListAsync();

            return new PagedList<EduCourse>(result, pageNumber, pageSize, total);
        }

        public async Task<PagedList<EduCourse>> FindAsync(Expression<Func<EduCourse, bool>> expression, int pageNumber = 0,
            int pageSize = 10)
        {
            var query = _context.Courses.Find(expression);
            var total = await query.CountDocumentsAsync();

            var result = await query.Skip(pageNumber * pageSize).Limit(pageSize).ToListAsync();

            return new PagedList<EduCourse>(result, pageNumber, pageSize, total);
        }


        public async Task<EduCourse?> FindAsync(ObjectId id)
        {
            return await _context
                .Courses
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<EduCourse?> FindByModuleAsync(ObjectId moduleId)
        {
            //Делаем проекцию - только Id и CourseId
            //Проекция нужна что бы был один класс для агреций Модуля, анонса, задания, теста

            return await GetCourseFromChild(moduleId, _context.Modules);

            //var definition = Builders<EduModule>.Projection
            //    .Include(u => u.CourseId);

            //var filter = Builders<EduModule>.Filter.Eq(x => x.Id, moduleId);

            //var q = _context.Modules.Aggregate()
            //    .Match(filter)

            //    .Project(definition)
            //    .As<EduChildOfCourseProjection>()

            //    .Lookup(_context.Courses, c => c.CourseId, m => m.Id, (EduChildOfCourseAggregation eo) => eo.Courses)
            //    .As<EduChildOfCourseAggregation>();

            //var aggregate = await q.FirstOrDefaultAsync();
            //if (aggregate == null)
            //{
            //    return null;
            //}

            //return aggregate.Courses.FirstOrDefault();
        }

        private async Task<EduCourse?> GetCourseFromChild<T>(ObjectId childId, IMongoCollection<T> mongoCollection) where T : ICourseData
        {
            var definition = Builders<T>.Projection
               .Include(u => u.CourseId);

            var filter = Builders<T>.Filter.Eq(x => x.Id, childId);

            var q = mongoCollection.Aggregate()
                .Match(filter)

                .Project(definition)
                .As<EduChildOfCourseProjection>()

                .Lookup(_context.Courses, c => c.CourseId, m => m.Id, (EduChildOfCourseAggregation eo) => eo.Courses)
                .As<EduChildOfCourseAggregation>();

            var aggregate = await q.FirstOrDefaultAsync();
            if (aggregate == null)
            {
                return null;
            }

            return aggregate.Courses.FirstOrDefault();
        }

        public async Task<EduCourse?> FindByAnnouncementAsync(ObjectId announcementId)
        {
            return await GetCourseFromChild(announcementId, _context.Announcements);
        }

        public async Task<EduCourse?> FindByTaskAsync(ObjectId taskId)
        {
            return await GetCourseFromChild(taskId, _context.EduTasks);
        }

        public async Task<EduCourse?> FindByTestAsync(ObjectId testId)
        {
            return await GetCourseFromChild(testId, _context.EduTests);
        }

        //UPDATE
        public async Task<bool> UpdateAsync(EduCourse course)
        {
            var old = await FindAsync(course.Id);
            if (old == null)
            {
                return await Task.FromResult(false);
            }

            old.Name = course.Name;
            old.Description = course.Description;
            old.Image = course.Image;
            old.Hours = course.Hours;
            old.Teacher = course.Teacher;
            old.Speciality = course.Speciality;
            old.Students = course.Students;

            await WriteToDatabase(old);

            return await Task.FromResult(true);
        }

        public async Task<bool> WriteToDatabase(EduCourse course)
        {
            var updateResult = await _context
                .Courses
                .ReplaceOneAsync(g => g.Id == course.Id, course);

            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        //DELETE
        public async Task<bool> DeleteAsync(ObjectId id)
        {
            var deleteResult = await _context.Courses
                .DeleteOneAsync(x => x.Id == id);

            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }
    }
}
