using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _04_periode3_opdracht1_microwave
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Microwave microwave = new Microwave();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this.microwave;
        }

        /// <summary>
        /// Handles opening/closing of the microwave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenClose_OnClick(object sender, RoutedEventArgs e)
        {
            // Open microwave
            if (microwave.MicrowaveDoor == Constants.Door.Closed)
            {
                this.microwave.MicrowaveDoor = Constants.Door.Open;
                this.microwave.Microwaving = Constants.Microwaving.No;
                this.microwave.MicrowaveLight = Constants.Light.Off;
            }
            // Close microwave
            else if (microwave.MicrowaveDoor == Constants.Door.Open)
            {
                this.microwave.MicrowaveDoor = Constants.Door.Closed;
                // Don't update light/microwaving, since microwave needs to be started separately
            }
        }

        /// <summary>
        /// Handles Starting / Stopping of the microwave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartStop_OnClick(object sender, RoutedEventArgs e)
        {
            // make sure the time is set before 0
            // If microwave.Time is > 0

            // Start Microwave
            if (microwave.MicrowaveDoor == Constants.Door.Closed 
                && microwave.Microwaving == Constants.Microwaving.No)
            {
                this.microwave.Microwaving = Constants.Microwaving.Yes;
                this.microwave.MicrowaveLight = Constants.Light.On;
            }
            // Stop microwave
            else if (microwave.Microwaving == Constants.Microwaving.Yes)
            {
                this.microwave.Microwaving = Constants.Microwaving.No;
                this.microwave.MicrowaveLight = Constants.Light.Off;
            }
        }

        /// <summary>
        /// Handles timer change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Time_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            // TODO implement this
            // TODO split functionality of the buttons in methods for re-usability. (stopping microwave, etc.)
          
            // If value is changed && this.microwave.Microwaving == Constants.Microwaving.Yes // turn microwave and timer off
            
            throw new NotImplementedException();
        }
    }
}