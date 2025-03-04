using DAL.Models;
using FluentValidation;


namespace BLL.Validators
{
    internal class TraineeValidator : AbstractValidator<Trainee>
    {
        public TraineeValidator()
        {
            RuleFor(trainee => trainee.Name).NotNull();
            RuleFor(trainee => trainee.Surname).NotNull();
            RuleFor(trainee => trainee.Email).EmailAddress();
        }
    }
}
