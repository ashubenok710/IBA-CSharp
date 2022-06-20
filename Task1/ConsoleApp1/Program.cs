class Program
{
    class User
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
            char[] delimiterChars = { '#', ' ', '/', ',', '.', ':', '\t' };
            string[] data = str.Split(delimiterChars);

            if (data.Length == 5)
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
        private int getAge() {
            return DateTime.Now.Year - this.birthday.Year;
        }
        private string getPeriodToNextBirthday() 
        {
            TimeSpan span = this.birthday.AddYears(this.getAge() + 1) - DateTime.Now;
            return String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
                span.Days, span.Hours, span.Minutes, span.Seconds);
        }
        private string getBirthdayAsString() 
        {
            return String.Format("{0:dddd dd MMMM yyyy}", this.birthday);                         
        }
    }

    static void Main(string[] args)
    {
        System.Console.WriteLine($"Пользователь, введите: Имя, Фамилию, Год рождения. Разделителем между частями может быть любой символ: пробел, #, / и так далее.");
        //Сергей.Осипенко.07/04/1997

        string str = "";
        while (true)
        {
            str = Console.ReadLine();

            if (str.Length > 0 & str.Length < 40)
            {
                break;
            }
            else 
            {
                System.Console.WriteLine($"Первоначальная строка не может быть пустой или больше 40 символов.");
            }
        }

        User user = new User(str);
        System.Console.WriteLine($"{user.ToString()}");
    }
}