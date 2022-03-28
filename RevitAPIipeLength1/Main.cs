using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPIipeLength1
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            IList<Reference> selectedElementsRefList = uidoc.Selection.PickObjects(ObjectType.Element, "Select elem");
            var elementList = new List<Element>();
            double sum = 0;
            foreach (var selectedElement in selectedElementsRefList)
            {
                Element element = doc.GetElement(selectedElement);
                if (element is Pipe)
                {
                    Parameter lengthParameter = element.LookupParameter("Length");
                    if (lengthParameter.StorageType == StorageType.Double)
                    {
                        double lengthValue = UnitUtils.ConvertFromInternalUnits(lengthParameter.AsDouble(), UnitTypeId.Meters);
                        sum += lengthValue;
                    }
                }
            }
            TaskDialog.Show("Сообщение", sum.ToString());
            return Result.Succeeded;
        }
    }
}