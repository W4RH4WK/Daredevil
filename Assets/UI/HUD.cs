using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text Speed;

    public Text Altitude;

    public Text FocusMode;

    public Text StrafeMode;

    public Text HighGTurn;

    public Text Stalling;

    public Image Compass;

    public GameObject Crosshair;

    public GameObject VelocityVector;

    public float VelocityVectorOffset = 10.0f;

    public GameObject TargetBracketPrefab;

    public GameObject ActiveTargetBracket;

    public GameObject LockOnBracket;

    IList<GameObject> TargetBrackets = new List<GameObject>();

    Controls Controls;

    FlightModel FlightModel;

    CombatModel CombatModel;

    void OnEnable()
    {
        Controls = FindObjectOfType<Controls>();

        FlightModel = FindObjectOfType<FlightModel>();

        CombatModel = FindObjectOfType<CombatModel>();
    }

    void Start()
    {
        // UI materials are shared. Modifying them during runtime causes them to
        // change permanently. We therefore clone them before modifying any
        // parameters.
        if (Compass)
            Compass.material = new Material(Compass.material);

        // Use a fixed number of target brackets, only enabling the ones we need.
        for (var i = 0; i < 32; i++)
        {
            var targetBracket = Instantiate(TargetBracketPrefab, transform);
            targetBracket.SetActive(true);

            TargetBrackets.Add(targetBracket);
        }
    }

    void Update()
    {
        if (!Controls || !FlightModel || !CombatModel)
        {
            Destroy(gameObject);
            return;
        }

        if (Speed)
            Speed.text = $"{FlightModel.Speed * 10.0f,4:000}|";

        if (Altitude)
        {
            float altitude;

            var ray = new Ray(FlightModel.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
                altitude = hit.distance;
            else
                altitude = FlightModel.transform.position.y;

            Altitude.text = $"|{altitude,4:0000}";
        }

        if (FocusMode)
            FocusMode.enabled = Controls.FocusMode;

        if (StrafeMode)
            StrafeMode.enabled = Controls.StrafeMode;

        if (HighGTurn)
            HighGTurn.enabled = Controls.HighGTurnMode;

        if (Stalling)
            Stalling.enabled = FlightModel.Stalling;

        if (Compass)
        {
            var offset = Camera.main.transform.rotation.eulerAngles.y / 360.0f;
            Compass.material.SetTextureOffset("_MainTex", new Vector2(offset, 0.0f));
        }

        UpdateCrosshair();

        if (VelocityVector)
        {
            VelocityVector.transform.position = (VelocityVectorOffset * FlightModel.Velocity.normalized) + FlightModel.transform.position;
            VelocityVector.transform.rotation = Camera.main.transform.rotation;
        }

        UpdateActiveTargetBracket();

        UpdateTargetBrackets();

        UpdateLockOnBracket();
    }

    void UpdateCrosshair()
    {
        if (!Crosshair)
            return;

        if (!CombatModel.ActiveTarget)
            goto Disable;

        var cameraToTarget = CombatModel.ActiveTarget.transform.position - Camera.main.transform.position;
        if (Vector3.Angle(FlightModel.transform.forward, cameraToTarget) > 18.0f)
            goto Disable;

        var targetPosition = CombatModel.ActiveTarget.transform.position;
        var targetPlane = (targetPosition - Camera.main.transform.position).normalized;
        var pointOnTargetPlane = LinePlaneIntersection(FlightModel.transform.position, FlightModel.transform.forward, targetPlane, targetPosition);
        if (!pointOnTargetPlane.HasValue)
            goto Disable;

        var viewportPosition = Camera.main.WorldToViewportPoint(pointOnTargetPlane.Value);
        if (viewportPosition.z < 0.0f)
            goto Disable;

        Crosshair.transform.position = Camera.main.ViewportToScreenPoint(viewportPosition);
        Crosshair.SetActive(true);
        return;

    Disable:
        Crosshair.SetActive(false);
    }

    void UpdateActiveTargetBracket()
    {
        if (CombatModel.ActiveTarget)
        {
            ActiveTargetBracket.transform.position = TargetScreenPosition(CombatModel.ActiveTarget);
            ActiveTargetBracket.SetActive(true);
        }
        else
        {
            ActiveTargetBracket.SetActive(false);
        }
    }

    void UpdateTargetBrackets()
    {
        var targetEnumerator = CombatModel.GetTargetsEnumerator();
        var targetBracketEnumerator = TargetBrackets.GetEnumerator();

        while (targetEnumerator.MoveNext() && targetBracketEnumerator.MoveNext())
        {
            var target = targetEnumerator.Current;
            var targetBracket = targetBracketEnumerator.Current;

            targetBracket.transform.position = TargetScreenPosition(target);
            targetBracket.SetActive(true);
        }

        if (targetEnumerator.Current)
            Debug.LogWarning("To few target brackets available");

        while (targetBracketEnumerator.MoveNext())
            targetBracketEnumerator.Current.SetActive(false);
    }

    void UpdateLockOnBracket()
    {
        if (!CombatModel.LockingOn && !CombatModel.LockedOn)
        {
            LockOnBracket.SetActive(false);
            return;
        }

        var origin = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        if (Crosshair)
            origin = Crosshair.transform.position;

        var targetPosition = TargetScreenPosition(CombatModel.ActiveTarget);

        LockOnBracket.transform.position = Vector3.Lerp(origin, targetPosition, CombatModel.LockOnTimeElapsed / CombatModel.LockOnTime);
        LockOnBracket.SetActive(true);
    }

    static Vector3 TargetScreenPosition(Target target)
    {
        return ScreenPosition(target.transform.position);
    }

    static Vector3 ScreenPosition(Vector3 position)
    {
        var viewportPosition = Camera.main.WorldToViewportPoint(position);

        if (!IsInsideViewport(viewportPosition))
        {
            var center = new Vector3(0.5f, 0.5f);
            viewportPosition -= center;

            if (viewportPosition.z < 0.0f)
                viewportPosition *= -1;

            var angle = Mathf.Atan2(viewportPosition.y, viewportPosition.x);
            angle -= 90.0f * Mathf.Deg2Rad;

            var cos = Mathf.Cos(angle);
            var sin = Mathf.Sin(angle);
            var m = cos / sin;

            if (cos > 0)
                viewportPosition = new Vector3(-0.5f / m, 0.5f);
            else
                viewportPosition = new Vector3(0.5f / m, -0.5f);

            if (viewportPosition.x > 0.5f)
                viewportPosition = new Vector3(0.5f, -0.5f * m);
            else if (viewportPosition.x < -0.5f)
                viewportPosition = new Vector3(-0.5f, 0.5f * m);

            viewportPosition += center;
        }

        return Camera.main.ViewportToScreenPoint(viewportPosition);
    }

    static bool IsInsideViewport(Vector3 point)
    {
        return point.x > 0.0f && point.x < 1.0f && point.y > 0.0f && point.y < 1.0f && point.z > 0.0f;
    }

    static Vector3? LinePlaneIntersection(Vector3 pointOnLine, Vector3 line, Vector3 planeNormal, Vector3 pointOnPlane)
    {
        var denominator = Vector3.Dot(line, planeNormal);
        if (denominator == 0.0f)
            return null;

        var distance = Vector3.Dot((pointOnPlane - pointOnLine), planeNormal) / denominator;
        return distance * line.normalized + pointOnLine;
    }
}
