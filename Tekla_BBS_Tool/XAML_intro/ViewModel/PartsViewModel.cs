using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using XAML_intro.Command;
using XAML_intro.Model;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using TSM = Tekla.Structures.Model;
using PieControls;


namespace XAML_intro.ViewModel
{
    public class PartsViewModel : INotifyPropertyChanged
    {
        // Private properties
        #region Private properties
        private ObservableCollection<Part> _TeklaPartsList;
        private ObservableCollection<TotalQuantities> _SelectedTotalQuantity;
        private ObservableCollection<Rebar> _AllSlectedPartRebarList;
        private ObservableCollection<Rebar> _SlectedPartRebarList;
        private ObservableCollection<Quantities> _SlectedPartQuantitiesList;
        private ObservableCollection<PieSegment> _pieCollection;
        private ObservableCollection<PieSegment> _pieCollection_2;
        private string modellink;
        private List<string> pnameList;
        private List<string> rnameList;


        #endregion

        // Property access
        #region Access Modifiers
        public ObservableCollection<Part> TeklaPartsList
        {
            get { return _TeklaPartsList; }
            set
            {
                if (value != null)
                {
                    _TeklaPartsList = value;
                    NotifyPropertyChanged("TeklaPartsList");
                }
            }
        }

        public ObservableCollection<TotalQuantities> SelectedTotalQuantity
        {
            get { return _SelectedTotalQuantity; }
            set
            {

                if (value != null)
                {
                    _SelectedTotalQuantity = value;
                    NotifyPropertyChanged("SelectedTotalQuantity");
                }
            }
        }

        public ObservableCollection<Rebar> AllSlectedPartRebarList
        {
            get { return _AllSlectedPartRebarList; }
            set
            {
                if (value != null)
                {
                    _AllSlectedPartRebarList = value;
                    NotifyPropertyChanged("SlectedPartRebarList");
                }
            }
        }

        public ObservableCollection<Rebar> SlectedPartRebarList
        {
            get { return _SlectedPartRebarList; }
            set
            {
                if (value != null)
                {
                    _SlectedPartRebarList = value;
                    NotifyPropertyChanged("SlectedPartRebarList");
                }
            }
        }

        public ObservableCollection<Quantities> SlectedPartQuantitiesList
        {
            get { return _SlectedPartQuantitiesList; }
            set
            {
                if (value != null)
                {
                    _SlectedPartQuantitiesList = value;
                    NotifyPropertyChanged("SlectedPartQuantitiesList");
                }
            }
        }

        public ObservableCollection<PieSegment> PieCollection
        {
            get { return _pieCollection; }
            set
            {
                if (value != null)
                {
                    _pieCollection = value;
                    NotifyPropertyChanged("PieCollection");
                }
            }

        }
        public ObservableCollection<PieSegment> PieCollection_2
        {
            get { return _pieCollection_2; }
            set
            {
                if (value != null)
                {
                    _pieCollection_2 = value;
                    NotifyPropertyChanged("PieCollection");
                }
            }

        }


        public string Modellink
        {
            get { return modellink; }
        }


        public List<string> PnameList
        {
            get { return pnameList; }
            set { pnameList = value; }
        }
        
        public List<string> RPosList
        {
            get { return rnameList; }
            set { rnameList = value; }
        }


        #endregion


        public PartsViewModel()
        {
            TeklaPartsList = new ObservableCollection<Part>();
            AllSlectedPartRebarList = new ObservableCollection<Rebar>();
            SlectedPartRebarList = new ObservableCollection<Rebar>();
            SlectedPartQuantitiesList = new ObservableCollection<Quantities>();
            SelectedTotalQuantity = new ObservableCollection<TotalQuantities>();
            try
            {
                modellink = new TSM.Model().GetInfo().ModelPath;
            }
            catch { MessageBox.Show("There is no instance of tekla available."); }
            PnameList = new List<string>();
            RPosList = new List<string>();
            PieCollection = new ObservableCollection<PieSegment>();
            PieCollection_2 = new ObservableCollection<PieSegment>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

