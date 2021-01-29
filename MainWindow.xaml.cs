using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;



namespace Sociedade_Serial
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Creates first tab
            addTab(TestsTabs.Items[0], new RoutedEventArgs());

            
           // Change title buttons 
           this.StateChanged += (object sender, EventArgs e) =>
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.RestoreButtom.Visibility = Visibility.Visible;
                    this.MaximizeButtom.Visibility = Visibility.Collapsed;
                }
                else if (this.WindowState.Equals(WindowState.Normal))
                {
                    this.RestoreButtom.Visibility = Visibility.Collapsed;
                    this.MaximizeButtom.Visibility = Visibility.Visible;
                }
            };
            
        }

        /// <summary>
        /// Add a new tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addTab(object sender, RoutedEventArgs e)
        {
               
            
            TabItem Curr = (TabItem)sender;
            Curr.GotFocus -= addTab;
            Curr.Content = new Test.GUI();

            // Header
            StackPanel Header = new StackPanel();
            TextBox Name = new TextBox();
            Button CloseButton = new Button();

            Name.Text = "Nova Aba";
            Name.Style = this.FindResource("TabTextBox") as Style;

            CloseButton.Click += TabCloseButton_Click;
            CloseButton.Style = this.FindResource("CloseButtonTab") as Style;


            //The two objects have the same name to univocal identifier the link
            CloseButton.Name = $"N{Curr.GetHashCode()}{DateTime.Now.ToString("ffffff")}";
            Curr.Name = CloseButton.Name;
            Curr.GotFocus += Focus;
            Curr.LostFocus += Focus;

            // Creates a horizontal stack with tha tab name and a close button
            Header.Orientation = Orientation.Horizontal;
            Header.Children.Add(Name);
            Header.Children.Add(CloseButton);
            Curr.Header = Header;
            Curr.Width = double.NaN;
            

            // Creates the plus button
            TabItem newTab = new TabItem();
            newTab.GotFocus += addTab;
            newTab.Name = "PlusButtom";

            Focus(Curr, new RoutedEventArgs());

            this.TestsTabs.Items.Add(newTab);

            if (TestsTabs.Items.Count > 4)
            {
                ((TabItem)TestsTabs.Items[4]).IsEnabled = false;
            }
        }

        /// <summary>
        /// Block the textbox of a unselected tab item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Focus(object sender, RoutedEventArgs e)
        {
            
            for (int i =0; i<TestsTabs.Items.Count-1;i++)
                ((StackPanel)((TabItem)TestsTabs.Items[i]).Header)
                    .Children[0].IsEnabled = ((TabItem)TestsTabs.Items[i]).IsSelected;
        }

        /// <summary>
        /// Deletes the tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabCloseButton_Click(object sender, RoutedEventArgs e)
        {

            TabItem last;
            if (TestsTabs.Items.Count == 2)
                return;
            else
                last = (TabItem)TestsTabs.Items[TestsTabs.Items.Count - 2];

            foreach (TabItem tb in TestsTabs.Items)
                if (tb.Name == ((Button)sender).Name)
                {
                    if (tb.Equals(last) && last.IsSelected)
                    {
                        ((TabItem)TestsTabs.Items[TestsTabs.Items.Count - 3]).IsSelected = true;
                    }
                    TestsTabs.Items.Remove(tb);
                    break;
                }

           ((TabItem)TestsTabs.Items[TestsTabs.Items.Count-1]).IsEnabled = true;

        }

        private void MinimizeButtom_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void RestoreButtom_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Normal;
        }

        private void MaximizeButtom_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }

        private void CloseButtom_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Theme_Change(object sender, RoutedEventArgs e)
        {
            App.Skin = ((ToggleButton)sender).IsChecked == true ?Skin.Dark : Skin.Default;
        }
    }
}
