using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GachaSimulator.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        //Textboxes inkl refresh functions
        #region 
        private double selGameCurr;
        private double selMoney;
        private string selGameDesc;
        private string selGameCurrType;
        private double tenShotNr;
        private double percOfSucc;

        public double SelMoney { get => selMoney; set { selMoney = value; RaisePropertyChanged(); OnMoneyChange(); } }

        private void OnMoneyChange()
        {
            if (SelMoney != 0)
                SelGameCurr = SelGame.exchangeRate * SelMoney;

        }

        public double SelGameCurr { get => selGameCurr; set { selGameCurr = value; RaisePropertyChanged(); OnGameCurrChange(); } }

        private void OnGameCurrChange()
        {
            if(TenShotNr!= SelGameCurr / SelGame.tenShotCost)
            TenShotNr = SelGameCurr / SelGame.tenShotCost;


            CalculatePercentage();
            if (SelMoney != SelGameCurr / SelGame.exchangeRate)
                SelMoney = SelGameCurr / SelGame.exchangeRate;
        }

        private void CalculatePercentage()
        {
               PercOfSucc = 1.00 - Math.Pow(SelGame.antiPercentage, TenShotNr * 10);
        }
        

        public double TenShotNr { get => tenShotNr; set { tenShotNr = value; RaisePropertyChanged(); OnTenShotChange(); } }

        private void OnTenShotChange()
        {
            SelGameCurr = TenShotNr * SelGame.tenShotCost;
        }

        public double PercOfSucc { get => percOfSucc; set { percOfSucc = value; RaisePropertyChanged(); } }

        #endregion

        //Handling of Selected Game
        public GameVM SelGame { get; set; }
        public string SelGameDesc { get => selGameDesc; set { selGameDesc = value; RaisePropertyChanged(); } }
        public string SelGameCurrType { get => selGameCurrType; set { selGameCurrType = value; RaisePropertyChanged(); } }
       
        //RelayCommands for Games
        public List<GameVM> Gamelist { get; set; }
        public RelayCommand FehClick { get; set; }
        public RelayCommand DragClick { get; set; }
        public RelayCommand FgoClick { get; set; }
        public RelayCommand PkmnClick { get; set; }


        //Checkbox RelayCommands
        private string ComboTmp;
        public RelayCommand MoneyCheck { get; set; }
        public RelayCommand GameCurrCheck { get; set; }
        public RelayCommand TenShotCheck { get; set; }


       

        

        public MainViewModel()
        {
            Gamelist = new List<GameVM>();
            GenGamelist();

            SelGame = Gamelist[1];
            SetGameAttributes();

            InitiateRelayCommands();

            InitiateComboBoxCommands();

            //Because SelMoney is ticked in xaml
            ComboTmp = "Money";

        }


        //Comboboxes only accessible if ComboTmp is null or checked(aka ComboTmp is filled respectively) 
        private void InitiateComboBoxCommands() 
        {

            MoneyCheck = new RelayCommand(() =>
            {
                if (ComboTmp != null)
                {
                   
                        ComboTmp = null;
                    

                }
                else
                {
                    ComboTmp = "Money";
                }
            }, () =>
            {
                return ComboTmp == null || ComboTmp.Equals("Money");
            });

            GameCurrCheck = new RelayCommand(() =>
            {
                if (ComboTmp != null)
                {
                   
                    
                        ComboTmp = null;
                    

                }
                else
                {
                    ComboTmp = "GameCurr";
                }
            }, () =>
            {
                return ComboTmp == null || ComboTmp.Equals("GameCurr");
            });

            TenShotCheck = new RelayCommand(() =>
            {
                if (ComboTmp != null)
                {
                    
                    
                        ComboTmp = null;
                    

                }
                else
                {
                    ComboTmp = "TenShot";
                }
            }, () =>
            {
                return ComboTmp == null || ComboTmp.Equals("TenShot");
            });
        }


        
        private void InitiateRelayCommands()
        {
        //Sets SelGame, Attributes, Value that will be carried over
            DragClick = new RelayCommand(
            () =>
            {
                SelGame = Gamelist[1];
                SetGameAttributes();
                CheckCarryValue();
            },
            () =>
            {
                return SelGame != Gamelist[1];
            });
            //--
            FehClick = new RelayCommand(
           () =>
           {
               SelGame = Gamelist[0];
               SetGameAttributes();
               CheckCarryValue();
           },
           () =>
           {
               return SelGame != Gamelist[0];
           });
            //--
            PkmnClick = new RelayCommand(
           () =>
           {
               SelGame = Gamelist[2];
               SetGameAttributes();
               CheckCarryValue();
           },
           () =>
           {
               return SelGame != Gamelist[2];
           });
            //--
            FgoClick = new RelayCommand(
           () =>
           {
               SelGame = Gamelist[3];
               SetGameAttributes();
               CheckCarryValue();
           },
           () =>
           {
               return SelGame != Gamelist[3];
           });
        }

        private void CheckCarryValue()
        {
            if (ComboTmp != null)
            {
                if (ComboTmp.Equals("Money"))
                    SelMoney = SelMoney;
                else if (ComboTmp.Equals("GameCurr"))
                    SelGameCurr = SelGameCurr;
                else if (ComboTmp.Equals("TenShot"))
                    TenShotNr = TenShotNr;
            }
            else
            {//Nothing is checked
                SelGameCurr = 0;
            }
        }

        private void SetGameAttributes()
        {
            SelGameDesc = SelGame.desc;
            SelGameCurrType = SelGame.gameCurr;
        }

        private void GenGamelist()
        {
            Gamelist.Add(new GameVM("Fire Emblem Heroes", "Orbs"));
            Gamelist.Add(new GameVM("Dragalia Lost", "Wyrmite"));
            Gamelist.Add(new GameVM("Pokemon Masters", "Gems"));
            Gamelist.Add(new GameVM("Fate/Grand Order", "Saint Quarz"));
        }
    }
}