using Education.Application.EduCourses.Dto;
using FurtherEducation.Common.Extension;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Education.API
{
    public class CourseStudentAuthorizationHandler : AuthorizationHandler<StudentRequirement, EduCourseDetailedDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                           StudentRequirement requirement,
                                                           EduCourseDetailedDto resource)
        {
            var userId = context.User.GetLoggedInUserId();

            //if (resource.Students.Contains(userId))
            //{
                context.Succeed(requirement);
            //}

            return Task.CompletedTask;
        }
    }

    public class StudentRequirement : IAuthorizationRequirement { }
}