// GENERATED AUTOMATICALLY FROM 'Assets/Logic/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Flight"",
            ""id"": ""d9df74b1-1da2-4d3a-a20d-c6900a387c80"",
            ""actions"": [
                {
                    ""name"": ""Pitch"",
                    ""type"": ""Value"",
                    ""id"": ""7bb373f2-205d-49d7-a101-ecf59ba27310"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Roll"",
                    ""type"": ""Value"",
                    ""id"": ""96b88db8-fc8a-4b42-930e-4bb163fd3484"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": ""Invert"",
                    ""interactions"": """"
                },
                {
                    ""name"": ""YawLeft"",
                    ""type"": ""Button"",
                    ""id"": ""24c03cd7-6f3e-4775-8ab3-f1d5954dc442"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""YawRight"",
                    ""type"": ""Button"",
                    ""id"": ""ca23a5fe-74b4-4db8-b7b7-e5ab2a377fa2"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Accelerate"",
                    ""type"": ""Value"",
                    ""id"": ""2673e1af-18e5-4dd6-9218-1ca8fb03522d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Decelerate"",
                    ""type"": ""Value"",
                    ""id"": ""2dbd1d97-e728-4f7c-be16-dbb67a234d6a"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""1e71c882-38f5-4c2a-aa00-0c53d2e69de7"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Target"",
                    ""type"": ""Button"",
                    ""id"": ""210f2dfa-6d50-4e2f-b923-9ff8635b7d53"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""48e7586d-b83c-4fa8-9b0f-ca4b95d88497"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""edcae297-3bab-46c6-86ba-fa1c70641645"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""118eab3b-4bcc-4053-bd26-2e698248691e"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""YawLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2477ccc5-55d6-409f-bc36-68e2fac1378f"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""YawRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce869451-3f8b-47d8-b092-6038691a334c"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""71885a06-9b61-4f5d-8723-0eea8ff77822"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Decelerate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e57051e0-5e4c-4777-93c0-dd789dc90ae4"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3b71a8b-b178-4705-a1b8-47018935c847"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Target"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Flight
        m_Flight = asset.FindActionMap("Flight", throwIfNotFound: true);
        m_Flight_Pitch = m_Flight.FindAction("Pitch", throwIfNotFound: true);
        m_Flight_Roll = m_Flight.FindAction("Roll", throwIfNotFound: true);
        m_Flight_YawLeft = m_Flight.FindAction("YawLeft", throwIfNotFound: true);
        m_Flight_YawRight = m_Flight.FindAction("YawRight", throwIfNotFound: true);
        m_Flight_Accelerate = m_Flight.FindAction("Accelerate", throwIfNotFound: true);
        m_Flight_Decelerate = m_Flight.FindAction("Decelerate", throwIfNotFound: true);
        m_Flight_Look = m_Flight.FindAction("Look", throwIfNotFound: true);
        m_Flight_Target = m_Flight.FindAction("Target", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Flight
    private readonly InputActionMap m_Flight;
    private IFlightActions m_FlightActionsCallbackInterface;
    private readonly InputAction m_Flight_Pitch;
    private readonly InputAction m_Flight_Roll;
    private readonly InputAction m_Flight_YawLeft;
    private readonly InputAction m_Flight_YawRight;
    private readonly InputAction m_Flight_Accelerate;
    private readonly InputAction m_Flight_Decelerate;
    private readonly InputAction m_Flight_Look;
    private readonly InputAction m_Flight_Target;
    public struct FlightActions
    {
        private @Controls m_Wrapper;
        public FlightActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pitch => m_Wrapper.m_Flight_Pitch;
        public InputAction @Roll => m_Wrapper.m_Flight_Roll;
        public InputAction @YawLeft => m_Wrapper.m_Flight_YawLeft;
        public InputAction @YawRight => m_Wrapper.m_Flight_YawRight;
        public InputAction @Accelerate => m_Wrapper.m_Flight_Accelerate;
        public InputAction @Decelerate => m_Wrapper.m_Flight_Decelerate;
        public InputAction @Look => m_Wrapper.m_Flight_Look;
        public InputAction @Target => m_Wrapper.m_Flight_Target;
        public InputActionMap Get() { return m_Wrapper.m_Flight; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FlightActions set) { return set.Get(); }
        public void SetCallbacks(IFlightActions instance)
        {
            if (m_Wrapper.m_FlightActionsCallbackInterface != null)
            {
                @Pitch.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnPitch;
                @Pitch.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnPitch;
                @Pitch.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnPitch;
                @Roll.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnRoll;
                @YawLeft.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnYawLeft;
                @YawLeft.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnYawLeft;
                @YawLeft.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnYawLeft;
                @YawRight.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnYawRight;
                @YawRight.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnYawRight;
                @YawRight.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnYawRight;
                @Accelerate.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnAccelerate;
                @Accelerate.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnAccelerate;
                @Accelerate.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnAccelerate;
                @Decelerate.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnDecelerate;
                @Decelerate.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnDecelerate;
                @Decelerate.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnDecelerate;
                @Look.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnLook;
                @Look.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnLook;
                @Look.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnLook;
                @Target.started -= m_Wrapper.m_FlightActionsCallbackInterface.OnTarget;
                @Target.performed -= m_Wrapper.m_FlightActionsCallbackInterface.OnTarget;
                @Target.canceled -= m_Wrapper.m_FlightActionsCallbackInterface.OnTarget;
            }
            m_Wrapper.m_FlightActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pitch.started += instance.OnPitch;
                @Pitch.performed += instance.OnPitch;
                @Pitch.canceled += instance.OnPitch;
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @YawLeft.started += instance.OnYawLeft;
                @YawLeft.performed += instance.OnYawLeft;
                @YawLeft.canceled += instance.OnYawLeft;
                @YawRight.started += instance.OnYawRight;
                @YawRight.performed += instance.OnYawRight;
                @YawRight.canceled += instance.OnYawRight;
                @Accelerate.started += instance.OnAccelerate;
                @Accelerate.performed += instance.OnAccelerate;
                @Accelerate.canceled += instance.OnAccelerate;
                @Decelerate.started += instance.OnDecelerate;
                @Decelerate.performed += instance.OnDecelerate;
                @Decelerate.canceled += instance.OnDecelerate;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Target.started += instance.OnTarget;
                @Target.performed += instance.OnTarget;
                @Target.canceled += instance.OnTarget;
            }
        }
    }
    public FlightActions @Flight => new FlightActions(this);
    public interface IFlightActions
    {
        void OnPitch(InputAction.CallbackContext context);
        void OnRoll(InputAction.CallbackContext context);
        void OnYawLeft(InputAction.CallbackContext context);
        void OnYawRight(InputAction.CallbackContext context);
        void OnAccelerate(InputAction.CallbackContext context);
        void OnDecelerate(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnTarget(InputAction.CallbackContext context);
    }
}
