// Only works on ARGB32, RGB24 and Alpha8 textures that are marked readable

/*Description: The TextureScale script is a class that provides methods for scaling a texture using either point sampling or bilinear interpolation.

The main two methods that the user can interact with are the "Point" and "Bilinear" methods, which take a Texture2D, width and height as arguments, and returns a scaled Texture2D.

The Point method uses point sampling to scale the texture, this means it takes the color of the closest pixel to the corresponding point in the new image. This method is fast but can result in a noticeable loss of quality.

The Bilinear method uses bilinear interpolation to scale the texture. This method takes the color values of the four closest pixels to a point in the new image and interpolates them to get the final color value. This method is slower than point sampling but generally results in a higher-quality image.

The ThreadData class is used to store information about the start and end positions of a thread, it is used to divide the task of scaling the texture across multiple threads.

The PointScale and BilinearScale methods are both used to perform the scaling operation. The PointScale method uses point sampling while the BilinearScale method uses bilinear interpolation.

The ColorLerpUnclamped method is used to interpolate between two colors. It takes two color values and a float value between 0 and 1 as arguments, and returns a color that is interpolated between the two input colors based on the float value.

The script makes use of threading to perform the scaling operation in parallel across multiple threads, this can greatly improve performance for large texture. */

/* TODO: Mayba*
One way to separate the script into single responsibility scripts is to create separate classes for each distinct functionality.
For example, you could create a class called "TextureScaler" that contains the Point and Bilinear methods, and the ThreadData, texColors, newColors, w, ratioX, ratioY, w2, finishCount and mutex fields.
Then, you could create another class called "ThreadHelper" that contains the PointScale and BilinearScale methods, and the ThreadedScale method could be refactored to utilize this new class.
Additionally, you could extract the logic for setting the properties of the texture object into its own class.
*/

using System.Threading;
using UnityEngine;

public class TextureScale
{
    public class ThreadData
    {
        public int start;
        public int end;
        public ThreadData(int s, int e)
        {
            start = s;
            end = e;
        }
    }

    private static Color[] texColors;
    private static Color[] newColors;
    private static int w;
    private static float ratioX;
    private static float ratioY;
    private static int w2;
    private static int finishCount;
    private static Mutex mutex;

    public static void Point(Texture2D tex, int newWidth, int newHeight)
    {
        ThreadedScale(tex, newWidth, newHeight, false);
    }

    public static Texture2D Bilinear(Texture2D tex, int newWidth, int newHeight)
    {
        Debug.Log("attempting to scale");
        return ThreadedScale(tex, newWidth, newHeight, true);
    }

    private static Texture2D ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
    {
        texColors = tex.GetPixels();
        newColors = new Color[newWidth * newHeight];
        if (useBilinear)
        {
            ratioX = 1.0f / ((float)newWidth / (tex.width - 1));
            ratioY = 1.0f / ((float)newHeight / (tex.height - 1));
        }
        else
        {
            ratioX = ((float)tex.width) / newWidth;
            ratioY = ((float)tex.height) / newHeight;
        }
        w = tex.width;
        w2 = newWidth;
        var cores = Mathf.Min(SystemInfo.processorCount, newHeight);
        var slice = newHeight / cores;

        finishCount = 0;
        if (mutex == null)
        {
            mutex = new Mutex(false);
        }
        if (cores > 1)
        {
            int i = 0;
            ThreadData threadData;
            for (i = 0; i < cores - 1; i++)
            {
                threadData = new ThreadData(slice * i, slice * (i + 1));
                ParameterizedThreadStart ts = useBilinear ? new ParameterizedThreadStart(BilinearScale) : new ParameterizedThreadStart(PointScale);
                Thread thread = new Thread(ts);
                thread.Start(threadData);
            }
            threadData = new ThreadData(slice * i, newHeight);
            if (useBilinear)
            {
                BilinearScale(threadData);
            }
            else
            {
                PointScale(threadData);
            }
            while (finishCount < cores)
            {
                Thread.Sleep(1);
            }
        }
        else
        {
            ThreadData threadData = new ThreadData(0, newHeight);
            if (useBilinear)
            {
                BilinearScale(threadData);
            }
            else
            {
                PointScale(threadData);
            }
        }

        tex.Resize(newWidth, newHeight);
        tex.SetPixels(newColors);
        Debug.Log(tex);
        tex.Apply();

        texColors = null;
        newColors = null;
        return tex;
    }

    public static void BilinearScale(System.Object obj)
    {
        ThreadData threadData = (ThreadData)obj;
        for (var y = threadData.start; y < threadData.end; y++)
        {
            int yFloor = (int)Mathf.Floor(y * ratioY);
            var y1 = yFloor * w;
            var y2 = (yFloor + 1) * w;
            var yw = y * w2;

            for (var x = 0; x < w2; x++)
            {
                int xFloor = (int)Mathf.Floor(x * ratioX);
                var xLerp = x * ratioX - xFloor;
                newColors[yw + x] = ColorLerpUnclamped(ColorLerpUnclamped(texColors[y1 + xFloor], texColors[y1 + xFloor + 1], xLerp),
                                                       ColorLerpUnclamped(texColors[y2 + xFloor], texColors[y2 + xFloor + 1], xLerp),
                                                       y * ratioY - yFloor);
            }
        }

        mutex.WaitOne();
        finishCount++;
        mutex.ReleaseMutex();
    }

    public static void PointScale(System.Object obj)
    {
        ThreadData threadData = (ThreadData)obj;
        for (var y = threadData.start; y < threadData.end; y++)
        {
            var thisY = (int)(ratioY * y) * w;
            var yw = y * w2;
            for (var x = 0; x < w2; x++)
            {
                newColors[yw + x] = texColors[(int)(thisY + ratioX * x)];
            }
        }

        mutex.WaitOne();
        finishCount++;
        mutex.ReleaseMutex();
    }

    private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
    {
        return new Color(c1.r + (c2.r - c1.r) * value,
                          c1.g + (c2.g - c1.g) * value,
                          c1.b + (c2.b - c1.b) * value,
                          c1.a + (c2.a - c1.a) * value);
    }
}