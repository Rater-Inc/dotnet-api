using FluentValidation;
using Rater.Domain.DataTransferObjects.RatingDto;

namespace Rater.Business.Validator.Dto.Rating
{
    public class RatingRequestDtoValidator : AbstractValidator<RatingRequestDto>
    {
        public RatingRequestDtoValidator()
        {
            RuleFor(x => x.RaterNickName).NotEmpty().WithMessage("Rater Nickname can not be empty!");

            RuleFor(x => x.SpaceId).NotEmpty().WithMessage("A valid Space ID is required.");

            RuleFor(x => x.RatingDetails).NotEmpty().WithMessage("Rating Details are required.");
            RuleForEach(x => x.RatingDetails).NotEmpty();
            RuleForEach(x => x.RatingDetails).ChildRules(ratingDetail =>
            {
                ratingDetail.RuleFor(x => x.Score).GreaterThanOrEqualTo(1).WithMessage("The score must be at least 1.").
                LessThanOrEqualTo(5).WithMessage("The score cannot exceed 5.");
            });
        }
    }
}
