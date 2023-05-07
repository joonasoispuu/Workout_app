using System.Runtime.InteropServices.ObjectiveC;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using WorkoutApp.Utils;

namespace WorkoutApp.Views;

public partial class ProfilePage : ContentPage
{
    readonly string USER_NAME = "user_name";
    readonly string USER_HEIGHT = "user_height";
    readonly string USER_WEIGHT = "user_weight";
    readonly string USER_AGE = "user_age";

    private readonly ProfileViewModel _profileViewModel;
    private UserModel _currentUser;

    public ProfilePage()
    {
        InitializeComponent();
        _profileViewModel = new ProfileViewModel();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Task.Run(() =>
        {
            var user = new UserModel
            {
                Id = 0,
                Username = Preferences.Default.Get(USER_NAME, ""),
                Height = Preferences.Default.Get(USER_HEIGHT, 0),
                Weight = Preferences.Default.Get(USER_WEIGHT, 0),
                Age = Preferences.Default.Get(USER_AGE, 0)
            };
            //_currentUser = user;
            ChooseUsername.Text = user.Username.ToString();

            bool hasUsername = Preferences.Default.ContainsKey(USER_NAME);
            if (!hasUsername && user.Username.Equals(""))
            {
                ChooseUsername.IsEnabled = true;

                while (!ChooseUsername.IsVisible)
                {
                    Task.Delay(10).Wait();
                }
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUsername.Focus();
                });
            }


            bool hasUserHeight = Preferences.Default.ContainsKey(USER_HEIGHT);
            if (!hasUserHeight && user.Height == 0)
            {
                ChooseUserHeight.IsEnabled = true;

            } else
            {
                ChooseUserHeight.Text = user.Height.ToString();
            }

            bool hasUserWeight = Preferences.Default.ContainsKey(USER_WEIGHT);
            if (!hasUserWeight && user.Weight == 0)
            {
                ChooseUserWeight.IsEnabled = true;
            }
            else
            {
                ChooseUserWeight.Text = user.Weight.ToString();
            }

            bool hasUserAge = Preferences.Default.ContainsKey(USER_AGE);
            if (!hasUserAge && user.Age == 0)
            {
                ChooseUserAge.IsEnabled = true;
                
            } 
            else
            {
                ChooseUserAge.Text = user.Age.ToString();
            }

            if (ChooseUsername.Text.Length > 0 && 
                ChooseUserHeight.Text.Length > 0 &&
                ChooseUserWeight.Text.Length > 0 &&
                ChooseUserAge.Text.Length > 0)
            {
                btnSaveUser.Text = "Edit";
            }
            //_profileViewModel.SetUserDetails(user);
        });
    }

    private async void OnSaveUser_Clicked(object sender, EventArgs e)
    {
        if (btnSaveUser.Text != "Edit")
        {
            //Checks if ChooseUsername.Text is empty
            if (string.IsNullOrWhiteSpace(ChooseUsername.Text))
            {
                await DisplayAlert("Error", "Please enter your name", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUsername.Focus();
                });
                return;
            }
            string username = ChooseUsername.Text;
            int _ = 0;
            //Checks if ChooseUsername.Text value is a number
            if (int.TryParse(ChooseUsername.Text, out _))
            {
                await DisplayAlert("Error", "Please only enter text in the username field", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUsername.Focus();
                });
                return;
            }

            int height = 0;
            //Checks if ChooseUserHeight.Text is empty and the value is a number
            if (string.IsNullOrWhiteSpace(ChooseUserHeight.Text))
            {
                await DisplayAlert("Error", "Please enter your height", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserHeight.Focus();
                });
                return;
            }
            //Checks if ChooseUserHeight.Text value is a number
            if (!int.TryParse(ChooseUserHeight.Text, out height))
            {
                await DisplayAlert("Error", "Please only enter numbers in the height field", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserHeight.Focus();
                });
                return;
            }

            int weight = 0;
            //Checks if ChooseUserWeight.Text is empty and the value is a number
            if (string.IsNullOrWhiteSpace(ChooseUserWeight.Text))
            {
                await DisplayAlert("Error", "Please enter your weight", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserWeight.Focus();
                });
                return;
            }
            //Checks if ChooseUserWeight.Text value is a number
            if (!int.TryParse(ChooseUserWeight.Text, out weight))
            {
                await DisplayAlert("Error", "Please only enter numbers in the weight field", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserWeight.Focus();
                });
                return;
            }

            int age = 0;
            //Checks if ageField.Text is empty and the value is a number
            if (string.IsNullOrWhiteSpace(ChooseUserAge.Text))
            {
                await DisplayAlert("Error", "Please enter your age", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserAge.Focus();
                });
                return;
            }
            //Checks if ageField.Text value is a number
            if (!int.TryParse(ChooseUserAge.Text, out age))
            {
                await DisplayAlert("Error", "Please only enter numbers in the age field", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserAge.Focus();
                });
                return;
            }

            if (age < 13)
            {
                await DisplayAlert("Error", "You have to be at least 13 years old to continue", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserAge.Focus();
                });
                return;
            }

            if (age > 999)
            {
                await DisplayAlert("Error", $"Age cannot be {age}.", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserAge.Focus();
                });
                return;
            }

            var user = new UserModel
            {
                Id = 0,
                Username = username,
                Height = height,
                Weight = weight,
                Age = age
            };

            // Save data to App Preferences
            _profileViewModel.SetUserDetails(user);
            btnSaveUser.Text = "Edit";
            new MyToast("Changes saved successfully").Display();
        } else
        {
            ChooseUsername.IsEnabled = true;
            ChooseUserHeight.IsEnabled = true;
            ChooseUserWeight.IsEnabled = true;
            //ChooseUserAge.IsEnabled = true;

            //new MyToast("This is a Toast").Display();

            btnSaveUser.Text = "Save";
        }
        

    }

    private async Task NavigateToPage(Page page)
    {
        await Navigation.PushAsync(page);
    }

    private void ChooseUsername_Focused(object sender, FocusEventArgs e)
    {

    }
}