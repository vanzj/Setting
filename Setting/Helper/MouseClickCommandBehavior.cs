using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Shapes;

namespace Setting.Helper
{
public class MouseClickCommandBehavior : Behavior<Rectangle>
{
    public static readonly DependencyProperty ClickCommandProperty =
        DependencyProperty.Register("ClickCommand", typeof(ICommand), typeof(MouseClickCommandBehavior));

    public ICommand ClickCommand
    {
        get => (ICommand)GetValue(ClickCommandProperty);
        set => SetValue(ClickCommandProperty, value);
    }

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.MouseLeftButtonDown -= Rectangle_MouseLeftButtonDown;
    }

    private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (ClickCommand != null && ClickCommand.CanExecute(null))
        {
            ClickCommand.Execute(null);
        }
    }
}
}
