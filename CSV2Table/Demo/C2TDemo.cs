using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class C2TDemo : MonoBehaviour 
{
	public TextAsset csv;
	public InputField input;
	public Text output;

	public void Start()
	{
		input.text = "";
		output.text = "";

		if(csv != null)
		{
			input.text = csv.text;
		}
	}

	public void Generate()
	{
		if(string.IsNullOrEmpty(input.text))
			return;

		string code = TableCodeGen.Generate(input.text, "SampleTable");
		output.text = code;
	}
}
