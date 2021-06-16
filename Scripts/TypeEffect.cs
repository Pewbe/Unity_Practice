using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    string targetMsg;
    public int CPS; //Char Per Seconds
    public GameObject dialogueButton;
    private Text msgText;
    private int textIndex;
    private float interval;
    private AudioSource audioSource;
    public bool isAnim;

    private void Awake()
    {
        try
        {
            msgText = GetComponent<Text>();
            audioSource = GetComponent<AudioSource>();
        } catch( MissingComponentException ){}
    }

    public void SetMessage(string msg)
    {
        if (isAnim) {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }
        else {
            targetMsg = msg;
            EffectStart();
        }
    }

    void EffectStart()
    {
        msgText.text = "";
        textIndex = 0;
        dialogueButton.SetActive(false);

        interval = 1.0f / CPS;
        Debug.Log( interval );

        isAnim = true;
        Invoke("Effecting", interval);
        //Invoke: 해당 함수(methodName)를 시간차(time)을 두고 실행하는 함수
    }

    void Effecting()
    {
        if (msgText.text == targetMsg)
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[textIndex]; //이게됨ㅋㅋ
        
        if( targetMsg[textIndex] != ' ' )
            audioSource.Play();
        
        textIndex++;
        
        Invoke("Effecting",  interval); //재귀함수
    }

    void EffectEnd()
    {
        isAnim = false;
        dialogueButton.SetActive(true);
    }
}//Tenshi-chan Is Love