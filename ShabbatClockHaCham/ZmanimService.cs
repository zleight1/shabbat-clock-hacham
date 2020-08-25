using System;

namespace ShabbatClockHaCham
{
    public class ZmanimService
    {
        public ZmanimService()
        {

        }
        internal DateTime ResolveShabbatTime(object location)
        {
            return DateTime.Now.AddMinutes(3);
        }
    }
}