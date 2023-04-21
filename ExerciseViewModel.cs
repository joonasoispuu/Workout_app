using System.Windows.Input;
using System.ComponentModel;

namespace WorkoutApp
{
    public class ExerciseViewModel : INotifyPropertyChanged
    {
        private List<ExerciseModel> _exercises;
        private int _currentExerciseIndex;

        public ExerciseViewModel()
        {
            InitializeData();
            SetExerciseDetails();
            DoneCommand = new Command(OnDone);
        }

        private void InitializeData()
        {
            using (var db = new ExerciseDbContext())
            {
                db.Database.EnsureCreated();
                _exercises = db.Exercises.ToList();
            }
            _currentExerciseIndex = 0;
        }

        private void SetExerciseDetails()
        {
            if (_exercises != null && _exercises.Count > _currentExerciseIndex)
            {
                CurrentExercise = _exercises[_currentExerciseIndex];
                ExerciseNumber = $"Exercise {_currentExerciseIndex + 1} of {_exercises.Count}";
            }
            else
            {
                CurrentExercise = null;
                ExerciseNumber = "Congratulations! You have completed all exercises.";
            }
            OnPropertyChanged(nameof(CurrentExercise));
            OnPropertyChanged(nameof(ExerciseNumber));
        }

        public ExerciseModel CurrentExercise { get; private set; }

        public string ExerciseNumber { get; private set; }

        public ICommand DoneCommand { get; private set; }

        private void OnDone()
        {
            _currentExerciseIndex++;
            SetExerciseDetails();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}