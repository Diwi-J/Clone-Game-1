using System;
using UnityEngine;


[Serializable]
public struct GameDate
{
    // amt of days 
    [Range(1, 31)]
    public int day;

    // amt of months 
    [Range(1, 12)]
    public int month;

    public int year; 

    public GameDate(int day, int month, int year)
    {
        this.day = day;
        this.month = month;
        this.year = year;
    }

    // converts the gamedate into build in date time object
    // so we can compare dates to find out if its expired etc or not 
    public DateTime ToDateTime()
    {
        try
        {
            return new DateTime(year, month, day);
        }

        catch
        {
            // logs a warming instead of crashing 
            Debug.LogWarning($"Invalid date: {day}/{month}/{year}");
            //returns to earliest datetime as a fallback 
            return DateTime.MinValue;
        }
    }

    // checks whether this date has already passed or not 
    public bool IsExpired(GameDate currentDate)
    {
        return ToDateTime() < currentDate.ToDateTime();
    }

    // converts this into a string so we can display in UI 
    public override string ToString()
    {
        return $"{day:00}/{month:00}/{year}";
    }
}



