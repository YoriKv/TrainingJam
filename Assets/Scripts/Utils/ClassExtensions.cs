using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;
using Random = UnityEngine.Random;

public static class ClassExtensions {
    public static Tweener DOFade(this tk2dSprite target, float endValue, float duration) {
        if(endValue < 0)
            endValue = 0;
        else if(endValue > 1)
            endValue = 1;
        return DOTween.ToAlpha(() => target.color, x => target.color = x, endValue, duration).SetTarget(target);
    }

    public static T Last1<T>(this IList<T> list) {
		return list[list.Count - 1];
	}

	public static T PickRandom<T>(this IList<T> list) {
		return list[Random.Range(0, list.Count)];
	}

	public static T PickRandom<T>(this IList<T> list, System.Random rnd) {
		return list[rnd.Next(0, list.Count)];
	}

	public static void Shuffle<T>(this IList<T> list) {
		int n = list.Count;
		while(n > 1) {
			n--;
			int k = Random.Range(0, n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static void Shuffle<T>(this IList<T> list, System.Random rnd) {
		int n = list.Count;
		while(n > 1) {
			n--;
			int k = rnd.Next(n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	public static void ShuffleRange<T>(this IList<T> list, int index, int length, System.Random rnd) {
		int n = length;
		while(n > 1) {
			n--;
			int k = rnd.Next(n + 1);
			T value = list[index + k];
			list[index + k] = list[index + n];
			list[index + n] = value;
		}
	}

	public static T ParseEnum<T>(string value) {
		return (T)Enum.Parse(typeof(T), value, true);
	}

	public static IEnumerable<T> GetEnumValues<T>() {
		return (T[])Enum.GetValues(typeof(T));
	}
}
