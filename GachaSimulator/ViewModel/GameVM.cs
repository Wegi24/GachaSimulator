﻿
namespace GachaSimulator.ViewModel
{
    public class GameVM
    {
        public string desc;
        public string gameCurr;
        public double exchangeRate; // = how much currency per €
        public int tenShotCost;
        public double antiPercentage;

        public GameVM(string desc, string gameCurr)
        {
            this.desc = desc;
            this.gameCurr = gameCurr;


            //Values are filled accordingly to real prices
            if(this.desc.Equals("Dragalia Lost"))
            {
                exchangeRate = 49.2;
                tenShotCost = 1200;
                antiPercentage = 0.995;
            }
            else if (this.desc.Equals("Pokemon Masters"))
            {
                exchangeRate = 108.88888;
                tenShotCost = 3000;
                antiPercentage = 0.98;
            }
            else if (this.desc.Equals("Fate/Grand Order"))
            {
                exchangeRate = 2.0875;
                tenShotCost = 30;
                antiPercentage = 0.993;
            }else if (this.desc.Equals("Fire Emblem Heroes"))
            {
                exchangeRate = 1.875;
                tenShotCost = 45;
                antiPercentage = 0.975;
            }
            else if (this.desc.Equals("Arknights"))
            {
                exchangeRate =375;  //varies between 300 and 450, depending on whether double bonus applies
                tenShotCost = 6000;
                antiPercentage = 0.99;
            }
            else if (this.desc.Equals("Epic Seven"))
            {
                exchangeRate = 1.82;
                tenShotCost = 50;
                antiPercentage = 0.9875;
            }
        }


    }
}