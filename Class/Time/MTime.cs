using System;

[Serializable]
public class MTime
{
    public int hour;        // 0-23
    public int minute;      // 0-59
    public int second;      // 0-59
    public int millisecond; // 0-999

    public MTime() { }
    public MTime(int hour, int minute, int second, int millisecond)
    {
        this.hour = hour;
        this.minute = minute;
        this.second = second;
        this.millisecond = millisecond;
    }
}
