using System;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Val_heim
{
    class InventoryManager
    {
        private static Regex INVENTORY_METHOD = new Regex("^\\/i\\(([4-9]|[1-9][0-9])\\)$", RegexOptions.IgnoreCase);

        public static void Manage(Player player)
        {
            IsInventoryChange(player);
            if (Utils.FromChat("/fix"))
            {
                FixAllInInventory(player);
            }
            if (Utils.FromChat("/ammo"))
            {
                StackAmmo(player);
            }
            if (Utils.FromChat("/stack"))
            {
                StackResources(player);
            }
        }

        private static void IsInventoryChange(Player player)
        {
            string result = Utils.FromChatRegex(INVENTORY_METHOD);
            if (result.Length > 0)
            {
                int height = Int32.Parse(result);
                typeof(Inventory)
                    .GetField("m_height", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(player.GetInventory(), height);
                Utils.ToChat("Inventory height set to: " + height);
            }
        }

        private static void StackResources(Player player)
        {
            Inventory inventory = player.GetInventory();
            foreach (ItemDrop.ItemData item in inventory.GetAllItems())
            {
                if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Material
                    || item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Consumable)
                {
                    item.m_stack = item.m_shared.m_maxStackSize;
                }
            }
            Utils.ToChat("Materials stacked");
        }

        private static void StackAmmo(Player player)
        {
            Inventory inventory = player.GetInventory();
            foreach (ItemDrop.ItemData item in inventory.GetAllItems())
            {
                if (item.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Ammo)
                {
                    item.m_stack = item.m_shared.m_maxStackSize;
                }
            }
            Utils.ToChat("Ammo stacked");
        }

        private static void FixAllInInventory(Player player)
        {
            Inventory inventory = player.GetInventory();
            foreach (ItemDrop.ItemData item in inventory.GetAllItems())
            {
                item.m_durability = item.GetMaxDurability();
            }
            Utils.ToChat("Fixed items");
        }
    }
}
