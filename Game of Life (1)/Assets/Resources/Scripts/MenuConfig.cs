using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MenuConfig : MonoBehaviour {
	#region UI Variables
	Slider slider;    // criando uma referência de slider na memória
	Toggle toggle;    //  criando uma referência  de toggle na memória
	InputField coluns; // criando uma referência de InputField na memória
	InputField raws; // criando uma referência  de InputField na memória
	Text updateSpdText; // criando uma referência do slider de Texto na memória 
	#endregion
	#region my Variables
	public  float _updateSpd; // Variavél responsável por armazenar a velocidade do Update
	public bool _autoUpdate; // Variavél responsável por armazenar se o Update vai ser automático
	public int colunsNumber;  // Variavél responsável por armazenar o número de Colunas
	public int rawsNumber;  //  Variavél responsável por armazenar o número de Linhas
	#endregion



	void Awake(){
		#region LoadingStaff
		// Carregando a referência dos objetos 
		slider = GameObject.Find ("Slider").GetComponent < Slider> ();    
		toggle = GameObject.Find ("Toggle").GetComponent<Toggle> ();         
		raws = GameObject.Find ("Raws").GetComponent<InputField> ();         
		coluns = GameObject.Find ("Coluns").GetComponent<InputField> ();     
		updateSpdText = GameObject.Find ("SpdText").GetComponent<Text> ();      
		#endregion
	}
	void Update(){
		// Atualizando os valores das variaveis
		_autoUpdate = toggle.isOn;         
		_updateSpd = slider.value;
		updateSpdText.text = _updateSpd.ToString ();
		if (coluns.text != "") {            // Esse if existe para evitar o erro que daria caso o computado tentasse passar uma string que não contém nada para inteiro
			colunsNumber = int.Parse (coluns.text);  // Passando o conteúdo do InputField para a variavel que salva o número de colunas 
		}
		if (raws.text != "") {             // Esse if existe para evitar o erro que daria caso o computado tentasse passar uma string que não contém nada para inteiro
			rawsNumber = int.Parse (raws.text);	 // Passando o conteúdo do InputField para a variavel que salva o número de linhas 
		}


	}
	public void GoToScene(string sceneName){    // Metódo responsável por ir para outras cenas
		DontDestroyOnLoad (this.gameObject);    // Evitando que a unity destrua esse objeto quando carregar a proxima cena 
		if (colunsNumber == 0) {         // Caso o usuário esqueça de inserir valores, esse é o valor defalt
			colunsNumber = 70;
		}
		if (rawsNumber == 0) {             // Caso o usuário esqueça de inserir valores, esse é o valor defalt
			rawsNumber = 70;
		}
		SceneManager.LoadScene (sceneName);  // Carregando a cena

	}
}
