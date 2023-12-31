﻿using System.Reactive;
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
        Key = "w";
        
        EnableCommand = ReactiveCommand.Create(Enable, this.WhenAnyValue(x => x.IsEnabled, b => !b));
        DisableCommand = ReactiveCommand.Create(Disable, this.WhenAnyValue(x => x.IsEnabled));
    }

    [Reactive] public double Frequency { get; set; }
    [Reactive] private bool IsEnabled { get; set; }
    [Reactive] public string Key { set; get; }

    public ReactiveCommand<Unit, Unit> EnableCommand { get; set; }
    public ReactiveCommand<Unit, Unit> DisableCommand { get; set; }

    private void Enable()
    {
        if (string.IsNullOrWhiteSpace(Key)) return;
        IsEnabled = true;
        Key = Key[0].ToString().ToLower();
        
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
        if (string.IsNullOrWhiteSpace(Key)) return;

        _robot.KeyDown(Key[0]);
        _robot.Delay(10);
        _robot.KeyUp(Key[0]);
    }
}