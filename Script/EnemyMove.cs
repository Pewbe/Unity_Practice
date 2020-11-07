using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour //적의 움직임, 상태 등 적의 AI구현
{
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider;
    public int nextMove;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        Invoke("Think", 5);
    }

    void FixedUpdate()
    {
        //움직임
        rigid.velocity = new Vector2( nextMove, rigid.velocity.y );

        //지형 체크
        Vector2 frontVec = new Vector2( rigid.position.x + nextMove*0.5f, rigid.position.y );
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayhit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("platform"));

        if (rayhit.collider == null)
            Turn();
    }

    //재귀함수
    void Think(){
        //다음 활동
        nextMove = Random.Range(-1, 2);

        float nextThinkTime = Random.Range(2f, 6f);

        anim.SetInteger("MoveSpeed", nextMove );
        //방향변경
        if ( nextMove != 0 )
            spriteRenderer.flipX = nextMove == 1;

        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove = nextMove * -1;
        spriteRenderer.flipX = nextMove == 1;
        CancelInvoke();
        Invoke("Think", 5);
    }

    public void OnDameged()
    {
        //스프라이트 투명도
        spriteRenderer.color = new Color( 1, 1, 1, 0.4f );
        //죽을 때 뒤집어지기(Y축 반전)
        spriteRenderer.flipY = true;
        //콜라이더 비활성화
        capsuleCollider.enabled = false;
        //죽을 때 이펙트
        rigid.AddForce( Vector2.up * 5, ForceMode2D.Impulse );
        //사라지기
        Invoke( "DeActive", 5 );
    }

    void DeActive()
    {
        gameObject.SetActive( false );
    }
}
