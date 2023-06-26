namespace WpfTestApplication;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    private Brush _buttonBackground;

    public MainWindow()
    {
        _buttonBackground = Brushes.Chocolate;
        ButtonCommand = new ButtonCommand(() => { MessageBox.Show("Hello There!");
            ButtonBackground = Brushes.Aqua;
        });
        InitializeComponent();
    }

    public Brush ButtonBackground
    {
        get => _buttonBackground;
        private set
        {
            _buttonBackground = value;
            OnPropertyChanged();
        }
    }

    public ICommand ButtonCommand { get; }
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class ButtonCommand : ICommand
{
    private readonly Action _action;

    public ButtonCommand(Action action)
    {
        _action = action;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        _action();
    }

    public event EventHandler? CanExecuteChanged;
}