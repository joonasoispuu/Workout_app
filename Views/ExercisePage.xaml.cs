using System.Windows.Input;

namespace WorkoutApp.Views;

public partial class ExercisePage : ContentPage
{
	public ExercisePage()
	{
		InitializeComponent();
	}

    public ICommand NavigateToPage1Command => new Command(async () => await NavigateToPage(new armsandchestPage()));
    public ICommand NavigateToPage2Command => new Command(async () => await NavigateToPage(new absworkoutPage()));

    private async Task NavigateToPage(Page page)
    {
        await Navigation.PushAsync(page);
    }
}