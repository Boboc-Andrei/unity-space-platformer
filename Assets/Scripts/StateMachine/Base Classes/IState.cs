using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IState {
    public void OnEnter();
    public void Update();
    public void FixedUpdate();
    public void OnExit();
}
