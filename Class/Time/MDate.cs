using System;

[Serializable]
public class MDate
{
    public int year;    // 0-9999
    public int month;   // 1-12
    public int day;     // 1-31 ()

    public MDate(int year, int month, int day)
    {
        this.year = year;
        this.month = month;
        this.day = day;
    }
}
