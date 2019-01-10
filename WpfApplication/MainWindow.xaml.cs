namespace WpfApplication
{
    using System.Windows;
    using Core;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SystemUnderTestTaskSchedulerTimer sut;

        public MainWindow()
        {
            InitializeComponent();

            // Starts recurring operation when the main window is initialized.
            sut = new SystemUnderTestTaskSchedulerTimer(new DispatcherTimerAdapter());
            sut.StartRecurring();
        }

        /// <summary>
        /// Displays the results of the recurring operation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            RecurringResult.Text = sut.Message;
        }
    }
}
