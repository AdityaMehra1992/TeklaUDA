using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;
using System.ComponentModel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace XAML_intro.Model
{
    public class Part : INotifyPropertyChanged
    {
        private TSM.Part teklaPart;
        private string teklaPartName;
        private int numofUnit;
        private Tekla.Structures.Identifier teklaPartIdentifier;
        private int? index;
        private string material;
        private double volume;
        private double weight;
        private string _position;
        

        private static int Partindex = 0;

        public Part(TSM.Part teklaPart)
        {
            this.teklaPartName = teklaPart.Name;
            this.teklaPartIdentifier = teklaPart.Identifier;
            this.numofUnit = 1;
            this.index = ++Partindex;
            this.material = teklaPart.Material.MaterialString;
            
        }

        public string TeklaPartName
        {
            get { return teklaPartName;}
        }

        public string Material
        {
            get { return material; }
        }

        public Tekla.Structures.Identifier TeklaPartIdentifier
        {
            get { return teklaPartIdentifier;}
        }

        public TSM.Part TeklaPart
        {
            get {
                    TSM.Model M = new TSM.Model();
                    teklaPart = (TSM.Part)M.SelectModelObject(TeklaPartIdentifier);
                    return teklaPart;
                }
        }



        public int NumofUnit
        {
            get
            {
                return numofUnit;
            }

            set { numofUnit = value; OnPropertyChanged("NumofUnit"); }

        }

        public int? Index
        {
            get
            {
                return index;
            }
        }

        public double Volume
        {
            get
            {
                double VOLUME = 0;
                TeklaPart.GetReportProperty("VOLUME", ref VOLUME);
                volume = Math.Round((Double)(VOLUME / 1000000), 3); 
                return volume;

            }
        }

        public string Position
        {
            get
            {
                string POSITION = "";
                TeklaPart.GetReportProperty("PART_POS", ref POSITION);
                _position = POSITION;
                return _position;
            }
        }

        public double Weight
        {
            get
            {
                double WEIGHT = 0;
                this.TeklaPart.GetReportProperty("WEIGHT", ref WEIGHT);
                weight = Math.Round((Double)(WEIGHT), 0);
                return weight;

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string p)
        {
            PropertyChangedEventHandler ph = PropertyChanged;
            if (ph != null)
                ph(this, new PropertyChangedEventArgs(p));
        }

    }
}
