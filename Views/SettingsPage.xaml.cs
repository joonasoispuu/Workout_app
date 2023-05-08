namespace WorkoutApp.Views;

public partial class SettingsPage : ContentPage
{
    private readonly ExerciseViewModel _exerciseViewModel;

    public SettingsPage()
	{
		InitializeComponent();
        _exerciseViewModel = new ExerciseViewModel();
    }

    private async void DeleteCustomExercises_Clicked(object sender, EventArgs e)
    {
        _exerciseViewModel.DeleteCustomExercises();
        //Preferences.Default.Clear();
        await DisplayAlert("Done", "You have deleted all your custom exercise list!", "OK");
    }
}