using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager TalkManager;
    public GameObject talkPanel;
    public Text talkText;
    public GameObject scanObject;
    public bool isActivated;
    public int talkIndex;
    public Image portraitImg;
    
    public void Action( GameObject scanObj )
    {
        scanObject = scanObj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk( objData.id, objData.isNPC );
        talkPanel.SetActive( isActivated );
    }

    void Talk( int id, bool isNPC )
    {
        string talkData = TalkManager.GetTalk(id, talkIndex);

        if (talkData == null) {
            isActivated = false;
            talkIndex = 0;
            return;
        }

        if (isNPC) {
            talkText.text = talkData.Split('#')[0];

            portraitImg.sprite = TalkManager.GetPortrait( id, int.Parse(talkData.Split('#')[1]) );
            portraitImg.color = new Color(1, 1, 1, 1);
        }
        else {
            talkText.text = talkData;

            portraitImg.color = new Color(1, 1, 1, 0);
        }

        isActivated = true;
        talkIndex++;
    }
}
