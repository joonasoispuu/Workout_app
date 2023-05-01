namespace WorkoutApp.Views;

public partial class customexercisePage : ContentPage
{
    private readonly ExerciseViewModel _exerciseViewModel;

    public customexercisePage()
	{
		InitializeComponent();
        _exerciseViewModel = new ExerciseViewModel();
        BindingContext = this;
    }

    private async void BeginCustomExercises_Clicked(object sender, EventArgs e)
    {
        if (_exerciseViewModel.GetCustomExercises().Count == 0)
        {
            await DisplayAlert("Error", "You havent added any custom exercises yet!", "OK");
        }
        else
        {
            await Navigation.PushAsync(new customexerciseTrainPage());
        }
    }

    private async void AddCustomExercises_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new customexercisesAddPage());
    }
}