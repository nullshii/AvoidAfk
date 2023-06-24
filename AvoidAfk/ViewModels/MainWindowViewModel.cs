using System.Reactive;
using System.Timers;
using Desktop.Robot;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace AvoidAfk.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly Robot _robot;
    private readonly Timer _timer;

    public MainWindowViewModel()
    {
        _robot = new Robot();
        _timer = new Timer();
        _timer.Elapsed += EmulateKey;

        Frequency = 5f;
        IsEnabled = false;

        EnableCommand = ReactiveCommand.Create(Enable, this.WhenAnyValue(x => x.IsEnabled, b => !b));
        DisableCommand = ReactiveCommand.Create(Disable, this.WhenAnyValue(x => x.IsEnabled));
    }

    [Reactive] public float Frequency { get; set; }
    [Reactive] private bool IsEnabled { get; set; }

    public ReactiveCommand<Unit, Unit> EnableCommand { get; set; }
    public ReactiveCommand<Unit, Unit> DisableCommand { get; set; }

    private void Enable()
    {
        IsEnabled = true;
        _timer.Interval = Frequency * 1000;
        _timer.Start();
    }

    private void Disable()
    {
        IsEnabled = false;
        _timer.Stop();
    }

    private void EmulateKey(object? sender, ElapsedEventArgs elapsedEventArgs)
    {
        _robot.KeyDown('w');
        _robot.Delay(10);
        _robot.KeyUp('w');
    }
}