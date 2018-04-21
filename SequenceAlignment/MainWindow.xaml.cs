using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SequenceAlignment.Algorithms;



namespace SequenceAlignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       private MatrixToDataViewConverter matrixToDataViewConverter = new MatrixToDataViewConverter();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = matrixToDataViewConverter;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if(cbAlgorithm.SelectedItem == Needleman)
            {
                this.DataContext = null;
                NeedlemanWunsch nW = new NeedlemanWunsch();
                createMatrix(nW);
                traceBack(nW);
            }
        }

        private void traceBack(NeedlemanWunsch nW)
        {
            String result1 = "";
            String result2 = "";
            nW.TraceBack(out result1, out result2);
            tbXAligned.Text = result1;
            tbYAligned.Text = result2;
        }

        private void createMatrix(NeedlemanWunsch nW)
        {
            nW.sequenceX = tbX.Text;
            nW.sequenceY = tbY.Text;
            int[,] matrix = nW.CreateMatrix();
            setDataContext(matrix);
        }

        private void setDataContext(int[,] matrix)
        {
            matrixToDataViewConverter.ColumnHeaders = tbX.Text.ToCharArray();
            matrixToDataViewConverter.RowHeaders = tbY.Text.ToCharArray();
            matrixToDataViewConverter.Values = matrix;
            this.DataContext = matrixToDataViewConverter;
        }
    }

    public class MatrixToDataViewConverter : IMultiValueConverter
    {
        public int[,] Values { get; set; }

        public char[] RowHeaders { get; set; }

        public char[] ColumnHeaders { get; set; }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var myDataTable = new DataTable();
                var columns = values[0] as char[];
                var rows = values[1] as char[];
                var rowsNew = new char[rows.Length + 1];
                var vals = values[2] as int[,];

                // Build Columns
                rowsNew[0] = ' ';
                rows.CopyTo(rowsNew, 1);
                iniColumns(myDataTable, vals);


                var tmp = new string[2 + columns.Length];

                // x-sequence on the top
                for (int i = 0; i < columns.Length; i++)
                {
                    tmp[i + 2] = columns[i].ToString();
                }
                myDataTable.Rows.Add(tmp);
                int index = 0;

                // filling the matrix
                foreach (char row in rowsNew)
                {
                    tmp = new string[2 + columns.Length];
                    tmp[0] = row.ToString();
                    

                    for (int secondIndex = 0; secondIndex < columns.Length+1; secondIndex++)
                    {
                         tmp[secondIndex + 1] = vals[secondIndex, index].ToString();
                    }
                    myDataTable.Rows.Add(tmp);
                    index++;
                }
                return myDataTable.DefaultView;
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }
            return null;
        }

        private void iniColumns(DataTable myDataTable, int[,] vals)
        {
            myDataTable.Columns.Add("C0");
            myDataTable.Columns.Add("C1");

            for (int x = 0; x < vals.GetLength(0)-1; x++)
            {
                myDataTable.Columns.Add("C" + (x + 2));
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
