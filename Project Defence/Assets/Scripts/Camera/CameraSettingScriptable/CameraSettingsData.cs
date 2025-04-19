using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Scriptables.Settings.CameraSettings
{
    [CreateAssetMenu(menuName = "Data/Settings/CameraSettings")]
    public class CameraSettingsData : SerializedScriptableObject
    {
        public float startDistance;
        public float xRotationSpeed;
        public float yRotationSpeed;
        public bool invert;
        public float yRotationMinLimit;
        public float yRotationMaxLimit;
        public float zoomSpeed;
        public float minZoomDistance;
        public float maxZoomDistance;
    }

}

