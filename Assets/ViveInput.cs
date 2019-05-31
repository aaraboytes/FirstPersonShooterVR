using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class ViveInput : MonoBehaviour
{
    public SteamVR_TrackedObject mTrackedObject = null;
    public SteamVR_Controller.Device mDevice;

    private void Awake()
    {
        mTrackedObject = GetComponent<SteamVR_TrackedObject>();
    }
    void Update()
    {
        mDevice = SteamVR_Controller.Input((int)mTrackedObject.index);
        #region Triggger
        //Down
        if (mDevice.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            print("TriggerDown");
        }
        //Up
        if (mDevice.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            print("TriggerUp");
        }
        Vector2 triggerVal = mDevice.GetAxis(EVRButtonId.k_EButton_SteamVR_Trigger);
        #endregion
        #region Grip
        //Down
        if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            print("GripDown");
        }
        //Up
        if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            print("GripDown");
        }
        #endregion
        #region TouchPad
        //Down
        if (mDevice.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
            print("TouchpadDown");
        //Up
        if (mDevice.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
            print("TouchpadUp");
        Vector2 touchValue = mDevice.GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
        #endregion
    }
}
