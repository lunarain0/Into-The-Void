using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using DG.Tweening;
using System;

public class CameraController : MonoBehaviour
{

    public float speed;
    public Transform target;
    public Vector3 offset;
    public Vector3 distance;
    private Vector3 rotatepoint;
    public int zoomedAmount = 0;
    public float relativeHeight;
    public Camera shadowCamera;
    public Shader shader;
    //bool zoomed;
    int layermask = 1 << 9;
    //Transform obstruction ;
    private void Start()
    {
        rotatepoint = offset + distance;
    }

    public void FilterModify(bool active)
    {
        if (active)
        DOTween.To(() => gameObject.GetComponent<PostProcessVolume>().weight, x => gameObject.GetComponent<PostProcessVolume>().weight = x, 1f, 0.5f);
        else
        DOTween.To(() => gameObject.GetComponent<PostProcessVolume>().weight, x => gameObject.GetComponent<PostProcessVolume>().weight = x, 0f, 0.5f);
    }
    
    // Update is called once per frame
    void Update()
    {
        CameraObstruction();
        if (Time.timeScale != 0 && GameObject.Find("character").GetComponent<PlayerController>().inCutscene == false)
        {


            relativeHeight = transform.position.y - target.position.y;
            if (Input.GetAxis("PanCamera") != 0)
            {
                rotatepoint = Quaternion.AngleAxis(Input.GetAxis("PanCamera") * -speed, transform.up) * rotatepoint;
            }
            if (Input.GetAxis("RollCamera") != 0)
            {
                if (relativeHeight > -8f && relativeHeight < 15f)
                {
                    rotatepoint = Quaternion.AngleAxis(Input.GetAxis("RollCamera") * speed, transform.right) * rotatepoint;


                }
                else if (relativeHeight > 15f)
                {
                    if (Input.GetAxis("RollCamera") < 0)
                    {
                        rotatepoint = Quaternion.AngleAxis(Input.GetAxis("RollCamera") * speed, transform.right) * rotatepoint;
                    }

                }
                else if (relativeHeight < -8f)
                {
                    if (Input.GetAxis("RollCamera") > 0)
                    {
                        rotatepoint = Quaternion.AngleAxis(Input.GetAxis("RollCamera") * speed, transform.right) * rotatepoint;
                    }
                }
            }
            transform.position = target.position + rotatepoint;

            transform.LookAt(target.position + offset);
        }
    }
    
    void CameraObstruction()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + transform.forward * 15, -transform.forward , out hit,15f, ~layermask, QueryTriggerInteraction.Ignore))
        {
            shadowCamera.enabled = true;
            shadowCamera.SetReplacementShader(shader,"");
            
        }
        else
        {
            shadowCamera.enabled = false;
            shadowCamera.ResetReplacementShader();
        }
    }
    
}
