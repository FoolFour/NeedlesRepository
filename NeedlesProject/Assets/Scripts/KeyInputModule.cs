﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Serialization;

public class KeyInputModule : BaseInputModule
{
    private float m_PrevActionTime;
    Vector2 m_LastMoveVector;
    int m_ConsecutiveMoveCount            = 0;

    [SerializeField]
    private string m_HorizontalAxis       = "Horizontal";

    [SerializeField]
    private string m_VerticalAxis         = "Vertical";
    
    [SerializeField]
    private string m_SubmitButton         = "Submit";
    
    [SerializeField]
    private string m_CancelButton         = "Cancel";
    
    [SerializeField]
    private float m_InputActionsPerSecond = 10;
    
    [SerializeField]
    private float m_RepeatDelay           = 0.5f;
    
    [SerializeField]
    [FormerlySerializedAs("m_AllowActivationOnMobileDevice")]
    private bool m_ForceModuleActive;

    public bool forceModuleActive
    {
        get { return m_ForceModuleActive; }
        set { m_ForceModuleActive = value; }
    }
    
    public float inputActionsPerSecond
    {
        get { return m_InputActionsPerSecond; }
        set { m_InputActionsPerSecond = value; }
    }
    
    public float repeatDelay
    {
        get { return m_RepeatDelay; }
        set { m_RepeatDelay = value; }
    }

    public string horizontalAxis
    {
        get { return m_HorizontalAxis; }
        set { m_HorizontalAxis = value; }
    }

    public string verticalAxis
    {
        get { return m_VerticalAxis; }
        set { m_VerticalAxis = value; }
    }
    
    public string submitButton
    {
        get { return m_SubmitButton; }
        set { m_SubmitButton = value; }
    }
    
    public string cancelButton
    {
        get { return m_CancelButton; }
        set { m_CancelButton = value; }
    }
    
    public override bool IsModuleSupported()
    {
        // Check for mouse presence instead of whether touch is supported,
        // as you can connect mouse to a tablet and in that case we'd want
        // to use StandaloneInputModule for non-touch input events.
        return m_ForceModuleActive || Input.mousePresent;
    }
    
    public override bool ShouldActivateModule()
    {
        if (!base.ShouldActivateModule())
        {
            return false;
        }

        var shouldActivate = m_ForceModuleActive;
        Input.GetButtonDown(m_SubmitButton);
        shouldActivate |= Input.GetButtonDown(m_CancelButton);
        shouldActivate |= !Mathf.Approximately(Input.GetAxisRaw(m_HorizontalAxis), 0.0f);
        shouldActivate |= !Mathf.Approximately(Input.GetAxisRaw(m_VerticalAxis), 0.0f);
        shouldActivate |= Input.GetMouseButtonDown(0);
        return shouldActivate;
    }
    
    public override void ActivateModule()
    {
        base.ActivateModule();
    
        var toSelect = eventSystem.currentSelectedGameObject;
        if (toSelect == null)
        {
            toSelect = eventSystem.firstSelectedGameObject;
        }
        eventSystem.SetSelectedGameObject(toSelect, GetBaseEventData());
    }
    
    public override void Process()
    {
        bool usedEvent = SendUpdateEventToSelectedObject();
    
        if (eventSystem.sendNavigationEvents)
        {
            if (!usedEvent)
            {
                usedEvent |= SendMoveEventToSelectedObject();
            }

            if (!usedEvent)
            {
                SendSubmitEventToSelectedObject();
            }
        }
    }
    
    /// <summary>
    /// Process submit keys.
    /// </summary>
    protected bool SendSubmitEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            return false;
        }

        var data = GetBaseEventData();
        if (Input.GetButtonDown(m_SubmitButton))
        {
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.submitHandler);
        }

        if (Input.GetButtonDown(m_CancelButton))
        {
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.cancelHandler);
        }
        return data.used;
    }
    
    private Vector2 GetRawMoveVector()
    {
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxisRaw(m_HorizontalAxis);
        move.y = Input.GetAxisRaw(m_VerticalAxis);
    
        if (Input.GetButtonDown(m_HorizontalAxis))
        {
            if (move.x < 0) { move.x = -1f; }
            if (move.x > 0) { move.x =  1f; }
        }
        if (Input.GetButtonDown(m_VerticalAxis))
        {
            if (move.y < 0) { move.y = -1f; }
            if (move.y > 0) { move.y =  1f; }
        }
        return move;
    }
    
    /// <summary>
    /// Process keyboard events.
    /// </summary>
    protected bool SendMoveEventToSelectedObject()
    {
        float time = Time.unscaledTime;
    
        Vector2 movement = GetRawMoveVector();
        if (Mathf.Approximately(movement.x, 0f) && Mathf.Approximately(movement.y, 0f))
        {
            m_ConsecutiveMoveCount = 0;
            return false;
        }
    
        // If user pressed key again, always allow event
        bool allow = Input.GetButtonDown(m_HorizontalAxis) || Input.GetButtonDown(m_VerticalAxis);
        bool similarDir = (Vector2.Dot(movement, m_LastMoveVector) > 0);
        if (!allow)
        {
            // Otherwise, user held down key or axis.
            // If direction didn't change at least 90 degrees, wait for delay before allowing consequtive event.
            if (similarDir && m_ConsecutiveMoveCount == 1)
            {
                allow = (time > m_PrevActionTime + m_RepeatDelay);
            }
            // If direction changed at least 90 degree, or we already had the delay, repeat at repeat rate.
            else
            {
                allow = (time > m_PrevActionTime + 1f / m_InputActionsPerSecond);
            }
        }
        if (!allow)
        {
            return false;
        }

        // Debug.Log(m_ProcessingEvent.rawType + " axis:" + m_AllowAxisEvents + " value:" + "(" + x + "," + y + ")");
        var axisEventData = GetAxisEventData(movement.x, movement.y, 0.6f);
        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, axisEventData, ExecuteEvents.moveHandler);
        if (!similarDir)
        {
            m_ConsecutiveMoveCount = 0;
        }
        m_ConsecutiveMoveCount++;
        m_PrevActionTime = time;
        m_LastMoveVector = movement;
        return axisEventData.used;
    }
    
    protected bool SendUpdateEventToSelectedObject()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            return false;
        }
        var data = GetBaseEventData();
        ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
        return data.used;
    }
}
