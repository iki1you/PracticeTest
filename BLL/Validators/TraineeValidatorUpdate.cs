using BLL.DTO;
using DAL.Interfaces;
using DAL.Models;
using FluentValidation;



namespace BLL.Validators
{
    public class TraineeValidatorUpdate : AbstractValidator<TraineeDTO>
    {
        public TraineeValidatorUpdate(IUnitOfWork unitOfWork)
        {
            RuleFor(trainee => trainee).NotNull()
                .WithMessage("Trainee doesn`t exist");
            RuleFor(trainee => trainee).MustAsync(
                async (trainee, cancellation) => {
                    if (trainee.Phone == null)
                        return true;
                    var t = await unitOfWork.Trainees.Retrieve(x => x.Phone == trainee.Phone);
                    return t == null || trainee.Id == t.Id;
                }
            ).WithMessage("Trainee with this phone number already exists");
            RuleFor(trainee => trainee).MustAsync(
                async (trainee, cancellation) => {
                    var t = await unitOfWork.Trainees.Retrieve(x => x.Email == trainee.Email);
                    return t == null || trainee.Id == t.Id;
                }
            ).WithMessage("Trainee with this email number already exists");
        }
    }
}
