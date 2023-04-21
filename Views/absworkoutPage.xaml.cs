namespace WorkoutApp.Views
{
    public partial class absworkoutPage : ContentPage
    {
        private readonly ExerciseDbContext _dbContext;
        public ExerciseViewModel ViewModel { get; set; }
        private ExerciseModel _currentExercise;
        private int _exerciseCount;

        public absworkoutPage()
        {
            InitializeComponent();

            _dbContext = new ExerciseDbContext();

            //Add exercises values to Abs category when the database is empty

            if (!_dbContext.Exercises.Any(x => x.Category == "Abs"))
            {
                _dbContext.Exercises.AddRange(new List<ExerciseModel>
                {
                    new ExerciseModel { Name = "Jumping Jacks", Reps = 30, Category = "Abs" },
                    new ExerciseModel { Name = "Heel Touch ", Reps = 30, Category = "Abs" },
                    new ExerciseModel { Name = "V-up", Reps = 16, Category = "Abs" },
                    new ExerciseModel { Name = "Crucnhes", Reps = 30, Category = "Abs" },
                    new ExerciseModel { Name = "Flutter kicks", Reps = 40, Category = "Abs" },
                    new ExerciseModel { Name = "Alt V-up", Reps = 16, Category = "Abs" },
                    new ExerciseModel { Name = "Push-up&rotation ", Reps = 24, Category = "Abs" },
                    new ExerciseModel { Name = "Mountain Climbers", Reps = 30, Category = "Abs" },
                    new ExerciseModel { Name = "V-cruch", Reps = 10, Category = "Abs" },
                    new ExerciseModel { Name = "Seated abs clockwise circle", Reps = 16, Category = "Abs" },
                    new ExerciseModel { Name = "Seated abs counterclockwise circles", Reps = 16, Category = "Abs" },
                    new ExerciseModel { Name = "Plank ", Reps = 60, Category = "Abs" },
              });


                _dbContext.SaveChanges();
            }

            // Set current exercise to first exercise in the database
            _currentExercise = _dbContext.Exercises.FirstOrDefault(x => x.Category == "Abs" && !x.Completed);
            _exerciseCount = 1;

            UpdateExerciseDisplay();
        }

        private void UpdateExerciseDisplay()
        {
            ExerciseNameLabel.Text = _currentExercise.Name;
            RepsLabel.Text = $"Reps: {_currentExercise.Reps}";
            ExerciseCountLabel.Text = $"Exercise {_exerciseCount} of 12";
        }

        private async void DoneButton_Clicked(object sender, EventArgs e)
        {
            // Marks current exercise as complete
            _currentExercise.Completed = true;
            _dbContext.SaveChanges();

            if (_exerciseCount < 12)
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