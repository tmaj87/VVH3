using UnityEngine;

namespace Val_heim
{
    public class Loader
    {
        static GameObject gameObj;

        public static void Init()
        {
            gameObj = new GameObject();
            gameObj.AddComponent<Hck>();
            GameObject.DontDestroyOnLoad(gameObj);
        }

        public static void Unload()
        {
            GameObject.Destroy(gameObj);
        }
    }
}
