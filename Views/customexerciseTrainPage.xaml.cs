namespace WorkoutApp.Views
{
    public partial class customexerciseTrainPage : ContentPage
    {
        private readonly ExerciseDbContext _dbContext;
        public ExerciseViewModel ViewModel { get; set; }
        private ExerciseModel _currentExercise;
        private int _exerciseCount;
        private int _totalExercises;
        private System.Timers.Timer _timer;

        public customexerciseTrainPage()
        {
            InitializeComponent();

            _dbContext = new ExerciseDbContext();

            _dbContext.Database.EnsureCreated();

            var exercises = _dbContext.Exercises.ToList();
            foreach (var exercise in exercises)
            {
                exercise.Completed = false;
                _dbContext.SaveChanges();
            }


            _currentExercise = _dbContext.Exercises.FirstOrDefault(x => x.Category == "Custom" && !x.Completed);

            _totalExercises = _dbContext.Exercises.Count(x => x.Category == "Custom");
            _exerciseCount = 1;

            UpdateExerciseDisplay();
        }

        private void UpdateExerciseDisplay()
        {
            ExerciseNameLabel.Text = $"Exercise: {_currentExercise.Name}";

            if (_currentExercise.Reps != 0)
            {
                ExerciseRepsLabel.Text = $"Reps: {_currentExercise.Reps}";
            }
            else
            {
                ExerciseRepsLabel.Text = "";
            }
            ExerciseCountLabel.Text = $"Exercise {_exerciseCount} of {_totalExercises}";

            if (_currentExercise.Seconds != 0)
            {
                ExerciseTimerLabel.Text = $"Time left: {_currentExercise.Seconds}s";
                var timerValue = _currentExercise.Seconds;
                var timer = new System.Timers.Timer(1000);
                timer.Elapsed += (sender, e) =>
                {
                    timerValue--;
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        ExerciseTimerLabel.Text = $"Time left: {timerValue}s";
                    });

                    if (timerValue == 0)
                    {
                        timer.Stop();
                        MainThread.InvokeOnMainThreadAsync(() => DoneButton_Clicked(null, null));
                    }
                };
                timer.Start();
                _timer = timer;
            }
            else
            {
                ExerciseTimerLabel.Text = "";
            }
        }


        private async void DoneButton_Clicked(object sender, EventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();
            }

            _currentExercise.Completed = true;
            _dbContext.SaveChanges();

            if (_exerciseCount < _totalExercises)
            {
                _currentExercise = _dbContext.Exercises.FirstOrDefault(x => x.Category == "Custom" && !x.Completed);
                _exerciseCount++;

                UpdateExerciseDisplay();
            }
            else
            {
                await DisplayAlert("Workout Complete", "Congratulations, you have completed your custom exercises!", "OK");

                var exercises = _dbContext.Exercises.ToList();
                foreach (var exercise in exercises)
                {
                    exercise.Completed = false;
                }
                _dbContext.SaveChanges();
                await Navigation.PopAsync();
            }
        }
    }
}