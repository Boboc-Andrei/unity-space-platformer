using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class SceneHelper {
    public static void LoadScene(string s, bool additive = false, bool setActive = false) {
        if (s == null) {
            s = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            s, additive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : 0);

        if (setActive) {
            // to mark it active we have to wait a frame for it to load.
            // Get the CallAfterDelay code at https://gist.github.com/kurtdekker/0da9a9721c15bd3af1d2ced0a367e24e
            CallAfterDelay.Create(0, () => {
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(
                    UnityEngine.SceneManagement.SceneManager.GetSceneByName(s));
            });
        }
    }

    public static void UnloadScene(string s) {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(s);
    }
}
