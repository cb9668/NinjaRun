﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCanvas : MonoBehaviour {

public void Restart()
    {
        GameManager.instance.LoadStart();
    }
}
