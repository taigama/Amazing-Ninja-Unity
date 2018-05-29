using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    enum STAGE
    {
        stand = 0,
        dead = 1,
        // cái này không cần thiết dùng tới
        attack = 2
    }    

    public PlayerController target;

    public Animator animator;
    public Rigidbody2D rigid;

    public GameObject ranger;

    private STAGE stage = new STAGE();
    private float jumpRelative;
    
    public void OnEnable()
    {
        rigid.bodyType = RigidbodyType2D.Kinematic;

        stage = STAGE.stand;
        
        // ko thấy hàm nào làm ranger inactive cả, nhưng lâu lâu nó bị
        ranger.SetActive(true);
    }

    
    public void TakeDmg()
    {
        Die();
    }

    void Die()
    {
        stage = STAGE.dead;
        
        animator.Play("red_dead");

        GameController.ScoreAdd(1);
        GameController.SoundPain();
    }
    
    public void Attack()
    {
        if (stage == STAGE.dead)
            return;

        stage = STAGE.attack;
        animator.Play("red_strike");
    }

    public void AttackAgain()
    {
        if (stage == STAGE.dead)
            return;

        // nếu như player chết rồi, thì thôi
        if ((target.stage != PlayerController.MOVE_STAGE.jump)
            && (target.stage != PlayerController.MOVE_STAGE.run))
            return;


        animator.Play("red_strike_back");
        // làm cho thằng này nhảy lên

        GameController.Stop();

        // get different of height
        jumpRelative = ((target.transform.position.y - transform.position.y) * 4.5f)
            + (target.GetComponent<Rigidbody2D>().velocity.y / 2.0f);

        if (jumpRelative < 0.5f)
            return;

        rigid.bodyType = RigidbodyType2D.Dynamic;

        // set velocity up
        rigid.velocity = new Vector3(0, jumpRelative * 2.0f);

        // set rigidbody gravity up
        rigid.gravityScale = jumpRelative;

    }

    public void Faded()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "box")
            animator.Play("red_ready", -1, 0.0f);
    }
}
