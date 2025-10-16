using UnityEngine;

public static class GameHelper
{
    public static float CalcCosAngle(GameObject floor)
    {
        float floorTiltAngle = floor.transform.localRotation.eulerAngles.x;
        float rotationRad = floorTiltAngle * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(rotationRad);
        return cosAngle;
    }
}
