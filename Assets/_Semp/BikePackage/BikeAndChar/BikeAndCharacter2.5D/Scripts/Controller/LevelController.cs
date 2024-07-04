using Kamgam.BikeAndCharacter25D;
using System.Collections;
using System.Linq;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController Instance;

    public BikeAndCharacter bikeController;
    public BC_Camera cameraman;
    public LevelPrefab levelPrefab;
    public bool IsPlaying = false;

    private GameObject BikeObject;
    private PlayerModel _player;

    [HideInInspector] public Vector3 lastCheckpoint;

    private void Awake()
    {
        Instance = this;
        _player = GameManager.Instance._player;
    }

    private void Start()
    {
        //StartCoroutine(SpawnBike(level.StartPoint.position));
        Physics2D.simulationMode = SimulationMode2D.Script;
        //level.GenMesh();
        UIManager.Instance.Ingame.Show();

        OnlevelLoad();
        
    }

    protected IEnumerator SpawnBike(Vector3 _pos)
    {
        lastCheckpoint = _pos;

        // destroy old bike
        if (bikeController != null)
        {
            Destroy(BikeObject);
            BikeObject = null;
            bikeController = null;
            cameraman.SetObjectToTrack(null);
            
        }

        // create new
        //BikeObject = Instantiate(BikePrefab, transform);
        BikeObject = Instantiate(DataManager.Instance.bikes.listBike[_player.currentBike - 1].bikeSource, _pos, Quaternion.identity,transform);
        //BikeObject.transform.position = _pos;
        bikeController = BikeObject.GetComponent<BikeAndCharacter>();
        bikeController.PauseBike(true);
        bikeController.HandleUserInput = false;
        bikeController.Bike.IsBraking = true;
        
        // inform cameraman
        cameraman.SetTarget(bikeController.Character.TorsoBody);

        yield return new WaitForSeconds(0.5f);
        bikeController.HandleUserInput = true;
        bikeController.PauseBike(false);
        IsPlaying = true;
        UIManager.Instance.Ingame.OnStart();
    }

    public void OnlevelLoad()
    {
        foreach(Transform t in transform)
        {
            Destroy(t.gameObject);
        }
        GameObject level = Instantiate(DataManager.Instance.levels.listLevel.Where(x => x.Id == _player.currentLevel).FirstOrDefault().LevelSource, transform);
        levelPrefab = level.GetComponent<LevelPrefab>();
        StartCoroutine(SpawnBike(levelPrefab.startPoint.position));
        UIManager.Instance.Ingame.Show();
    }




    public void SavePoint(Transform savePoint)
    {
        lastCheckpoint = savePoint.position;
        lastCheckpoint += Vector3.up * 0.5f;
    }



    public void FixedUpdate()
    {
        //if (!paused)
        Physics2D.Simulate(Time.fixedDeltaTime);
    }
    public void OnLose()
    {
        Debug.Log("Thua oi`");
    }
    public void OnWIn()
    {
        if (!IsPlaying) return;
        IsPlaying = false;
        UIManager.Instance.Ingame.OnWin();
        bikeController.Bike.IsBraking = true;
        StartCoroutine(ShowComplete());
        
    }
    public void OnFail()
    {
        if (!IsPlaying) return;
        IsPlaying = false;
        bikeController.Bike.enabled = false;
        bikeController.Character.enabled = false;
        UIManager.Instance.Ingame.OnFail();
        StartCoroutine(ShowFail());
    }
    public IEnumerator ShowComplete()
    {
        yield return new WaitForSeconds(1.5f);
        UIManager.Instance.Complete.Show();
        UIManager.Instance.Ingame.Hide(false);
    }
    public IEnumerator ShowFail()
    {
        yield return new WaitForSeconds(1.5f);
        UIManager.Instance.Fail.Show();
        UIManager.Instance.Ingame.Hide(false);
    }

}
