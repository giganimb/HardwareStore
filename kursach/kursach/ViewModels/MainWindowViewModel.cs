using kursach.Infostructure.Commands;
using kursach.Information;
using kursach.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using kursach.Models.Control;
using System.Collections.ObjectModel;
using kursach.Models;
using Microsoft.Win32;
using kursach.ViewModels.Access;
using kursach.Views.Windows;
using kursach.Models.Control.UnitOfWork;
using System.Threading.Tasks;

namespace kursach.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        #region MainFields

        private List<Product> _tempProducts;
        public List<Product> tempProducts
        {
            get => _tempProducts;
            set
            {
                if (Equals(_tempProducts, value)) return;
                _tempProducts = value;
                OnPropertyChanged("tempProducts");
            }
        }

        private List<Product> _sortProducts;
        public List<Product> sortProducts
        {
            get => _sortProducts;
            set
            {
                if (Equals(_sortProducts, value)) return;
                _sortProducts = value;
                OnPropertyChanged("sortProducts");
            }
        }

        private List<Product> _searchProducts;
        public List<Product> searchProducts
        {
            get => _searchProducts;
            set
            {
                if (Equals(_searchProducts, value)) return;
                _searchProducts = value;
                OnPropertyChanged("searchProducts");
            }
        }

        private List<SoldProduct> _tempSoldProducts;
        public List<SoldProduct> tempSoldProducts
        {
            get => _tempSoldProducts;
            set
            {
                if (Equals(_tempSoldProducts, value)) return;
                _tempSoldProducts = value;
                OnPropertyChanged("tempSoldProducts");
            }
        }

        private List<SoldProduct> _searchSoldProducts;
        public List<SoldProduct> searchSoldProducts
        {
            get => _searchSoldProducts;
            set
            {
                if (Equals(_searchSoldProducts, value)) return;
                _searchSoldProducts = value;
                OnPropertyChanged("searchSoldProducts");
            }
        }

        #endregion

        UnitOfWork unitOfWork;

        public MainWindowViewModel()
        {
            unitOfWork = new UnitOfWork();

            CloseApplicationCommand = new RelayCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommanExecute);
            ChangeAccountCommand = new RelayCommand(OnChangeAccountCommandExecuted, CanChangeAccountCommanExecute);
            ChangeThemeOnLightCommand = new RelayCommand(OnChangeThemeOnLightCommandExecuted, CanChangeThemeOnLighCommandExecute);
            ChangeThemeOnDarkCommand = new RelayCommand(OnChangeThemeOnDarkCommandExecuted, CanChangeThemeOnDarkCommandExecute);
            ChangeThemeOnMainCommand = new RelayCommand(OnChangeThemeOnMainCommandExecuted, CanChangeThemeOnMainCommandExecute);
            InformationAboutAppCommand = new RelayCommand(OnInformationAboutAppCommandExecuted, CanInformationAboutAppCommandExecute);
            ChangeLangOnRuCommand = new RelayCommand(OnChangeLangOnRuCommandExecuted, CanChangeLangOnRuCommandExecute);
            ChangeLangEnRuCommand = new RelayCommand(OnChangeLangOnEnCommandExecuted, CanChangeLangOnEnCommandExecute);


            LoadPictureCommand = new RelayCommand(OnLoadPictureCommandExecuted, CanLoadPictureCommandExecute);
            ClearFieldsCommand = new RelayCommand(OnClearFieldsCommandExecuted, CanClearFieldsCommandExecute);
            AddProductCommand = new RelayCommand(OnAddProductCommandExecuted, CanAddProductCommandExecute);
            OutputProductsCommand = new RelayCommand(OnOutputProductsCommandExecuted, CanOutputProductsCommandExecute);


            DeleteProductCommand = new RelayCommand(OnDeleteProductCommandExecuted, CanDeleteProductCommandExecute);
            SaveChangesCommand = new RelayCommand(OnSaveChangesCommandExecuted, CanSaveChangesCommandExecute);
            ClearChDelFieldsCommand = new RelayCommand(OnClearChDelFieldsCommandExecuted, CanClearChDelFieldsCommandExecute);
            OutputChDelProductsCommand = new RelayCommand(OnOutputChDelProductsCommandExecuted, CanOutputChDelProductsCommandExecute);


            SortCommand = new RelayCommand(OnSortCommandExecuted, CanSortCommandExecute);


            SearchCommand = new RelayCommand(OnSearchCommandExecuted, CanSearchCommandExecute);
            ClearFieldsInSearchingProductsCommand = new RelayCommand(OnClearFieldsInSearchingProductsCommandExecuted, CanClearFieldsInSearchingProductsCommandExecute);




            AddProductToSoldCommand = new RelayCommand(OnAddProductToSoldCommandExecuted, CanAddProductToSoldCommandExecute);
            ClearFieldsInSoldProductsCommand = new RelayCommand(OnClearFieldsInSoldProductsCommandExecuted, CanClearFieldsInSoldProductsCommandExecute);
            OutputSoldProductsCommand = new RelayCommand(OnOutputSoldProductsCommandExecuted, CanOutputSoldProductsCommandExecute);
            DeleteSoldProductCommand = new RelayCommand(OnDeleteSoldProductCommandExecuted, CanDeleteSoldProductCommandExecute);

            SearchSoldCommand = new RelayCommand(OnSearchSoldCommandExecuted, CanSearchSoldCommandExecute);
            ClearFieldsInSearchingSoldProductsCommand = new RelayCommand(OnClearFieldsInSearchingSoldProductsCommandExecuted, CanClearFieldsInSearchingSoldProductsCommandExecute);
        }



        #region Commands

            #region MenuCommands

                #region ChangeAccount
                public ICommand ChangeAccountCommand { get; }

                private bool CanChangeAccountCommanExecute(object p) => true;

                private void OnChangeAccountCommandExecuted(object p)
                {
                    Authorization authorization = new Authorization();
                    authorization.Show();
                }
                #endregion


                #region ExitFromApp
                public ICommand CloseApplicationCommand { get; }

                private bool CanCloseApplicationCommanExecute(object p) => true;

                private void OnCloseApplicationCommandExecuted(object p)
                {
                    Application.Current.Shutdown();
                }
                #endregion


                #region LightTheme
                public ICommand ChangeThemeOnLightCommand { get; }
                private bool CanChangeThemeOnLighCommandExecute(object p) => true;

                private void OnChangeThemeOnLightCommandExecuted(object p)
                {
                    Uri uri = new Uri("Views/Windows/Resources/MainWindow/Styles/LightTheme.xaml", UriKind.Relative);
                    ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                    Application.Current.Resources.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                }
                #endregion


                #region DarkTheme
                public ICommand ChangeThemeOnDarkCommand { get; }
                private bool CanChangeThemeOnDarkCommandExecute(object p) => true;

                private void OnChangeThemeOnDarkCommandExecuted(object p)
                {
                    Uri uri = new Uri("Views/Windows/Resources/MainWindow/Styles/DarkTheme.xaml", UriKind.Relative);
                    ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                    Application.Current.Resources.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                }
                #endregion


                #region MainTheme
                public ICommand ChangeThemeOnMainCommand { get; }
                private bool CanChangeThemeOnMainCommandExecute(object p) => true;

                private void OnChangeThemeOnMainCommandExecuted(object p)
                {
                    Uri uri = new Uri("Views/Windows/Resources/MainWindow/Styles/MainTheme.xaml", UriKind.Relative);
                    ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                    Application.Current.Resources.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(resourceDict);
                }
                #endregion


                #region RuLang
                public ICommand ChangeLangOnRuCommand { get; }
                private bool CanChangeLangOnRuCommandExecute(object p) => true;

                private void OnChangeLangOnRuCommandExecuted(object p)
                {
                    Properties.Settings.Default.languageCode = "ru-RU";
                    Properties.Settings.Default.Save();
                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_RestartApp, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                #endregion


                #region EnLang
                public ICommand ChangeLangEnRuCommand { get; }
                private bool CanChangeLangOnEnCommandExecute(object p) => true;

                private void OnChangeLangOnEnCommandExecuted(object p)
                {
                    Properties.Settings.Default.languageCode = "en-US";
                    Properties.Settings.Default.Save();
                    MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_RestartApp, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                #endregion


                #region InformationAboutApp
                public ICommand InformationAboutAppCommand { get; }
                private bool CanInformationAboutAppCommandExecute(object p) => true;

                private void OnInformationAboutAppCommandExecuted(object p)
                {
                    InformationAboutApp inf = InformationAboutApp.GetInstance();
                    MessageBox.Show(inf.ToString(), Views.Windows.Resources.Languages.Lang.m_AboutApp, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                #endregion

            #endregion


            #region AddProductCommands

                #region LoadPicture
                public ICommand LoadPictureCommand { get; }
                public bool CanLoadPictureCommandExecute(object p)
                {
                    if(Admission.GetAccess())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                private void OnLoadPictureCommandExecuted(object p)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Image files|*.png;*.jpg";
                    if(openFileDialog.ShowDialog() == true)
                    {
                        try
                        {
                            pathToPicture = openFileDialog.FileName;
                        }
                        catch
                        {
                            MessageBox.Show(Views.Windows.Resources.Languages.Lang.m_The_selected_file_cannot_be_opened, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                #endregion


                #region ClearFields
                public ICommand ClearFieldsCommand { get; }
                public bool CanClearFieldsCommandExecute(object p) => true;

                private void OnClearFieldsCommandExecuted(object p)
                {
                    nameOfProduct = "";
                    pathToPicture = "";
                    category = "";
                    rating = "";
                    price = "";
                    number = "";
                    size = "";
                    description = "";
                }
                #endregion


                #region AddProduct
                public ICommand AddProductCommand { get; }
                public bool CanAddProductCommandExecute(object p)
                {
                    if (Admission.GetAccess())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                private void OnAddProductCommandExecuted(object p)
                {
                    string resultAdd = "";
                    Product item = new Product(nameOfProduct, pathToPicture, category, rating, price, number, size, description);
                    resultAdd = unitOfWork.Products.Create(item);
                    if (resultAdd == null)
                    {
                        return;
                    }
                    else
                    {
                        MessageBox.Show(resultAdd, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                #endregion


                #region OutputProducts
                public ICommand OutputProductsCommand { get; }
                public bool CanOutputProductsCommandExecute(object p) => true;

                private void OnOutputProductsCommandExecuted(object p)
                {
                    tempProducts = unitOfWork.Products.GetAll();
                }
                #endregion

             #endregion


            #region ChDelProductCommands

                #region DeleteProduct
                public ICommand DeleteProductCommand { get; }
                public bool CanDeleteProductCommandExecute(object p)
                {
                    if(Admission.GetAccess())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                private void OnDeleteProductCommandExecuted(object p)
                {
                    string resultDelete = "";
                    resultDelete = unitOfWork.Products.Delete(nameOfDelProduct);
                    MessageBox.Show(resultDelete, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                }
                #endregion


                #region ClearChDelFields
                public ICommand ClearChDelFieldsCommand { get; }
                public bool CanClearChDelFieldsCommandExecute(object p) => true;

                private void OnClearChDelFieldsCommandExecuted(object p)
                {
                    nameOfDelProduct = "";
                    nameOfChProduct = "";
                    ChDelCategory = "";
                    ChDelRating = "";
                    ChDelPrice = "";
                    ChDelNumber = "";
                    ChDelSize = "";
                    ChDelDescription = "";
                }
                #endregion


                #region OutputChDelProducts
                public ICommand OutputChDelProductsCommand { get; }
                public bool CanOutputChDelProductsCommandExecute(object p) => true;

                private  void OnOutputChDelProductsCommandExecuted(object p)
                {
                    tempProducts = unitOfWork.Products.GetAll();
                }
                #endregion


                #region SaveChanges
                public ICommand SaveChangesCommand { get; }
                public bool CanSaveChangesCommandExecute(object p)
                {
                    if (Admission.GetAccess())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }

                private void OnSaveChangesCommandExecuted(object p)
                {
                    string resultChange = "";
                    Product item = new Product(nameOfChProduct, "", ChDelCategory, ChDelRating, ChDelPrice, ChDelNumber, ChDelSize, ChDelDescription);
                    resultChange = unitOfWork.Products.Update(item);
                    if (resultChange == null)
                    {
                        return;
                    }
                    else
                    {
                        MessageBox.Show(resultChange, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                #endregion

            #endregion


            #region SortProductCommands

                #region Sort
                public ICommand SortCommand { get; }
                public bool CanSortCommandExecute(object p) => true;

                private void OnSortCommandExecuted(object p)
                {
                    sortProducts = ProductControl.SortProduct(IsCheckNOPSort, IsCheckRatingSort);
                }
                #endregion

            #endregion


            #region SearchProductCommands

                #region Search
                public ICommand SearchCommand { get; }
                public bool CanSearchCommandExecute(object p) => true;

                private void OnSearchCommandExecuted(object p)
                {
                    searchProducts = ProductControl.SearchProduct(SearchByNOP, SearchByCategory, SearchByNumber, SearchBySize, SearchByStartPrice, SearchByFinishPrice);
                }
                #endregion


                #region ClearFieldsInSearchingProducts
                public ICommand ClearFieldsInSearchingProductsCommand { get; }
                public bool CanClearFieldsInSearchingProductsCommandExecute(object p) => true;

                private void OnClearFieldsInSearchingProductsCommandExecuted(object p)
                {
                    SearchByNOP = "";
                    SearchByCategory = "";
                    SearchByNumber = "";
                    SearchBySize = "";
                    SearchByStartPrice = "";
                    SearchByFinishPrice = "";

                }
        #endregion

            #endregion


            #region SoldProductCommands

                #region AddProductToSold
                public ICommand AddProductToSoldCommand { get; }
                public bool CanAddProductToSoldCommandExecute(object p) => true;

                private void OnAddProductToSoldCommandExecuted(object p)
                {
                    string resulCreate = "";
                    SoldProduct item = new SoldProduct(nameOfSoldProduct, "", "", numberOfSoldProduct, dateOfSale, timeOfSale, userLoginOfSoldProduct);
                    resulCreate = unitOfWork.SoldProducts.Create(item);
                    if (resulCreate == null)
                    {
                        return;
                    }
                    else
                    {
                        MessageBox.Show(resulCreate, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                #endregion


                #region OutputSoldProducts
                public ICommand OutputSoldProductsCommand { get; }
                public bool CanOutputSoldProductsCommandExecute(object p) => true;

                private void OnOutputSoldProductsCommandExecuted(object p)
                {
                    tempSoldProducts = unitOfWork.SoldProducts.GetAll();
                }
                #endregion


                #region ClearFieldsInSoldProducts
                public ICommand ClearFieldsInSoldProductsCommand { get; }
                public bool CanClearFieldsInSoldProductsCommandExecute(object p) => true;

                private void OnClearFieldsInSoldProductsCommandExecuted(object p)
                {
                    nameOfSoldProduct = "";
                    numberOfSoldProduct = "";
                    dateOfSale = "";
                    timeOfSale = "";
                    userLoginOfSoldProduct = "";
                }
        #endregion


                #region DeleteSoldProduct
                public ICommand DeleteSoldProductCommand { get; }
                public bool CanDeleteSoldProductCommandExecute(object p) => true;

                private void OnDeleteSoldProductCommandExecuted(object p)
                {
                    string resultDelete = "";
                    resultDelete = unitOfWork.SoldProducts.Delete(nameOfDelSoldProduct);
                    MessageBox.Show(resultDelete, Views.Windows.Resources.Languages.Lang.m_Message, MessageBoxButton.OK, MessageBoxImage.Information);
                }
        #endregion

            #endregion


            #region SearchSoldProductCommands

                #region Search
                public ICommand SearchSoldCommand { get; }
                public bool CanSearchSoldCommandExecute(object p) => true;

                private void OnSearchSoldCommandExecuted(object p)
                {
                    searchSoldProducts = SoldProductControl.SearchProduct(SearchByNOPSold, SearchByUserSold, SearchByNumberSold, SearchByDateSold, SearchByStartPriceSold, SearchByFinishPriceSold);
                }
                #endregion


                #region ClearFieldsInSearchingSoldProducts
                public ICommand ClearFieldsInSearchingSoldProductsCommand { get; }
                public bool CanClearFieldsInSearchingSoldProductsCommandExecute(object p) => true;

                private void OnClearFieldsInSearchingSoldProductsCommandExecuted(object p)
                {
                    SearchByNOPSold = "";
                    SearchByUserSold = "";
                    SearchByNumberSold = "";
                    SearchByDateSold = "";
                    SearchByStartPriceSold = "";
                    SearchByFinishPriceSold = "";
                }
                #endregion

            #endregion

        #endregion



        #region Fields

            #region FieldsOfProduct

            private string _nameOfProduct;
            public string nameOfProduct
            {
                get => _nameOfProduct;
                set
                {
                    if (Equals(_nameOfProduct, value)) return;
                    _nameOfProduct = value;
                    OnPropertyChanged("nameOfProduct");
                }
            }

            private string _pathToPicture;
            public string pathToPicture
            {
                get => _pathToPicture;
                set
                {
                    if (Equals(_pathToPicture, value)) return;
                    _pathToPicture = value;
                    OnPropertyChanged("pathToPicture");
                }
            }

            private string _category;
            public string category
            {
                get => _category;
                set
                {
                    if (Equals(_category, value)) return;
                    _category = value;
                    OnPropertyChanged("category");
                }
            }

            private string _rating;
            public string rating
            {
                get => _rating;
                set
                {
                    if (Equals(_rating, value)) return;
                    _rating = value;
                    OnPropertyChanged("rating");
                }
            }

            private string _price;
            public string price
            {
                get => _price;
                set
                {
                    if (Equals(_price, value)) return;
                    _price = value;
                    OnPropertyChanged("price");
                }
            }

            private string _number;
            public string number
            {
                get => _number;
                set
                {
                    if (Equals(_number, value)) return;
                    _number = value;
                    OnPropertyChanged("number");
                }
            }

            private string _size;
            public string size
            {
                get => _size;
                set
                {
                    if (Equals(_size, value)) return;
                    _size = value;
                    OnPropertyChanged("size");
                }
            }

            private string _description;
            public string description
            {
                get => _description;
                set
                {
                    if (Equals(_description, value)) return;
                    _description = value;
                    OnPropertyChanged("description");
                }
            }

            #endregion


            #region FieldsOfChDelProduct

                private string _nameOfDelProduct;
                public string nameOfDelProduct
                {
                    get => _nameOfDelProduct;
                    set
                    {
                        if (Equals(_nameOfDelProduct, value)) return;
                        _nameOfDelProduct = value;
                        OnPropertyChanged("nameOfDelProduct");
                    }
                }

                private string _nameOfChProduct;
                public string nameOfChProduct
                {
                    get => _nameOfChProduct;
                    set
                    {
                        if (Equals(_nameOfChProduct, value)) return;
                        _nameOfChProduct = value;
                        OnPropertyChanged("nameOfChProduct");
                    }
                }
            
                private string _ChDelCategory;
                public string ChDelCategory
                {
                    get => _ChDelCategory;
                    set
                    {
                        if (Equals(_ChDelCategory, value)) return;
                        _ChDelCategory = value;
                        OnPropertyChanged("ChDelCategory");
                    }
                }

                private string _ChDelRating;
                public string ChDelRating
                {
                    get => _ChDelRating;
                    set
                    {
                        if (Equals(_ChDelRating, value)) return;
                        _ChDelRating = value;
                        OnPropertyChanged("ChDelRating");
                    }
                }

                private string _ChDelPrice;
                public string ChDelPrice
                {
                    get => _ChDelPrice;
                    set
                    {
                        if (Equals(_ChDelPrice, value)) return;
                        _ChDelPrice = value;
                        OnPropertyChanged("ChDelPrice");
                    }
                }

                private string _ChDelNumber;
                public string ChDelNumber
                {
                    get => _ChDelNumber;
                    set
                    {
                        if (Equals(_ChDelNumber, value)) return;
                        _ChDelNumber = value;
                        OnPropertyChanged("ChDelNumber");
                    }
                }

                private string _ChDelSize;
                public string ChDelSize
                {
                    get => _ChDelSize;
                    set
                    {
                        if (Equals(_ChDelSize, value)) return;
                        _ChDelSize = value;
                        OnPropertyChanged("ChDelSize");
                    }
                }

                private string _ChDelDescription;
                public string ChDelDescription
                {
                    get => _ChDelDescription;
                    set
                    {
                        if (Equals(_ChDelDescription, value)) return;
                        _ChDelDescription = value;
                        OnPropertyChanged("ChDelDescription");
                    }
                }

        #endregion


            #region FieldsOfSort

                private bool _IsCheckNOPSort;
                public bool IsCheckNOPSort
                {
                    get => _IsCheckNOPSort;
                    set
                    {
                        if (Equals(_IsCheckNOPSort, value)) return;
                        _IsCheckNOPSort = value;
                        OnPropertyChanged("IsCheckNOPSort");
                    }
                }

                private bool _IsCheckRatingSort;
                public bool IsCheckRatingSort
                {
                    get => _IsCheckRatingSort;
                    set
                    {
                        if (Equals(_IsCheckRatingSort, value)) return;
                        _IsCheckRatingSort = value;
                        OnPropertyChanged("IsCheckRatingSort");
                    }
                }

        #endregion


            #region FieldsOfSearch

            private string _SearchByNOP;
            public string SearchByNOP
            {
                get => _SearchByNOP;
                set
                {
                    if (Equals(_SearchByNOP, value)) return;
                    _SearchByNOP = value;
                    OnPropertyChanged("SearchByNOP");
                }
            }

            private string _SearchByCategory;
            public string SearchByCategory
            {
                get => _SearchByCategory;
                set
                {
                    if (Equals(_SearchByCategory, value)) return;
                    _SearchByCategory = value;
                    OnPropertyChanged("SearchByCategory");
                }
            }

            private string _SearchByNumber;
            public string SearchByNumber
            {
                get => _SearchByNumber;
                set
                {
                    if (Equals(_SearchByNumber, value)) return;
                    _SearchByNumber = value;
                    OnPropertyChanged("SearchByNumber");
                }
            }

            private string _SearchBySize;
            public string SearchBySize
            {
                get => _SearchBySize;
                set
                {
                    if (Equals(_SearchBySize, value)) return;
                    _SearchBySize = value;
                    OnPropertyChanged("SearchBySize");
                }
            }

            private string _SearchByStartPrice;
            public string SearchByStartPrice
            {
                get => _SearchByStartPrice;
                set
                {
                    if (Equals(_SearchByStartPrice, value)) return;
                    _SearchByStartPrice = value;
                    OnPropertyChanged("SearchByStartPrice");
                }
            }

            private string _SearchByFinishPrice;
            public string SearchByFinishPrice
            {
                get => _SearchByFinishPrice;
                set
                {
                    if (Equals(_SearchByFinishPrice, value)) return;
                    _SearchByFinishPrice = value;
                    OnPropertyChanged("SearchByFinishPrice");
                }
            }

        #endregion


            #region FieldsOfSoldProduct

            private string _nameOfSoldProduct;
            public string nameOfSoldProduct
            {
                get => _nameOfSoldProduct;
                set
                {
                    if (Equals(_nameOfSoldProduct, value)) return;
                    _nameOfSoldProduct = value;
                    OnPropertyChanged("nameOfSoldProduct");
                }
            }
            
            private string _numberOfSoldProduct;
            public string numberOfSoldProduct
            {
                get => _numberOfSoldProduct;
                set
                {
                    if (Equals(_numberOfSoldProduct, value)) return;
                    _numberOfSoldProduct = value;
                    OnPropertyChanged("numberOfSoldProduct");
                }
            }

            private string _dateOfSale;
            public string dateOfSale
            {
                get => _dateOfSale;
                set
                {
                    if (Equals(_dateOfSale, value)) return;
                    _dateOfSale = value;
                    OnPropertyChanged("dateOfSale");
                }
            }

            private string _timeOfSale;
            public string timeOfSale
            {
                get => _timeOfSale;
                set
                {
                    if (Equals(_timeOfSale, value)) return;
                    _timeOfSale = value;
                    OnPropertyChanged("timeOfSale");
                }
            }

            private string _userLoginOfSoldProduct;
            public string userLoginOfSoldProduct
            {
                get => _userLoginOfSoldProduct;
                set
                {
                    if (Equals(_userLoginOfSoldProduct, value)) return;
                _userLoginOfSoldProduct = value;
                    OnPropertyChanged("userLoginOfSoldProduct");
                }
            }



            private string _nameOfDelSoldProduct;
            public string nameOfDelSoldProduct
            {
                get => _nameOfDelSoldProduct;
                set
                {
                    if (Equals(_nameOfDelSoldProduct, value)) return;
                _nameOfDelSoldProduct = value;
                    OnPropertyChanged("nameOfDelSoldProduct");
                }
            }

        #endregion


            #region FieldsOfSearchSold

            private string _SearchByNOPSold;
            public string SearchByNOPSold
            {
                get => _SearchByNOPSold;
                set
                {
                    if (Equals(_SearchByNOPSold, value)) return;
                    _SearchByNOPSold = value;
                    OnPropertyChanged("SearchByNOPSold");
                }
            }

            private string _SearchByUserSold;
            public string SearchByUserSold
            {
                get => _SearchByUserSold;
                set
                {
                    if (Equals(_SearchByUserSold, value)) return;
                    _SearchByUserSold = value;
                    OnPropertyChanged("SearchByUserSold");
                }
            }

            private string _SearchByNumberSold;
            public string SearchByNumberSold
            {
                get => _SearchByNumberSold;
                set
                {
                    if (Equals(_SearchByNumberSold, value)) return;
                    _SearchByNumberSold = value;
                    OnPropertyChanged("SearchByNumberSold");
                }
            }

            private string _SearchByDateSold;
            public string SearchByDateSold
            {
                get => _SearchByDateSold;
                set
                {
                    if (Equals(_SearchByDateSold, value)) return;
                    _SearchByDateSold = value;
                    OnPropertyChanged("SearchByDateSold");
                }
            }

            private string _SearchByStartPriceSold;
            public string SearchByStartPriceSold
            {
                get => _SearchByStartPriceSold;
                set
                {
                    if (Equals(_SearchByStartPriceSold, value)) return;
                    _SearchByStartPriceSold = value;
                    OnPropertyChanged("SearchByStartPriceSold");
                }
            }

            private string _SearchByFinishPriceSold;
            public string SearchByFinishPriceSold
            {
                get => _SearchByFinishPriceSold;
                set
                {
                    if (Equals(_SearchByFinishPriceSold, value)) return;
                    _SearchByFinishPriceSold = value;
                    OnPropertyChanged("SearchByFinishPriceSold");
                }
            }
            #endregion

        #endregion






        #region Instance


        public static MainWindowViewModel instance = null;
        public static MainWindowViewModel getInstance()
        {
            if (instance == null)
                instance = new MainWindowViewModel();
            return instance;
        }


        #endregion
    }
}
