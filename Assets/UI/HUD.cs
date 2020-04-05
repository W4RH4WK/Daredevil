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

    public float CrosshairOffset = 10.0f;

    public GameObject TargetBracketPrefab;

    IList<GameObject> TargetBrackets = new List<GameObject>();

    FlightModel FlightModel;

    CombatModel CombatModel;

    void Awake()
    {
        FlightModel = FindObjectOfType<FlightModel>();
        Assert.IsNotNull(FlightModel);

        CombatModel = FindObjectOfType<CombatModel>();
        Assert.IsNotNull(CombatModel);

        // Materials are shared. Modifying them during runtime causes them to
        // change permanently. We therefore clone them before modifying any
        // parameters.
        {
            if (Compass)
                Compass.material = new Material(Compass.material);
        }

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
            FocusMode.enabled = FlightModel.Input.FocusMode;

        if (StrafeMode)
            StrafeMode.enabled = FlightModel.Input.StrafeMode;

        if (HighGTurn)
            HighGTurn.enabled = FlightModel.Input.HighGTurnMode;

        if (Stalling)
            Stalling.enabled = FlightModel.Stalling;

        if (Compass)
        {
            var offset = Camera.main.transform.rotation.eulerAngles.y / 360.0f;
            Compass.material.SetTextureOffset("_MainTex", new Vector2(offset, 0.0f));
        }

        if (Crosshair)
        {
            Crosshair.transform.position = FlightModel.transform.rotation * (CrosshairOffset * Vector3.forward) + FlightModel.transform.position;
            Crosshair.transform.rotation = Camera.main.transform.rotation;
        }

        if (VelocityVector)
        {
            VelocityVector.transform.position = /*FlightModel.transform.rotation **/ (CrosshairOffset * FlightModel.Velocity.normalized) + FlightModel.transform.position;
            VelocityVector.transform.rotation = Camera.main.transform.rotation;
        }

        UpdateTargetBrackets();
    }

    void UpdateTargetBrackets()
    {
        var targetEnumerator = CombatModel.GetTargetsEnumerator();
        var targetBracketEnumerator = TargetBrackets.GetEnumerator();

        while (targetEnumerator.MoveNext() && targetBracketEnumerator.MoveNext())
        {
            var target = targetEnumerator.Current;
            var screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);
            if (screenPosition.z < 0.0f)
                continue;

            var targetBracket = targetBracketEnumerator.Current;
            targetBracket.SetActive(true);
            targetBracket.transform.position = screenPosition;
        }

        while (targetBracketEnumerator.MoveNext())
            targetBracketEnumerator.Current.SetActive(false);
    }
}
