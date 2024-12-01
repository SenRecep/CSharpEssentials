using System;

namespace CSharpEssentials.Time;

public interface IDateTimeProvider
{
    DateTime UtcNowDateTime { get; }
    DateTimeOffset UtcNow { get; }

    DateOnly UtcNowDate { get; }
    TimeOnly UtcNowTime { get; }
}
