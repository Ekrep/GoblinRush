using System.Collections;
using System.Collections.Generic;
using Scriptables.MapCreation.MapData;
using Scriptables.Settings.CameraSettings;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private Transform target;
    [SerializeField] private CameraSettingsData camSettingsData;
    private float distance;
    private float x = 0.0f;
    private float y = 0.0f;
    void OnEnable()
    {
        GridMap.OnGridMapInitialized += GridMap_GridMapInitialized;
    }
    private void GridMap_GridMapInitialized(Transform mapParent, MapData mapData)
    {
        target = mapParent;
    }
    void OnDisable()
    {
        GridMap.OnGridMapInitialized -= GridMap_GridMapInitialized;
    }

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        distance = camSettingsData.startDistance;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void LateUpdate()
    {
        if (target && !EventSystem.current.IsPointerOverGameObject())
        {
            if (InputController.Instance.Inputs.Gameplay.MouseRightHold.IsInProgress())
            {
                x += InputController.Instance.Inputs.Gameplay.MouseDelta.ReadValue<Vector2>().normalized.x * camSettingsData.xRotationSpeed * 0.02f;
                y -= InputController.Instance.Inputs.Gameplay.MouseDelta.ReadValue<Vector2>().normalized.y * camSettingsData.yRotationSpeed * 0.02f;
                y = ClampAngle(y, camSettingsData.yRotationMinLimit, camSettingsData.yRotationMaxLimit);
            }

            distance -= InputController.Instance.Inputs.Gameplay.MouseScroll.ReadValue<float>() * camSettingsData.zoomSpeed;
            distance = Mathf.Clamp(distance, camSettingsData.minZoomDistance, camSettingsData.maxZoomDistance);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360f) angle += 360f;
        if (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
