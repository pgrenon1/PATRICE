// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""ccd7bf0f-981c-4aef-a41c-15f2ccc6a2e4"",
            ""actions"": [
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""2ca1db16-9f5c-4e05-a9c0-33d14a048fcc"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""f9517cc7-97a8-4c3e-8c26-e94f71af7b8a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LeftJoystick"",
                    ""type"": ""Value"",
                    ""id"": ""d62e357c-0392-437d-8f06-bf90c9fb18f6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RightJoystick"",
                    ""type"": ""Value"",
                    ""id"": ""f97f435e-ff33-41be-9fd4-0c2d5cdfb665"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SwitchDimension"",
                    ""type"": ""Button"",
                    ""id"": ""0467286e-67ab-43ba-90f3-6c8bb7a1def0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Boost"",
                    ""type"": ""Button"",
                    ""id"": ""a3575d87-75fd-457e-961a-721d321a5664"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Brake"",
                    ""type"": ""Button"",
                    ""id"": ""3cc7f6dd-70f8-4b9a-a99a-38244dd11299"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""84a9d580-a2aa-4abe-9a31-e93ab9891949"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""99d6ea14-085c-43b6-b083-8e877703da7b"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""293699b3-3717-4b36-8b1d-0afb8ab74555"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RightJoystick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a0a8f67-8d20-46b7-b25a-7c5b6bc3b7d0"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""rb+lb"",
                    ""id"": ""cdb488af-c6ca-4adc-9945-08b80cdeded3"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchDimension"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""2c443af1-b9e0-4f8c-905e-4a877b520004"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchDimension"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""3ad95d1d-68c5-46ed-b913-5c726f89afdc"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchDimension"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""lb+rb"",
                    ""id"": ""45714883-f760-4a57-884f-4b78369939a6"",
                    ""path"": ""ButtonWithOneModifier"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchDimension"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier"",
                    ""id"": ""899df481-b1e0-4fad-a97f-2ef30e09aa9d"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchDimension"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""948f7a02-d9ce-47da-9ba1-f757c2b27edb"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchDimension"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""61e80ae9-9ab4-49a9-8430-ef4dbf67327f"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7e35b59-ff98-48d8-b179-a8b85df66c39"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Boost"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d3dac08a-b255-45f7-86b5-42daab258a75"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Brake"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_Shoot = m_Gameplay.FindAction("Shoot", throwIfNotFound: true);
        m_Gameplay_SwitchWeapon = m_Gameplay.FindAction("SwitchWeapon", throwIfNotFound: true);
        m_Gameplay_LeftJoystick = m_Gameplay.FindAction("LeftJoystick", throwIfNotFound: true);
        m_Gameplay_RightJoystick = m_Gameplay.FindAction("RightJoystick", throwIfNotFound: true);
        m_Gameplay_SwitchDimension = m_Gameplay.FindAction("SwitchDimension", throwIfNotFound: true);
        m_Gameplay_Boost = m_Gameplay.FindAction("Boost", throwIfNotFound: true);
        m_Gameplay_Brake = m_Gameplay.FindAction("Brake", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_Shoot;
    private readonly InputAction m_Gameplay_SwitchWeapon;
    private readonly InputAction m_Gameplay_LeftJoystick;
    private readonly InputAction m_Gameplay_RightJoystick;
    private readonly InputAction m_Gameplay_SwitchDimension;
    private readonly InputAction m_Gameplay_Boost;
    private readonly InputAction m_Gameplay_Brake;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Shoot => m_Wrapper.m_Gameplay_Shoot;
        public InputAction @SwitchWeapon => m_Wrapper.m_Gameplay_SwitchWeapon;
        public InputAction @LeftJoystick => m_Wrapper.m_Gameplay_LeftJoystick;
        public InputAction @RightJoystick => m_Wrapper.m_Gameplay_RightJoystick;
        public InputAction @SwitchDimension => m_Wrapper.m_Gameplay_SwitchDimension;
        public InputAction @Boost => m_Wrapper.m_Gameplay_Boost;
        public InputAction @Brake => m_Wrapper.m_Gameplay_Brake;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @Shoot.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
                @Shoot.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
                @Shoot.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnShoot;
                @SwitchWeapon.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchWeapon;
                @SwitchWeapon.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchWeapon;
                @SwitchWeapon.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchWeapon;
                @LeftJoystick.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftJoystick;
                @LeftJoystick.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftJoystick;
                @LeftJoystick.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnLeftJoystick;
                @RightJoystick.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightJoystick;
                @RightJoystick.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightJoystick;
                @RightJoystick.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnRightJoystick;
                @SwitchDimension.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchDimension;
                @SwitchDimension.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchDimension;
                @SwitchDimension.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnSwitchDimension;
                @Boost.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBoost;
                @Boost.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBoost;
                @Boost.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBoost;
                @Brake.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBrake;
                @Brake.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBrake;
                @Brake.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnBrake;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Shoot.started += instance.OnShoot;
                @Shoot.performed += instance.OnShoot;
                @Shoot.canceled += instance.OnShoot;
                @SwitchWeapon.started += instance.OnSwitchWeapon;
                @SwitchWeapon.performed += instance.OnSwitchWeapon;
                @SwitchWeapon.canceled += instance.OnSwitchWeapon;
                @LeftJoystick.started += instance.OnLeftJoystick;
                @LeftJoystick.performed += instance.OnLeftJoystick;
                @LeftJoystick.canceled += instance.OnLeftJoystick;
                @RightJoystick.started += instance.OnRightJoystick;
                @RightJoystick.performed += instance.OnRightJoystick;
                @RightJoystick.canceled += instance.OnRightJoystick;
                @SwitchDimension.started += instance.OnSwitchDimension;
                @SwitchDimension.performed += instance.OnSwitchDimension;
                @SwitchDimension.canceled += instance.OnSwitchDimension;
                @Boost.started += instance.OnBoost;
                @Boost.performed += instance.OnBoost;
                @Boost.canceled += instance.OnBoost;
                @Brake.started += instance.OnBrake;
                @Brake.performed += instance.OnBrake;
                @Brake.canceled += instance.OnBrake;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnShoot(InputAction.CallbackContext context);
        void OnSwitchWeapon(InputAction.CallbackContext context);
        void OnLeftJoystick(InputAction.CallbackContext context);
        void OnRightJoystick(InputAction.CallbackContext context);
        void OnSwitchDimension(InputAction.CallbackContext context);
        void OnBoost(InputAction.CallbackContext context);
        void OnBrake(InputAction.CallbackContext context);
    }
}
