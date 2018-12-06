using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameSolver.NET.Benchmarking;
using Xamarin.Forms;

namespace GS.NET.X
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            ResultsLabel.Text = "";
            if (true)
            {
                foreach (var x in Benchmark.Pure())
                {
                    ResultsLabel.Text += "\n" + x;
                }
            }
        }
    }
}
