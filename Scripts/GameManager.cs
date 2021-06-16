using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TalkManager talkManager;
    public Animator talkPanel;
    public TypeEffect talk;
    public GameObject scanObject;
    public GameObject menuSet;
    public GameObject player;
    public Text QuestName;
    public bool isActivated;
    public int talkIndex;
    public Image portraitImg;
    public QuestManager questManager;
    public Animator portChangeing;
    public Sprite prevSprite;

    private void Start()
    {
        GameLoad();
        QuestName.text =  questManager.CheckQuest();
    }

    private void Update()
    {
        //서브 메뉴
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuSet.activeSelf) {
                menuSet.SetActive(false);
                Time.timeScale = 1;
            } else {
                menuSet.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }

    public void Action( GameObject scanObj )
    {
        scanObject = scanObj;
        ObjectData objData = scanObject.GetComponent<ObjectData>();
        Talk( objData.id, objData.isNPC );
        
        //대화창 표시
        talkPanel.SetBool("isShow", isActivated);
    }

    void Talk( int id, bool isNPC )
    {
        int questTalkIndex;
        string talkData;
        
        if (talk.isAnim) {
            talk.SetMessage("");
            return;
        }
        else {
            questTalkIndex = questManager.GetQuestTalkIndex(id);
            talkData = talkManager.GetTalk(id + questTalkIndex, talkIndex);
        }

        //대화 끝남
        if (talkData == null) {
            isActivated = false;
            talkIndex = 0;
            QuestName.text = questManager.CheckQuest(id);
            return;
        }
        
        //대화 계속하기
        if (isNPC) {
            talk.SetMessage( talkData.Split('#')[0] );

            portraitImg.sprite = talkManager.GetPortrait( id, int.Parse(talkData.Split('#')[1]) );
            portraitImg.color = new Color(1, 1, 1, 1);
            
            //스프라이트 변경 애니메이션(이전 스프라이트와 바꾸려는 스프라이트가 다를 떄)
            if (!prevSprite.Equals(portraitImg.sprite)) {
                portChangeing.SetTrigger("Changeing");
                prevSprite = portraitImg.sprite;
            }
        }
        else {
            talk.SetMessage(talkData);

            portraitImg.color = new Color(1, 1, 1, 0);
            
        }

        isActivated = true;
        talkIndex++;
    }

    public void GameSave()
    {
        //플레이어의 위치(X)
        PlayerPrefs.SetFloat( "playerX", player.transform.position.x );
        //플레이어의 위치(Y)
        PlayerPrefs.SetFloat( "playerY", player.transform.position.y );
        //퀘스트 ID
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        //퀘스트 액션 인덱스(questActionIndex)
        PlayerPrefs.SetInt("QuestActionIndex", questManager.questActionIndex);
        
        PlayerPrefs.Save();
        
        SetDeactive();
    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
            return;
        
        float x = PlayerPrefs.GetFloat("playerX");
        float y = PlayerPrefs.GetFloat("playerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionIndex = PlayerPrefs.GetInt("QuestActionIndex");

        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionIndex;
        
        questManager.ControlObject();
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void SetDeactive()
    {
        menuSet.SetActive(false);
        Time.timeScale = 1;
    }
}
