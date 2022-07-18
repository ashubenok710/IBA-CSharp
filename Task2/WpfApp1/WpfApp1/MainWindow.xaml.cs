using ClosedXML.Excel;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using WpfApp1.Model;
using WpfApp1.Pagination;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DBMoviesContext _context = new DBMoviesContext();

        //private resultQuery;


        public MainWindow()
        {
            InitializeComponent();
        }
        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }


        public void closeItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
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
                        await _context.SaveChangesAsync();
                    }
                    //Trace.WriteLine("director.id" + director.PersonId);

                    var movie = new Movie
                    {
                        Name = words[2],
                        ProductionDate = words[3],
                        Raiting = words[4],
                        DirectorId = (director != null ? director.PersonId : person.PersonId)
                    };

                    _context.Movies.Add(movie);
                }
                await _context.SaveChangesAsync();
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

        private void clickLoadData(object sender, RoutedEventArgs e)
        {
            using (_context)
            {
                var result = from person in _context.People
                             join movie in _context.Movies on person.PersonId equals movie.DirectorId
                             select new
                             {
                                 FirstName = person.FirstName,
                                 LastName = person.LastName,
                                 Movie = movie.Name,
                                 Year = movie.ProductionDate,
                                 Raiting = movie.Raiting
                             };
                var skip = 1000;
                var take = 100;

                var list = result.OrderBy(b => b.Year).ThenBy(b => b.Raiting).Skip(skip).Take(take).ToList();
                DisplayGrid.ItemsSource = list;

                /*
                using (XmlWriter writer = XmlWriter.Create("TestLoadData.xml"))
                {
                    DataContractSerializer serializer = new DataContractSerializer(list.GetType());
                    serializer.WriteObject(writer, list);
                    writer.Close();
                }
                */
            }

            //DisplayGrid.ItemsSource = _context.Movies.ToList();
        }

        private async void Button_Click_1Async(object sender, RoutedEventArgs e)
        {
            var query = _context.Movies.OrderBy(b => b.ProductionDate).ThenBy(b => b.Raiting).AsQueryable();
            try
            {
                var task = PaginationService.GetPagination(query, 1, "", false, 100);
                var list = await task;

                DisplayGrid.ItemsSource = list.Result;
            }
            finally
            {
            }
        }


        private async void Button_Click_1AsyncTest(object sender, RoutedEventArgs e)
        {
            using (_context)
            {
                var result = from person in _context.People
                             join movie in _context.Movies on person.PersonId equals movie.DirectorId
                             select new
                             {
                                 FirstName = person.FirstName,
                                 LastName = person.LastName,
                                 Movie = movie.Name,
                                 Year = movie.ProductionDate,
                                 Raiting = movie.Raiting
                             };
                var skip = 1000;
                var take = 100;

                //var list = result.OrderBy(b => b.Year).ThenBy(b => b.Raiting).Skip(skip).Take(take).ToList();
                var q = result.AsQueryable();

                try
                {
                    var task = PaginationService.GetPagination(q, 1, "", false, 100);
                    var list = await task;

                    DisplayGrid.ItemsSource = list.Result;
                }
                finally
                {
                }
            }
        }

        private void clickExportXLS(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel |*.xlsx";
            saveFileDialog1.Title = "Save Excel File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                var workbook = new XLWorkbook();
                DBMoviesContext _context = new DBMoviesContext();
                using (_context)
                {
                    var movies = _context.Movies.ToList();
                    var dataTable = DbContextExtensions.ToDataTable(movies);

                    IXLWorksheet worksheet = workbook.AddWorksheet(dataTable);
                    worksheet.Name = "MoviesCatalog";
                }
                workbook.SaveAs(saveFileDialog1.FileName + ".xlsx");
            }
        }


        private void clickExportXML(object sender, RoutedEventArgs e)
        {
            var workbook = new XLWorkbook();
            //workbook.AddWorksheet("MoviesCatalog");
            //var ws = workbook.Worksheet("MoviesCatalog");

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel |*.xml";
            saveFileDialog1.Title = "Save XML File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                DBMoviesContext _context = new DBMoviesContext();
                using (_context)
                {
                    var m = _context.Movies.ToList();
                    //var dt = DbContextExtensions.ToDataTable(m);
                    //https://stackoverflow.com/questions/6234290/serialize-entity-framework-object-with-children-to-xml-file
                    using (XmlWriter writer = XmlWriter.Create(saveFileDialog1.FileName + ".xml"))
                    {
                        DataContractSerializer serializer = new DataContractSerializer(m.GetType());
                        serializer.WriteObject(writer, m);
                        writer.Close();
                    }

                }
            }
        }

    }
}
