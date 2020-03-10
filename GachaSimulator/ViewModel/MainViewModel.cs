using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Threading;

namespace GachaSimulator.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        //Textboxes inkl refresh functions
        #region GUI Felder
        private double selGameCurr;
        private double selMoney;
        private string selGameDesc;
        private string selGameCurrType;
        private double tenShotNr;
        private double percOfSucc;

        public double SelMoney { get => selMoney; set { selMoney = value; RaisePropertyChanged(); OnMoneyChange(); } }

        private void OnMoneyChange()
        {
            if (SelMoney != 0|| SelGameCurr != SelGame.exchangeRate * SelMoney)
                SelGameCurr = SelGame.exchangeRate * SelMoney;

        }

        public double SelGameCurr { get => selGameCurr; set { selGameCurr = value; RaisePropertyChanged(); OnGameCurrChange(); } }

        private void OnGameCurrChange()
        {
            if (TenShotNr != SelGameCurr / SelGame.tenShotCost)
                TenShotNr = SelGameCurr / SelGame.tenShotCost;

            CalculatePercentage();

            if (SelMoney != SelGameCurr / SelGame.exchangeRate)
                SelMoney = SelGameCurr / SelGame.exchangeRate;
        }

        private void CalculatePercentage()
        {
            PercOfSucc = 1.00 - Math.Pow(CalcPerc, TenShotNr * 10);
        }



        public double TenShotNr { get => tenShotNr; set { tenShotNr = value; RaisePropertyChanged(); OnTenShotChange(); } }

        private void OnTenShotChange()
        {
            if(SelGameCurr != TenShotNr * SelGame.tenShotCost)
            SelGameCurr = TenShotNr * SelGame.tenShotCost;
        }

        public double PercOfSucc { get => percOfSucc; set { percOfSucc = value; RaisePropertyChanged(); } }

       

        //Percentage in GUI
        public double GUIPercentage { get => gUIPercentage; set { gUIPercentage = value;  RaisePropertyChanged(); SetCalcPercentage(); } }
        //variable for calculations
        public double CalcPerc { get; set; }
        //Override percentage y/n, standard no
        private bool OverridePerc = false;
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
        public RelayCommand ArkClick { get; set; }

        //Checkbox RelayCommands
        private string ComboTmp;
        private double gUIPercentage;

        public RelayCommand MoneyCheck { get; set; }
        public RelayCommand GameCurrCheck { get; set; }
        public RelayCommand TenShotCheck { get; set; }
        public RelayCommand OverridePercentageCheck { get; set; }

        private void SetCalcPercentage()
        {
            
            //Override Percentage
            if (OverridePerc == true)
            {
                CalcPerc = 1 - GUIPercentage;
                CalculatePercentage();
            }
            else   //dont Override Percentage
            {
               
                if (GUIPercentage != 1 - SelGame.antiPercentage)
                {
                    GUIPercentage = 1 - SelGame.antiPercentage;
                    CalcPerc = SelGame.antiPercentage;
                    CalculatePercentage();
                }
            }
        }





        public MainViewModel()
        {
            

            Gamelist = new List<GameVM>();
            GenGamelist();

            SelGame = Gamelist[1];
            SetCalcPercentage();

            SetGameAttributes();

            InitiateRelayCommands();

            InitiateComboBoxCommands();

            //Because SelMoney is ticked in xaml
            ComboTmp = "Money";

        }


        //Comboboxes only accessible if ComboTmp is null or checked(aka ComboTmp is filled respectively) 
        private void InitiateComboBoxCommands()
        {

            OverridePercentageCheck = new RelayCommand(() =>
              {
                  if (OverridePerc == false)
                  {
                      OverridePerc = true;
                     
                  }
                  else
                  {
                      OverridePerc = false;
                      SetCalcPercentage();
                  }
              });

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
                SetCalcPercentage();
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
               SetCalcPercentage();
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
               SetCalcPercentage();
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
               SetCalcPercentage();
           },
           () =>
           {
               return SelGame != Gamelist[3];
           });
            ArkClick = new RelayCommand(
            () =>
            {
                SelGame = Gamelist[4];
                SetGameAttributes();
                CheckCarryValue();
                SetCalcPercentage();
            },
            () =>
            {
                return SelGame != Gamelist[4];
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
            Gamelist.Add(new GameVM("Arknights", "Orundum"));
        }
    }
}