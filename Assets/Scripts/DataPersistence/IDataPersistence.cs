using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


interface IDataPersistence {
    public string Scene { get; }
    public void LoadData(GameData data);
    public void SaveData(ref GameData data);
}
