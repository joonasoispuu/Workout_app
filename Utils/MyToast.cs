using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutApp.Utils
{
    public class MyToast
    {
        string text;
        double fontSize;
        ToastDuration duration;

        public MyToast(string _text, double _fontSize = 14) {
            text = _text;
            fontSize = _fontSize;
            duration = ToastDuration.Short;
        }

        public async void Display()
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            var toast = Toast.Make(text, duration, fontSize);
            await toast.Show(cancellationTokenSource.Token);
        }
    }
}
