using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
	#region My variables
	private Material _deadSkin;                                                                 //Material que representa a célula morta
	private	Material _liveSkin;                                                                 //Material que representa a célula viva
	private bool _alive;                                                                        // bolena que determina se a célula ta viva ou morta 
	private bool _willBeAlive;                                                                  // boleana que determina o proximo estado da célula (vivo ou morto)
	private Cell[] _neighbours = new Cell[8];                                                   // Array de células que é usado para armazenar seus vizinhos 
	private  int _myNeighboursAlive;                                                            // Valor que diz quantos vizinhos vivo tem a célula
    #endregion

	public	void Inicialization(){                                                              // Carregando e setando materiais e estatos da célula 
		LoadMaterials ();                                                                       // Metódo que carrega os materias responsaveis por dizer se a célula tá viva ou morta 
		RandomState ();                                                                         // Metódo que gera um estádo randomico para a célula (vivo ou morta)

		                                                                                        // Esse if é responsável por inserir o material da célula de acordo com seu estado
		if (!_alive) {   
		
			GetComponent<MeshRenderer> ().material = _deadSkin;
		} else {
		
			GetComponent<MeshRenderer> ().material = _liveSkin;
		}
	
	}

	void RandomState(){                                                                        // Metódo que gera um estádo randomico para a célula (vivo ou morta)
		int random = (int)Random.Range (0, 5);                                                 // Gerando um número randomico de 0 a 4 e obrigando que me retorne um inteiro (estou usando 0,4 porque o método random range exclui o valor máximo)e (50% gera muita célula viva, deixando a primeira geração bem estranha).
		if (random <= 3) {
			_alive = false;
		}else
			_alive = true;

	}
	void LoadMaterials(){                                                                      // Metódo que carrega os materias responsaveis por dizer se a célula tá viva ou morta 
		_liveSkin = Resources.Load ("Mateials/Live") as Material;
		_deadSkin = Resources.Load ("Mateials/Dead") as Material;
	
	}
	public void SetmyNeighbours(Cell[] myNeighbours){                                          // Metódo que carrega as células vizinhas
		this._neighbours = myNeighbours;
	
	}
    public void OnMouseDown() {                                                               // Caso o usuário queira testar alguma coisa, é possível mudar o estádo da célula em tempo de execução

        ChangeState();
    }
	public void ChangeState() {                                                               // Método que muda a célula de estado (de vivo para morto ou o contrário)
        if (!_alive)
        {
            _alive = true;
            GetComponent<MeshRenderer>().material = _liveSkin;
        }
        else {
            _alive = false;
            GetComponent<MeshRenderer>().material = _deadSkin;
        }

    }

	public void CheckNeighbours() {                                                          // Método que checka a quantidade de vizinhos vivos da célulta
        int myNeighboursAlive = 0;
		for (int i = 0; i < _neighbours.Length; i++) {                                       // Aqui que vejo quantos vizinhos a célula tem 
            if (_neighbours[i]._alive) {
                myNeighboursAlive++;
            }


        }
	
		UpdateCell (myNeighboursAlive);                                                      // Método que aplica as leis do jogo 
       
    }
	public void NewGeneration(){
		_alive = _willBeAlive;
		UpdateSkin ();

	}
	void UpdateSkin(){
		if (_alive) {
			GetComponent<MeshRenderer> ().material = _liveSkin;

		} else {

			GetComponent<MeshRenderer>().material = _deadSkin;
		}

	}
	void UpdateCell(int myNeighboursAlive){                                                 // Método que aplica as leis do jogo
		
		if (_alive) {                                                                       // Esse if é responsável por matar a célular ou mante-la viva
			if (myNeighboursAlive != 2 && myNeighboursAlive != 3) { 
				
				_willBeAlive = false;
			} else {
			
				_willBeAlive = true;
			}
		
		} else {   
		
			if (myNeighboursAlive==3) {                                                     // Aqui é responsável pelo nascimento da célula
			

				_willBeAlive=true;
			
			}
		
		}
	
	}
		
	



}
