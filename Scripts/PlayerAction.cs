using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public float speed;
    float h;
    float v;
    bool isHorizonMove;
    private Vector3 dirVec;
    private GameObject scanObject;
    public GameManager manager;
    
    Rigidbody2D rigid;
    Animator anim;

    void Awake()
    {
        try
        {
            rigid = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        } catch( MissingComponentException ) {}
    }

    void Update()
    {
        //대화창이 생성되어있다면 위치 고정시키기
        h = manager.isActivated ? 0 : Input.GetAxisRaw("Horizontal");
        v = manager.isActivated ? 0 : Input.GetAxisRaw("Vertical");

        bool hDown = manager.isActivated ? false : Input.GetButtonDown("Horizontal");
        bool vDown = manager.isActivated ? false : Input.GetButtonDown("Vertical");
        bool hUp = manager.isActivated ? false : Input.GetButtonUp("Horizontal");
        bool vUp = manager.isActivated ? false : Input.GetButtonUp("Vertical");

        if (hDown)//가로방향 이동 버튼을 눌렀는가
            isHorizonMove = true;
        else if (vDown)//세로방향 이동 버튼을 눌렀는가
            isHorizonMove = false;
        else if (hUp || vUp)
            isHorizonMove = h != 0;

        if (anim.GetInteger("hAxisRaw") != (int) h){
            anim.SetBool("isChange", true);
            anim.SetInteger("hAxisRaw", (int) h);
        }
        else if (anim.GetInteger("vAxisRaw") != (int) v){
            anim.SetBool("isChange", true);
            anim.SetInteger("vAxisRaw", (int) v);
        }
        else
            anim.SetBool("isChange", false);
        
        //바라보고 있는 방향 지정
        if (vDown && v == 1) //상하 버튼을 누름과 동시에 그 방향이 위(1)일 때
            dirVec = Vector3.up;
        else if( vDown && v == -1 )//상하 버튼을 누름과 동시에 그 방향이 아래(-1)일 때
            dirVec = Vector3.down;
        else if( hDown && h == 1 )//좌우 버튼을 누름과 동시에 그 방향이 오른쪽(1)일 때
            dirVec = Vector3.right;
        else if (hDown && h == -1) //좌우 버튼을 누름과 동시에 그 방향이 왼쪽(-1)일 때
            dirVec = Vector3.left;
        
        //raycast에 스캔된 오브젝트 출력
        if (Input.GetButtonDown( "Click" ) && scanObject != null) //raycast에 오브젝트가 잡혀있음과 동시에 z키를 눌렀을 경우
            manager.Action( scanObject );
    }

    void FixedUpdate()
    {
        //이동
        if ( Input.GetKey("left shift") )
        {
            Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
            rigid.velocity = moveVec * (speed+3);
        }
        else
        {
            Vector2 moveVec = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
            rigid.velocity = moveVec * speed;
        }
        
        //조사 스캔(ray)
        Debug.DrawRay( rigid.position, dirVec * 0.7f, new Color(0, 1, 0) );
        RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if (rayhit.collider != null) //rathit의 콜라이더가 null이 아닐 때(뭔가 잡혔을 때)
            scanObject = rayhit.collider.gameObject;//레이저에 닿은 오브젝트의 객체를 가져옴
        else
            scanObject = null;
    }
}
