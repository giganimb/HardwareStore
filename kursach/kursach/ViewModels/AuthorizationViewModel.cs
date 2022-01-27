using kursach.Infostructure.Commands;
using kursach.Models;
using kursach.Models.Control;
using kursach.ViewModels.Base;
using kursach.ViewModels.Access;
using kursach.Views.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Threading.Tasks;

namespace kursach.ViewModels
{
    class AuthorizationViewModel : ViewModel
    {


        public AuthorizationViewModel()
        {
            ComeInCommand = new RelayCommand(OnComeInCommandExecuted, CanComeInCommandExecute);
            TransitionToRegistrationCommand = new RelayCommand(OnTransitionToRegistrationCommandExecuted, CanTransitionToRegistrationCommandExecute); 
        }



        #region Commands

            #region ComeIn
            public ICommand ComeInCommand { get; }
            private bool CanComeInCommandExecute(object p) => true;

            private async void OnComeInCommandExecuted(object p)
            {
                string resultComeIn = "";
                resultComeIn = await Task.Run(() => UserControl.ComeIn(login, password));
                if (resultComeIn == null)
                {
                    MainWindow mainWindow = new MainWindow();
                    string temp = await Task.Run(() => UserControl.IsAdmin(login));
                    Admission.GiveAccess(temp);
                    mainWindow.Show();


                    login = "";
                    password = "";
                }
                else
                {
                    Authorization authorization = new Authorization();
                    login = "";
                    authorization.Show();
                    MessageBox.Show(resultComeIn, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            #endregion


            #region TransitionToRegistration
            public ICommand TransitionToRegistrationCommand { get; }
            private bool CanTransitionToRegistrationCommandExecute(object p) => true;

            private void OnTransitionToRegistrationCommandExecuted(object p)
            {
                Registration registration = new Registration();
                registration.Show();


                login = "";
                password = "";
            }
            #endregion

        #endregion



        #region Fields

            private string _login;
            public string login
            {
                get => _login;
                set
                {
                    if (Equals(_login, value)) return;
                    _login = value;
                    OnPropertyChanged("login");
                }
            }


            private string _password;
            public string password
            {
                get => _password;
                set
                {
                    if (Equals(_password, value)) return;
                    _password = value;
                    OnPropertyChanged("password");
                }
            }

        #endregion



        #region Instance


        public static AuthorizationViewModel instance = null;
        public static AuthorizationViewModel getInstance()
        {
            if (instance == null)
                instance = new AuthorizationViewModel();
            return instance;
        }


        #endregion
    }
}
