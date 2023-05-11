using UnityEngine;

public static class Calculator
{
    public static Vector3 CalculateDirection(Transform myTransform, Transform targetTransform)
    {
        Vector3 direction = (targetTransform.position - myTransform.position).normalized;
        return direction;
    }
    public static float CalculateRotationZRad(Transform myTransform, Transform targetTransform)
    {
        Vector3 direction;
        float rotationZRad;

        direction = (targetTransform.position - myTransform.position).normalized;
        rotationZRad = Mathf.Atan2(direction.normalized.y, direction.normalized.x);

        return rotationZRad;
    }
    public static float CalculateRotationZDeg(Transform myTransform, Transform targetTransform)
    {
        Vector3 direction;
        float rotationZRad;
        float rotationZDeg;

        direction = (targetTransform.position - myTransform.position).normalized;
        rotationZRad = Mathf.Atan2(direction.normalized.y, direction.normalized.x);
        rotationZDeg = rotationZRad * Mathf.Rad2Deg;

        return rotationZDeg;
    }
    public static Vector3 RandomizeShootingDirection(Transform myTransform, Transform targetTransform, float missDeg)
    {
        float newShootingAngle = CalculateRotationZDeg(myTransform, targetTransform) + Random.Range(-missDeg, missDeg);
        Random.InitState(System.DateTime.Now.Millisecond);
        Vector3 newShootingDirection = new Vector2(Mathf.Cos(newShootingAngle * Mathf.Deg2Rad), Mathf.Sin(newShootingAngle * Mathf.Deg2Rad));

        return newShootingDirection;
    }

    public static Vector2 DegToVector2(float deg)
    {
        Vector2 vector = new Vector2(Mathf.Cos(deg * Mathf.Deg2Rad), Mathf.Sin(deg * Mathf.Deg2Rad));
        return vector;
    }

    public static float Vector2ToDeg(Vector2 vector)
    {
        float deg;
        deg = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        return deg;
    }
}
