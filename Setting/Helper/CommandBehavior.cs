using System.Windows;
using System.Windows.Input;
namespace Setting.Helper

{
    public static class CommandBehavior
    {
        public static readonly DependencyProperty MouseDownCommandProperty =
        DependencyProperty.RegisterAttached("MouseDownCommand", typeof(ICommand), typeof(CommandBehavior),
            new PropertyMetadata(MouseDownCommandChanged));

    public static void SetMouseDownCommand(DependencyObject d, ICommand value)
    {
        d.SetValue(MouseDownCommandProperty, value);
    }

    public static ICommand GetMouseDownCommand(DependencyObject d)
    {
        return (ICommand)d.GetValue(MouseDownCommandProperty);
    }

    private static void MouseDownCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is UIElement element)
        {
            if (e.OldValue != null)
            {
                element.RemoveHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(OnMouseDown));
            }
            if (e.NewValue != null)
            {
                element.AddHandler(UIElement.MouseDownEvent, new MouseButtonEventHandler(OnMouseDown));
            }
        }
    }

    private static void OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        var command = GetMouseDownCommand((DependencyObject)sender);
        if (command != null && command.CanExecute(e))
        {
            command.Execute(e);
        }
    }
}
}
