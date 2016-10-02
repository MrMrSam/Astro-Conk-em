﻿using UnityEngine;
using System.Collections;
//TODO: change implementation to look for more interesting things every x-seconds
//instead of re-chosing interest when timer runs out 
public class BillBoardCam : MonoBehaviour
{
    //references to managers of objects of interest
    private PlayerScript m_player;
    
    private BallSpawner m_ballManager;

    private Vector3 m_target;
    private bool m_lookForNewTarget;
    private INTEREST m_currentInterest;
    private float m_weightUpdateDelta;
    private float m_timeFromLastWeightUpdate;

    private BBCamInterests[] m_interests;

    //billboard camera interests with precedence (0 is greatest)
    public enum INTEREST
    {
        CLOSE_ENEMY,
        BUNCH_OF_ENEMIES,
        BALL_HIT,
        PLAYER,
        SINLGE_ENEMY,
        WIDE_ANGLE_FIELD,

        //length of enum
        LENGTH
    }

	// Use this for initialization
	void Start ()
    {
       
        m_ballManager = GameObject.Find("BallSpawner").GetComponent<BallSpawner>();
        m_player = GameObject.Find("PLAYER").GetComponent<PlayerScript>();

        m_interests = new BBCamInterests[(int)INTEREST.LENGTH];
        m_interests[(int)INTEREST.CLOSE_ENEMY] = new CloseEnemy(this);
        m_interests[(int)INTEREST.BUNCH_OF_ENEMIES] = new CloseEnemy(this);
        m_interests[(int)INTEREST.BALL_HIT] = new CloseEnemy(this);
        m_interests[(int)INTEREST.PLAYER] = new CloseEnemy(this);
        m_interests[(int)INTEREST.SINLGE_ENEMY] = new CloseEnemy(this);
        m_interests[(int)INTEREST.WIDE_ANGLE_FIELD] = new CloseEnemy(this);
        m_currentInterest = INTEREST.WIDE_ANGLE_FIELD;

        m_weightUpdateDelta = 1.0f;
        m_timeFromLastWeightUpdate = 0.0f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (m_timeFromLastWeightUpdate >= m_weightUpdateDelta)
        {
            updateInterestWeights();//this is serperate from looking for new interests incase we want different intervals
            checkForNewInterest();          //but can optimise this out later if that doesn't happen (not even using it now, for example)
            m_timeFromLastWeightUpdate = 0;
        }
        
        m_interests[(int)m_currentInterest].interestUpdate();
        m_timeFromLastWeightUpdate += Time.deltaTime;
    }


    private float precedenceMultiplier(int _index)
    {
        return ((float)INTEREST.LENGTH - (float)_index) * 0.2f;
    }
    private void updateInterestWeights()
    {
        //systematically look for most desirable interest
        //that isn't the current interest
        //NOTE: @@maybe have the chance for a random interest  
        //sometimes, to mix it up
        for (int i = 0; i < m_interests.Length; ++i)
        {
            m_interests[i].recalcWeight();
        }
    }
    private void checkForNewInterest()
    {
        float highestWeight = 0;
        for (int i = 0; i < m_interests.Length; ++i)
        {
            float weight = m_interests[i].currentWeight() * precedenceMultiplier(i);
            //use > not >= to give higher precedence to better interests if equal weight
            if ((INTEREST)i != m_currentInterest && weight > highestWeight)
            {
                m_currentInterest = (INTEREST)i;
                highestWeight = weight;
            }
        }
    }
}