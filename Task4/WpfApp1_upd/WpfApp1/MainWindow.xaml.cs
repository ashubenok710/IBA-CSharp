using ClosedXML.Excel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using WebApplication1.Repository;
using WpfApp1.Model;
using WpfApp1.Pagination;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        private DBMoviesContext _context = new DBMoviesContext();
        private MovieRepository movieRepository = new MovieRepository();

        //private resultQuery;

        private int numberOfRecPerPage; //Initialize our Variable, Classes and the List

        static Paging PagedTable = new Paging();

        IList<Movie> myList;
        public MainWindow()
        {
            InitializeComponent();

            //refreshDataAsync();
        }
        private void refreshDataAsync()
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:7001/");
            Task<HttpResponseMessage> response = client.GetAsync("api/Movies/");
            if (response.Result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                myList = response.Result.Content.ReadAsAsync<List<Movie>>().Result;
                PagedTable.PageIndex = 1;

                int[] RecordsToShow = { 10, 20, 30, 50, 100 };
                foreach (int RecordGroup in RecordsToShow)
                {
                    NumberOfRecords.Items.Add(RecordGroup);
                }

                numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);

                DataTable firstTable = PagedTable.SetPaging(ConvertToListOf<Movie>(myList.ToList()), numberOfRecPerPage);

                //DisplayGrid.ItemsSource = firstTable.DefaultView;
                //myLable.Content = "deleted " + id;
            }

        }

        private IList<T> ConvertToListOf<T>(IList iList)
        {
            IList<T> result = new List<T>();

            foreach (T value in iList)
                result.Add(value);

            return result;
        }

        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }


        public void closeItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            DBMoviesContext context = new DBMoviesContext();

            using (context)
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

                var movies = myList.ToList();
                var dataTable = DbContextExtensions.ToDataTable(movies);

                IXLWorksheet worksheet = workbook.AddWorksheet(dataTable);
                worksheet.Name = "MoviesCatalog";

                workbook.SaveAs(saveFileDialog1.FileName + ".xlsx");
            }
        }


        private void clickExportXML(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "XML |*.xml";
            saveFileDialog1.Title = "Save XML File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                var m = myList.ToList();
                //https://stackoverflow.com/questions/6234290/serialize-entity-framework-object-with-children-to-xml-file
                using (XmlWriter writer = XmlWriter.Create(saveFileDialog1.FileName + ".xml"))
                {
                    DataContractSerializer serializer = new DataContractSerializer(m.GetType());
                    serializer.WriteObject(writer, m);
                    writer.Close();
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DisplayGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        public string PageNumberDisplay()
        {
            int PagedNumber = numberOfRecPerPage * (PagedTable.PageIndex + 1);
            if (PagedNumber > myList.Count)
            {
                PagedNumber = myList.Count;
            }
            return PagedNumber + " of " + myList.Count;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DisplayGrid.ItemsSource = PagedTable.Last(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DisplayGrid.ItemsSource = PagedTable.Next(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DisplayGrid.ItemsSource = PagedTable.Previous(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void NumberOfRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            numberOfRecPerPage = Convert.ToInt32(NumberOfRecords.SelectedItem);
            DisplayGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            myList = _context.Movies.Where(x => (string.IsNullOrWhiteSpace(x.Name) || x.Name.Contains(txtMovieName.Text))
            && (string.IsNullOrWhiteSpace(x.ProductionDate) || x.ProductionDate.Contains(txtYear.Text))).
                OrderBy(b => b.ProductionDate).ThenBy(b => b.Raiting).AsQueryable().ToList();

            DataTable firstTable = PagedTable.SetPaging(ConvertToListOf<Movie>(myList.ToList()), numberOfRecPerPage);

            DisplayGrid.ItemsSource = firstTable.DefaultView;
            PageInfo.Content = PageNumberDisplay();
        }

        private void Button_DeleteMovie(object sender, RoutedEventArgs e)
        {
            if (DisplayGrid.SelectedItems.Count > 0)
            {
                foreach (var element in DisplayGrid.SelectedItems)
                {
                    DataRowView dr = element as DataRowView;
                    var id = dr["id"].ToString();

                    using var client = new HttpClient();
                    client.BaseAddress = new Uri("https://localhost:7001/");
                    var response = client.DeleteAsync("api/Movies/" + id).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        myLable.Content = "deleted " + id;
                    }
                }
                refreshDataAsync();
                DisplayGrid.ItemsSource = PagedTable.First(myList, numberOfRecPerPage).DefaultView;
            }
        }

        private async void Button_Click_5Async(object sender, RoutedEventArgs e)
        {
            string clientId = "";
            string clientSecret = "";

            await DoOAuthAsync(clientId, clientSecret);

            refreshDataAsync();
        }

        // ref http://stackoverflow.com/a/3978040
        public static int GetRandomUnusedPort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }

        public async Task DoOAuthAsync(string clientId, string clientSecret)
        {
            // Generates state and PKCE values.
            string state = GenerateRandomDataBase64url(32);
            string codeVerifier = GenerateRandomDataBase64url(32);
            string codeChallenge = Base64UrlEncodeNoPadding(Sha256Ascii(codeVerifier));
            const string codeChallengeMethod = "S256";

            // Creates a redirect URI using an available port on the loopback address.
            string redirectUri = $"http://{IPAddress.Loopback}:{GetRandomUnusedPort()}/";
            Log("redirect URI: " + redirectUri);

            // Creates an HttpListener to listen for requests on that redirect URI.
            var http = new HttpListener();
            http.Prefixes.Add(redirectUri);
            Log("Listening..");
            http.Start();

            // Creates the OAuth 2.0 authorization request.
            string authorizationRequest = string.Format("{0}?response_type=code&scope=openid%20profile&redirect_uri={1}&client_id={2}&state={3}&code_challenge={4}&code_challenge_method={5}",
                "https://accounts.google.com/o/oauth2/v2/auth",
                Uri.EscapeDataString(redirectUri),
                clientId,
                state,
                codeChallenge,
                codeChallengeMethod);

            // Opens request in the browser.
            Process.Start(authorizationRequest);

            // Waits for the OAuth authorization response.
            var context = await http.GetContextAsync();

            // Brings the Console to Focus.
            BringConsoleToFront();

            // Sends an HTTP response to the browser.
            var response = context.Response;
            string responseString = "<html><head><meta http-equiv='refresh' content='10;url=https://google.com'></head><body>Please return to the app.</body></html>";
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            var responseOutput = response.OutputStream;
            await responseOutput.WriteAsync(buffer, 0, buffer.Length);
            responseOutput.Close();
            http.Stop();
            Log("HTTP server stopped.");

            // Checks for errors.
            string error = context.Request.QueryString.Get("error");
            if (error is object)
            {
                Log($"OAuth authorization error: {error}.");
                return;
            }
            if (context.Request.QueryString.Get("code") is null
                || context.Request.QueryString.Get("state") is null)
            {
                Log($"Malformed authorization response. {context.Request.QueryString}");
                return;
            }

            // extracts the code
            var code = context.Request.QueryString.Get("code");
            var incomingState = context.Request.QueryString.Get("state");

            // Compares the receieved state to the expected value, to ensure that
            // this app made the request which resulted in authorization.
            if (incomingState != state)
            {
                Log($"Received request with invalid state ({incomingState})");
                return;
            }
            Log("Authorization code: " + code);

            // Starts the code exchange at the Token Endpoint.
            await ExchangeCodeForTokensAsync(code, codeVerifier, redirectUri, clientId, clientSecret);
        }

        async Task ExchangeCodeForTokensAsync(string code, string codeVerifier, string redirectUri, string clientId, string clientSecret)
        {
            Log("Exchanging code for tokens...");

            // builds the  request
            string tokenRequestUri = "https://www.googleapis.com/oauth2/v4/token";
            string tokenRequestBody = string.Format("code={0}&redirect_uri={1}&client_id={2}&code_verifier={3}&client_secret={4}&scope=&grant_type=authorization_code",
                code,
                Uri.EscapeDataString(redirectUri),
                clientId,
                codeVerifier,
                clientSecret
                );

            // sends the request
            HttpWebRequest tokenRequest = (HttpWebRequest)WebRequest.Create(tokenRequestUri);
            tokenRequest.Method = "POST";
            tokenRequest.ContentType = "application/x-www-form-urlencoded";
            tokenRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            byte[] tokenRequestBodyBytes = Encoding.ASCII.GetBytes(tokenRequestBody);
            tokenRequest.ContentLength = tokenRequestBodyBytes.Length;
            using (Stream requestStream = tokenRequest.GetRequestStream())
            {
                await requestStream.WriteAsync(tokenRequestBodyBytes, 0, tokenRequestBodyBytes.Length);
            }

            try
            {
                // gets the response
                WebResponse tokenResponse = await tokenRequest.GetResponseAsync();
                using (StreamReader reader = new StreamReader(tokenResponse.GetResponseStream()))
                {
                    // reads response body
                    string responseText = await reader.ReadToEndAsync();
                    Console.WriteLine(responseText);

                    // converts to dictionary
                    Dictionary<string, string> tokenEndpointDecoded = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                    string accessToken = tokenEndpointDecoded["access_token"];
                    await RequestUserInfoAsync(accessToken);
                }
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        Log("HTTP: " + response.StatusCode);
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            // reads response body
                            string responseText = await reader.ReadToEndAsync();
                            Log(responseText);
                        }
                    }

                }
            }
        }

        private async Task RequestUserInfoAsync(string accessToken)
        {
            Log("Making API Call to Userinfo...");

            // builds the  request
            string userinfoRequestUri = "https://www.googleapis.com/oauth2/v3/userinfo";

            // sends the request
            HttpWebRequest userinfoRequest = (HttpWebRequest)WebRequest.Create(userinfoRequestUri);
            userinfoRequest.Method = "GET";
            userinfoRequest.Headers.Add(string.Format("Authorization: Bearer {0}", accessToken));
            userinfoRequest.ContentType = "application/x-www-form-urlencoded";
            userinfoRequest.Accept = "Accept=text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            // gets the response
            WebResponse userinfoResponse = await userinfoRequest.GetResponseAsync();
            using (StreamReader userinfoResponseReader = new StreamReader(userinfoResponse.GetResponseStream()))
            {
                // reads response body
                string userinfoResponseText = await userinfoResponseReader.ReadToEndAsync();
                Log(userinfoResponseText);
            }
        }

        /// <summary>
        /// Appends the given string to the on-screen log, and the debug console.
        /// </summary>
        /// <param name="output">String to be logged</param>
        private void Log(string output)
        {
            Console.WriteLine(output);
        }

        /// <summary>
        /// Returns URI-safe data with a given input length.
        /// </summary>
        /// <param name="length">Input length (nb. output will be longer)</param>
        /// <returns></returns>
        private static string GenerateRandomDataBase64url(uint length)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[length];
            rng.GetBytes(bytes);
            return Base64UrlEncodeNoPadding(bytes);
        }

        /// <summary>
        /// Returns the SHA256 hash of the input string, which is assumed to be ASCII.
        /// </summary>
        private static byte[] Sha256Ascii(string text)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            using (SHA256Managed sha256 = new SHA256Managed())
            {
                return sha256.ComputeHash(bytes);
            }
        }

        /// <summary>
        /// Base64url no-padding encodes the given input buffer.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        private static string Base64UrlEncodeNoPadding(byte[] buffer)
        {
            string base64 = Convert.ToBase64String(buffer);

            // Converts base64 to base64url.
            base64 = base64.Replace("+", "-");
            base64 = base64.Replace("/", "_");
            // Strips padding.
            base64 = base64.Replace("=", "");

            return base64;
        }

        // Hack to bring the Console window to front.
        // ref: http://stackoverflow.com/a/12066376

        [DllImport("kernel32.dll", ExactSpelling = true)]
        public static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public void BringConsoleToFront()
        {
            SetForegroundWindow(GetConsoleWindow());
        }

    }
}
