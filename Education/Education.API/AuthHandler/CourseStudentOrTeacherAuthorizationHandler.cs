using Education.Application.EduCourses.Dto;
using FurtherEducation.Common.Extension;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Education.API
{
    public class CourseStudentOrTeacherAuthorizationHandler : AuthorizationHandler<StudentOrTeacherRequirement, EduCourseDetailedDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                           StudentOrTeacherRequirement requirement,
                                                           EduCourseDetailedDto resource)
        {
            var userId = context.User.GetLoggedInUserId();

            //if (resource.Students.Contains(userId) || resource.Teacher == userId)
            //{
                context.Succeed(requirement);
            //}

            return Task.CompletedTask;
        }
    }

    public class StudentOrTeacherRequirement : IAuthorizationRequirement { }
}