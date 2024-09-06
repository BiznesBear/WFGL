﻿namespace WFGL;


public sealed class Layer(byte drawWeight)
{
    public readonly static Layer Defalut = new(0);
    internal static List<Layer> List { get; private set; } = [Defalut];
    public byte DrawWeight { get; } = drawWeight;

    public static List<Layer> Registration
    {
        set
        {
            List = [Defalut];
            List.AddRange(value);
            UpdateList();
        }
    }

    public void Register(Layer layer)
    {
        List.Add(layer);
        UpdateList();
    }


    /// <summary>
    /// Sorts layers list by draw order
    /// </summary> 
    public static void UpdateList()
    {
        int n = List.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (List[j].DrawWeight > List[j + 1].DrawWeight)
                {
                    var temp = List[j];
                    List[j] = List[j + 1];
                    List[j + 1] = temp;
                }
            }
        }
    }
}