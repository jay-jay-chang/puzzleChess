using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Skill {
	public Dictionary<string, object> Params = new Dictionary<string, object>();
	public System.Action< Dictionary<string, object> > skillDel;

	public void Trigger( Dictionary<string,object> Params)
	{
		if(skillDel != null)
		{
			foreach( KeyValuePair<string, object> pair in this.Params)
				Params.Add(pair.Key, pair.Value);
			skillDel( Params );
		}
	}
}
