﻿using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class ApplyToWhom {
    public int Generation { get; set; }
    public string PersonID { get; set; }
}

public class Flag {
    public string Name { get; set; }
    public ApplyToWhom ApplyToWhom { get; set; }
    public string Bio { get; set; }
}

[XmlRoot("Evidence")]
public class EvidenceBaseData {
    public string Name { get; set; }
    public string ID { get; set; }
    public string Icon { get; set; }
    public string[] HoverTextOptions { get; set; }
    public Flag[] Flags { get; set; }

}

public class EvidenceBase : MonoBehaviour {

    public string Name; 
    public string ID;
    public string Icon; 
    public string[] HoverTextOptions; 
    public Flag[] Flags;

    public void setFromBaseData(EvidenceBaseData setFrom) {
        print("Setting the evidence base...");
        Name = (string)setFrom.Name.Clone();
        ID = (string)setFrom.ID.Clone();
        Icon = (string)setFrom.Icon.Clone();
        HoverTextOptions = setFrom.HoverTextOptions;
        Flags = setFrom.Flags;
    }
}
