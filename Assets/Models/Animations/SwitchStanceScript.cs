using UnityEngine;
using UnityEngine.UIElements;
using Vuforia;

public class SwitchStanceScript : MonoBehaviour
{
    private float triggerDistance = 0.7f; 
    private float fightDistance = 0.55f;
    public int charID = 1;
    private Animator anim;
    public ObserverBehaviour otherTarget; 
    private ObserverBehaviour thisTarget;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        thisTarget = GetComponent<ObserverBehaviour>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
{
    bool thisVisible = thisTarget.TargetStatus.Status == Status.TRACKED || 
                       thisTarget.TargetStatus.Status == Status.EXTENDED_TRACKED;

    bool otherVisible = otherTarget != null && 
                        (otherTarget.TargetStatus.Status == Status.TRACKED || 
                         otherTarget.TargetStatus.Status == Status.EXTENDED_TRACKED);

    if (thisVisible && otherVisible)
    {
        float distance = Vector3.Distance(transform.position, otherTarget.transform.position);

        if (distance < triggerDistance && distance > fightDistance)
        {
            anim.SetInteger("AnimState", 1);
            rotateCharacter();
        }
        else if(distance < fightDistance)
        {
            switch (charID)
            {
                case 1:
                    anim.SetInteger("AnimState", 2);
                    rotateCharacter();
                    break;
                case 2:
                    anim.SetInteger("AnimState", 3);
                    rotateCharacter();
                    break;
                default:
                    break;
            }
        }
        else
        {
            anim.SetInteger("AnimState", 0);
            resetCharacterRotation();
        }

    }
    else{
        anim.SetInteger("AnimState", 0);
    }
}

    private void rotateCharacter()
    {
        if (anim == null || otherTarget == null) return;

        Vector3 targetPosition = otherTarget.transform.position;
        targetPosition.y = anim.transform.position.y;

        Vector3 direction = targetPosition - anim.transform.position;

        if (direction.sqrMagnitude > 0.001f)
        {
            anim.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void resetCharacterRotation()
    {
        if (anim != null)
        {
            anim.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        }
    }
}
