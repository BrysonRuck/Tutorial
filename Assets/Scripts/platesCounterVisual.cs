using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class platesCounterVisual : MonoBehaviour
{

    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private Transform plateVisualPrefab;
    [SerializeField] private PlatesCounter platesCounter;

    private List<GameObject> platesVisualGameObjectList;

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
    }

    private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
    {
        GameObject plateGameObject = platesVisualGameObjectList[platesVisualGameObjectList.Count - 1];
        platesVisualGameObjectList.Remove(plateGameObject);
        Destroy(plateGameObject);
    }

    private void Awake()
    {
               platesVisualGameObjectList = new List<GameObject>();

    }
    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);
        float plateOffsetY = .1f;
        plateVisualTransform.localPosition = new Vector3(0,plateOffsetY * platesVisualGameObjectList.Count,0);

        platesVisualGameObjectList.Add(plateVisualTransform.gameObject);
    }



}
