using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using WpfApp1.Model;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DBMoviesContext _context = new DBMoviesContext();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async Task LoadAsync(string FileName) 
        {
            using (FileStream fs = File.Open(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] words = line.Split(';');
                    
                    var person = new Person
                    {
                        FirstName = words[0],
                        LastName = words[1],
                        Role = "Director"
                    };

                    var director = _context.People.SingleOrDefault(c => c.FirstName == person.FirstName && c.LastName == person.LastName);
                    if (director == null)
                    {
                        _context.People.Add(person);
                        _context.SaveChanges();
                    }
                    //Trace.WriteLine("director.id" + director.PersonId);


                    var movie = new Movie
                    {
                        Name = words[2],
                        ProductionDate = words[3],
                        Raiting = words[4],
                        DirectorId = (director != null ? director.PersonId: person.PersonId)
                    };

                    _context.Movies.Add(movie);
                    _context.SaveChanges();                 
                    
                }
            }
        } 

        private async void loadFile_Click(object sender, RoutedEventArgs e) 
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = @"D:\C#\C# git\Task2";
            openFileDialog.Filter = "CSV Files (*.csv)|*.csv";

            if (openFileDialog.ShowDialog() == true)
            {
                await LoadAsync(openFileDialog.FileName);
            }                
        }

        private void closeItem_Click(object sender, RoutedEventArgs e)
        {            
            Close();
        }
                        
    }
}
