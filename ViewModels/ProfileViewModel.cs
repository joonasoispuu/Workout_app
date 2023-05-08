using System.ComponentModel;
using System.Windows.Input;
using System.Xml.Linq;
using WorkoutApp.Models;

namespace WorkoutApp
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        readonly string USER_NAME = "user_name";
        readonly string USER_HEIGHT = "user_height";
        readonly string USER_WEIGHT = "user_weight";
        readonly string USER_AGE = "user_age";

        public ProfileViewModel()
        {
            InitializeData();
        }

        private void InitializeData()
        {
            // get/set preferences
            //bool hasKey = Preferences.Default.ContainsKey("user_name");
            //Console.WriteLine("HAS_PREFERENCES_USER_NAME: " + hasKey);
            //if (string.IsNullOrWhiteSpace(User.Username))
            //{

            //}
            //Console.WriteLine("PREFERENCES_USER_NAME: " + User.Username);

            //CurrentUser.Username = Preferences.Default.Get(USER_NAME, "");
            //CurrentUser.Height = Preferences.Default.Get(USER_HEIGHT, 0);
            //CurrentUser.Weight = Preferences.Default.Get(USER_WEIGHT, 0);
            //CurrentUser.Age = Preferences.Default.Get(USER_AGE, 0);
        }

        public void SetUserDetails(UserModel user)
        {
            Preferences.Default.Set(USER_NAME, user.Username);
            Preferences.Default.Set(USER_HEIGHT, user.Height);
            Preferences.Default.Set(USER_WEIGHT, user.Weight);
            Preferences.Default.Set(USER_AGE, user.Age);
        }

        public UserModel CurrentUser { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
