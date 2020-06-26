using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saveable : MonoBehaviour
{
    [SerializeField] private int _id = default;

    public int ID { get => _id; set => _id = value; }
}