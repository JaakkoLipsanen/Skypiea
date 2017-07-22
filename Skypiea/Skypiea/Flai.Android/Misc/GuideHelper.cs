using System;

namespace Flai.Misc
{
    public static class GuideHelper
    {
        public static void ShowKeyboardInput(string title, string description, Action<string> resultAction)
        {
            throw new NotImplementedException("");
          /*  if (Guide.IsVisible)
            {
                return;
            }

            Guide.BeginShowKeyboardInput(PlayerIndex.One, title, description, "", result =>
            {
                string input = Guide.EndShowKeyboardInput(result);
                if (resultAction != null)
                {
                    resultAction(input);
                }
            }, null); */
        }

        public static void ShowMessageBox(string title, string description, string[] buttons, Action<int?> callback)
        {
            throw new NotImplementedException("");
            /*
            if (Guide.IsVisible)
            {
                return;
            }

            Guide.BeginShowMessageBox(title, description, buttons, 0, MessageBoxIcon.None, cb =>
            {
                int? result = Guide.EndShowMessageBox(cb);
                if (callback != null)
                {
                    callback(result);
                }
            }, null);
            */
        }
    }
}
