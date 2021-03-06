﻿using System;
using System.Net;
using UnityEngine;

// Changed to Utilities instead of Utils to avoid collision with the game Utils class
namespace Longship.Utilities
{
    public class UpdatesChecker
    {
        private const string RELEASES_URL = "https://api.github.com/repos/alexmog/longship/releases/latest";
        
        public static bool CheckForUpdate(out string url)
        {
            var client = new WebClient();
            client.Headers.Add("User-Agent: Longship Valheim Mod");
            try
            {
                var lastRelease = JsonUtility.FromJson<LastRelease>(client.DownloadString(RELEASES_URL));
                if (!lastRelease.draft && !lastRelease.prerelease)
                {
                    url = lastRelease.url;
                    return lastRelease.tag_name != Longship.BuildTag;
                }
            }
            catch(Exception e)
            {
                Longship.Instance.Log.LogWarning("Can't check if updates are available.");
                Longship.Instance.Log.LogDebug(e);
            }

            url = null;
            return false;
        }

        private class LastRelease
        {
            public string url;
            public string tag_name;
            public bool draft;
            public bool prerelease;
        }
    }
}