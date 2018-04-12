using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerCustom : NetworkManager
{
	// in the Network Manager component, you must put your player prefabs 
	// in the Spawn Info -> Registered Spawnable Prefabs section 
	public short playerPrefabIndex = 1;

	void Start() {
		if (GameManager.instance.gameMode == "single") {
			gameObject.SetActive (false);
		}
	}

	public override void OnStartServer()
	{
		playerPrefabIndex = 0;
		NetworkServer.RegisterHandler(MsgTypes.PlayerPrefab, OnResponsePrefab);
		base.OnStartServer();
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		client.RegisterHandler(MsgTypes.PlayerPrefab, OnRequestPrefab);
		base.OnClientConnect(conn);
	}

	private void OnRequestPrefab(NetworkMessage netMsg)
	{
		MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
		msg.controllerID = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>().controllerID;
		msg.prefabIndex = playerPrefabIndex;
		client.Send(MsgTypes.PlayerPrefab, msg);
	}

	private void OnResponsePrefab(NetworkMessage netMsg)
	{
		MsgTypes.PlayerPrefabMsg msg = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>();  
		playerPrefab = spawnPrefabs[msg.prefabIndex];
		base.OnServerAddPlayer(netMsg.conn, msg.controllerID);
		Debug.Log ("prefabIndex: " + playerPrefabIndex.ToString ());
		Debug.Log(playerPrefab.name + " spawned!");
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
		msg.controllerID = playerControllerId;
		NetworkServer.SendToClient(conn.connectionId, MsgTypes.PlayerPrefab, msg);
	}

	// I have put a toggle UI on gameObjects called PC1 and PC2 to select two different character types.
	// on toggle, this function is called, which updates the playerPrefabIndex
	// The index will be the number from the registered spawnable prefabs that 
	// you want for your player
	public void UpdatePC ()
	{
		if (GameObject.Find("PC1").GetComponent<Toggle>().isOn)
		{
			playerPrefabIndex = 3;
		}
		else if (GameObject.Find("PC2").GetComponent<Toggle>().isOn)
		{
			playerPrefabIndex= 4;
		}
	}


}

public class MsgTypes
{
	public const short PlayerPrefab = MsgType.Highest + 1;

	public class PlayerPrefabMsg : MessageBase
	{
		public short controllerID;    
		public short prefabIndex;
	}
}
