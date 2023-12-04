using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interface for objects that are linked to a switch and have effects when turned on or off
// This interface is not used by the switch object itself, only things that are affected by a switch object
public interface IToggleable
{
    public void ToggleOn();
    public void ToggleOff();
}
