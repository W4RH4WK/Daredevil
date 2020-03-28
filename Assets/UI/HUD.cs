using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text Speed;

    public Text Altitude;

    public Text Modes;

    public Text Stalling;

    FlightModel FlightModel;

    void Awake()
    {
        FlightModel = FindObjectOfType<FlightModel>();
        Assert.IsNotNull(FlightModel);
    }

    void Update()
    {
        if (Speed)
            Speed.text = $"{FlightModel.Speed * 10.0f:0}";

        if (Altitude)
            Altitude.text = $"{FlightModel.Altitude:0}";

        if (Stalling)
            Stalling.enabled = FlightModel.Stalling;

        UpdateModes();
    }

    void UpdateModes()
    {
        var focus = FlightModel.FocusMode ? "Focus" : "";
        var strafe = FlightModel.StrafeMode ? "Strafe" : "";
        var highG = FlightModel.HighGTurnMode ? "HighG" : "";

        if (Modes)
            Modes.text = $"{focus}\n{strafe}\n{highG}";
    }
}
