using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Data;

namespace TecCargo_Læreplads_DB.Inc
{
    class Datagrid
    {
        public ICollectionView personInfo { get; private set; }
        public ICollectionView candidatesInfo { get; private set; }
        public ICollectionView languageInfo { get; private set; }

        public Datagrid()
        {
            var _personInfo = new List<person>();
            var _candidates = new List<person>();
            var _language = new List<person>();

            personInfo = CollectionViewSource.GetDefaultView(_personInfo);
            candidatesInfo = CollectionViewSource.GetDefaultView(_candidates);
            languageInfo = CollectionViewSource.GetDefaultView(_language);
        }
    }

    public class person
    {

        public string name { get; set; }
        public string mobil { get; set; }
        public string postion { get; set; }
        public string mail { get; set; }
        public bool print { get; set; }
    }

    
}