using Education.Application.EduCourses.Dto;
using FurtherEducation.Common.Extension;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Education.API
{
    public class CourseTeacherAuthorizationHandler : AuthorizationHandler<TeacherRequirement, EduCourseDetailedDto>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                           TeacherRequirement requirement,
                                                           EduCourseDetailedDto resource)
        {
            var userId = context.User.GetLoggedInUserId();

            //if (userId == resource.Teacher)
            //{
                context.Succeed(requirement);
            //}

            return Task.CompletedTask;
        }
    }

    public class TeacherRequirement : IAuthorizationRequirement { }
}