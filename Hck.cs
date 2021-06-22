using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Val_heim
{
    public class Hck : MonoBehaviour
    {
        private static Regex PLAYERS_METHOD = new Regex("^\\/p\\((\\d{1,3})\\)$", RegexOptions.IgnoreCase);
        private static Regex PROJECTILE_BURST_METHOD = new Regex("^\\/pb\\(([1-9][0-9]?[0-9]?)\\)$", RegexOptions.IgnoreCase);

        public void Start()
        {
            Utils.SILENT_MODE = true;
            StartCoroutine(Waiter());
        }

        public void Update()
        {
            // empty
        }

        IEnumerator Waiter()
        {
            yield return new WaitForSeconds(0.2f);
            foreach (Player player in Player.GetAllPlayers())
            {
                if (IsLocal(player))
                {
                    UpgradePlayer(player);
                    HandleCommands(player);
                }
            }
            StartCoroutine(Waiter());
        }

        private bool IsLocal(Player player)
        {
            return player == Player.m_localPlayer;
        }

        private void UpgradePlayer(Player player)
        {
            player.m_baseCameraShake = 0f;
            player.m_tolerateFire = true;
            player.m_tolerateSmoke = true;
            player.m_tolerateWater = true;
            player.m_maxCarryWeight = 600f;
            player.AddStamina(player.GetMaxStamina());
            player.Heal(player.GetMaxHealth(), false);
            UpgradeBow(player);
            UpgradeFood(player);
        }

        private void UpgradeBow(Player player)
        {
            ItemDrop.ItemData weapon = player.GetCurrentWeapon();
            if (weapon != null)
            {
                ItemDrop.ItemData.SharedData shared = weapon.m_shared;
                if (shared.m_itemType == ItemDrop.ItemData.ItemType.Bow)
                {
                    shared.m_holdDurationMin = 0.01f;
                    shared.m_dodgeable = false;
                    shared.m_blockable = false;
                }
            }
        }

        private void UpgradeFood(Player player)
        {
            List<Player.Food> foods = (List<Player.Food>)typeof(Player)
                .GetField("m_foods", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(Player.m_localPlayer);
            foreach (Player.Food food in foods)
            {
                food.m_health = 128f;
                food.m_stamina = 128f;
            }
        }

        private void HandleCommands(Player player)
        {
            IsDifficultyChange();
            SpeedManager.Manage(player);
            InventoryManager.Manage(player);
            TeleportManager.Manage();
            IsProjectileBurstChange(player);
            if (Utils.FromChat("/kill"))
            {
                KillAllMonsters();
            }
        }

        private void IsDifficultyChange()
        {
            string result = Utils.FromChatRegex(PLAYERS_METHOD);
            if (result.Length > 0)
            {
                int difficulty = Int32.Parse(result);
                Game.instance.SetForcePlayerDifficulty(difficulty);
                Utils.ToChat("Difficulty set to: " + difficulty);
            }
        }

        private void IsProjectileBurstChange(Player player)
        {
            string result = Utils.FromChatRegex(PROJECTILE_BURST_METHOD);
            if (result.Length > 0)
            {
                int burst = Int32.Parse(result);
                ItemDrop.ItemData weapon = player.GetCurrentWeapon();
                if (weapon != null)
                {
                    if (weapon.m_shared.m_itemType == ItemDrop.ItemData.ItemType.Bow)
                    {
                        weapon.m_shared.m_attack.m_projectileBursts = burst;
                    }
                }
                Utils.ToChat("Projectile burst set to: " + burst);
            }
        }

        private void KillAllMonsters()
        {
            foreach (Character character in Character.GetAllCharacters())
            {
                if (character.IsMonsterFaction())
                {
                    character.Damage(new HitData() { m_damage = { m_damage = character.m_health * 4f } });
                }
            }
            Utils.ToChat("All monsters killed");
        }
    }
}
