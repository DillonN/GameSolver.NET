using GameSolver.NET.Hosts.Benchmarking;
using System;
using Xamarin.Forms;

namespace GS.NET.X
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            ResultsLabel.Text = "blarg";
            Console.WriteLine("blarg");
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
