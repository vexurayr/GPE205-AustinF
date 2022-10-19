using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTreadAnimator : MonoBehaviour
{
    public float moveSpeedX = 1.0f;
    public float moveSpeedY = 0f;
    private float offsetX;
    private float offsetY;

    public GameObject leftTread;
    public GameObject rightTread;

    private void Start()
    {
        offsetX = Time.deltaTime * moveSpeedX;
        offsetY = Time.deltaTime * moveSpeedY;
    }

    public void Forward()
    {
        leftTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offsetX, offsetY);
        rightTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }

    public void Backwards()
    {
        leftTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-offsetX, offsetY);
        rightTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-offsetX, offsetY);
    }

    public void Clockwise()
    {
        leftTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offsetX, offsetY);
        rightTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-offsetX, offsetY);
    }

    public void Counterclockwise()
    {
        leftTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(-offsetX, offsetY);
        rightTread.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}