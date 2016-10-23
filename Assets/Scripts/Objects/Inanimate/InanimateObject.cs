﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

using System.Xml.Serialization;

namespace Objects.Inanimate
{
    public abstract class InanimateObject : MacabreObject
    {
        [XmlIgnore]
        private GameObject gameObject;
       
        public string name;
        public string description;
    }
}