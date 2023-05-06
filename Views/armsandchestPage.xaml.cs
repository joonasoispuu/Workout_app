namespace WorkoutApp.Views
{
    public partial class armsandchestPage : ContentPage
{
    private readonly ExerciseDbContext _dbContext;
    public ExerciseViewModel ViewModel { get; set; }
    private ExerciseModel _currentExercise;
    private int _exerciseCount;
    private int _totalExercises;
    private System.Timers.Timer _timer;

    public armsandchestPage()
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

            if (!_dbContext.Exercises.Any(x => x.Category == "Arms and Chest"))
            {
                _dbContext.Exercises.AddRange(new List<ExerciseModel>
                {
                    new ExerciseModel { Name = "Jumping Jacks", Reps = 0, Seconds=30, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Arm Circles Clockwise", Reps = 0, Seconds=30, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Arm Circles CounterClockWise", Reps = 0, Seconds=30, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Burpees ", Reps = 10, Seconds=0, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Staggered push-ups", Reps = 12, Seconds=0, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Pushup & Rotation", Reps = 12, Seconds=0, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Diamond push ups", Reps = 10, Seconds=0, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Regular pushups", Reps = 12, Seconds=0, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Wide arm pushups ", Reps = 16, Seconds=0, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Plank", Reps = 0, Seconds=60, Category = "Arms and Chest" },
                });


                _dbContext.SaveChanges();
        }

        _currentExercise = _dbContext.Exercises.FirstOrDefault(x => x.Category == "Arms and Chest" && !x.Completed);
        _totalExercises = _dbContext.Exercises.Count(x => x.Category == "Arms and Chest" && !x.Completed);
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
                _currentExercise = _dbContext.Exercises.FirstOrDefault(x => x.Category == "Arms and Chest" && !x.Completed);
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
                await DisplayAlert("Workout Complete", "Congratulations, you have completed the Arms and Chest workout routine!", "OK");
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