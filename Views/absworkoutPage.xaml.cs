namespace WorkoutApp.Views
{
    public partial class absworkoutPage : ContentPage
    {
        private readonly ExerciseDbContext _dbContext;
        public ExerciseViewModel ViewModel { get; set; }
        private ExerciseModel _currentExercise;
        private int _exerciseCount;
        private int _totalExercises;
        private System.Timers.Timer _timer;

        public absworkoutPage()
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

            //Add exercises values to Abs category when the database is empty

            if (!_dbContext.Exercises.Any(x => x.Category == "Abs"))
            {
                _dbContext.Exercises.AddRange(new List<ExerciseModel>
                {
                    new ExerciseModel { Name = "Jumping Jacks", Reps = 0, Seconds = 30, Category = "Abs" },
                    new ExerciseModel { Name = "Heel Touch ", Reps = 30, Seconds = 0, Category = "Abs" },
                    new ExerciseModel { Name = "V-up", Reps = 0, Seconds = 16, Category = "Abs" },
                    new ExerciseModel { Name = "Crucnhes", Reps = 30, Seconds = 0, Category = "Abs" },
                    new ExerciseModel { Name = "Flutter kicks", Reps = 0, Seconds = 40, Category = "Abs" },
                    new ExerciseModel { Name = "Alt V-up", Reps = 0, Seconds = 16, Category = "Abs" },
                    new ExerciseModel { Name = "Push-up&rotation ", Reps = 0, Seconds = 24, Category = "Abs" },
                    new ExerciseModel { Name = "Mountain Climbers", Reps = 30, Seconds = 0, Category = "Abs" },
                    new ExerciseModel { Name = "V-cruch", Reps = 0, Seconds = 10, Category = "Abs" },
                    new ExerciseModel { Name = "Seated abs clockwise circle", Reps = 0, Seconds = 16, Category = "Abs" },
                    new ExerciseModel { Name = "Seated abs counterclockwise circles", Reps = 0, Seconds = 16, Category = "Abs" },
                    new ExerciseModel { Name = "Plank ", Reps = 0, Seconds = 60, Category = "Abs" },
              });


                _dbContext.SaveChanges();
            }

            // Set current exercise to first exercise in the database
            _currentExercise = _dbContext.Exercises.FirstOrDefault(x => x.Category == "Abs" && !x.Completed);

            _totalExercises = _dbContext.Exercises.Count(x => x.Category == "Abs");
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
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        ExerciseTimerLabel.Text = $"Time left: {timerValue}s";
                    });

                    if (timerValue == 0)
                    {
                        timer.Stop();
                        Device.BeginInvokeOnMainThread(() => DoneButton_Clicked(null, null));
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

            // Marks current exercise as complete
            _currentExercise.Completed = true;
            _dbContext.SaveChanges();

            if (_exerciseCount < _totalExercises)
            {
                // Get the next exercise in database
                _currentExercise = _dbContext.Exercises.FirstOrDefault(x => x.Category == "Abs" && !x.Completed);
                _exerciseCount++;

                UpdateExerciseDisplay();
            }
            else
            {
                await DisplayAlert("Workout Complete", "Congratulations, you have completed the abs workout routine!", "OK");

                // Reset the data
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