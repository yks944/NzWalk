using FluentValidation;

namespace NzWalks.Api.Validators
{
    public class AddWalkDifficultyValidator : AbstractValidator<Models.DTO.AddWalkDifficultyRequest>
    {
        public AddWalkDifficultyValidator()
        {
            RuleFor(x => x.Code).NotEmpty();
        }
    }
}
