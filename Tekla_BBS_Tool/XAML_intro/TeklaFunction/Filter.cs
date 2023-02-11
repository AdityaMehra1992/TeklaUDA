using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Filtering;
using Tekla.Structures.Filtering.Categories;
using TSM = Tekla.Structures.Model;
using TSMUI = Tekla.Structures.Model.UI;

namespace XAML_intro.TeklaFunction
{
    public static class Filter
    {
        public static void WriteFilter(string ModelPath, List<string> PartNames, List<string> RebarPositions)
        {
            BinaryFilterExpressionCollection ExpressionCollection = new BinaryFilterExpressionCollection();


            if (PartNames.Count != 0)
            {
                PartFilterExpressions.PositionNumber PartPos = new PartFilterExpressions.PositionNumber();

                List<string> Ppos = PartNames;
                IEnumerable<String> E1 = Ppos;
                StringConstantFilterExpression SP = new StringConstantFilterExpression(Ppos);
                BinaryFilterExpression Expression1 = new BinaryFilterExpression(PartPos, StringOperatorType.IS_EQUAL, SP);
                ExpressionCollection.Add(new BinaryFilterExpressionItem(Expression1, BinaryFilterOperatorType.BOOLEAN_AND));
            }
            else
            {
                PartFilterExpressions.PositionNumber PartPos = new PartFilterExpressions.PositionNumber();
                StringConstantFilterExpression SP = new StringConstantFilterExpression("");
                BinaryFilterExpression Expression1 = new BinaryFilterExpression(PartPos, StringOperatorType.IS_EQUAL, SP);
                ExpressionCollection.Add(new BinaryFilterExpressionItem(Expression1, BinaryFilterOperatorType.BOOLEAN_AND));
            }
            if (RebarPositions.Count != 0)
            {
                ReinforcingBarFilterExpressions.PositionNumber RebarPos = new ReinforcingBarFilterExpressions.PositionNumber();
                List<string> RPList = RebarPositions;
                IEnumerable<String> E2 = RPList;
                StringConstantFilterExpression SR = new StringConstantFilterExpression(RPList);
                BinaryFilterExpression Expression2 = new BinaryFilterExpression(RebarPos, StringOperatorType.IS_EQUAL, SR);
                ExpressionCollection.Add(new BinaryFilterExpressionItem(Expression2));
            }
            else
            {
                ReinforcingBarFilterExpressions.PositionNumber RebarPos = new ReinforcingBarFilterExpressions.PositionNumber();
                StringConstantFilterExpression SR = new StringConstantFilterExpression("");
                BinaryFilterExpression Expression2 = new BinaryFilterExpression(RebarPos, StringOperatorType.IS_EQUAL, SR);
                ExpressionCollection.Add(new BinaryFilterExpressionItem(Expression2));
            }


            string AttributesPath = Path.Combine(ModelPath, "attributes");
            string FilterName = Path.Combine(AttributesPath, "BBS_ScriptFilter");

            Tekla.Structures.Filtering.Filter Filter = new Tekla.Structures.Filtering.Filter(ExpressionCollection);
            Filter.CreateFile(FilterExpressionFileType.OBJECT_GROUP_VIEW, FilterName);

            TSMUI.ModelViewEnumerator PVE = TSMUI.ViewHandler.GetPermanentViews();

            while (PVE.MoveNext())
            {
                TSMUI.View V3d = PVE.Current;
                if (V3d.Name == "3d")

                {
                    TSMUI.ViewHandler.ShowView(V3d);
                    V3d.ViewFilter = "BBS_ScriptFilter";
                    V3d.Modify();
                }
            }


        }

        public static void EmptyFilter(string ModelPath)
        {
            BinaryFilterExpressionCollection ExpressionCollection = new BinaryFilterExpressionCollection();

            string AttributesPath = Path.Combine(ModelPath, "attributes");
            string FilterName = Path.Combine(AttributesPath, "Empty_ScriptFilter");

            Tekla.Structures.Filtering.Filter Filter = new Tekla.Structures.Filtering.Filter(ExpressionCollection);
            Filter.CreateFile(FilterExpressionFileType.OBJECT_GROUP_VIEW, FilterName);

            TSMUI.ModelViewEnumerator PVE = TSMUI.ViewHandler.GetPermanentViews();

            while (PVE.MoveNext())
            {
                TSMUI.View V3d = PVE.Current;
                if (V3d.Name == "3d")

                {
                    TSMUI.ViewHandler.ShowView(V3d);
                    V3d.ViewFilter = "Empty_ScriptFilter";
                    V3d.Modify();
                }
            }
        }
    }
}
