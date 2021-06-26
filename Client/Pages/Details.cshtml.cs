using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Client.Data;
using System.Data;
using FusionCharts.DataEngine;
using FusionCharts.Visualization;

namespace Client.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public string ChartJson { get; internal set; }


        public DetailsModel(Client.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Client.Data.Controller controller { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            controller = await _context.Controllers.FirstOrDefaultAsync(m => m.Id == id);

            string[] values = controller.Values;

            if (values.Length == 0)
                return Page();

            int start_point = 0;

            // create data table to store data
            DataTable ChartData = new DataTable();
            // Add columns to data table

            if (controller.Type == "humidity")
            {
                ChartData.Columns.Add("time", typeof(System.String));
                ChartData.Columns.Add("mm/pt", typeof(System.Double));
                foreach (var item in values)
                {
                    if (item != "")
                    {
                        ChartData.Rows.Add(start_point, item.Split('|')[1]);
                        start_point += 10;
                    }
                }
            }
            else if (controller.Type == "temperature")
            {
                ChartData.Columns.Add("time", typeof(System.String));
                ChartData.Columns.Add("Degrees", typeof(System.Double));
                foreach (var item in values)
                {
                    double dd = 0;
                    if (item != "")
                    {
                        ChartData.Rows.Add(start_point, item.Split('|')[1]);
                        start_point += 10;
                    }
                }
            }
            else if (controller.Type == "pressure")
            {
                ChartData.Columns.Add("time", typeof(System.String));
                ChartData.Columns.Add("bar", typeof(System.Double));
                foreach (var item in values)
                {
                    //DateTime dt = DateTime.Parse(item.Split('|')[0]);
                    ChartData.Rows.Add(start_point, item.Split('|')[3]);
                    start_point += 10;
                }
                start_point = 0;
            }
            else if (controller.Type == "movement")
            {
                ChartData.Columns.Add("time", typeof(System.String));
                ChartData.Columns.Add("actions", typeof(System.Double));
                foreach (var item in values)
                {
                    if (item != "")
                    {
                        //DateTime dt = DateTime.Parse(item.Split('|')[0]);
                        int value = 0;
                        if (item.Split('|')[1] == "true")
                            value = 1;
                        ChartData.Rows.Add(start_point, value);
                        start_point += 10;
                    }

                }
            }
            else if (controller.Type == "lightning")
            {
                ChartData.Columns.Add("time", typeof(System.String));
                ChartData.Columns.Add("lux", typeof(System.Double));
                foreach (var item in values)
                {
                    if (item != "")
                    {
                        //DateTime dt = DateTime.Parse(item.Split('|')[0]);
                        ChartData.Rows.Add(start_point, item.Split('|')[1]);
                        start_point += 10;
                    }
                }
            }
            else
            {
                ChartData.Columns.Add("sample", typeof(System.String));
                ChartData.Columns.Add("sample2", typeof(System.Double));
            }
            // Add rows to data table



            //ChartData.Rows.Add("Java", 62000);
            //ChartData.Rows.Add("Python", 46000);
            //ChartData.Rows.Add("Javascript", 38000);
            //ChartData.Rows.Add("C++", 31000);
            //ChartData.Rows.Add("C#", 27000);
            //ChartData.Rows.Add("PHP", 14000);
            //ChartData.Rows.Add("Perl", 14000);

            // Create static source with this data table
            StaticSource source = new StaticSource(ChartData);
            // Create instance of DataModel class
            DataModel model = new DataModel();
            // Add DataSource to the DataModel
            model.DataSources.Add(source);
            // Instantiate Column Chart
            Charts.ColumnChart column = new Charts.ColumnChart("first_chart");
            // Set Chart's width and height
            column.Width.Pixel(700);
            column.Height.Pixel(400);
            // Set DataModel instance as the data source of the chart
            column.Data.Source = model;
            // Set Chart Title
            column.Caption.Text = controller.Type;
            // Set chart sub title
            // hide chart Legend
            column.Legend.Show = false;
            // set XAxis Text
            column.XAxis.Text = "Time";
            // Set YAxis title
            column.YAxis.Text = "Value";
            // set chart theme
            column.ThemeName = FusionChartsTheme.ThemeName.FUSION;
            // set chart rendering json
            ChartJson = column.Render();

            if (controller == null)
            {
                return NotFound();
            }
            return Page();
        }


        //// create a public property. OnGet method() set the chart configuration json in this property.
        //// When the page is being loaded, OnGet method will be  invoked
        //public void OnGet()
        //{

        //    // create data table to store data
        //    DataTable ChartData = new DataTable();
        //    // Add columns to data table
        //    ChartData.Columns.Add("Programming Language", typeof(System.String));
        //    ChartData.Columns.Add("Users", typeof(System.Double));
        //    // Add rows to data table

        //    ChartData.Rows.Add("Java", 62000);
        //    ChartData.Rows.Add("Python", 46000);
        //    ChartData.Rows.Add("Javascript", 38000);
        //    ChartData.Rows.Add("C++", 31000);
        //    ChartData.Rows.Add("C#", 27000);
        //    ChartData.Rows.Add("PHP", 14000);
        //    ChartData.Rows.Add("Perl", 14000);

        //    // Create static source with this data table
        //    StaticSource source = new StaticSource(ChartData);
        //    // Create instance of DataModel class
        //    DataModel model = new DataModel();
        //    // Add DataSource to the DataModel
        //    model.DataSources.Add(source);
        //    // Instantiate Column Chart
        //    Charts.ColumnChart column = new Charts.ColumnChart("first_chart");
        //    // Set Chart's width and height
        //    column.Width.Pixel(700);
        //    column.Height.Pixel(400);
        //    // Set DataModel instance as the data source of the chart
        //    column.Data.Source = model;
        //    // Set Chart Title
        //    column.Caption.Text = "Most popular programming language";
        //    // Set chart sub title
        //    column.SubCaption.Text = "2017-2018";
        //    // hide chart Legend
        //    column.Legend.Show = false;
        //    // set XAxis Text
        //    column.XAxis.Text = "Programming Language";
        //    // Set YAxis title
        //    column.YAxis.Text = "User";
        //    // set chart theme
        //    column.ThemeName = FusionChartsTheme.ThemeName.FUSION;
        //    // set chart rendering json
        //    ChartJson = column.Render();
        //}
    }
}



//namespace Client.Pages
//{
//    public class ChartModel : PageModel
//    {
//        // create a public property. OnGet method() set the chart configuration json in this property.
//        // When the page is being loaded, OnGet method will be  invoked
//        public string ChartJson { get; internal set; }
//        public void OnGet()
//        {

//            // create data table to store data
//            DataTable ChartData = new DataTable();
//            // Add columns to data table
//            ChartData.Columns.Add("Programming Language", typeof(System.String));
//            ChartData.Columns.Add("Users", typeof(System.Double));
//            // Add rows to data table

//            ChartData.Rows.Add("Java", 62000);
//            ChartData.Rows.Add("Python", 46000);
//            ChartData.Rows.Add("Javascript", 38000);
//            ChartData.Rows.Add("C++", 31000);
//            ChartData.Rows.Add("C#", 27000);
//            ChartData.Rows.Add("PHP", 14000);
//            ChartData.Rows.Add("Perl", 14000);

//            // Create static source with this data table
//            StaticSource source = new StaticSource(ChartData);
//            // Create instance of DataModel class
//            DataModel model = new DataModel();
//            // Add DataSource to the DataModel
//            model.DataSources.Add(source);
//            // Instantiate Column Chart
//            Charts.ColumnChart column = new Charts.ColumnChart("first_chart");
//            // Set Chart's width and height
//            column.Width.Pixel(700);
//            column.Height.Pixel(400);
//            // Set DataModel instance as the data source of the chart
//            column.Data.Source = model;
//            // Set Chart Title
//            column.Caption.Text = "Most popular programming language";
//            // Set chart sub title
//            column.SubCaption.Text = "2017-2018";
//            // hide chart Legend
//            column.Legend.Show = false;
//            // set XAxis Text
//            column.XAxis.Text = "Programming Language";
//            // Set YAxis title
//            column.YAxis.Text = "User";
//            // set chart theme
//            column.ThemeName = FusionChartsTheme.ThemeName.FUSION;
//            // set chart rendering json
//            ChartJson = column.Render();
//        }
//    }
//}
