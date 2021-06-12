using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;

public class FirebaseManerger : MonoBehaviour
{
    public int _delayTime = 3;
    public Text[] _text;
    DatabaseReference _reference;
    private void Awake()
    {
        // Get the root reference location of the database.
        _reference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    // Start is called before the first frame update
    void Start()
    {
        ListenDB();
        StartCoroutine(Unsubscribe_delay_Sec(_delayTime));
    }

    IEnumerator Unsubscribe_delay_Sec(int time)
    {
        yield return new WaitForSeconds(time);
        print("Unsubscribe");
        _reference.ValueChanged -= HandleValueChanged;
    }

    private void OnDisable()
    {
        print("Unsubscribe");
        _reference.ValueChanged -= HandleValueChanged;
    }

    void ListenDB()
    {
        _reference.ValueChanged += HandleValueChanged;
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        // Do something with the data in args.Snapshot

        DataSnapshot snapshot = args.Snapshot;

        string userName = snapshot.Child("name").Value.ToString();
        string userValue = snapshot.Child("value").Value.ToString();

        _text[0].text = userName;
        _text[1].text = userValue;
    }
}
