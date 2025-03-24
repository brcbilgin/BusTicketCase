using Newtonsoft.Json.Converters;
using System.Globalization;

namespace BusTicketCase.Application.Common.Convertor
{
    public class DateTimeConvertor : IsoDateTimeConverter
    {
        public DateTimeConvertor()
        {
            DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        }
    }
}
