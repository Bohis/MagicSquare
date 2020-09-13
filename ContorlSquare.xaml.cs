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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MagicSquare {
	/// <summary>
	/// Логика взаимодействия для ContorlSquare.xaml
	/// </summary>
	public partial class ContorlSquare : UserControl {
		public delegate void baseDelegate(ContorlSquare senderObject);
		public  event baseDelegate ClickButton;

		public ContorlSquare() {
			InitializeComponent();

			ButtonSquare.Click += ButtonSquare_Click;
		}

		public void AnimationBackground() {
			this.Background = new SolidColorBrush(Colors.AliceBlue);

			ColorAnimation ControlAnimation = new ColorAnimation();
			ControlAnimation.From = Colors.White;
			ControlAnimation.To = Colors.GreenYellow;
			ControlAnimation.Duration = TimeSpan.FromSeconds(1);
			this.Background.BeginAnimation(SolidColorBrush.ColorProperty, ControlAnimation);

			ControlAnimation = new ColorAnimation();
			ControlAnimation.From = Colors.GreenYellow;
			ControlAnimation.To = Colors.DarkGray;
			ControlAnimation.Duration = TimeSpan.FromSeconds(1);
			this.Background.BeginAnimation(SolidColorBrush.ColorProperty, ControlAnimation);
		}

		private void ButtonSquare_Click(Object sender, RoutedEventArgs e) {
			ClickButton?.Invoke(this);
		} 

		public (int, int) IJCoor { get; set; }
		public bool IsPartOfSum { get; set; }
	}
}
