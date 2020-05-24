using System;
using System.Collections.Generic;
using UnityEngine;

namespace Config.Maps
{
    [Serializable]
    public class MapConfigs
    {
        public ItemMapConfig[] maps;
        public override string ToString()
        {
            foreach (var item in maps)
            {
                item.ToString();
            }

            return null;
        }

        public int GetKindOfBlock(int level, int i, int j)
        {
            ItemMapConfig itemMapConfig = maps[level - 1];
            if (itemMapConfig != null)
            {
                ItemSetMapConfig itemSetMapConfig = itemMapConfig.config[itemMapConfig.config.Length - j - 1];
                if (itemSetMapConfig != null)
                {
                    return itemSetMapConfig.set[i];
                }
            }

            return 0;
        }
    }
}