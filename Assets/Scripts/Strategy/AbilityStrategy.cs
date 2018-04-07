using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class AbilityStrategy : MonoBehaviour {

    [SerializeField]
    private float m_StartingSpeed = 4;

    [SerializeField]
	private float m_Speed = 4;

    protected GameObject m_player;

    Vector2 m_fearDirection;
    float m_fearSpeed = 0;
    Timer m_fearTimer;

    bool m_isInit = false;



    [SerializeField]
    protected float m_ability1Cooldown = 5.0f;
    protected Timer m_ability1Timer;

    
    private void Start()
    {
        m_ability1Timer = TimerFactory.INSTANCE.getTimer();
        m_ability1Timer.StartTimer(m_ability1Cooldown);
        m_ability1Timer.Stop();
    }


    public void Init( GameObject a_player)
    {
        m_player = a_player;
        m_fearTimer = TimerFactory.INSTANCE.getTimer();

        m_isInit = true;
    }

    public bool IsFear()
    {
        return m_fearTimer && m_fearTimer.IsTimerRunning();
    }


    //Give direction of movement
    Vector2 GetDirection()
    {
        if (!m_isInit) return Vector2.zero;
        Vector2 result = Vector2.zero;
        if (IsFear())
        {
            result =  m_fearDirection;
        }
        else
        {
            // Handle simple movement
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 playerPos = m_player.transform.position;
            if (Vector2.Distance(mousePos, playerPos) > 0.05f )
            {

                result = mousePos - playerPos;
            }

        }
        result.Normalize();

        return result;
    }

    //give speed
    float GetSpeed()
    {
        if (!m_isInit) return 0;

        float result = 0;
        if (IsFear())
        {
            result = m_fearSpeed;
        }
        else
        {
            if ((Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z)))
            {
                result = m_Speed;
            }

        }
        return result;
    }



    // Movement
    public virtual void PlayerMovement()
	{
        if (!m_isInit) return;

        Vector2 direction = GetDirection();
        m_player.GetComponent<Rigidbody2D>().velocity = direction * GetSpeed();
		Vector3 movingDir = new Vector3(direction.x, direction.y, 0f);
		float angle = Mathf.Atan2(movingDir.y, movingDir.x) * Mathf.Rad2Deg;
        m_player.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

	}


    public void UseAbility1()
    {
        if (m_ability1Timer.IsTimeUp())
        {
            Ability1();
            m_ability1Timer.RestartTimer();
        }
    }

    // Ability1
    protected virtual void Ability1() {}



    // Ability2
    protected virtual void Ability2(){}


    public void Fear(Vector3 a_position, float a_speed, float a_time)
    {
        if (!IsFear())
        {
            m_fearDirection = -1 * (a_position - gameObject.transform.position);
            m_fearSpeed = a_speed;
            m_fearTimer.StartTimer(a_time);
        }
    }


    // Death
    public virtual void PlayerDeath(){}


    private void OnDestroy()
    {
        if (m_fearTimer)
        {
            m_fearTimer.Destroy();
        }
        if (m_ability1Timer)
        {
            m_ability1Timer.Destroy();
        }
    }

    public void SetSpeed(float a_speed)
    {
        m_Speed = a_speed;
    }
}
