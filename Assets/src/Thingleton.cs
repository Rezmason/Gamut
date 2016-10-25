using System;

public abstract class Thingleton<Clazz> where Clazz : Thingleton<Clazz>, new() {
	static Clazz _instance;
	public static Clazz instance
	{
		get { 
			if (_instance == null) {
				_instance = new Clazz();
			}
			return _instance; 
		}
	}

	public Thingleton() {

	}
}