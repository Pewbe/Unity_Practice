using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour//플레이어 캐릭터의 움직임, 행동 등을 관리
{
    public GameManager gameManager;
    public float maxSpeed;
    public float jumpPower;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    CapsuleCollider2D capsuleCollider;

    void Awake()
    {
        //초기화. 초기화는 항상 Awake에
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        //움직임 속도
        float h = Input.GetAxisRaw("Horizontal");
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        //점프
        if (Input.GetButtonDown("Jump") && !anim.GetBool("isJumping") ){
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
        }
        //멈춤 속도 조정
        if( Input.GetButtonUp("Horizontal"))
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f, rigid.velocity.y);

        //방향 변경 스프라이트 반전
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        //애니메이션
        if (Mathf.Abs( rigid.velocity.x ) < 1)
            anim.SetBool("isRunning", false);
        else
            anim.SetBool("isRunning", true);
    }
    void FixedUpdate()
    {
        //최고 속도
        if (rigid.velocity.x > maxSpeed)
            rigid.velocity = new Vector2( maxSpeed, rigid.velocity.y );
        else if (rigid.velocity.x < maxSpeed * (-1))
            rigid.velocity = new Vector2(maxSpeed * (-1), rigid.velocity.y);

        //땅에 닿았을 때
        if( rigid.velocity.y < 0){
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

            RaycastHit2D rayhit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("platform"));

            if (rayhit.collider != null)
            {
                if (rayhit.distance < 0.5f)
                    anim.SetBool("isJumping", false);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //적 피격 시
        if (collision.gameObject.tag == "Enemy")
        {
            //밟아서 공격
            if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
                OnAttack( collision.transform );
            else //피해
                OnDameged(collision.transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.gameObject.tag == "item" )
        {
            //점수 획득
            bool isNormal = collision.gameObject.name.Contains("DarkCoin");//오브젝트의 이름으로 구분하여 점수 획득

            if( isNormal )
                gameManager.stagePoint += 100;
            else
                gameManager.stagePoint += 1000;
            //사라지기
            collision.gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "finish")
        {
            //다음 스테이지로
            gameManager.NextStage();
        }
    }
    void OnAttack( Transform enemy )
    {
        //점수 획득
        gameManager.stagePoint += 100;
        //밟았을 때 반응(위로 튀어오르기)
        rigid.AddForce( Vector2.up * 5, ForceMode2D.Impulse );
        //적 사망
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDameged();
    }

    void OnDameged( Vector2 targetPos )
    {
        //체력 깎임
        gameManager.HealthDown();
        //피격 시 레이어 조정
        gameObject.layer = 11;

        //피격 시 투명도 조절
        spriteRenderer.color = new Color( 1, 1, 1, 0.4f );

        //피격 시 움직임
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce( new Vector2( dirc, 1 )*7, ForceMode2D.Impulse );

        //애니메이션
        anim.SetTrigger("doDameged");

        Invoke("OffDameged", 2);
    }

    void OffDameged()
    {
        //무적시간 후 레이어 원래대로 변경
        gameObject.layer = 10;

        //무적시간 후 추명도 원래대로 변경
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void OnDie()
    {
        capsuleCollider.enabled = false;
        //스프라이트 투명도
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        //죽을 때 뒤집어지기(Y축 반전)
        spriteRenderer.flipY = true;
        //콜라이더 비활성화
        capsuleCollider.enabled = false;
        //죽을 때 이펙트
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
    }
}
