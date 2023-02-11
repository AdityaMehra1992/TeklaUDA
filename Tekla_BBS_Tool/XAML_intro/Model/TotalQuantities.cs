using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XAML_intro.Model
{
    public class TotalQuantities
    {

        private double _partWeight;
        private double _volume;

        private double _rebarWeight;
        private double _rebar6mm;
        private double _rebar8mm;
        private double _rebar10mm;
        private double _rebar12mm;
        private double _rebar16mm;
        private double _rebar20mm;
        private double _rebar25mm;
        private double _rebar32mm;
        private double _rebar36mm;
        private double _rebar40mm;

        public double Volume 
        { 
            get { return Math.Round((Double)_volume, 1); } 
            set { _volume = value; }
        }
        public double Weight
        { 
            get { return Math.Round((Double)_partWeight, 1); } 
            set { _partWeight=value; }
        }

        public double RebarWeight
        {
            get { return Math.Round((Double)_rebarWeight, 1); }
            set { _rebarWeight = value; }
        }

        public double Rebar6mm
        {
            get { return Math.Round((Double)_rebar6mm, 1); }
            set { _rebar6mm = value; }
        }

        public double Rebar8mm
        {
            get { return Math.Round((Double)_rebar8mm, 1); }
            set { _rebar8mm = value; }
        }

        public double Rebar10mm
        {
            get { return Math.Round((Double)_rebar10mm, 1); }
            set { _rebar10mm = value; }
        }

        public double Rebar12mm
        {
            get { return Math.Round((Double)_rebar12mm, 1); }
            set { _rebar12mm = value; }
        }

        public double Rebar16mm
        {
            get { return Math.Round((Double)_rebar16mm, 1); }
            set { _rebar16mm = value; }
        }

        public double Rebar20mm
        {
            get { return Math.Round((Double)_rebar20mm, 1); }
            set { _rebar20mm = value; }
        }

        public double Rebar25mm
        {
            get { return Math.Round((Double)_rebar25mm, 1); }
            set { _rebar25mm = value; }
        }

        public double Rebar32mm
        {
            get { return Math.Round((Double)_rebar32mm, 1); }
            set { _rebar32mm = value; }
        }

        public double Rebar36mm
        {
            get { return Math.Round((Double)_rebar36mm, 1); }
            set { _rebar36mm = value; }
        }

        public double Rebar40mm
        {
            get { return Math.Round((Double)_rebar40mm, 1); }
            set { _rebar40mm = value; }
        }
    }
}
