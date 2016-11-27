﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects.Inanimate.Buildings
{
    public abstract partial class BuildingController : InanimateObjectController
    {
        // The character associated with the controller, found in the data structure
        public Building building
        {
            get
            {
                return Buildings.BuildingDictionary[name];
            }
        }

        // This is the object for the character controller
        protected override MacabreObject model
        {
            get
            {
                return building;
            }
        }
    }
}