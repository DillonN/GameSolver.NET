using GameSolver.NET.Hosts.Benchmarking;
using System;
using Xamarin.Forms;

namespace GS.NET.X
{
    public partial class MainPage : ContentPage
    {
        /// <summary>
        /// Host for smartphone testing
        /// Class can be referenced by an iOS application
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            // Label will show results in app
            ResultsLabel.Text = "";
            if (true)
            {
                foreach (var x in Benchmark.Mixed(5, 100))
                {
                    ResultsLabel.Text += "\n" + x;
                    Console.WriteLine(x);
                }
            }
        }
    }
}
