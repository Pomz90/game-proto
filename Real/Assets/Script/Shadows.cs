using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadows : MonoBehaviour
{

    public float ghostDelay;
    private float ghostDelaySecond;
    public GameObject ghost;
    public bool makeGhost = false;


    void Update()
    {
        if (makeGhost)
        {
            if (ghostDelaySecond > 0)
            {
                ghostDelaySecond -= Time.deltaTime;
            }
            else
            {
                GameObject obj = Instantiate(ghost, transform.position, transform.rotation);
                Sprite objSprite = GetComponent<SpriteRenderer>().sprite;
                obj.GetComponent<SpriteRenderer>().sprite = objSprite;
                obj.transform.localScale = this.transform.localScale;
                ghostDelaySecond = ghostDelay;
                Destroy(obj, 0.1f);
            }

        }

    }

    private void Start()
    {
        ghostDelaySecond = ghostDelay;
    }

}
