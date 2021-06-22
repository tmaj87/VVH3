using System;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

namespace Val_heim
{
    class TeleportManager
    {
        public static void Manage()
        {
            if (Utils.FromChat("/tp"))
            {
                TpToPinTp();
            }
            if (Utils.FromChat("/td"))
            {
                TeleportToDeadBody();
            }
        }

        private static void TpToPinTp()
        {
            List<Minimap.PinData> pins = (List<Minimap.PinData>)typeof(Minimap)
                .GetField("m_pins", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(Minimap.instance);
            foreach (Minimap.PinData pin in pins)
            {
                if (pin.m_name.Equals("tp", StringComparison.OrdinalIgnoreCase))
                {
                    Teleport(pin.m_pos);
                }
            }
        }

        private static void TeleportToDeadBody()
        {
            Minimap.PinData pin = (Minimap.PinData)typeof(Minimap)
                .GetField("m_deathPin", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(Minimap.instance);
            if (pin != null)
            {
                Teleport(pin.m_pos);
            }
        }

        private static void Teleport(Vector3 position)
        {
            Player.m_localPlayer.TeleportTo(position, Player.m_localPlayer.transform.rotation, true);
        }
    }
}
