﻿using UnityEngine;
using System.Collections;

namespace Kamgam.BridgeBuilder25D
{
    public interface ITrigger2DReceiver
    {
        void OnCustomTriggerEnter2D(Trigger2D trigger, Collider2D other);
        void OnCustomTriggerStay2D(Trigger2D trigger, Collider2D other);
        void OnCustomTriggerExit2D(Trigger2D trigger, Collider2D other);
    }
}