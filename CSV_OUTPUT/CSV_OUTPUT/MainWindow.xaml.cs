using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
using Microsoft.VisualBasic.FileIO;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;

namespace CSV_OUTPUT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _filePath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

           
        }

        #region Event´s

        /// <summary>
        /// Sets the file path to the file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Select_File(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "C:\\",
                RestoreDirectory = true,
                Filter = @"CSV Files (*.csv)|*.csv"
            };

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OutputFileName.Text = Path.GetFileName(openFileDialog.FileName);
                _filePath = openFileDialog.FileName;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Table_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void AllEditable_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void FontSizeTable_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the whole prozess
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load_Click(object sender, RoutedEventArgs e) => Start();

        /// <summary>
        /// Shows help
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Help_Click(object sender, RoutedEventArgs e)
        {
            Output("First you have to select a File         ==> File > Select File\nSecond determain the delimeter    ==> Delimeter input\nThird check if you want 'First row contains field names'\nPress Load");
        }
        #endregion

        /// <summary>
        /// Starts the whole prozess 
        /// </summary>
        private void Start()
        {
            try
            {
                if (DelimetersInput.Text.Equals(string.Empty))
                    throw new Exception("You have to enter a delimeter!");
                else if (_filePath.Equals(string.Empty))
                    throw new Exception("You have to select a file!");
                else
                    DataTable.DataContext = NewDataTable(_filePath, DelimetersInput.Text, Convert.ToBoolean(FirstRowContainsFieldNamesCheckBox.IsChecked)).DefaultView;
            }
            catch (Exception e)
            {
                Output(e.Message, "Error!", MessageBoxImage.Error);
            }

        }

        #region Table

        /// <summary>
        /// Generates a new DataTable for the DataGrid from a file
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <param name="delimiters">delimiters</param>
        /// <param name="firstRowContainsFieldNames">firstRowContainsFieldNames</param>
        /// <returns>A new DataTable for the Datagrid</returns>
        public DataTable NewDataTable(string filePath, string delimiters, bool firstRowContainsFieldNames = true)
        {
            DataTable result = new DataTable();

            using (TextFieldParser tfp = new TextFieldParser(filePath))
            {
                tfp.SetDelimiters(delimiters);

                // Get Some Column Names
                if (!tfp.EndOfData)
                {
                    string[] fields = tfp.ReadFields();
                    for (int i = 0; i < fields.Count(); i++)
                    {
                        if (firstRowContainsFieldNames)
                            result.Columns.Add(fields[i]);
                        else
                            result.Columns.Add("Col " + i);
                    }

                    // If first line is data then add it
                    if (!firstRowContainsFieldNames)
                        result.Rows.Add(fields);
                }

                // Get Remaining Rows
                while (!tfp.EndOfData)
                    result.Rows.Add(tfp.ReadFields());
            }

            return result;
        }
        #endregion

        #region Output

        /// <summary>
        /// Simple logging output function
        /// </summary>
        /// <param name="msg">Message to display</param>
        private void Log(string msg) => Output("[" +DateTime.Now + "] " + msg, "Log");

        /// <summary>
        /// Simple output function
        /// </summary>
        /// <param name="msg">Message to display</param>
        /// <param name="caption">Caption to display</param>
        /// <param name="btn">Button to press</param>
        /// <param name="icon">Icon to display</param>
        private void Output(string msg, string caption ="Info", MessageBoxImage icon = MessageBoxImage.Asterisk, MessageBoxButton btn = MessageBoxButton.OK) => MessageBox.Show(msg,caption, btn, icon);


        #endregion

     
    }
}
