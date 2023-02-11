using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using TSM = Tekla.Structures.Model;
using TSG = Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.UI;
using XAML_intro.Model;
using System.Windows;
using static Tekla.Structures.Model.UI.Picker;

namespace XAML_intro.TeklaFunction
{
    internal class BasicFunctions
    {
        public static TSM.Part PickPart()
        {
            Picker Picker = new Picker();
            TSM.Part Ass = null;// = new TSM.Assembly();
            try
            {
                Ass = (TSM.Part)Picker.PickObject(Picker.PickObjectEnum.PICK_ONE_OBJECT);
                return Ass;


            }
            catch { return Ass; }
        }

        public static ModelObjectEnumerator PickParts()
        {
            Picker Picker = new Picker();
            ModelObjectEnumerator Ass = null;
            try
            {
                Ass = Picker.PickObjects(PickObjectsEnum.PICK_N_OBJECTS, "Pick all the Parts for which quantity is required. ");
                return Ass;


            }
            catch { return Ass; }
        }

        public static void ReinformentQuantity(XAML_intro.Model.Part par, List<Rebar> rebarList,List<Quantities>QuanList, TotalQuantities TQ)
        {
            try
            {
                TSM.ModelObjectEnumerator Reinenu = par.TeklaPart.GetReinforcements();
                Quantities Q = new Quantities(par, par.NumofUnit);

                while (Reinenu.MoveNext())
                {
                    TSM.Reinforcement R = Reinenu.Current as TSM.Reinforcement;
                    Rebar r = new Rebar(R, par.NumofUnit, par.TeklaPartName);
                    rebarList.Add(r);
                    double r_size = Convert.ToDouble(r.Size);
                    #region Switch
                    switch (r_size)
                    {
                        case 6:
                            Q.Rebar6mm += r.TotalWeight;
                            Q.RebarWeight += r.TotalWeight;
                            TQ.Rebar6mm += r.TotalWeight;
                            TQ.RebarWeight += r.TotalWeight;
                            break;

                        case 8:
                            Q.Rebar8mm += r.TotalWeight;
                            Q.RebarWeight += r.TotalWeight;
                            TQ.Rebar8mm += r.TotalWeight;
                            TQ.RebarWeight += r.TotalWeight;
                            break;

                        case 10:
                            Q.Rebar10mm += r.TotalWeight;
                            Q.RebarWeight += r.TotalWeight;
                            TQ.Rebar10mm += r.TotalWeight;
                            TQ.RebarWeight += r.TotalWeight;
                            break;

                        case 12:
                            Q.Rebar12mm += r.TotalWeight;
                            Q.RebarWeight += r.TotalWeight;
                            TQ.Rebar12mm += r.TotalWeight;
                            TQ.RebarWeight += r.TotalWeight;
                            break;

                        case 16:
                            Q.Rebar16mm += r.TotalWeight;
                            Q.RebarWeight += r.TotalWeight;
                            TQ.Rebar16mm += r.TotalWeight;
                            TQ.RebarWeight += r.TotalWeight;
                            break;

                        case 20:
                            Q.Rebar20mm += r.TotalWeight;
                            Q.RebarWeight += r.TotalWeight;
                            TQ.Rebar20mm += r.TotalWeight;
                            TQ.RebarWeight += r.TotalWeight;
                            break;

                        case 25:
                            Q.Rebar25mm += r.TotalWeight;
                            Q.RebarWeight += r.TotalWeight;
                            TQ.Rebar25mm += r.TotalWeight;
                            TQ.RebarWeight += r.TotalWeight;
                            break;

                        case 32:
                            Q.Rebar32mm += r.TotalWeight;
                            Q.RebarWeight += r.TotalWeight;
                            TQ.Rebar32mm += r.TotalWeight;
                            TQ.RebarWeight += r.TotalWeight;
                            break;

                        case 40:
                            Q.Rebar40mm += r.TotalWeight;
                            Q.RebarWeight += r.TotalWeight;
                            TQ.Rebar40mm += r.TotalWeight;
                            TQ.RebarWeight += r.TotalWeight;
                            break;

                    }
                    #endregion
                }

                QuanList.Add(Q);
                TQ.Weight += Q.Weight;
                TQ.Volume += Q.Volume;
            }
            catch
            {

                MessageBox.Show("ERROR!! Selected Parts ("+ par.TeklaPartName + ") can not be found. Remove it and reselect it.");
            }
        }

        
    }
}
