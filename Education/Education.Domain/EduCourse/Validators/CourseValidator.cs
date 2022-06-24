using Education.Domain.EduCourses;
using FluentValidation;


namespace Education.Application.EduCourses.Validators
{
    public class CourseValidator : AbstractValidator<EduCourse>
    {
        public CourseValidator()
        {
            RuleFor(e => e.Name)
                .NotEmpty()
                .WithMessage("Name is empty")
                .MinimumLength(4)
                .WithMessage("Name minimum length is 4")
                .MaximumLength(128)
                .WithMessage("Name maximum length is 128")
                //\u0400-\u04FF - Кириллица
                .Matches("^[0-9a-zA-Z\u0400-\u04FF\"\\'\\!\\№\\;\\:\\&\\*\\(\\)\\+\\,\\. ]+$")
                .WithMessage("Doesnt match RegEx ^[0-9a-zA-Z\"\\'\\!\\№\\;\\:\\&\\*\\(\\)\\+\\,\\. ]+$ + кириллица");

            RuleFor(e => e.Description)
                 .NotEmpty()
                 .WithMessage("Description is empty")
                 .MinimumLength(4)
                 .WithMessage("Description minimum length is 4")
                 .MaximumLength(2048)
                 .WithMessage("Description maximum length is 2048")
                 .Matches("^[0-9a-zA-Z\u0400-\u04FF\"\\'\\!\\№\\;\\:\\&\\*\\(\\)\\+\\,\\. ]+$")
                 .WithMessage("Doesnt match RegEx ^[0-9a-zA-Z\"\\'\\!\\№\\;\\:\\&\\*\\(\\)\\+\\,\\. ]+$ + кириллица");


            //RuleFor(e => e.Price)
            //    .NotEqual(666);
        }
    }
}
