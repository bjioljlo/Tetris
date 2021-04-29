using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public abstract class UnityAds : MonoBehaviour ,IUnityAdsListener{
    public string UnityPlacementID;
    public string UnityGameID;

    public void loadUnityADS(bool _istest)
    {
        Advertisement.AddListener (this);
        Advertisement.Initialize(UnityGameID,_istest);
        
    }
	public void ShowUnityADS()
    {
        if (Advertisement.IsReady(UnityPlacementID)){
            Advertisement.Show(UnityPlacementID);
        }
        else {
            Debug.Log(UnityPlacementID + " is not ready at the moment! Please try again later!");
        }
    }

	public void OnUnityAdsDidFinish(string surfacingId,ShowResult result)
    {
        if(surfacingId != UnityPlacementID) return;
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The "+ UnityPlacementID + " ad was successfully shown.");
				resultFinished();
                break;
            case ShowResult.Skipped:
                Debug.Log("The "+ UnityPlacementID + " ad was skipped before reaching the end.");
                resultSkipped();
                break;
            case ShowResult.Failed:
                Debug.LogError("The "+ UnityPlacementID + " ad failed to be shown.");
                resultFailed();
                break;
        }
    }

    public abstract void resultFinished();
    public abstract void resultSkipped();
    public abstract void resultFailed();

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsReady (string surfacingId) {
        // If the ready Ad Unit or legacy Placement is rewarded, activate the button: 
        if(surfacingId == UnityPlacementID){
            Debug.Log(surfacingId + " is ready to show");
        }
        
    }
    public void OnUnityAdsDidError (string message) {
        // Log the error.
    }

    public void OnUnityAdsDidStart (string surfacingId) {
        // Optional actions to take when the end-users triggers an ad.
    } 
}
