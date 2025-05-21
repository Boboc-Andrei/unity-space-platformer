using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class ICharacterInput {
    public float HorizontalMovement { get; set; }
    public float VerticalMovement { get; set; }
    public bool Jump { get; set; }
    public bool CancelJump { get; set; }
    public bool Grab { get; set; }
    public bool HoldJump { get; set; }
}
