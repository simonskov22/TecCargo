using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;

namespace TecCargo_Faktura.Models
{
    class DataGridSources
    {

        public ICollectionView godsIndhold { get; private set; }
        public ICollectionView FakturaTransportDataView { get; private set; }

        public ICollectionView AdminKundeDataView { get; private set; }
        public ICollectionView AdminFragtbrevPdfDataView { get; private set; }
        public ICollectionView AdminSaveFragtbrevDataView { get; private set; }

        public ICollectionView FakturaPriceDataView { get; private set; }

        public DataGridSources()
        {
            var _godsLinjer = new List<godsIndholdData>();
            var _FakturaTransportDataView = new List<FakturaTransportDataClass>();

            var _AdminKundeDataView = new List<AdminKundeDataClass>();
            var _AdminFragtbrevPdfDataView = new List<AdminFragtbrevPdfDataClass>();
            var _AdminSaveFragtbrevDataView = new List<AdminSaveFragtbrevDataClass>();

            var _FakturaPriceDataView = new List<FakturaPricelistDataClass>();

            godsIndhold = CollectionViewSource.GetDefaultView(_godsLinjer);
            FakturaTransportDataView = CollectionViewSource.GetDefaultView(_FakturaTransportDataView);

            AdminKundeDataView = CollectionViewSource.GetDefaultView(_AdminKundeDataView);
            AdminFragtbrevPdfDataView = CollectionViewSource.GetDefaultView(_AdminFragtbrevPdfDataView);
            AdminSaveFragtbrevDataView = CollectionViewSource.GetDefaultView(_AdminSaveFragtbrevDataView);

            FakturaPriceDataView = CollectionViewSource.GetDefaultView(_FakturaPriceDataView);
        }
    }

    public class emptyValues
    {
        public string value1 { get; set; }
        public string value2 { get; set; }
        public string value3 { get; set; }
        public string value4 { get; set; }
        public string value5 { get; set; }
    }

    public class godsIndholdData
    {
        public string AdresseNumber { get; set; }
        public string Indhold { get; set; }
        public int Antal { get; set; }
        public string Art { get; set; }
        public string PakkeStr { get; set; }
        public string Rumfang { get; set; }
        public double Weight { get; set; }
        public int ladmeterC { get; set; }
        public int pallepladsC { get; set; }
        public int volumeC { get; set; }
    }

    //public class FakturaTransportKurrerDataClass
    //{
    //    [DisplayName("Reference nummer")]

    //    public int RefNumber { get; set; }
    //    public string DecText { get; set; }
    //    public double Kilo { get; set; }
    //    public string[] MAdresser { get; set; }
    //}

    //public class FakturaTransportPakkeDataClass
    //{
    //    public int RefNumber { get; set; }
    //    public string[] PakkeStr { get; set; }
    //    public double RealKilo { get; set; }
    //    public double Prices { get; set; }
    //    public string[] MAdresser { get; set; }
    //}

    //public class FakturaTransportGodsDataClass
    //{
    //    public int RefNumber { get; set; }
    //    public string[] Takst { get; set; }
    //    public string[] BeregnType { get; set; }

    //    public double LengthL { get; set; }
    //    public double WidthtB { get; set; }
    //    public double HeightH { get; set; }

    //    public double RealKilo { get; set; }
    //    public double BeregnKilo { get; set; }
    //    public double Prices { get; set; }

    //    public string[] MAdresser { get; set; }
    //}

    public class FakturaPricelistDataClass
    {
        public string name { get; set; }
        public string price { get; set; }
    }

    public class FakturaTransportDataClass
    {
        public int idNumber { get; set; }
        public string DecText { get; set; }
        public double RealKilo { get; set; }
        public double BeregnKilo { get; set; }
        public double Prices { get; set; }

        public string MAdresser { get; set; }
        public string PakkeStr { get; set; }
        public string Takst { get; set; }
        public string BeregnType { get; set; }

        public double LengthL { get; set; }
        public double WidthtB { get; set; }
        public double HeightH { get; set; }
    }



    public class AdminKundeDataClass
    {
        public string KontaktNumber { get; set; }
        public string FirmaName { get; set; }
        public string Adresse { get; set; }
        public string PostNumber { get; set; }
        public bool Delete { get; set; }
    }

    public class AdminFragtbrevPdfDataClass
    {
        public string FileName { get; set; }
        public string LastModify { get; set; }
        public bool Delete { get; set; }
    }

    public class AdminSaveFragtbrevDataClass
    {
        public string FileName { get; set; }
        public string LastModify { get; set; }
        public bool Delete { get; set; }
    }
}
