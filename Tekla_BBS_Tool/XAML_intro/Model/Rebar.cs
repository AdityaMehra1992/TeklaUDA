using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using Tekla.Structures.Model.UI;
using System.ComponentModel;

namespace XAML_intro.Model
{
    public class Rebar : INotifyPropertyChanged
    {
        private TSM.Reinforcement _rebar;
        private string _position;
        private int _rebarNum;
        private string _size;
        private string _parentname;
        private int numofUnit;
        private string _shape;
        private double _length;
        private double _A; private double _B; private double _C; private double _D; private double _E; private double _F; private double _G;
        private double _total_weight;

        public Rebar(TSM.Reinforcement teklaRebar, int units, string parentname)
        { 
            this._rebar = teklaRebar;
            this._rebarNum = teklaRebar.GetNumberOfRebars();
            this.numofUnit = units;
            this._parentname = parentname;
        }

        public int NumofUnit
        {
            get
            {
                return numofUnit;
            }

            set { numofUnit = value;}

        }

        public string ParentName
        {
            get
            {
                return _parentname;
            }

            //set { _parentname = value; }

        }


        public TSM.Reinforcement TeklaRebar
        { get { return _rebar; } }

        public string Position
        {
            get {
                    string POSITION = "";
                    _rebar.GetReportProperty("POS", ref POSITION);
                    _position = POSITION;
                    return _position;
            }
        }

        public int RebarNum
        {
            get
            {
                return _rebarNum * numofUnit;
            }
        }

        public string Size
        {
            get
            {
                string SIZE = "";
                _rebar.GetReportProperty("SIZE", ref SIZE);
                _size = SIZE;
                return _size;
            }

        }

        public string Shape
        {
            get
            {
                string SHAPE = "";
                _rebar.GetReportProperty("SHAPE", ref SHAPE);
                _shape = SHAPE;
                return _shape;
            }

        }


        public double Length
        {
            get
            {
                double LENGTH = 0;
                _rebar.GetReportProperty("LENGTH", ref LENGTH);
                _length = LENGTH;
                return _length;
            }

        }

        #region dim
        public double Dim_A
        {
            get
            {
                double DIMA = 0;
                _rebar.GetReportProperty("DIM_A", ref DIMA);
                _A = DIMA;
                return _A;
            }

        }

        public double Dim_B
        {
            get
            {
                double DIMB = 0;
                _rebar.GetReportProperty("DIM_B", ref DIMB);
                _B = DIMB;
                return _B;
            }

        }

        public double Dim_C
        {
            get
            {
                double DIMC = 0;
                _rebar.GetReportProperty("DIM_C", ref DIMC);
                _C = DIMC;
                return _C;
            }

        }

        public double Dim_D
        {
            get
            {
                double DIMD = 0;
                _rebar.GetReportProperty("DIM_B", ref DIMD);
                _D = DIMD;
                return _D;
            }

        }

        public double Dim_E
        {
            get
            {
                double DIME = 0;
                _rebar.GetReportProperty("DIM_E", ref DIME);
                _E = DIME;
                return _E;
            }

        }

        public double Dim_F
        {
            get
            {
                double DIMF = 0;
                _rebar.GetReportProperty("DIM_F", ref DIMF);
                _F = DIMF;
                return _F;
            }

        }

        public double Dim_G
        {
            get
            {
                double DIMG = 0;
                _rebar.GetReportProperty("DIM_G", ref DIMG);
                _G = DIMG;
                return _G;
            }

        }


        #endregion

        public double TotalWeight
        {
            get
            {
                double TOTAL_WEIGHT = 0;
                _rebar.GetReportProperty("TOTAL_WEIGHT", ref TOTAL_WEIGHT);
                _total_weight = Math.Round((Double)TOTAL_WEIGHT, 1);
                return _total_weight* numofUnit;
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
