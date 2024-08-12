using FluentValidation;
using Rater.Domain.DataTransferObjects.SpaceDto;

namespace Rater.Business.Validator.Dto.Space
{
    public class GrandSpaceRequestDtoValidator : AbstractValidator<GrandSpaceRequestDto>
    {
        public GrandSpaceRequestDtoValidator()
        {
            RuleFor(x => x.creatorNickname)
                .NotEmpty()
                .WithMessage("The creator's nickname must be provided.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("The space must have a name.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("A password is required.");
            
            RuleFor(x => x.Password)
                .MaximumLength(16)
                .WithMessage("The password cannot exceed 16 characters.");

            RuleFor(x => x.Metrics)
                .NotEmpty()
                .WithMessage("At least one metric is required.");

            RuleForEach(x => x.Metrics)
                .NotEmpty()
                .WithMessage("Each metric must be provided.");

            RuleForEach(x => x.Metrics).ChildRules(metrics =>
            {
                metrics.RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Metric name not provided");
            });

            RuleFor(x => x.Participants)
                .NotEmpty()
                .WithMessage("At least one participant is required.");
            
            RuleForEach(x => x.Participants).ChildRules(participants =>
            {
                participants.RuleFor(x => x.ParticipantName)
                .NotEmpty()
                .WithMessage("Each participant must have a name.");
            });

            RuleFor(x => x.Participants)
                .Must(participants =>
                {
                    var duplicates = participants
                    .GroupBy(p => p.ParticipantName)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();

                    return !duplicates.Any();
                })
                .WithMessage("Participant names must be unique.");
        }
    }
}
