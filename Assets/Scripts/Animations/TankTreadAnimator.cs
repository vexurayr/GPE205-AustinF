using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTreadAnimator : MonoBehaviour
{
    public float moveSpeedX = 1.0f;
    public float moveSpeedY = 0f;
    private float OffsetX;
    private float OffsetY;

    public GameObject leftTread;
    public GameObject rightTread;

    private void Start()
    {
        float OffsetX = Time.deltaTime * moveSpeedX;
        float OffsetY = Time.deltaTime * moveSpeedY;
    }

    public void Forward()
    {
        leftTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
        rightTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
    }

    public void Backwards()
    {
        leftTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-OffsetX, OffsetY);
        rightTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-OffsetX, OffsetY);
    }

    public void Clockwise()
    {
        leftTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
        rightTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-OffsetX, OffsetY);
    }

    public void Counterclockwise()
    {
        leftTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-OffsetX, OffsetY);
        rightTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(OffsetX, OffsetY);
    }
}
