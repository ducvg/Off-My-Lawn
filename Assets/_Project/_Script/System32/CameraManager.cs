using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] Vector3 lawnViewPosition;
    [SerializeField] Vector3 monsterViewPosition;
    Camera mainCamera;
    Tween cameraTween;

    public void Init()
    {
        mainCamera = Camera.main;
    }

    public void ToLawnView(float duration = 1f)
    {
        cameraTween.Stop();
        cameraTween = Tween.Position(mainCamera.transform, lawnViewPosition, duration, ease: Ease.OutQuad);
    }

    public void ToRoadView(float duration = 1f)
    {
        cameraTween.Stop();
        cameraTween = Tween.Position(mainCamera.transform, monsterViewPosition, duration, ease: Ease.OutQuad);
    }
}