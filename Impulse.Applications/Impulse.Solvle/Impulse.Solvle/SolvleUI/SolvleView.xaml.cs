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

namespace Impulse.Solvle.SolvleUI;
/// <summary>
/// Interaction logic for SolvleView.xaml
/// </summary>
public partial class SolvleView : UserControl
{
    private SolvleViewModel ViewModel => (SolvleViewModel)this.DataContext;

    public SolvleView()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.MakeGuess();
    }

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        //undo
    }

    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        //reset
    }
}
