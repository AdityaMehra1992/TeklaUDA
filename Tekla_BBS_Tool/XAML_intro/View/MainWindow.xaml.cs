using System.Windows;
using System.Windows.Controls;
using XAML_intro.ViewModel;
using XAML_intro.TeklaFunction;
using TSM = Tekla.Structures.Model;
using System.Windows.Media.Media3D;
using System.Linq;
using XAML_intro.Model;
using System;
using System.Collections.Generic;
using PieControls;
using System.Windows.Media;
using System.Drawing;
using Color = System.Drawing.Color;
using Tekla.Structures.Model;

namespace XAML_intro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new PartsViewModel();

            // Hi, This code was written to test the functionality of WPF with the Tekla API.
            // I acknowledge that the code lacks proper structure and does not follow basic programming
            // practices due to my eagerness to see the results.
            // I plan to update and improve the code as soon as I have the time.

        }

        private void AddPartsToList(object sender, RoutedEventArgs e)
        {
            PartsViewModel pvm = (PartsViewModel)DataContext;
            ModelObjectEnumerator AA = BasicFunctions.PickParts();
            if (AA != null)
            {
                while (AA.MoveNext())
                    if (AA.Current as TSM.Part != null)
                    {
                        pvm.TeklaPartsList.Add(new XAML_intro.Model.Part(AA.Current as TSM.Part));
                    }
            }

        }

        private void RemovePartsFromList(object sender, RoutedEventArgs e)
        {
            PartsViewModel pvm = (PartsViewModel)DataContext;

            List<int> selectedindex = new List<int>();
            List<int> PartListindex = new List<int>();

            foreach (Model.Part p in TeklaPartsListXAML.SelectedItems)
            { selectedindex.Add((int)p.Index); }
            selectedindex.Sort();

            foreach (Model.Part p in pvm.TeklaPartsList)
            { PartListindex.Add((int)p.Index); }

            int num = 0;
            foreach (int i in selectedindex)
            {
                int DltIndex = PartListindex.IndexOf(i);
                pvm.TeklaPartsList.RemoveAt(DltIndex - num);
                num++;

            }
        }

        private void UpdateQuantities(object sender, RoutedEventArgs e)
        {
            PartsViewModel pvm = (PartsViewModel)DataContext;
            pvm.AllSlectedPartRebarList.Clear();
            pvm.SlectedPartQuantitiesList.Clear();
            pvm.SelectedTotalQuantity.Clear();
            TotalQuantities TQ = new TotalQuantities();

            if (TeklaPartsListXAML.SelectedItems.Count != 0)
            {


                foreach (Model.Part p in TeklaPartsListXAML.SelectedItems)
                {
                    List<Rebar> rebarList = new List<Rebar>();
                    List<Quantities> QuanList = new List<Quantities>();

                    BasicFunctions.ReinformentQuantity(p, rebarList, QuanList, TQ);
                    foreach (Rebar R in rebarList) { pvm.AllSlectedPartRebarList.Add(R); }
                    foreach (Quantities Q in QuanList) { pvm.SlectedPartQuantitiesList.Add(Q); }
                }
            }
            pvm.SelectedTotalQuantity.Add(TQ);
            UpdateVisualizationTab();

            //Update the ComboBox
            ComBoxSelected.Items.Clear();
            
            foreach (Model.Quantities P in pvm.SlectedPartQuantitiesList)
            {
                ComBoxSelected.Items.Add(P.PartName);
            }
                


        }

        private void UpdateUnit(object sender, RoutedEventArgs e)
        {

            PartsViewModel pvm = (PartsViewModel)DataContext;
            try
            {
                int UnitTobeUpdated = Convert.ToInt32(UnitTextBox.Text);

                List<int> selectedindex = new List<int>();
                List<int> PartListindex = new List<int>();
                foreach (Model.Part p in TeklaPartsListXAML.SelectedItems)
                { selectedindex.Add((int)p.Index); }

                foreach (Model.Part p in pvm.TeklaPartsList)
                { PartListindex.Add((int)p.Index); }

                foreach (int i in selectedindex)
                {
                    int Index = PartListindex.IndexOf(i);
                    pvm.TeklaPartsList[Index].NumofUnit = UnitTobeUpdated;
                }
            }
            catch { }
        }


        private void BBSListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PartsViewModel pvm = (PartsViewModel)DataContext;
            string Mlink = pvm.Modellink;
            List<string> P = pvm.PnameList;
            List<string> R2 = pvm.RPosList;
            R2.Clear();
            foreach (Rebar R in BBSListView.SelectedItems)
            {
                R2.Add(R.Position);
            }

            List<string> R2unique = R2.Distinct().ToList();

            if (ActivateFilterCB.IsChecked == true)
            {
                XAML_intro.TeklaFunction.Filter.WriteFilter(Mlink, P, R2unique);
            }
        }

        private void QuanListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            PartsViewModel pvm = (PartsViewModel)DataContext;
            string Mlink = pvm.Modellink;
            List<string> P = pvm.PnameList;
            List<string> R2 = pvm.RPosList;

            pvm.SlectedPartRebarList.Clear();
            P.Clear();

            foreach (Quantities p in QuanListview.SelectedItems)
            {
                P.Add(p.Position);
                foreach (Rebar R in pvm.AllSlectedPartRebarList)
                {
                    if (R.ParentName == p.PartName)
                    {
                        pvm.SlectedPartRebarList.Add(R);
                    }
                }
            }

            List<string> Punique = P.Distinct().ToList();

            if (ActivateFilterCB.IsChecked == true)
            {
                XAML_intro.TeklaFunction.Filter.WriteFilter(Mlink, Punique, R2);
            }

        }

        private void SelectAllPartButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)SelectAllPartButton.Content == "Select All Parts")
            {
                SelectAllPartButton.Content = "DeSelect All Parts";
                QuanListview.SelectAll();
            }

            else if ((string)SelectAllPartButton.Content == "DeSelect All Parts")
            {
                SelectAllPartButton.Content = "Select All Parts";
                QuanListview.UnselectAll();
            }
        }

        private void SelectAllReinButton_Click(object sender, RoutedEventArgs e)
        {
            if ((string)SelectAllReinButton.Content == "Select All Rein")
            {
                SelectAllReinButton.Content = "DeSelect All Rein";
                BBSListView.SelectAll();
            }

            else  //((string)SelectAllPartButton.Content == "DeSelect All Rein")
            {
                SelectAllReinButton.Content = "Select All Rein";
                BBSListView.UnselectAll();
            }
        }

        private void ActivateFilterCB_Click(object sender, RoutedEventArgs e)
        {
            if (ActivateFilterCB.IsChecked == false)
            {
                PartsViewModel pvm = (PartsViewModel)DataContext;
                XAML_intro.TeklaFunction.Filter.EmptyFilter(pvm.Modellink);

            }
        }

        private void Select_SelectAllPickedPartList(object sender, RoutedEventArgs e)
        {
            if ((string)SelectAllPickedPartList.Content == "Select All")
            {
                SelectAllPickedPartList.Content = "DeSelect All";
                TeklaPartsListXAML.SelectAll();
            }

            else
            {
                SelectAllPickedPartList.Content = "Select All";
                TeklaPartsListXAML.UnselectAll();
            }
        }

        private void UpdateVisualizationTab()
        {
            PartsViewModel pvm = (PartsViewModel)DataContext;
            TotalQuantities T = pvm.SelectedTotalQuantity[0];
            pvm.PieCollection.Clear();

            double coef = 1;
            string uni = "[Kg]";

            if (RadioB1.IsChecked == false) { coef = 1000; uni = "[ton]"; }

            TotalQuantityTextBox.Text = String.Format("Weight{13}:   {0} \nVol[m3]:        {1} \nRebar{13}:     {2}\n\nRebarQuantity:" +
                "\nDia6mm{13}: {3}\nDia8mm{13}: {4}\nDia10mm{13}: {5}" +
                "\nDia12mm{13}: {6}\nDia16mm{13}: {7}\nDia20mm{13}: {8}\nDia25mm{13}: {9}" +
                "\nDia32mm{13}: {10}\nDia36mm{13}: {11}\nDia40mm{13}: {12} ", Math.Round((Double)(T.Weight / coef), 2), Math.Round((Double)(T.Volume), 2),
                Math.Round((Double)(T.RebarWeight / coef), 2), Math.Round((Double)(T.Rebar6mm / coef), 2), Math.Round((Double)(T.Rebar8mm / coef), 2),
                Math.Round((Double)(T.Rebar10mm / coef), 2), Math.Round((Double)(T.Rebar12mm / coef), 2), Math.Round((Double)(T.Rebar16mm / coef), 2),
                Math.Round((Double)(T.Rebar20mm / coef), 2), Math.Round((Double)(T.Rebar25mm / coef), 2), Math.Round((Double)(T.Rebar32mm / coef), 2),
                Math.Round((Double)(T.Rebar36mm / coef), 2), Math.Round((Double)(T.Rebar40mm / coef), 2), uni);


            if (T.Rebar6mm != 0)
            { pvm.PieCollection.Add(new PieSegment { Color = Colors.Yellow, Value = Math.Round((Double)(T.Rebar6mm / coef), 2), Name = "Dia6mm" + uni }); }
            if (T.Rebar8mm != 0)
            { pvm.PieCollection.Add(new PieSegment { Color = Colors.Green, Value = Math.Round((Double)(T.Rebar8mm / coef), 2), Name = "Dia8mm" + uni }); }
            if (T.Rebar10mm != 0)
            { pvm.PieCollection.Add(new PieSegment { Color = Colors.Red, Value = Math.Round((Double)(T.Rebar10mm / coef), 2), Name = "Dia10mm" + uni }); }
            if (T.Rebar12mm != 0)
            { pvm.PieCollection.Add(new PieSegment { Color = Colors.Blue, Value = Math.Round((Double)(T.Rebar12mm / coef), 2), Name = "Dia12mm" + uni }); }
            if (T.Rebar16mm != 0)
            { pvm.PieCollection.Add(new PieSegment { Color = Colors.PeachPuff, Value = Math.Round((Double)(T.Rebar16mm / coef), 2), Name = "Dia16mm" + uni }); }
            if (T.Rebar20mm != 0)
            { pvm.PieCollection.Add(new PieSegment { Color = Colors.Green, Value = Math.Round((Double)(T.Rebar20mm / coef), 2), Name = "Dia20mm" + uni }); }
            if (T.Rebar25mm != 0)
            { pvm.PieCollection.Add(new PieSegment { Color = Colors.SkyBlue, Value = Math.Round((Double)(T.Rebar25mm / coef), 2), Name = "Dia25mm" + uni }); }
            if (T.Rebar32mm != 0)
            { pvm.PieCollection.Add(new PieSegment { Color = Colors.SteelBlue, Value = Math.Round((Double)(T.Rebar32mm / coef), 2), Name = "Dia32mm" + uni }); }
            if (T.Rebar36mm != 0)
            { pvm.PieCollection.Add(new PieSegment { Color = Colors.Black, Value = Math.Round((Double)(T.Rebar36mm / coef), 2), Name = "Dia36mm" + uni }); }
            if (T.Rebar40mm != 0)
            { pvm.PieCollection.Add(new PieSegment { Color = Colors.Brown, Value = Math.Round((Double)(T.Rebar40mm / coef), 2), Name = "Dia40mm" + uni }); }

            TotalQuantChart.Data = pvm.PieCollection;

        }
        private void UpdateVisualizationTab_2()
        {
            PartsViewModel pvm = (PartsViewModel)DataContext;
            Quantities T = pvm.SlectedPartQuantitiesList[ComBoxSelected.SelectedIndex];
            pvm.PieCollection_2.Clear();
            //Math.Round((Double)TOTAL_WEIGHT, 1)
            double coef = 1;
            string uni = "[Kg]";

            if (RadioB1.IsChecked == false) { coef = 1000; uni = "[ton]"; }

           
            SelectedTextBox.Text = String.Format("Part:   {14}\n Grade:   {15}\nUnits   {16}\nWeight{13}:   {0} \nVol[m3]:        {1} \nRebar{13}:     {2}\n\nRebarQuantity:" +
                "\nDia6mm{13}: {3}\nDia8mm{13}: {4}\nDia10mm{13}: {5}" +
                "\nDia12mm{13}: {6}\nDia16mm{13}: {7}\nDia20mm{13}: {8}\nDia25mm{13}: {9}" +
                "\nDia32mm{13}: {10}\nDia36mm{13}: {11}\nDia40mm{13}: {12} ", Math.Round((Double)(T.Weight / coef), 2), Math.Round((Double)(T.Volume), 2),
                Math.Round((Double)(T.RebarWeight / coef),2), Math.Round((Double)(T.Rebar6mm / coef), 2), Math.Round((Double)(T.Rebar8mm / coef), 2),
                Math.Round((Double)(T.Rebar10mm / coef),2), Math.Round((Double)(T.Rebar12mm / coef), 2), Math.Round((Double)(T.Rebar16mm / coef), 2),
                Math.Round((Double)(T.Rebar20mm / coef), 2), Math.Round((Double)(T.Rebar25mm / coef), 2), Math.Round((Double)(T.Rebar32mm / coef), 2),
                Math.Round((Double)(T.Rebar36mm / coef), 2), Math.Round((Double)(T.Rebar40mm / coef), 2), uni, T.PartName, T.Material, T.Unit);



            if (T.Rebar6mm != 0)
            { pvm.PieCollection_2.Add(new PieSegment { Color = Colors.Yellow, Value = Math.Round((Double)(T.Rebar6mm / coef), 2), Name = "Dia6mm" + uni }); }
            if (T.Rebar8mm != 0)
            { pvm.PieCollection_2.Add(new PieSegment { Color = Colors.Green, Value = Math.Round((Double)(T.Rebar8mm / coef), 2), Name = "Dia8mm" + uni }); }
            if (T.Rebar10mm != 0)
            { pvm.PieCollection_2.Add(new PieSegment { Color = Colors.Red, Value = Math.Round((Double)(T.Rebar10mm / coef), 2), Name = "Dia10mm" + uni }); }
            if (T.Rebar12mm != 0)
            { pvm.PieCollection_2.Add(new PieSegment { Color = Colors.Blue, Value = Math.Round((Double)(T.Rebar12mm / coef), 2), Name = "Dia12mm" + uni }); }
            if (T.Rebar16mm != 0)
            { pvm.PieCollection_2.Add(new PieSegment { Color = Colors.PeachPuff, Value = Math.Round((Double)(T.Rebar16mm / coef), 2), Name = "Dia16mm" + uni }); }
            if (T.Rebar20mm != 0)
            { pvm.PieCollection_2.Add(new PieSegment { Color = Colors.Green, Value = Math.Round((Double)(T.Rebar20mm / coef), 2), Name = "Dia20mm" + uni }); }
            if (T.Rebar25mm != 0)
            { pvm.PieCollection_2.Add(new PieSegment { Color = Colors.SkyBlue, Value = Math.Round((Double)(T.Rebar25mm / coef), 2), Name = "Dia25mm" + uni }); }
            if (T.Rebar32mm != 0)
            { pvm.PieCollection_2.Add(new PieSegment { Color = Colors.SteelBlue, Value = Math.Round((Double)(T.Rebar32mm / coef), 2), Name = "Dia32mm" + uni }); }
            if (T.Rebar36mm != 0)
            { pvm.PieCollection_2.Add(new PieSegment { Color = Colors.Black, Value = Math.Round((Double)(T.Rebar36mm / coef), 2), Name = "Dia36mm" + uni }); }
            if (T.Rebar40mm != 0)
            { pvm.PieCollection_2.Add(new PieSegment { Color = Colors.Brown, Value = Math.Round((Double)(T.Rebar40mm / coef), 2), Name = "Dia40mm" + uni }); }

            SelectedQuantChart.Data = pvm.PieCollection_2;
        }


        private void ComBoxSelected_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComBoxSelected.SelectedItem != null)
            {
                UpdateVisualizationTab_2();
            }
        }


        private void RadioB1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ComBoxSelected.Items.Count != 0)
            {
                UpdateVisualizationTab();
                ComBoxSelected.SelectedIndex = 0;
                UpdateVisualizationTab_2();
            }
        }

        private void RadioB2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (ComBoxSelected.Items.Count != 0)
            {
                UpdateVisualizationTab();
                ComBoxSelected.SelectedIndex = 0;
                UpdateVisualizationTab_2();
            }
        }
    }
}
