using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace NAK.StateBehaviors.ParameterDriver
{
    [CreateAssetMenu(fileName = "Data", menuName = "StateBehaviors/NAKParamData_Simple", order = 1)]
    public class NAKParamData_Simple : ScriptableObject
    {
        //set & add
        public List<int> driverType;
        //public DriverMode driverMode = DriverMode.OnEnter;
        public List<string> settingName;
        public List<float> settingValue;

        //random
        public List<float> chance;
        public List<float> valMin;
        public List<float> valMax;
        
        //copy
        public List<string> sourceName;
        //copy + convert range
        public List<bool> convertRange;
        public List<float> srcMin;
        public List<float> srcMax;
        public List<float> dstMin;
        public List<float> dstMax;
    }
}

/**
public void ReadData()
{
    int total = stateEnter_data.driverType.Count();
    stateEnter_Entries = new List<NAKParameterDriverEntry>(total);
    
    for (int i = 0; i < total; i++)
    {
        if (stateEnter_data.settingName[i] == "") continue;
        
        var entry = new NAKParameterDriverEntry();
        entry.driverType = (NAKParameterDriverEntry.DriverType)stateEnter_data.driverType[i];
        entry.settingName = stateEnter_data.settingName[i];
        entry.settingValue = stateEnter_data.settingValue[i];
        entry.chance = stateEnter_data.chance[i];
        entry.valMin = stateEnter_data.valMin[i];
        entry.valMax = stateEnter_data.valMax[i];
        entry.sourceName = stateEnter_data.sourceName[i];
        entry.convertRange = stateEnter_data.convertRange[i]; 
        entry.srcMax = stateEnter_data.srcMax[i]; 
        entry.srcMin = stateEnter_data.srcMin[i]; 
        entry.dstMin = stateEnter_data.dstMin[i]; 
        entry.dstMax = stateEnter_data.dstMax[i];
        stateEnter_Entries.Add(entry);
    }
}

public void WriteData()
{
    //this is where i write the stored data... thx unity quirks
    for (int i = 0; i < stateEnter_Entries.Count; i++)
    {
        var self = stateEnter_Entries[i];
        stateEnter_data.driverType[i] = (int)self.driverType;
        stateEnter_data.settingName[i] = self.settingName;
        stateEnter_data.settingValue[i] = self.settingValue;
        stateEnter_data.chance[i] = self.chance;
        stateEnter_data.valMin[i] = self.valMin;
        stateEnter_data.valMax[i] = self.valMax;
        stateEnter_data.sourceName[i] = self.sourceName;
        stateEnter_data.convertRange[i] = self.convertRange;
        stateEnter_data.srcMax[i] = self.srcMax;
        stateEnter_data.srcMin[i] = self.srcMin;
        stateEnter_data.dstMin[i] = self.dstMin;
        stateEnter_data.dstMax[i] = self.dstMax;
    }
}
**/