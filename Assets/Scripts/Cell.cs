using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : Container<Unit>
{
    public override Unit Content 
    { 
        get => base.Content;
        set
        {
            base.Content = value;
            if(value != null)
            {
                value.currentCell = this;
            }
        }
    }
}
