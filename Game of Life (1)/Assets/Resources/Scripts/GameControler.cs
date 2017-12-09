using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameControler : MonoBehaviour {
	#region My variables
	private Cell _cellPrefab;                                                                           //Prefab da célula 
	private GameObject _boxPrefab;                                                                      // Prefab de um cubo que será usado de corpo para a célula
	private GameObject _board;                                                                          // Objeto que será pai de todas as células (para ficar mais organizado)
	private double _positionX = -10.8;                                                                  // valor que será usado para armazenar a posição X das celulas a menida que for Instanciando elas (Não é 0 porque eu estou criando do canto superior da tela até o final)
	private double _positionY =5.5;                                                                     // valor que será usado para armazenar a posição Y das celulas a menida que for Instanciando elas (Não é 0 porque eu estou criando do canto superior da tela até o final)
	private bool _isAutomatic;                                                                          // Valor que determina se a atualização das células será automático
	private int raws=70;                                                                                // Número de Linhas 
	private int coluns = 70;                                                                            // Número de Colunhas
	private Cell[,] _cells;                                                                             // Array bidimensional de células 
	private float _timeToUpdateGeneration =1f;                                                          // Tempo de Update (Tempo que leva para passar para a proxima geração)
	private MenuConfig menuConfig;                                                                      // Pegando a referência do menu, para carregar todas as informações que o usuário escolheu
	private float time;                                                                                 // Cronometro para saber o tempo atual 


	private Action cellUpdates;                                                                         //Ação que carregará todos os métodos de atualizar de cada célula
	private Action cellApplyUpdates;                                                                    // action which calls cells' apply update methods
	#endregion

	private void Awake(){
		#region Loading Variables of MenuConfig
		menuConfig = GameObject.FindObjectOfType<MenuConfig> ();                                        // Achando a referência do menu
		raws = menuConfig.rawsNumber;                                                                   // Pegando o número de linhas que usuário escolheu 
		coluns = menuConfig.colunsNumber;                                                               // Pegando o número de colunas que usuário escolheu 
		_isAutomatic = menuConfig._autoUpdate;                                                          // Vendo se o usuário escolheu se queria Atualização automática ou não
		_timeToUpdateGeneration = menuConfig._updateSpd;                                                // Pegando o tempo de atualização que usuário escolheu 
		Destroy (menuConfig.gameObject);                                                                // Destruindo porque ele não vai ter mais utilidade na cena
		#endregion
		cellUpdates = null;
		cellApplyUpdates = null;
		#region Loading Staff
		_boxPrefab =  Resources.Load ("Prefab/Box") as GameObject;   // Carregando prefab de uma quadrado nas pastas
		_board = GameObject.Find ("Board");           // Pegando referência da mesa na hierarquia 
		if(!_boxPrefab.GetComponent<Cell>()){                    // Evitando bug de ter vários scipts de célula em um único objeto
			_boxPrefab.AddComponent(typeof(Cell));                       // Criando um componente de célula dentro da caixa
		}
		_cellPrefab = _boxPrefab.GetComponent<Cell>();        // Carregando prefab da célula final
		#endregion
		SpawnCell(raws,coluns);                                                                         // Chamando o método para criar o "tabuleiro"
	}
	void SpawnCell(int raws,int coluns){                                                                // Metódo responsavel por criar o tabuleiro 
		_cells = new Cell[raws, coluns];                                                                // Criando array bidimensional de células com os valores de linhas e colunas passados pelo usuário 
		for (int i = 0; i < raws; i++) {                                                                
			for (int j = 0; j < coluns; j++) {                                                         
				Cell cellTemp = Instantiate (_cellPrefab, new Vector3 ((float) _positionX,(float)_positionY, 0), Quaternion.identity) as Cell;                                      //Criando as células na cena
				cellTemp.transform.SetParent(_board.transform);                                         // Fazendo a célula criada ser filha da mesa 
				_cells [i, j] = cellTemp;                                                               // Adicionando a célula criada para o array bidimensional de células 
				cellTemp.transform.name = "Cell" + " " + i + " " + j;                                   // Mudando nome do objeto para célula + sua posição (linha e coluna) (Para ficar mais organizado e fácil de ver se acontece algum bug)
				cellTemp.Inicialization();                                                              // Mandando a célula carregar algumas variaveis 
				_positionY--;                                                                           // Para fazer com que a proxima célula a ser criada nasca em baixo da atual
				cellUpdates += cellTemp.CheckNeighbours;
				cellApplyUpdates += cellTemp.NewGeneration;


			}
			_positionX++;                                                                               // Para criar a coluna do lado direito da anterior 
			_positionY = 5.5f;                                                                          // Resetando valor de Y para criar uma tabela 

		}

		                                                                                                // Criando loop responsável por determinar todos os vizinhos de cada célula 
		for (int i = 0; i < raws; i++) {  
			for (int j = 0; j < coluns; j++) {
				_cells[i,j].SetmyNeighbours (GettingNeighbours(i,j));
			}
		}
	
	}
	                                                                                                   
	private Cell [] GettingNeighbours (int raw, int colun) {                                            // Metodo que retorna todos os vizinhos de cada célula do tabuleiro
        Cell[]neighbours = new Cell [8];
		neighbours[0] = _cells[raw, (colun + 1) % coluns];                                              // baixo 
		neighbours[1] = _cells[(raw + 1) % raws, (colun + 1) % coluns];                                 // diagonal baixo direita
		neighbours[2] = _cells[(raw + 1) % raws, colun % coluns];                                       // direita 
		neighbours[3] = _cells[(raw + 1) % raws, (coluns + colun - 1) % coluns];                        //diagonal cima direita 
		neighbours[4] = _cells[raw % raws, (coluns + colun - 1) % coluns];                              //cima
		neighbours[5] = _cells[(raws + raw - 1) % raws, (coluns + colun - 1) % coluns];                 // diagonal cima esquerda
		neighbours[6] = _cells[(raws + raw - 1) % raws, colun % coluns];                                // esquerda
		neighbours[7] = _cells[(raws + raw - 1) % raws, (colun + 1) % coluns];                          // diagonal baixo esquerda
		
		return neighbours;
	
	}

	void Update(){                                                                                      // Usando a thread de Update da unity (Defalt)
		if (_isAutomatic) {                                                                             // Caso o usuário tenho escolhido que a atualização seja automática
			time += Time.deltaTime;
			if (time >= _timeToUpdateGeneration) {
				cellUpdates ();
				cellApplyUpdates ();
				time = 0;
			}		
		}else
			if (Input.GetKeyDown (KeyCode.Space)) {                                                     // Caso ele não tenha escolhido a atualização automática, ele pode atuliazação manualmente as gerações apertando barra de espaço.
				cellUpdates ();
				cellApplyUpdates ();

			}
	}
}
