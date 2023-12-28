﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using Bcfier.Bcf.Bcf2;
using Bcfier.Data.Utils;

namespace Bcfier.RengaPlugin.Data
{
  //Methods for working with views
  public static class RengaView
  {
    //<summary>
    //Generate a VisualizationInfo of the current view
    //</summary>
    //<returns></returns>
    public static VisualizationInfo GenerateViewpoint(Renga.IApplication app)
    {
      try
      {
        var modelView = app.ActiveView as Renga.IModelView;
        if (modelView == null)
          return null;

        var view3DParams = modelView as Renga.IView3DParams;
        if (view3DParams == null)
          return null;

        var rengaCamera = view3DParams.Camera;
        var bcfViewCamera = new Bcfier.Bcf.Bcf2.PerspectiveCamera();

        // Position
        bcfViewCamera.CameraViewPoint.X = rengaCamera.Position.X / 1000;
        bcfViewCamera.CameraViewPoint.Y = rengaCamera.Position.Y / 1000;
        bcfViewCamera.CameraViewPoint.Z = rengaCamera.Position.Z / 1000;

        // Direction
        var cameraVector = new Vector3D(
          rengaCamera.FocusPoint.X - rengaCamera.Position.X, 
          rengaCamera.FocusPoint.Y - rengaCamera.Position.Y, 
          rengaCamera.FocusPoint.Z - rengaCamera.Position.Z);
        cameraVector.Normalize();

        bcfViewCamera.CameraDirection.X = cameraVector.X;
        bcfViewCamera.CameraDirection.Y = cameraVector.Y;
        bcfViewCamera.CameraDirection.Z = cameraVector.Z;

        // Up
        bcfViewCamera.CameraUpVector.X = rengaCamera.UpVector.X;
        bcfViewCamera.CameraUpVector.Y = rengaCamera.UpVector.Y;
        bcfViewCamera.CameraUpVector.Z = rengaCamera.UpVector.Z;

        bcfViewCamera.FieldOfView = rengaCamera.FovHorizontal * (180 / Math.PI);

        var buildingModel = app.Project?.Model;
        if (buildingModel == null)
          return null;

        var objects = buildingModel.GetObjects();
        var hiddenComponents = new List<Component>();
        foreach (int id in objects.GetIds())
        {
          if (modelView.IsObjectVisible(id))
            continue;

          var ifcGuid = IfcGuid.ToIfcGuid(objects.GetById(id).UniqueId);
          var hiddenComponent = new Component();
          hiddenComponent.IfcGuid = ifcGuid;
          hiddenComponents.Add(hiddenComponent);
        }

        var result = new VisualizationInfo();
        result.PerspectiveCamera = bcfViewCamera;
        result.Components = new Components();
        result.Components.Visibility = new ComponentVisibility();
        result.Components.Visibility.DefaultVisibility = true;
        result.Components.Visibility.Exceptions = hiddenComponents.ToArray();
        return result;
      }
      catch (System.Exception ex1)
      {
        app.UI.ShowMessageBox(Renga.MessageIcon.MessageIcon_Error, "Error!", ex1.Message);
      }

      return null;
    }
  }
}
