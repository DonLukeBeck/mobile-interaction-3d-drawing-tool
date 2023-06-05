using UnityEngine;

public static class ExtensionMethods
{
    // https://dobrian.github.io/cmp/topics/filters/lowpassfilter.html
    // https://stackoverflow.com/questions/4272033/how-to-implement-a-lowpass-filter
    public static void LowPassFilter(ref this Vector3 currValue, Vector3 newValue, float alpha)
    {
        currValue.x = alpha * currValue.x + (1 - alpha) * newValue.x;
        currValue.y = alpha * currValue.y + (1 - alpha) * newValue.y;
        currValue.z = alpha * currValue.z + (1 - alpha) * newValue.z;
    }
    
    public static void LowPassFilter(ref this float currValue, float newValue, float alpha)
    {
        currValue = alpha * currValue + (1 - alpha) * newValue;
    }
}