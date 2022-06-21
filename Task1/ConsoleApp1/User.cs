using System.Text.RegularExpressions;
public class User
{
    public string FirstName = "";
    public string LastName = "";
    public DateTime Birthday;

    public User()
    {
        FirstName = "Не определено";
        LastName = "Не определено";
        Birthday = DateTime.Now;
    }

    public User(string str) : this()
    {
        Regex pattern = new Regex(@"\.|/|#| |,|:|\t");       
        string[] data = pattern.Split(str);

        if (data.Length > 4)
        {
            FirstName = data[0];
            LastName = data[1];
            Birthday = DateTime.ParseExact(data[2] + "-" + data[3] + "-" + data[4], "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
    }

    public override string ToString()
    {
        return "Имя: " + FirstName + "\n" +
          "Фамилия: " + LastName + "\n" +
          "Родился: " + GetBirthdayAsString() + "\n" +
          "Кол-во полных лет: " + GetAge() + "\n" +
          "Следующий день рождение через: " + GetPeriodToNextBirthday();
    }
    public int GetAge()
    {
        return DateTime.Now.Year - Birthday.Year;
    }
    public string GetPeriodToNextBirthday()
    {
        DateTime today = DateTime.Today;
        DateTime next = Birthday.AddYears(today.Year - Birthday.Year);

        if (next.Date <= today.Date)
            next = next.AddYears(1);

        TimeSpan span = next - DateTime.Now;
        return String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
          span.Days, span.Hours, span.Minutes, span.Seconds);
    }
    private string GetBirthdayAsString()
    {
        return String.Format("{0:dddd dd MMMM yyyy}", Birthday);
    }

    public bool IsBirthdayToday()
    {
        if (Birthday.Day == DateTime.Now.Day & Birthday.Month == DateTime.Now.Month)
        {
            return true;
        }
        return false;
    }
}