using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace iPlantMLM
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MyLoading : Window
    {
        public MyLoading()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                RotateTransform wRotateTransform = new RotateTransform();
                Ellipse_Load.RenderTransform = wRotateTransform;
                wRotateTransform.Angle = 0;

                DoubleAnimation wBorderAnimation = new DoubleAnimation();
                wBorderAnimation.From = 0;
                wBorderAnimation.To = 360;
                wBorderAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
                wBorderAnimation.RepeatBehavior = RepeatBehavior.Forever;

                Storyboard.SetTarget(wBorderAnimation, Ellipse_Load);
                Storyboard.SetTargetProperty(wBorderAnimation, new PropertyPath("RenderTransform.Angle"));

                Storyboard wStoryboard_Image = new Storyboard();
                wStoryboard_Image.Children.Add(wBorderAnimation);
                wStoryboard_Image.Begin();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
