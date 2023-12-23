using UnityEngine;
using MAKStateMachine;

public class GameStateData : IBaseStateData {
    public UnityStateMachineRunner<E_GameStates, GameStateData> stateMachine;
    public GameObject player;
    public GameObject enemy;
    public GameStateManager gameStateManager;

    /// <summary>
    /// send the player, enemy game objects respectively
    /// </summary>
    /// <param name="go1"></param>
    /// <param name="go2"></param>
    public GameStateData(GameObject go1, GameObject go2, UnityStateMachineRunner<E_GameStates, GameStateData> stateMachineRunner,
    GameStateManager _gameStateManager) {
        player = go1;
        enemy = go2;
        stateMachine = stateMachineRunner;
        gameStateManager = _gameStateManager;
    }
}