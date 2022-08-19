using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WpfApp1.Model;

namespace WpfApp1.Pagination
{
    public class Paging
    {
        public int PageIndex { get; set; }

        DataTable PagedList = new DataTable(); //Initialize a DataTable Locally

        public DataTable Next(IList<Movie> ListToPage, int RecordsPerPage)
        {
            PageIndex++;
            if (PageIndex >= ListToPage.Count / RecordsPerPage)
            {
                PageIndex = ListToPage.Count / RecordsPerPage;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable Previous(IList<Movie> ListToPage, int RecordsPerPage)
        {
            PageIndex--;
            if (PageIndex <= 0)
            {
                PageIndex = 0;
            }
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable First(IList<Movie> ListToPage, int RecordsPerPage)
        {
            PageIndex = 0;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable Last(IList<Movie> ListToPage, int RecordsPerPage)
        {
            PageIndex = ListToPage.Count / RecordsPerPage;
            PagedList = SetPaging(ListToPage, RecordsPerPage);
            return PagedList;
        }

        public DataTable SetPaging(IList<Movie> ListToPage, int RecordsPerPage)
        {
            int PageGroup = PageIndex * RecordsPerPage;

            IList<Movie> PagedList = new List<Movie>();

            PagedList = ListToPage.Skip(PageGroup).Take(RecordsPerPage).ToList(); //This is where the Magic Happens. If you have a Specific sort or want to return ONLY a specific set of columns, add it to this LINQ Query.

            DataTable FinalPaging = PagedTable(PagedList);

            return FinalPaging;
        }

        private DataTable PagedTable<T>(IList<T> SourceList)
        {
            Type columnType = typeof(T);
            DataTable TableToReturn = new DataTable();

            foreach (var Column in columnType.GetProperties())
            {
                TableToReturn.Columns.Add(Column.Name, Column.PropertyType);
            }

            foreach (object item in SourceList)
            {
                DataRow ReturnTableRow = TableToReturn.NewRow();
                foreach (var Column in columnType.GetProperties())
                {
                    ReturnTableRow[Column.Name] = Column.GetValue(item);
                }
                TableToReturn.Rows.Add(ReturnTableRow);
            }
            return TableToReturn;
        }
    }
}
