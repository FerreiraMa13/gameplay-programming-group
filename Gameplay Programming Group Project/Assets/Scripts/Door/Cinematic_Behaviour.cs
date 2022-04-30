using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Cinematic_Behaviour : MonoBehaviour
{
    public enum SequenceState
    {
        IDLE = 0,
        AB_TRANSITION = 1,
        STAY_B = 2,
        BA_TRANSITION = 3,
        DONE = 4
    }
    PlayerMovController player_controller;
    public Cinemachine.CinemachineVirtualCamera cameraB;
    public PlayableDirector timeline;

    public SequenceState current_state = SequenceState.IDLE;
    public SequenceState past_state = SequenceState.IDLE;
    [System.NonSerialized]
    public bool triggered = false;
    void Awake()
    {
        if(timeline == null)
        {
            timeline = GetComponent<PlayableDirector>();
        }
        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovController>();
    }

    private void Update()
    {
        if(current_state != past_state)
        {
            switch(current_state)
            {
                case (SequenceState.AB_TRANSITION):
                    {
                        StartAB();
                        break;
                    }
                case (SequenceState.STAY_B):
                    {
                        
                        timeline.Pause();
                        
                        break;
                    }
                case (SequenceState.BA_TRANSITION):
                    {

                        StartBA();
                        if (cameraB.Priority > 10)
                        {
                            cameraB.Priority = 9;
                        }
                        break;
                    }
                case (SequenceState.DONE):
                    {
                        player_controller.EnableInput();
                        break;
                    }
            }
            past_state = current_state;
        }
    }
    void StartAB()
    {
        timeline.Play();
        cameraB.Priority = 20;
        player_controller.DisableInput();
    }
    void StartBA()
    {
        timeline.time = 2;
        timeline.Resume();
    }
    public void SignalAB()
    {
        current_state = SequenceState.STAY_B;
    }
    public void SignalEND()
    {
        current_state = SequenceState.DONE;
    }
    public void SignalSTATE( SequenceState new_state)
    {
        current_state = new_state;
    }
    public void SignalSTARTING()
    {
        current_state = SequenceState.AB_TRANSITION;
        triggered = true;
    }
}
