using System;
using System.Collections.Generic;
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

namespace MagicSquare {
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();

			this.Loaded += MainWindow_Loaded;
			ButtonReset.Click += ButtonReset_Click;
			ComboBoxSize.SelectionChanged += ComboBoxSize_SelectionChanged;

			ButtonHelp.Click += ButtonHelp_Click;
		}

		private Help windowHelp;

		private void ButtonHelp_Click(Object sender, RoutedEventArgs e) {
			windowHelp = new Help();

			windowHelp.Show();

			this.Closing += MainWindow_Closing;
		}

		private void MainWindow_Closing(Object sender, System.ComponentModel.CancelEventArgs e) {
			windowHelp?.Close();
		}

		private void ComboBoxSize_SelectionChanged(Object sender, SelectionChangedEventArgs e) {
			CreateMatrix();
			PaintSquare();

			TextBlockSumma.Text = "Сумма =";
			_count = 0;
		}

		private void ButtonReset_Click(Object sender, RoutedEventArgs e) {
			for (int i = 0; i < FieldSquare.GetLength(0); ++i) {
				for (int j = 0; j < FieldSquare.GetLength(1); ++j) {
					FieldSquare[i, j].ButtonSquare.IsEnabled = true;
					FieldSquare[i, j].IsPartOfSum = false;
					FieldSquare[i, j].Background = new SolidColorBrush(Colors.AliceBlue);
				}
			}

			TextBlockSumma.Text = "Сумма =";
			_count = 0;
		}

		private void MainWindow_Loaded(Object sender, RoutedEventArgs e) {

			TextBlockSumma.Text = "Сумма =";

			List<int> sizes = new List<int>(){4,5,6,7,8,9,10};

			ComboBoxSize.ItemsSource = sizes;
			ComboBoxSize.SelectedIndex = 0;

			//CreateMatrix();
			//PaintSquare();
		}

		private int _summValue;
		private int[,] _square;
		private int[] _row;
		private int[] _column;
		public ContorlSquare[,] FieldSquare;
		private int _count = 0;
		private IEnumerable<Log> _logSum = new List<Log>();


		private void CreateMatrix() {
			int size = int.Parse(ComboBoxSize.SelectedItem.ToString());

			_square = new int[size, size];

			Random random = new Random(DateTime.Now.Millisecond);

			_row = new int[size];
			_column = new int[size];
			_summValue = 0;

			for (int i = 0; i < size; ++i) {
				_row[i] = random.Next(1, size * 10);
			}

			for (int i = 0; i < size; ++i) {
				_column[i] = random.Next(1, size * 10);
			}

			for (int i = 0; i < _square.GetLength(0); ++i) {
				for (int j = 0; j < _square.GetLength(1); ++j) {
					_square[i, j] = _row[i] + _column[j];
				}
			}

			for (int i = 0; i < _row.Length; ++i)
				_summValue += _row[i];

			for (int j = 0; j < _row.Length; ++j)
				_summValue += _column[j];
		}

		private void PaintSquare() {
			FieldSquare = new ContorlSquare[_square.GetLength(0), _square.GetLength(1)];
			WrapPanelField.Children.Clear();
			
			double SizeX =  (WrapPanelField.ActualWidth / _square.GetLongLength(0));
			double SizeY =  (WrapPanelField.ActualHeight / _square.GetLongLength(1));


			for (int i = 0; i < FieldSquare.GetLength(0); ++i) {
				for (int j = 0; j < FieldSquare.GetLength(1); ++j) {
					ContorlSquare temp = new ContorlSquare(){Width = SizeX,Height = SizeY};
					temp.IJCoor = (i, j);
					temp.TextBlockTextButton.Text = _square[i, j].ToString();
					temp.ClickButton += Temp_ClickButton;
					WrapPanelField.Children.Add(temp);
					FieldSquare[i, j] = temp;
					temp.IsPartOfSum = false;
				}
			}
		}

		private void Temp_ClickButton(ContorlSquare senderObject) {
			senderObject.IsPartOfSum = true;

			for (int i = 0; i < FieldSquare.GetLength(0); ++i) {
				FieldSquare[i, senderObject.IJCoor.Item2].AnimationBackground();
				FieldSquare[i, senderObject.IJCoor.Item2].ButtonSquare.IsEnabled = false;
			}

			for (int j = 0; j < FieldSquare.GetLength(1); ++j) {
				FieldSquare[senderObject.IJCoor.Item1, j].AnimationBackground();
				FieldSquare[senderObject.IJCoor.Item1, j].ButtonSquare.IsEnabled = false;
			}

			_count++;

			PrintSum(senderObject.TextBlockTextButton.Text);

			if (_count == _square.GetLength(0)) {
				TextBlockSumma.Text += "=> " + _summValue.ToString();
				List<Log> temp = _logSum.ToList();
				temp.Add( new Log(){Record = TextBlockSumma.Text });
				_logSum = temp;
				ListViewLog.ItemsSource = _logSum;
			}
		}

		private void PrintSum(string value) {
			if (TextBlockSumma.Text.ToCharArray()[TextBlockSumma.Text.Length - 1] != '=') 
				TextBlockSumma.Text += " + ";
	
			TextBlockSumma.Text += " " + value.ToString();

		}
	}
}