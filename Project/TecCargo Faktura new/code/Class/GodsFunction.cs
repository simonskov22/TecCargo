using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecCargo_Faktura.Class
{
    class GodsFunction
    {
        public double GetCalKilo(int Bregning, int VolumeL = 0, int VolumeB = 0, int VolumeH = 0)
        {
            const int Cal_Ladmeter = 0;
            const int Cal_Palleplads = 1;
            const int Cal_Volume = 2;


            switch (Bregning)
            {
                case Cal_Ladmeter:
                    double enLadmeter = 1500 / 100;
                    return (enLadmeter * VolumeL);

                case Cal_Palleplads:
                    return 600;  
                                      
                case Cal_Volume:
                    return (VolumeL * VolumeB * VolumeH * 250) / 1000000;                    
            }

            return -1;
        }
        public double GetPrice(int Takst, double Kilo)
        {
            Takst--; //gå en mindre så man kan bruge 0

            //stop hvis taskt bliver mindre end 0
            if (Takst < 0)
                return 0;

            int kiloID = Models.FakturaPrisliste.godsPriser.Kilos.Length -1;

            //hent kilo index
            for (int i = 0; i < Models.FakturaPrisliste.godsPriser.Kilos.Length; i++)
            {
                if (Kilo <= Models.FakturaPrisliste.godsPriser.Kilos[i])
                {
                    kiloID = i;
                    break;
                }
            }

            //hent start pris
            double startPrice = Models.FakturaPrisliste.godsPriser.mainPrice[kiloID];

            //find procent
            double procentKategori = Math.Ceiling((kiloID + 1) / 5f);
            int procentKategoriId = 0;
            int.TryParse(procentKategori.ToString(), out procentKategoriId);//gør så man kan bruge den som array index

            double procentPrice = (startPrice / 100) * Models.FakturaPrisliste.godsPriser.PriceProcent[procentKategoriId - 1][Takst];
            
            return startPrice + procentPrice;
        }
    }
}
