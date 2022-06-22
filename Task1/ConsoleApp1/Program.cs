using Timer = System.Timers.Timer;

class Program
{
    public static Timer Timer = new Timer();

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

        Timer.Elapsed += (sender, eventArgs) => {
            if (user.IsBirthdayToday())
            {
                Console.WriteLine("Поздравляем вам исполнилось " + user.GetAge());
                Timer.Stop();
                Environment.Exit(0);
            }
            TimerHandler(user);
        };
        Timer.Interval = (1000);
        Timer.Enabled = true;
        Timer.Start();
        Console.ReadKey();
    }

}