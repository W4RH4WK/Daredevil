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

    FlightModel FlightModel;

    void Awake()
    {
        FlightModel = FindObjectOfType<FlightModel>();
        Assert.IsNotNull(FlightModel);

        // Materials are shared. Modifying them during runtime causes them to
        // change permanently. We therefore clone them before modifying any
        // parameters.
        {
            if (Compass)
                Compass.material = new Material(Compass.material);
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
            FocusMode.enabled = FlightModel.FocusMode;

        if (StrafeMode)
            StrafeMode.enabled = FlightModel.StrafeMode;

        if (HighGTurn)
            HighGTurn.enabled = FlightModel.HighGTurnMode;

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
    }
}
