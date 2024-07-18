using System.ComponentModel.DataAnnotations;

namespace Rater.Domain.DataTransferObjects.RatingDto
{
    public class RatingDetailDto
    {
        [Required]
        public int RateeId { get; set; }

        [Required]
        public int MetricId { get; set; }

        [Required]
        [Range(0, 5)]
        public int Score { get; set; }
    }


    //Range attribute ile bunu yapabiliyoruz.

    //[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    //public class MaxInt : ValidationAttribute
    //{
    //    private int _maximum;

    //    public MaxInt(int maximum)
    //    {
    //        _maximum = maximum;
    //    }

    //    public override bool IsValid(object? value)
    //    {
    //        if (value == null)
    //        {
    //            return true;
    //        }

    //        if (int.TryParse(value.ToString(), out int intValue))
    //        {
    //            return intValue <= _maximum;
    //        }

    //        return false;
    //    }
    //}
}
