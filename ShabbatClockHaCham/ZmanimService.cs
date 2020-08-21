using System;

namespace ShabbatClockHaCham
{
    public class ZmanimService
    {
        public ZmanimService()
        {

        }
        internal DateTime ResolveSunsetDateTime(object location)
        {
            return DateTime.Now.AddMinutes(3);
        }
    }
}