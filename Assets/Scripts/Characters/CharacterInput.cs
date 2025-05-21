using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class CharacterInput : MonoBehaviour {
    public virtual float HorizontalMovement { get; set; }
    public virtual float VerticalMovement { get; set; }
    public virtual bool Jump { get; set; }
    public virtual bool CancelJump { get; set; }
    public virtual bool Grab { get; set; }
}
