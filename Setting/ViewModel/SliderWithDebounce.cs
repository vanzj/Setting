using GalaSoft.MvvmLight.Messaging;
using Setting.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Setting.ViewModel
{
    public class SliderWithDebounce : Slider
    {
        private readonly DispatcherTimer _timer;
        private double _lastValue;

        public SliderWithDebounce()
        {
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000) // 设置防抖时间
            };
            _timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _timer.Stop();
            Console.WriteLine($"111111111111111111111111111111111{_lastValue}-{Value}");
            if (_lastValue != Value)
            {
                Messenger.Default.Send(new LumianceChangeEvent { });
                NewValueChanged?.Invoke(this, new RoutedPropertyChangedEventArgs<double>(_lastValue, Value));
            }
       
       
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!_timer.IsEnabled)
            {
                _lastValue = Value;
                _timer.Start();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!_timer.IsEnabled)
            {
                _lastValue = Value;
                _timer.Start();
            }
        }

        public new event RoutedPropertyChangedEventHandler<double> NewValueChanged;
    }
}
