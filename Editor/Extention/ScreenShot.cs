using UnityEngine;
using UnityEditor;

namespace MyFw
{
    public class ScreenshotMenu
    {
        [MenuItem("Tools/Screenshot PNG保存 #%F11")]
        public static void ScreenshotAsPNG()
        {
            var time = System.DateTime.Now;
            var directoryName = "Screenshot";
            var filePath = $"Screenshot/{time.Year:0000}_{time.Month:00}{time.Day:00}_{time.Hour:00}{time.Minute:00}{time.Second:00}.png";
            if (!System.IO.Directory.Exists(directoryName))
            {
                System.IO.Directory.CreateDirectory(directoryName);
            }

            Debug.Log("Run ScreenShot : " + filePath);
            ScreenCapture.CaptureScreenshot(filePath);
        }
    }
}
