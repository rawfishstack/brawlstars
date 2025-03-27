#nullable enable
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;

namespace brawlstars.Brawlstars.Utils;

public static class Utils {
    public static void Clamp(this ref float value, float min, float max) {
        if (value < min) value = min;
        if (value > max) value = max;
    }

    public static void Clamp(this ref int value, int min, int max) {
        if (value < min) value = min;
        if (value > max) value = max;
    }

    public static float ToClamp(this float value, float min, float max) {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }

    public static int ToClamp(this int value, int min, int max) {
        if (value < min) return min;
        if (value > max) return max;
        return value;
    }


    public static void FrontPlus(this ref Rectangle rectangle, int direction, Vector2 vector) {
        rectangle.X += direction * (int)vector.X;
        rectangle.Y += (int)vector.Y;
    }

    public static string ColorText(this string color, string text) {
        return $"[c/{color}:{text}]";
    }

    public static string ColorTextSubBefore(this string color) {
        return $"[c/{color}:";
    }

    public static string ColorTextSubAfter() {
        return "]";
    }

    // 真叽霸麻烦
    public static string ColorTextInsert(this string color, string text, string primitiveColor) {
        return ColorTextSubAfter() + color.ColorText(text) + primitiveColor.ColorTextSubBefore();
    }

    public static TooltipLine Tooltip(string name, string text) {
        return new TooltipLine(BrawlStars.Instance, name, text);
    }

    public static void Tooltip(this List<TooltipLine> tooltips, string name, object? obj) {
        tooltips.Add(Tooltip(name, obj?.ToString() ?? string.Empty));
    }

    public static Vector2 TextureCenter(this Texture2D texture) {
        return new Vector2(texture.Width / 2f, texture.Height / 2f);
    }

    public static Vector2 PlusX(this Vector2 vector, float x) {
        vector.X += x;
        return vector;
    }

    public static Vector2 PlusY(this Vector2 vector, float y) {
        vector.Y += y;
        return vector;
    }


    public static T Apply<T>(this T value, Action<T> func) {
        func(value);
        return value;
    }


    public static T[] Array<T>(this int size, Func<int, T> func) {
        var array = new T[size];
        for (var i = 0; i < size; i++) {
            array[i] = func(i);
        }

        return array;
    }

    public static List<T> List<T>(this int size, Func<int, T> func) {
        var list = new List<T>(size);
        for (var i = 0; i < size; i++) {
            list.Add(func(i));
        }

        return list;
    }

    public static void Repeat(this int times, Action<int> action) {
        for (var i = 0; i < times; i++) {
            action(i);
        }
    }

    public static void Repeat(this int times, Func<int, bool> action) {
        for (var i = 0; i < times; i++) {
            if (action(i)) {
                return;
            }
        }
    }

    public static void ForEach<T>(this T[] array, Action<T> action) {
        foreach (var e in array) {
            action(e);
        }
    }

    public static void ForEachIndexed<T>(this T[] array, Action<int, T> action) {
        for (var i = 0; i < array.Length; i++) {
            action(i, array[i]);
        }
    }

    public static TR[] Map<T, TR>(this T[] array, Func<T, TR> func) {
        var r = new TR[array.Length];
        for (var i = 0; i < array.Length; i++) {
            r[i] = func(array[i]);
        }

        return r;
    }

    public static TR[] MapIndexed<T, TR>(this T[] array, Func<int, T, TR> func) {
        var r = new TR[array.Length];
        for (var i = 0; i < array.Length; i++) {
            r[i] = func(i, array[i]);
        }

        return r;
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
        foreach (var x1 in source) {
            action(x1);
        }
    }

    public static int ToOneBased(this int value) {
        return value + 1;
    }

    public static int ToZeroBased(this int value) {
        return value - 1;
    }
}