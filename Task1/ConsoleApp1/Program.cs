class Program
{
    public static System.Timers.Timer timer = new System.Timers.Timer();

    public static void TimerHandler(User user)
    {
        Console.WriteLine(user.GetPeriodToNextBirthday());
    }

    static void Main(string[] args)
    {
        System.Console.WriteLine($"Пользователь, введите: Имя, Фамилию, Год рождения. Разделителем между частями может быть любой символ: пробел, #, / и так далее.");
        //Сергей.Осипенко.07/04/1997

        string str = "";
        while (true)
        {
            //"Сергей.Осипенко.07/04/1997";
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
        Console.WriteLine($"{user.ToString()}");

        timer.Elapsed += (sender, eventArgs) => {
            if (user.IsBirthdayToday())
            {
                Console.WriteLine("Поздравляем вам исполнилось " + user.GetAge());
                timer.Stop();
                Environment.Exit(0);
            }
            TimerHandler(user);
        };
        timer.Interval = (1000);
        timer.Enabled = true;
        timer.Start();
        Console.ReadKey();
    }

}