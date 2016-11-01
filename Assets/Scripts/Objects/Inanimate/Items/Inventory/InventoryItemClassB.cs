﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Data.Database;

namespace Objects.Inanimate.Items.Inventory
{
    public class InventoryItemClassB
    {
        public Item item;
        public string name
        {
            get { return item.name; }
        }
        
        // Contructor for converting objects on the ground
        public InventoryItemClassB(Item item)
        {
            this.item = item;
        }
    }
}