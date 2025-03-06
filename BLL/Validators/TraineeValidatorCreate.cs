using BLL.DTO;
using DAL.Interfaces;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace BLL.Validators
{
    public class TraineeValidatorCreate : AbstractValidator<TraineeDTO>
    {
        public TraineeValidatorCreate(IUnitOfWork unitOfWork)
        {
            RuleFor(trainee => trainee).Must((trainee) => new PhoneAttribute().IsValid(trainee.Phone))
                .WithMessage("Wrong phone number format");
            RuleFor(trainee => trainee).Must((trainee) => new EmailAddressAttribute().IsValid(trainee.Email))
                .WithMessage("Wrong email format");
            RuleFor(trainee => trainee).MustAsync(
                async (trainee, cancellation) => 
                    await unitOfWork.Directions.Retrieve(x => x.Id == trainee.Direction.Id) != null
            ).WithMessage("Direction doesn`t exist");
            RuleFor(trainee => trainee).MustAsync(
                async (trainee, cancellation) =>
                    await unitOfWork.Projects.Retrieve(x => x.Id == trainee.Project.Id) != null
            ).WithMessage("Project doesn`t exist");
            RuleFor(trainee => trainee).MustAsync(
                async (trainee, cancellation) =>
                    await unitOfWork.Trainees.Retrieve(x => x.Email == trainee.Email) == null
            ).WithMessage("Trainee with this email already exists");
            RuleFor(trainee => trainee).MustAsync(
                async (trainee, cancellation) => ( trainee.Phone == null ||
                    await unitOfWork.Trainees.Retrieve(x => x.Phone == trainee.Phone) == null)
            ).WithMessage("Trainee with this phone already exists");

        }
    }
}
