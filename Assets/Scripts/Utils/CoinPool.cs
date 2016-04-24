using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CoinPool:MonoBehaviour {
	// Singleton
	public static CoinPool I;

	// Prefab
	public Coin coinPrefab;

	// Pool
	public int poolSize;
	private List<Coin> _pool;
	private int _current;
	private int _nextCounter;

	void Awake() {
		I = this;
		// Seed the _pool
		_current = 0;
		_pool = new List<Coin>(poolSize);

	    for(int i = 0; i < poolSize; i++) {
			Coin c = (Coin)Instantiate(coinPrefab, Vector3.zero, Quaternion.identity);
			c.Finish();
			c.transform.parent = transform;
			c.name = coinPrefab.name + "_" + i;
			_pool.Add(c);
		}
	}

	private void Next() {
		_current++;
		_current = _current % poolSize;
	}

	public Coin GetNext() {
		Coin c = _pool[_current];
		Next();
		// Keep looping through effects until we find one that isn't in use
		_nextCounter = 0;
		while(c.gameObject.activeSelf && _nextCounter < poolSize) {
			c = _pool[_current];
			Next();
			_nextCounter++;
		}
		// In case we're still active for some reason just use it , finish up
		if(c.gameObject.activeSelf) {
			c.Finish();
		}
		return c;
	}

	public Coin PlayNext(Vector3 position) {
		Coin c = GetNext();
		c.Play(position);
		return c;
	}
}
