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
    private float BMI;

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
                Height = Preferences.Default.Get(USER_HEIGHT, 0f),
                Weight = Preferences.Default.Get(USER_WEIGHT, 0f),
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
                CalculateBMI(user.Weight, user.Height, user.Age);

                
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

            float height = 0f;
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
            if (!float.TryParse(ChooseUserHeight.Text, out height))
            {
                await DisplayAlert("Error", "Please only enter numbers in the height field", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserHeight.Focus();
                });
                return;
            }
            if (height > 240)
            {
                await DisplayAlert("Error", "Too tall. Maximum height is 240(cm)", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserWeight.Focus();
                });
                return;
            }
            else if (height < 120)
            {
                await DisplayAlert("Error", "Too short. Minimum height requirement is 120(cm)", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserWeight.Focus();
                });
                return;
            }

            float weight = 0f;
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
            if (!float.TryParse(ChooseUserWeight.Text, out weight))
            {
                await DisplayAlert("Error", "Please only enter numbers in the weight field", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserWeight.Focus();
                });
                return;
            }
            if (weight > 635)
            {
                await DisplayAlert("Error", "Too fat. Maximum weight is 635(kg)", "OK");
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ChooseUserWeight.Focus();
                });
                return;
            } 
            else if (weight < 20) 
            {
                await DisplayAlert("Error", "Too light. Minimum weight requirement is 20(kg)", "OK");
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
            CalculateBMI(user.Weight, user.Height, user.Age);

            ChooseUsername.IsEnabled = false;
            ChooseUserHeight.IsEnabled = false;
            ChooseUserWeight.IsEnabled = false;
            ChooseUserAge.IsEnabled = false;
        } else
        {
            ChooseUsername.IsEnabled = true;
            ChooseUserHeight.IsEnabled = true;
            ChooseUserWeight.IsEnabled = true;
            ChooseUserAge.IsEnabled = true;

            //new MyToast("This is a Toast").Display();

            btnSaveUser.Text = "Save";
        }
        

    }

    private void CalculateBMI(float weight, float height, int age)
    {
        Console.WriteLine("calculateInitialBMI");

        float bmiheight = height / 100; // cm -> m
        BMI = weight / (bmiheight * bmiheight); // metric BMI

        DisplayBMI(age);
    }

    private void DisplayBMI(int age)
    {
        String bmiString = $"BMI: {BMI:0.#}";
        txtBMI.Text = bmiString;
        if (age < 18)
        {
            txtBMIWarning.Text = "This app is made for people over the age of 18\nSome results may vary.";
        }
        else
        {
            txtBMIWarning.Text = "";
        }
        if (BMI < 16)
        {
            txtDescription.Text = "Severely Skinny";
        }
        else if (BMI < 17)
        {
            txtDescription.Text = "Moderately Skinny";
        }
        else if (BMI < 18.5 && BMI >= 17)
        {
            txtDescription.Text = "Mildly Skinny";
        }
        else if (BMI < 25 && BMI >= 18.5)
        {
            txtDescription.Text = "Healthy";
        }
        else if (BMI < 30 && BMI >= 25)
        {
            txtDescription.Text = "Overweight";
        }
        else if (BMI < 34.9 && BMI >= 30)
        {
            txtDescription.Text = "Obese";
        }
        else
        {
            txtDescription.Text = "Obese+2";
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