namespace WorkoutApp
{
    public class ExerciseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Reps { get; set; }
        public int Seconds { get; set; }
        public string Category { get; set; }
        public bool Completed { get; set; }
    }
}