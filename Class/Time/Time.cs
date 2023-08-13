using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

[Serializable]
public class Time
{
	/// <summary>
	/// Start at
	/// </summary>
	/// Date - if date is the same to system date continue (any time) else wait
	/// Time - if time is the same to system time continue (any date) else wait
	/// Date and Time - if date and time is the same to system data and time continue else wait

	public static Time dataDateTime = new Time(new MTime(0, 0, 0, 50), new MTime(0, 0, 0, 120));

	public MTime delayAt;		// Delay at & Sleep
	public MTime delayTo;		// Delay to

	public Time(MTime delayAt, MTime delayTo)
	{
		this.delayAt = delayAt;
		this.delayTo = delayTo;
	}
	public Time(MTime delaySleep)
	{
		delayAt = delaySleep;
	}
    public override string ToString()
    {
		string text = "";

		/* Show delay at or sleep time */
		if (delayTo != null)
		{
			/* Dealy at to */
			text += " Delay:[";
			text += delayAt.hour.ToString() + ":";
			text += delayAt.minute.ToString() + ":";
			text += delayAt.second.ToString() + ".";
			text += delayAt.millisecond.ToString() + "]";

			text += "-[";
			text += delayTo.hour.ToString() + ":";
			text += delayTo.minute.ToString() + ":";
			text += delayTo.second.ToString() + ".";
			text += delayTo.millisecond.ToString() + "]";
		}
		else
		{
			/* Sleep */
			text += " Sleep:[";
			text += delayAt.hour.ToString() + ":";
			text += delayAt.minute.ToString() + ":";
			text += delayAt.second.ToString() + ".";
			text += delayAt.millisecond.ToString() + "]";
		}

		return text;
	}

    #region Times
    public static void numericTextBoxOnlyNumber(KeyEventArgs e)
	{
		if (!Char.IsDigit((char)KeyInterop.VirtualKeyFromKey(e.Key)) & e.Key != Key.Back)
		{
			e.Handled = true;
		}
	}
	public static void numericTextBoxValue(TextBox textBox, int minValue, int maxValue)
	{
		int value;
		/* Get value with TextBox and try parse to int */
		int.TryParse(textBox.Text, out value);  // Convert textbox text to int

		if (value < minValue) value = minValue;			// Min value
		else if (value > maxValue) value = maxValue;  // Max value
		/* Set value in TextBox */
		textBox.Text = value.ToString();
		textBox.SelectionStart = textBox.Text.Length;
	}
	public static void numericTextBoxUp(TextBox textBox, int maxValue)
    {
		/* Get value with TextBox and try parse to int */
		int value = int.Parse(textBox.Text);
		value++;

		if (value > maxValue) value = maxValue;   // Max value
		/* Set value in TextBox */
		textBox.Text = value.ToString();
	}
	public static void numericTextBoxDown(TextBox textBox, int minValue)
	{
		/* Get value with TextBox and try parse to int */
		int value = int.Parse(textBox.Text);
		value--;

		if (value < minValue) value = minValue;   // Min value
		/* Set value in TextBox */
		textBox.Text = value.ToString();
	}
    #endregion
}
