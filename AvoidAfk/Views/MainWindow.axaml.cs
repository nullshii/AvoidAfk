using System;
using Avalonia.Controls;
using Avalonia.Input;
using AvoidAfk.ViewModels;

namespace AvoidAfk.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void SliderPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (DataContext is not MainWindowViewModel viewModel) return;
        
        viewModel.Frequency += e.Delta.Y * 0.1;
        viewModel.Frequency = Math.Clamp(viewModel.Frequency, FrequencySlider.Minimum, FrequencySlider.Maximum);
    }
}