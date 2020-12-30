namespace Stize.DotNet.Providers.DateTime
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public System.DateTime Now => System.DateTime.UtcNow;
    }
}