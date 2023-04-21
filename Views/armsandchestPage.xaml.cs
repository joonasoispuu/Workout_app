namespace WorkoutApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class armsandchestPage : ContentPage
    {
        private readonly ExerciseDbContext _dbContext;
        private ExerciseModel _currentExercise;
        private int _exerciseCount;

        public armsandchestPage()
        {
            InitializeComponent();

            _dbContext = new ExerciseDbContext();

            _dbContext.Database.EnsureCreated();

            if (!_dbContext.Exercises.Any(x => x.Category == "Arms and Chest"))
            {
                _dbContext.Exercises.AddRange(new List<ExerciseModel>
                {
                    new ExerciseModel { Name = "Jumping Jacks", Reps = 30, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Arm Circles Clockwise", Reps = 30, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Arm Circles CounterClockWise", Reps = 30, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Burpees ", Reps = 10, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Staggered push-ups", Reps = 12, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Pushup & Rotation", Reps = 12, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Diamond push ups", Reps = 10, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Regular pushups", Reps = 12, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Wide arm pushups ", Reps = 16, Category = "Arms and Chest" },
                    new ExerciseModel { Name = "Plank", Reps = 60, Category = "Arms and Chest" },
                });


                _dbContext.SaveChanges();
            }

            _currentExercise = _dbContext.Exercises.FirstOrDefault(x => x.Category == "Arms and Chest" && !x.Completed);
            _exerciseCount = 1;

            UpdateExerciseDisplay();
        }

        private void UpdateExerciseDisplay()
        {
            ExerciseNameLabel.Text = _currentExercise.Name;
            RepsLabel.Text = $"Reps: {_currentExercise.Reps}";
            ExerciseCountLabel.Text = $"Exercise {_exerciseCount} of 10";
        }

        private async void DoneButton_Clicked(object sender, EventArgs e)
        {
            _currentExercise.Completed = true;
            _dbContext.SaveChanges();

            if (_exerciseCount < 10)
            {
                _currentExercise = _dbContext.Exercises.FirstOrDefault(x => x.Category == "Arms and Chest" && !x.Completed);
                _exerciseCount++;

                UpdateExerciseDisplay();
            }
            else
            {
                await DisplayAlert("Workout Complete", "Congratulations, you have completed the arms and chest workout routine!", "OK");

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