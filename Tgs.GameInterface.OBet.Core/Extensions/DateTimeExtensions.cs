namespace Tgs.GameInterface.OBet.Core.Extensions;

public static class DateTimeExtensions
{
    private static TimeZoneInfo TimeZone => TimeZoneInfo.CreateCustomTimeZone(id: "Taipei Standard Time", baseUtcOffset: TimeSpan.FromHours(+8), "Taipei Standard Time", "Taipei Standard Time");

    public static string DatetimeConvertToString(this DateTimeOffset date)
    {
        return TimeZoneInfo.ConvertTime(date.ToUniversalTime(), TimeZone).ToString("yyyyMMddHHmmss");
    }

    public static DateTimeOffset StringConvertToDatetime(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return DateTime.UnixEpoch;
        }
        var date = DateTime.Parse(str);

        return new DateTimeOffset(date, TimeZone.GetUtcOffset(date));
    }
}