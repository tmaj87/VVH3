namespace Val_heim
{
    class SpeedManager
    {
        private static bool once = false;
        private static float initialSpeed, initialRunSpeed, initialWalkSpeed, initialJumpForce, initialJumpForceForward, initialSwimSpeed;

        public static void Manage(Player player)
        {
            OnlyOnce(player);
            if (Utils.FromChat("/F5"))
            {
                Rollback(player);
            }
            if (Utils.FromChat("/+"))
            {
                SpeedUp(player);
            }
        }

        private static void OnlyOnce(Player player)
        {
            if (!once)
            {
                initialSpeed = player.m_speed;
                initialRunSpeed = player.m_runSpeed;
                initialWalkSpeed = player.m_walkSpeed;
                initialJumpForce = player.m_jumpForce;
                initialJumpForceForward = player.m_jumpForceForward;
                initialSwimSpeed = player.m_swimSpeed;
                once = true;
                Utils.ToChat("Values saved");
            }
        }

        private static void Rollback(Player player)
        {
            if (once)
            {
                player.m_speed = initialSpeed;
                player.m_runSpeed = initialRunSpeed;
                player.m_walkSpeed = initialWalkSpeed;
                player.m_jumpForce = initialJumpForce;
                player.m_jumpForceForward = initialJumpForceForward;
                player.m_swimSpeed = initialSwimSpeed;
                once = false;
                Utils.ToChat("Values rolled back");
            }
        }

        private static void SpeedUp(Player player)
        {
            player.m_speed *= 1.1f;
            player.m_runSpeed *= 1.1f;
            player.m_walkSpeed *= 1.1f;
            player.m_jumpForce *= 1.1f;
            player.m_jumpForceForward *= 1.1f;
            player.m_swimSpeed *= 1.1f;
            Utils.ToChat("Speed Up");
        }
    }
}
