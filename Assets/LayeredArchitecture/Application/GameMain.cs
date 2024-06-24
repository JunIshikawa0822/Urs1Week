using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    [SerializeField]
    GameStatus gameStat;

    List<SystemBase> allSystems;

    List<IOnUpdate> allUpdateSystems;
    List<IOnPreUpdate> allPreUpdateSystems;
    List<IOnLateUpdate> allLateUpdateSystems;
    List<IOnFixedUpdate> allFixedUpdateSystems;

    private void Awake()
    {
        allSystems = new List<SystemBase>()
        {
            new PlayerSystem()
            ,new PhotonSystem()
            ,new CameraSystem()
        };

        allUpdateSystems = new List<IOnUpdate>();
        allPreUpdateSystems = new List<IOnPreUpdate>();
        allLateUpdateSystems = new List<IOnLateUpdate>();
        allFixedUpdateSystems = new List<IOnFixedUpdate>();

        foreach(SystemBase system in allSystems)
        {
            if (system is IOnPreUpdate) allPreUpdateSystems.Add(system as IOnPreUpdate);
            if (system is IOnUpdate) allUpdateSystems.Add(system as IOnUpdate);
            if (system is IOnLateUpdate) allLateUpdateSystems.Add(system as IOnLateUpdate);
            if (system is IOnFixedUpdate) allFixedUpdateSystems.Add(system as IOnFixedUpdate);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (SystemBase system in allSystems) system.SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (IOnPreUpdate system in allPreUpdateSystems) system.OnPreUpdate();
        foreach (IOnUpdate system in allUpdateSystems) system.OnUpdate();
    }

    private void LateUpdate()
    {
        foreach (IOnLateUpdate system in allLateUpdateSystems) system.OnLateUpdate();
    }

    private void FixedUpdate()
    {
        foreach (IOnFixedUpdate system in allFixedUpdateSystems) system.OnFixedUpdate();
    }
}
