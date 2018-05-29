using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum MOVE_STAGE
    {
        run = 0,
        jump = 1,
        stop_killed = 2,
        stop_touch_blue = 3,
        stop_kill_blue = 4,
        stop_fall = 5
    }

    public MOVE_STAGE stage;
    private Rigidbody2D rigid;
    private Vector3 posOriginal;

    // --- animation
    public GameObject body;
    public Animator headAnim;
    public Animator legAnim;
    private Animator animator;

    // --- âm thanh
    new private AudioSource audio;
    public AudioClip clipStep1;
    public AudioClip clipStep2;
    public AudioClip clipJump;
    public AudioClip clipBump;
    public AudioClip clipHit;

    // --- nhảy
    public float jumpForce;
    private int jumpLevel;

    // --- chạy
    public float stepTime;
    private float timer = 0;
    private bool isStepLeft = false;

    //public float speedMove;
    //private Vector3 vecMove;
    private Vector3 vecPos;
    private Transform transCam;

    void Start()
    {
        transCam = GameObject.FindGameObjectWithTag("MainCamera").transform;
        posOriginal = transform.position;
        //vecMove = new Vector3(speedMove, 0);
        vecPos = new Vector3(posOriginal.x + transCam.position.x, 0f);

        stage = MOVE_STAGE.run;
        jumpLevel = 0;

        animator = body.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(GameController.IsRun())
        {
            vecPos.x = posOriginal.x + transCam.position.x;
            vecPos.y = transform.position.y;

            transform.position = vecPos;
            //transform.position = 
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            rigid.gravityScale = 0f;
            transform.position += Vector3.up;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            rigid.gravityScale = 10f;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Jump();
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Attack();
        }

        if (!GameController.IsRun())
            return;
        if (stage == MOVE_STAGE.run)
            SoundRun();
    }


    void SoundRun()
    {
        if(timer <= 0)
        {
            timer = stepTime;

            if(isStepLeft)
            {
                audio.clip = clipStep1;
                audio.Play();
                isStepLeft = false;
            }
            else
            {
                audio.clip = clipStep2;
                audio.Play();
                isStepLeft = true;
            }
            return;
        }

        timer -= Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "ally")
        {
            col.gameObject.GetComponent<Blue>().TakeDmg();
            DieLie();
        }
        else if (col.gameObject.tag == "ground")
        {
            if (stage != MOVE_STAGE.jump)
            {
                return;
            }

            // lấy vector pháp tuyến của bề mặt
            Vector3 normal = col.contacts[0].normal;

            //Debug.Log("normal: " + normal);

            // nếu vector pháp tuyến chỉ lên trên -> đáp đất
            if (normal.y > 0)
            {
                stage = MOVE_STAGE.run;
                animator.Play("run", -1, 0.0f);

                headAnim.Play("head_ready", -1, 0.0f);
                legAnim.Play("foot_ready", -1, 0.0f);
                
                jumpLevel = 0;

                // reset thoi gian phat am thanh
                audio.clip = clipStep1;
                audio.Play();
                timer = stepTime;
            }
            if (normal.x < -0.5) // thêm if này mới toàn vẹn
            {
                audio.clip = clipBump;
                audio.Play();

                rigid.velocity = new Vector3(-10.0f, 10.0f);
                TouchWall();
                // rung camera
                ShakeCamera.Shake();
            }
        }
    }

    void TouchWall()
    {
        stage = MOVE_STAGE.stop_fall;

        animator.Play("player_fall", -1, 0.0f);
        headAnim.Play("head_nothing", -1, 0.0f);
        legAnim.Play("foot_notrun", -1, 0.0f);

        GetComponent<Collider2D>().enabled = false;
        rigid.constraints = RigidbodyConstraints2D.None;
                

        GameController.Stop();
    }

    void OnCollisionExit2D(Collision2D col)
    {
        // rời khỏi cục gạch
        if(col.gameObject.tag == "ground" && stage == MOVE_STAGE.run)
        {
            Vector3 normal = col.contacts[0].normal;

            // rớt thẳng xuống (chứ không phải tuột trên bề mặt dốc)
            if (normal.x == 0f)
            {


                stage = MOVE_STAGE.jump;
                jumpLevel = 1;

                TrulyJump();
            }
        }
    }

    public void TakeDmg(int direct)
    {
        if(direct == 1)
            transform.localScale = new Vector3(-1.0f, 1.0f);

        // chết trên không
        if (stage == MOVE_STAGE.jump)
        {
            TouchWall();
            rigid.velocity = new Vector3(10.0f, 10.0f);            
        }
        else if (stage == MOVE_STAGE.run)
        {
            DieLie();
        }
    }

    void DieLie()
    {
        stage = MOVE_STAGE.stop_killed;

        headAnim.Play("head_nothing", -1, 0.0f);
        legAnim.Play("foot_notrun", -1, 0.0f);
        animator.Play("dead_lie", -1, 0.0f);

        GameController.Stop();
    }

    public void KillAlly()
    {
        stage = MOVE_STAGE.stop_kill_blue;
        headAnim.Play("head_nothing", -1, 0.0f);
        legAnim.Play("foot_notrun", -1, 0.0f);
        animator.Play("kill_blue", -1, 0.0f);
    }

    public void Jump()
    {
        if (stage == MOVE_STAGE.run)
        {
            jumpLevel = 1;
            stage = MOVE_STAGE.jump;

            rigid.velocity = new Vector3(0.0f, jumpForce);
            TrulyJump();
        }
        else if ((stage == MOVE_STAGE.jump) && (jumpLevel == 1))
        {
            jumpLevel = 2;

            rigid.velocity = new Vector3(0.0f, jumpForce);
            TrulyJump();
        }
    }

    void TrulyJump()
    {
        animator.Play("player_jump_circle", -1, 0.0f);
        headAnim.Play("head_nothing", -1, 0.0f);
        legAnim.Play("foot_notrun", -1, 0.0f);

        audio.clip = clipJump;
        audio.Play();
    }

    public void Attack()
    {
        if (stage == MOVE_STAGE.run)
        {
            headAnim.Play("head_attack", -1, 0.0f);
        }
    }
    
    public void Reset()
    {
        gameObject.SetActive(true);

        stage = MOVE_STAGE.run;
        jumpLevel = 0;

        animator.Play("run", -1, 0.0f);
        headAnim.Play("head_ready", -1, 0.0f);
        legAnim.Play("foot_ready", -1, 0.0f);


        transform.position = new Vector3(-4.25f,-2.8f);
        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.localScale = Vector3.one;

        rigid.constraints = RigidbodyConstraints2D.FreezePositionX
            | RigidbodyConstraints2D.FreezeRotation;

        
        //foreach(Transform child in transform)
        //{
        //    // lâu lâu lỗi rotation, dùng cái này fix (thực ra ko biết đúng ko)
        //    //child.localRotation = Quaternion.identity;

        //    //child.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        //    child.rotation = new Quaternion(0f, 0f, 0f, 0f);
        //}

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).rotation = new Quaternion(0f, 0f, 0f, 0f);
        }

        rigid.velocity = Vector3.zero;
        rigid.gravityScale = 10f;
        
        GetComponent<Collider2D>().enabled = true;

        transform.position = posOriginal;
    }

    /* đúng ra cái này để trong mấy con Xanh xanh
        nhưng để đây thì tối ưu phần cứng hơn */
    public void SoundHit()
    {
        audio.clip = clipHit;
        audio.Play();
    }
}