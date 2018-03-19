﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public List<GameObject> damageUpgrade = new List<GameObject>();
    public List<GameObject> speedUpgrade = new List<GameObject>();
    public List<GameObject> target = new List<GameObject>();

    public Transform bulletPoint;
    public GameObject bullet;

    public float damageUpgradeCost;
    public float speedUpgradeCost;
    public float reloadTime;
    public float range;
    public float speed;
    public float cost;
    
    GameObject currentTarget;
    Coroutine a;
    bool shot;

    void Start ()
    {
        shot = false;
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            target.Add(other.gameObject);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            target.Remove(other.gameObject);
        }
    }

    void Update()
    {
        for (int a = 0; a < target.Count; a++)
        {
            if (target[a].GetComponent<Enemy>().enemyHealth <= 0)
            {
                target.Remove(target[a]);
            }
        }

        if (target.Count > 0)
        {
            currentTarget = target[0];
            Vector3 targetDir = currentTarget.transform.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, speed * Time.deltaTime, 0.0F);
            newDir.y = 0;
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        if (!shot & target.Count > 0)
        {
            a = StartCoroutine(Shoot());
            shot = true;
        }
        if (currentTarget != null && currentTarget.gameObject.GetComponent<Enemy>().enemyHealth <= 0)
        {
            target.Remove(currentTarget.gameObject);
        }
        if(target == null)
        {
            StopCoroutine(a);
            shot = false;
            target.Remove(currentTarget.gameObject);
        }
    }

    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(reloadTime);

        Instantiate(bullet, bulletPoint.transform.position, bulletPoint.transform.rotation);
        shot = false;
    }
}