namespace WorkoutApp.Views
{
    public partial class customexercisesAddPage : ContentPage
    {
        private readonly ExerciseViewModel _exerciseViewModel;

        public customexercisesAddPage()
        {
            InitializeComponent();
            _exerciseViewModel = new ExerciseViewModel();
        }

        private async void OnNewExercise_Clicked(object sender, EventArgs e)
        {

            //Checks if ChooseExerciseName.Text is empty
            if (string.IsNullOrWhiteSpace(ChooseExerciseName.Text))
            {
                await DisplayAlert("Error", "Please give your exercise a name", "OK");
                return;
            }

            //Checks if ChooseExerciseReps.Text or ChooseExerciseTimer.Text in empty
            if (string.IsNullOrWhiteSpace(ChooseExerciseReps.Text) && string.IsNullOrWhiteSpace(ChooseExerciseTimer.Text))
            {
                await DisplayAlert("Error", "Please enter either how long or how many times you want to do an exercise ", "OK");
                return;
            }

            int reps = 0;
            //Checks if ChooseExerciseReps.Text is empty and also if the value is a number
            if (!string.IsNullOrWhiteSpace(ChooseExerciseReps.Text) && !int.TryParse(ChooseExerciseReps.Text, out reps))
            {
                await DisplayAlert("Error", "Please only enter numbers in the reps field ", "OK");
                return;
            }

            int seconds = 0;
            //Checks if ChooseExerciseTimer.Text is empty and also if the value is a number
            if (!string.IsNullOrWhiteSpace(ChooseExerciseTimer.Text) && !int.TryParse(ChooseExerciseTimer.Text, out seconds))
            {
                await DisplayAlert("Error", "Please only enter numbers in the timer field ", "OK");
                return;
            }

            var exercise = new ExerciseModel
            {
                Name = ChooseExerciseName.Text,
                Reps = reps,
                Seconds = seconds,
                Category = "Custom",
                Completed = false
            };

            _exerciseViewModel.AddCustomExercise(exercise);

            await DisplayAlert("Exercise Added!", "The exercise has succesfully been added to the custom exercises list", "OK");
        }
    }
}