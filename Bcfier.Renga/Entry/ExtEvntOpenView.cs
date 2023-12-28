using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bcfier.Bcf.Bcf2;
using Bcfier.Data.Utils;

namespace Bcfier.RengaPlugin.Entry
{
  public class ExtEvntOpenView //: IExternalEventHandler
  {
    public VisualizationInfo v;

    public void Execute(Renga.IApplication app)
    {
      try
      {
        var modelView = app.ActiveView as Renga.IModelView;
        if (modelView == null) 
          return;

        var camera = (modelView as Renga.IView3DParams)?.Camera;
        if (camera == null)
          return;

        if (v.PerspectiveCamera == null)
          return;

        var rengaCameraPos = new Renga.FloatPoint3D();
        var bcfCameraPos = v.PerspectiveCamera.CameraViewPoint;
        rengaCameraPos.X = (float)bcfCameraPos.X * 1000;
        rengaCameraPos.Y = (float)bcfCameraPos.Y * 1000;
        rengaCameraPos.Z = (float)bcfCameraPos.Z * 1000;

        var rengaFocusPoint = new Renga.FloatPoint3D();
        var bcfCameraDirection = v.PerspectiveCamera.CameraDirection;
        rengaFocusPoint.X = rengaCameraPos.X + (float)bcfCameraDirection.X * 1000;
        rengaFocusPoint.Y = rengaCameraPos.Y + (float)bcfCameraDirection.Y * 1000;
        rengaFocusPoint.Z = rengaCameraPos.Z + (float)bcfCameraDirection.Z * 1000;

        var rengaUpVector = new Renga.FloatVector3D();
        var bcfUpVector = v.PerspectiveCamera.CameraUpVector;
        rengaUpVector.X = (float)bcfUpVector.X;
        rengaUpVector.Y = (float)bcfUpVector.Y;
        rengaUpVector.Z = (float)bcfUpVector.Z;
        camera.LookAt(rengaFocusPoint, rengaCameraPos, rengaUpVector);

        var buildingModel = app.Project?.Model;
        if (buildingModel == null)
          return;

        var defaultVisibility = v.Components.Visibility.DefaultVisibility;
        if (defaultVisibility == null)
          defaultVisibility = true;

        var objects = buildingModel.GetObjects();
        var exceptionIds = new List<int>();
        var otherIds = new List<int>();

        Func<string, bool> isExceptionObject = (string ifcGuid) => 
        {
          return Array.Find(v.Components.Visibility.Exceptions, exception => exception.IfcGuid == ifcGuid) != null;
        };

        foreach (int id in objects.GetIds())
        {
          var rengaUuid = objects.GetById(id).UniqueId;
          var ifcGuid = IfcGuid.ToIfcGuid(rengaUuid);
          if (isExceptionObject(ifcGuid))
            exceptionIds.Add(id);
          else
            otherIds.Add(id);
        }

        modelView.SetObjectsVisibility(otherIds.ToArray(), defaultVisibility);
        modelView.SetObjectsVisibility(exceptionIds.ToArray(), !defaultVisibility);
      }
      catch (Exception ex)
      {
        app.UI.ShowMessageBox(Renga.MessageIcon.MessageIcon_Error, "Error!", ex.Message);
      }
    }

    //private IEnumerable<ViewFamilyType> getFamilyViews(Document doc)
    //{

    //  return from elem in new FilteredElementCollector(doc).OfClass(typeof(ViewFamilyType))
    //         let type = elem as ViewFamilyType
    //         where type.ViewFamily == ViewFamily.ThreeDimensional
    //         select type;
    //}
    //private IEnumerable<View3D> get3DViews(Document doc)
    //{
    //  return from elem in new FilteredElementCollector(doc).OfClass(typeof(View3D))
    //         let view = elem as View3D
    //         select view;
    //}
    //private IEnumerable<View> getSheets(Document doc, int id, string stname)
    //{
    //  ElementId eid = new ElementId(id);
    //  return from elem in new FilteredElementCollector(doc).OfClass(typeof(View))
    //         let view = elem as View
    //         //Get the view with the given Id or given name
    //         where view.Id == eid | view.Name == stname
    //         select view;
      
    //}
    public string GetName()
    {
      return "Open 3D View";
    }
    // returns XYZ and ZOOM/FOV value
  }

}