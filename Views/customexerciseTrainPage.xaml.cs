namespace WorkoutApp.Views
{
    public partial class CustomexerciseTrainPage : ContentPage
    {
        private readonly ExerciseDbContext _dbContext;
        public ExerciseViewModel ViewModel { get; set; }
        private ExerciseModel _currentExercise;
        private int _exerciseCount;
        private int _totalExercises;
        private System.Timers.Timer _timer;

        public CustomexerciseTrainPage()
        {
            InitializeComponent();

            _dbContext = new ExerciseDbContext();

            _dbContext.Database.EnsureCreated();

            GetReadyText.IsVisible = false;
            NextExerciseText.IsVisible = false;
            ExerciseBreakTImer.IsVisible = false;
            NextExerciseName.IsVisible = false;
            NextExerciseSeconds.IsVisible = false;
            NextExerciseReps.IsVisible = false;

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
            ExerciseNameLabel.Text = $"{_currentExercise.Name}";

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

                ExerciseTimerLabel.IsVisible = false;
                ExerciseNameLabel.IsVisible = false;
                ExerciseRepsLabel.IsVisible = false;
                ExerciseCountLabel.IsVisible = false;
                ExerciseText.IsVisible = false;
                DoneButton.IsVisible = false;

                int breaktimer = 5;

                while (breaktimer >= 1)
                {
                    GetReadyText.IsVisible = true;
                    NextExerciseText.IsVisible = true;
                    ExerciseBreakTImer.IsVisible = true;
                    NextExerciseName.IsVisible = true;

                    if (_currentExercise.Reps != 0)
                    {
                        NextExerciseReps.IsVisible = true;
                        NextExerciseReps.Text = $"Reps: {_currentExercise.Reps}";
                    }

                    if (_currentExercise.Seconds != 0)
                    {
                        NextExerciseSeconds.IsVisible = true;
                        NextExerciseSeconds.Text = $"Timer: {_currentExercise.Seconds}s";
                    }
                    NextExerciseName.Text = _currentExercise.Name;
                    ExerciseBreakTImer.Text = breaktimer.ToString();
                    await Task.Delay(1000);
                    breaktimer--;
                }

                GetReadyText.IsVisible = false;
                NextExerciseReps.IsVisible = false;
                NextExerciseSeconds.IsVisible = false;
                NextExerciseText.IsVisible = false;
                ExerciseBreakTImer.IsVisible = false;
                NextExerciseName.IsVisible = false;

                DoneButton.IsVisible = true;
                ExerciseTimerLabel.IsVisible = true;
                ExerciseNameLabel.IsVisible = true;
                ExerciseRepsLabel.IsVisible = true;
                ExerciseCountLabel.IsVisible = true;
                ExerciseText.IsVisible = true;

                UpdateExerciseDisplay();
            }

            else
            {
                await DisplayAlert("Workout Complete", "Congratulations, you have completed your Custom made workout routine!", "OK");
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