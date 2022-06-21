using System;

public class User
{
    public string firstName = "";
    public string lastName = "";
    public DateTime birthday;

    public User()
    {
        this.firstName = "Не определено";
        this.lastName = "Не определено";
        this.birthday = DateTime.Now;
    }

    public User(string str) : this()
    {
        char[] delimiterChars = {
      '#',
      ' ',
      '/',
      ',',
      '.',
      ':',
      '\t'
    };
        string[] data = str.Split(delimiterChars);

        if (data.Length > 5)
        {
            this.firstName = data[0];
            this.lastName = data[1];
            this.birthday = DateTime.ParseExact(data[2] + "-" + data[3] + "-" + data[4], "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public override string ToString()
    {
        return "Имя: " + this.firstName + "\n" +
          "Фамилия: " + this.lastName + "\n" +
          "Родился: " + this.getBirthdayAsString() + "\n" +
          "Кол-во полных лет: " + this.getAge() + "\n" +
          "Следующий день рождение через: " + this.getPeriodToNextBirthday();
    }
    public int getAge()
    {
        return DateTime.Now.Year - this.birthday.Year;
    }
    public string getPeriodToNextBirthday()
    {
        DateTime today = DateTime.Today;
        DateTime next = birthday.AddYears(today.Year - birthday.Year);

        if (next.Date <= today.Date)
            next = next.AddYears(1);

        TimeSpan span = next - DateTime.Now;
        return String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
          span.Days, span.Hours, span.Minutes, span.Seconds);
    }
    private string getBirthdayAsString()
    {
        return String.Format("{0:dddd dd MMMM yyyy}", this.birthday);
    }

    public bool isBirthdayToday()
    {
        if (this.birthday.Day == DateTime.Now.Day & this.birthday.Month == DateTime.Now.Month)
        {
            return true;
        }
        return false;
    }
}