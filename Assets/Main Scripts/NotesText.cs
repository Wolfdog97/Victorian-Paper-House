using UnityEngine;
using System.IO;
using TMPro;

public class NotesText : MonoBehaviour
{

	private TextMeshProUGUI tMPGUI;
	public TextAsset textFile;

	private void Awake()
	{
		tMPGUI = GetComponent<TextMeshProUGUI>();
	}

	// Use this for initialization
	void Start ()
	{
		tMPGUI.text = textFile.text;
	}
	
	// not used
	void readTextFile(string file_path)
	{
		StreamReader inp_stm = new StreamReader(file_path);

		while(!inp_stm.EndOfStream)
		{
			string inp_ln = inp_stm.ReadLine( );
			// Do Something with the input.
		}

		inp_stm.Close( );
	}
}
