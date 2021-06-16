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
        //��ȭâ�� �����Ǿ��ִٸ� ��ġ ������Ű��
        h = manager.isActivated ? 0 : Input.GetAxisRaw("Horizontal");
        v = manager.isActivated ? 0 : Input.GetAxisRaw("Vertical");

        bool hDown = manager.isActivated ? false : Input.GetButtonDown("Horizontal");
        bool vDown = manager.isActivated ? false : Input.GetButtonDown("Vertical");
        bool hUp = manager.isActivated ? false : Input.GetButtonUp("Horizontal");
        bool vUp = manager.isActivated ? false : Input.GetButtonUp("Vertical");

        if (hDown)//���ι��� �̵� ��ư�� �����°�
            isHorizonMove = true;
        else if (vDown)//���ι��� �̵� ��ư�� �����°�
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
        
        //�ٶ󺸰� �ִ� ���� ����
        if (vDown && v == 1) //���� ��ư�� ������ ���ÿ� �� ������ ��(1)�� ��
            dirVec = Vector3.up;
        else if( vDown && v == -1 )//���� ��ư�� ������ ���ÿ� �� ������ �Ʒ�(-1)�� ��
            dirVec = Vector3.down;
        else if( hDown && h == 1 )//�¿� ��ư�� ������ ���ÿ� �� ������ ������(1)�� ��
            dirVec = Vector3.right;
        else if (hDown && h == -1) //�¿� ��ư�� ������ ���ÿ� �� ������ ����(-1)�� ��
            dirVec = Vector3.left;
        
        //raycast�� ��ĵ�� ������Ʈ ���
        if (Input.GetButtonDown( "Click" ) && scanObject != null) //raycast�� ������Ʈ�� ���������� ���ÿ� zŰ�� ������ ���
            manager.Action( scanObject );
    }

    void FixedUpdate()
    {
        //�̵�
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
        
        //���� ��ĵ(ray)
        Debug.DrawRay( rigid.position, dirVec * 0.7f, new Color(0, 1, 0) );
        RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));

        if (rayhit.collider != null) //rathit�� �ݶ��̴��� null�� �ƴ� ��(���� ������ ��)
            scanObject = rayhit.collider.gameObject;//�������� ���� ������Ʈ�� ��ü�� ������
        else
            scanObject = null;
    }
}
