using System.ComponentModel.DataAnnotations;

namespace BusTicketCase.MVC.Validation
{
    public class MinDateTodayAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime date)
            {
                return date.Date >= DateTime.Now.Date;
            }
            return false;
        }
    }
}
