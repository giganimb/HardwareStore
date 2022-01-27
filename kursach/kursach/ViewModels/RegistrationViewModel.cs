using kursach.Infostructure.Commands;
using kursach.Models;
using kursach.Models.Control;
using kursach.Models.Control.UnitOfWork;
using kursach.ViewModels.Base;
using kursach.Views.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace kursach.ViewModels
{
    class RegistrationViewModel : ViewModel
    {
        UnitOfWork unitOfWork;

        public RegistrationViewModel()
        {
            unitOfWork = new UnitOfWork();

            TransactionToAuthorizationCommand = new RelayCommand(OnTransactionToAuthorizationCommandExecuted, CanTransactionToAuthorizationCommandExecute);
            RegistrationCommand = new RelayCommand(OnRegistrationCommandExecuted, CanRegistrationCommandExecute);
        }



        #region Commands

            #region TransactionToAuthorization
            public ICommand TransactionToAuthorizationCommand { get; }
            private bool CanTransactionToAuthorizationCommandExecute(object p) => true;

            private void OnTransactionToAuthorizationCommandExecuted(object p)
            {
                Authorization mainWindow = new Authorization();
                mainWindow.Show();


                login = "";
                firstPassword = "";
                secondPassword = "";
            }
            #endregion


            #region Registration
            public ICommand RegistrationCommand { get; }
            private bool CanRegistrationCommandExecute(object p) => true;

            private void OnRegistrationCommandExecuted(object p)
            {
                string resultCreate = "";
                if(UserControl.IsFpassEqualsSpass(firstPassword, secondPassword))
                {
                    User item = new User(login, firstPassword, "NO");
                    resultCreate = unitOfWork.Users.Create(item);
                }
                else
                {
                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_Password_mismatch, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                }

                if (resultCreate == null || resultCreate == "")
                {
                    return;
                }
                else
                {
                    MessageBox.Show(resultCreate, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                }

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


            private string _firstPassword;
            public string firstPassword
            {
                get => _firstPassword;
                set
                {
                    if (Equals(_firstPassword, value)) return;
                    _firstPassword = value;
                    OnPropertyChanged("firstPassword");
                }
            }


            private string _secondPassword;
            public string secondPassword
            {
                get => _secondPassword;
                set
                {
                    if (Equals(_secondPassword, value)) return;
                    _secondPassword = value;
                    OnPropertyChanged("secondPassword");
                }
            }

        #endregion



        #region Instance


        public static RegistrationViewModel instance = null;
        public static RegistrationViewModel getInstance()
        {
            if (instance == null)
                instance = new RegistrationViewModel();
            return instance;
        }


        #endregion
    }
}
