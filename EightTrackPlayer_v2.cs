using System.Collections;
using UnityEngine;
using UnityEngine.UI; //Rob/TSquare/Piazza notes: "...allows us to create variables based on Unity UI components

[RequireComponent(typeof(AudioSource))] //"automatically adds a Component to this gameObject when the script is added"
public class EightTrackPlayer : MonoBehaviour
{
    //Rob/TSquare/Piazza notes: "public variables can be viewed and assigned in the Editor
    //e.g. to assign a UI.Text Component from another GameObject to this one, 
    //drag the GameObject containing that Component (like the Text child 
    //of your Button) to the field. 
    //Alternatively, you can click the small 'o' next to the field and
    //Unity will show you a browser with all valid objects in your Hierarchy
    //that contain that Component."

    public float songPitch = 1f; //Define song's pitch to default value of 1float. 

    public AudioSource songSource; //assign via GetComponent<> below.
    public AudioClip[] songClips = new AudioClip[4];//Student/TSquare/Piazza: "this should be assigned in the Editor, 
    //referenced/accessed by clips[0], clips[1], clips[2], and clips[3].
    //You can set the AudioSource clip to any of these and play it with AudioSource.Play()."

    public AudioSource otherSource;
    public AudioClip[] otherClips = new AudioClip[7];//Control sounds to be assigned to this array.
    public Text textField;//Rob/TSquare/Piazza: "this is assigned in the Editor, need using UnityEngine.UI for this to work"

    public float playbackProgress;

    public int firstSong = 0;
    public int secondSong = 1;
    public int thirdSong = 2;
    public int fourthSong = 3;
    public int currentSong;
           
    public AudioSource songVolControl;
    public float songVolume;
    public float sound_fx_Volume = 0.7f;

    public int playButton = 0;
    public int stopButton = 1;
    public int pauseButton = 2;
    public int ffButton = 3;
    public int nextButton = 4;
    public int previousButton = 5;
    public int resetButton = 6;
    public int thisSound;

    // Rob/TSquare/Piazza: "Awake() fires on all scripts before Start() is called on any of them... 
    // add this function if you need to do something before your Start() functions fire
    // there are other functions you can implement that are part of MonoBehaviour in Unity's documentation"
    void Awake()
    {
        songVolControl = gameObject.AddComponent<AudioSource>(); //Assign volume control game object.
        songVolume = gameObject.AddComponent<AudioSource>().volume; //Assign volume var to vol control g.o.
    }

    void Start()
    {   
        songSource.time = playbackProgress;// Initialize time monitor.
        songSource.pitch = songPitch;// Initialize pitch of songSource AudioSource to songPitch, which has.... 
                                     //...a default value of 1float.
    }

    void Update() //Driver for the song runtime display.
    {
        if (songSource.clip == songClips[firstSong])
        {
            currentSong = firstSong;
        }
        else if (songSource.clip == songClips[secondSong])
        {
            currentSong = secondSong;
        }
        else if (songSource.clip == songClips[thirdSong])
        {
            currentSong = thirdSong;
        }
        else
        {
            if (songSource.clip == songClips[fourthSong])
            {
                currentSong = fourthSong;
            }
        }
        playbackProgress = songSource.time;
        songPitch = songSource.pitch;
        if (textField != null && songPitch > 0) //Check to see if textField is correctly assigned to a project script in Unity editor. 
        {
            textField.text = "Current Time: " + songSource.time.ToString(); //Current time appears as a float running time on canvas...
            //...display for song currently playing.
        }
    }
   
    public void Play()
    {
        if (otherSource.isPlaying == false && songSource.isPlaying == false) //Check to see that neither and otherSource AudioClip or...
            //...songSource AudioClip are playing prior to loop initialization.
        {
            songSource.pitch = 1;
            otherSource.PlayOneShot(otherClips[playButton], sound_fx_Volume); //Play once, then destroy.
            playbackProgress = songSource.time;// Time check/monitor
            songSource.PlayDelayed(5);// Delay playing of song by 5 seconds to allow playButton soundfx to finish playing first. 
            songSource.loop = true;// This AudioClip will loop back to zero and replay once it reaches audioclips[clip].length.
        }
    }
    
    ///Instuctions: "Stop: Stops playback of the currently selected track/file.If played again, it should resume where it left off."
    public void Stop()
    {
        songSource.pitch = 1;
        otherSource.PlayOneShot(otherClips[stopButton], sound_fx_Volume);// SoundFX volume is pre-defined as 0.7.
        songSource.Pause();// AudioSource.Pause() function used instead of AudioSource.Stop()...Pause() would keep time at same position...
        //..when playback begins again, either by re-pressing the Play or Pause buttons on the canvas/UI in the editor.  
        playbackProgress = songSource.time;// Time monitor.
    }

    ///TSquare/Assignment One Instructions: "Pause: Stops playback of the currently selected track/file, then resumes where it left off when pressed again.
    ///(It functions like the Play button when the audio is stopped, and functions like the Stop button 
    ///while the audio is playing)"
    public void Pause()
    {
        otherSource.PlayOneShot(otherClips[pauseButton], sound_fx_Volume); //Play Stop button soundFX one time
        if (songSource.isPlaying) // If audio clip is playing...
        {
            songSource.pitch = 1;
            songSource.Pause(); // Pause the song, keeping it at that time so it may resume at that time once Play...
            //...or Pause button are pressed.
            playbackProgress = songSource.time; // Implement time monitor.
        }
        else
        {
            songSource.pitch = 1;
            playbackProgress = songSource.time; // Implement time monitor.
            songSource.PlayDelayed(5); // Play song after 5 second delay, to allow the soundFX clip to play unambiguously.
        }
    }

    ///Instructions: "Fast Forward: Plays the current track forwards at a fast rate of speed until it reaches the end of the file or
    ///the STOP button is pressed. (note: 8 track players could not rewind) See AudioSource.pitch.
    ///>>>>>>>>>When fast forwarding, the audio should playback (this will make it easier for us to hear that it is working)"
    public void FastForward() 
    {
        otherSource.PlayOneShot(otherClips[ffButton], sound_fx_Volume); //Sound effect for clicking FF button on UI is played once...
        //...then destroyed.
        songPitch = 1;
        if (songSource.isPlaying) //If a song is already playing when user presses the FF button...
        {
            songSource.Pause(); //Pause/Stop the song clip.
            songSource.time = playbackProgress; //Implement time monitor.
            songSource.pitch = 3;
            songSource.PlayDelayed(5); // Play song with delay (to playOneShot of soundFX) since song was paused.
            songSource.pitch = 3; //Speed up the song's pitch by a factor of three to trigger fast forwarding mechanism.
            playbackProgress = songSource.time; //Implement time monitor.
            if (playbackProgress == songClips[currentSong].length) //If the song reaches its end before user stops the fast forward...
            //...mechanism, do the following:
            {
                songSource.Stop(); //Stop/Pause the song.
                playbackProgress = songSource.time; //Implement time monitor.
                songSource.time = 0; //Song will start from beginning, and play at normal pitch (1f).
                songSource.pitch = 1; //Change pitch back to default of 1float.
                songSource.Play(); //Replay song from time = 0
                songSource.pitch = 1;
            }
        }
        else
        {
            playbackProgress = songSource.time; //Implement time monitor.
            songSource.pitch = 3;
            songSource.PlayDelayed(5); // Play song before speeding up the song's pitch.
            songSource.pitch = 3; // Speed up the pitch by a factor of 3, to fast forward through song until Stop or Pause button...
            //...are pressed. 
            songSource.time = playbackProgress; //Implement time monitor.
            if (playbackProgress == songClips[currentSong].length) //If the song reaches its end before user stops the fast forward...
            //...mechanism, do the following:
            {
                songSource.Stop(); //Stop/Pause the song.
                playbackProgress = songSource.time; //Implement time monitor.
                songSource.time = 0; //Song will start from beginning, and play at normal pitch (1f).
                songSource.pitch = 1; //Change pitch back to default of 1float.
                songSource.Play(); //Replay song from time = 0
                songSource.pitch = 1;
            }
        }
    }
    
    ///Instructions: "Next Track: Switches from the current track/file to the next one.If the track/file is currently playing when
    ///switched, it should continue playback at the same playback point(relative to the beginning) when the new track 
    ///is loaded.If the playback position is greater than the length of the track being switched to, then you should
    ///set the playback position to the beginning and play from there."
    public void Next()
    {
        otherSource.PlayOneShot(otherClips[nextButton], sound_fx_Volume); //Play Next button on UI soundFX.
        songSource.pitch = 1;
        if (currentSong == fourthSong && (songSource.time >= songClips[firstSong].length)) //If the current song playing has an index...
            //...of 3 in the song clip array, and the time reached in that clip has surpassed the length of the next song (with index of 0)
            //...in the array loop, do the the following: 
        {
            songSource.Pause(); //Pause/Stop the song's playback.
            playbackProgress = songSource.time; //Implement time monitor.
            songSource.clip = songClips[firstSong]; //Audio clip to play is assigned as the 0 index in clip array.
            songSource.time = 0; //Time in "next" song is pre-set to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before playing the song.
        }
        else if (currentSong == fourthSong && (songSource.time < songClips[firstSong].length))
        // If the current song's index is equal to 3, and the time of the songSource audio clip is less than the length of...
        //...the clip at array index 0, do the following:
        {
            songSource.Pause(); //Pause/Stop the song.
            playbackProgress = songSource.time; //Take note of the playback-time for the current song, upon pausing. 
            songSource.clip = songClips[firstSong]; //Assign the song at the 0 index to the songSource audio clip.
            songSource.time = playbackProgress; //Assign the song at index 0 a time equal to the time of currentSong at Pause.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before playing song.           
        }
        else if (currentSong == thirdSong && (songSource.time >= songClips[fourthSong].length))
            //If current song's index is 2, and the current song's playback time has reached... 
            //...a point beyond the length of the next song (at array index [3]),...
            //...do the following:
        {
            songSource.Pause(); //Pause/Stop the song.
            playbackProgress = songSource.time; //Implement the time monitor.
            songSource.clip = songClips[fourthSong]; //...assign the audio clip at index [3] to the...
            //...songSource audiosource.
            songSource.time = 0; //Reset time for starting the next song to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before starting audio clip, to allow the Next button soundFX to play...
            //...discernibly.
        }
        else if (currentSong == thirdSong && (songSource.time < songClips[fourthSong].length))
        //If current song's index is 2, and the current song's playback time has reached... 
        //...a point before the length of the next song (at array index [3]),...
        //...do the following:
        {
            songSource.Pause(); //Pause/Stop the song.
            playbackProgress = songSource.time; //Implement the time monitor.
            songSource.clip = songClips[fourthSong]; //...assign the audio clip at index [3] to the...
            //...songSource audiosource.
            songSource.time = playbackProgress; //Reset time for starting the next song to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before starting audio clip, to allow the Next button soundFX to play...
            //...discernibly.
        }
        else if (currentSong == secondSong && (songSource.time >= songClips[thirdSong].length))
        //If current song's index is 1, and the current song's playback time has reached... 
        //...a point beyond the length of the next song (at array index [2]),...
        //...do the following:
        {
            songSource.Pause(); //Pause/Stop the song.
            playbackProgress = songSource.time; //Implement the time monitor.
            songSource.clip = songClips[thirdSong]; //...assign the audio clip at index [2] to the...
            //...songSource audiosource.
            songSource.time = 0; //Reset time for starting the next song to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before starting audio clip, to allow the Next button soundFX to play...
            //...discernibly.
        }
        else if (currentSong == secondSong && (songSource.time < songClips[thirdSong].length))
        //If current song's index is 1, and the current song's playback time has reached... 
        //...a point before the length of the next song (at array index [2]),...
        //...do the following:
        {
            songSource.Pause(); //Pause/Stop the song.
            playbackProgress = songSource.time; //Implement the time monitor.
            songSource.clip = songClips[thirdSong]; //...assign the audio clip at index [2] to the...
            //...songSource audiosource.
            songSource.time = playbackProgress; //Reset time for starting the next song to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before starting audio clip, to allow the Next button soundFX to play...
            //...discernibly.
        }
        else if (currentSong == firstSong && (songSource.time >= songClips[secondSong].length))
        //If current song's index is 0, and the current song's playback time has reached... 
        //...a point beyond the length of the next song (at array index [1]),...
        //...do the following:
        {
            songSource.Pause(); //Pause/Stop the song.
            playbackProgress = songSource.time; //Implement the time monitor.
            songSource.clip = songClips[secondSong]; //...assign the audio clip at index [1] to the...
            //...songSource audiosource.
            songSource.time = 0; //Reset time for starting the next song to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before starting audio clip, to allow the Next button soundFX to play...
            //...discernibly.
        }
        else
        {
            if (currentSong == firstSong && (songSource.time < songClips[secondSong].length))
            //If current song's index is 0, and the current song's playback time has reached... 
            //...a point before the length of the next song (at array index [1]),...
            //...do the following:
            {
                songSource.Pause(); // else Pause/Stop then song
                playbackProgress = songSource.time; //Take note of the playback time of the current song, so that next song can...
                //...start at the same time. 
                songSource.clip = songClips[secondSong]; // The audio clip to be assigned to the songSource audiosource...
                //...will be the clip in the array with an index of [secondSong].
                songSource.time = playbackProgress; //Implement time monitor.
                songSource.Stop();
                songSource.PlayDelayed(5); //Wait 5 seconds before playing song.
            } 
        }
    }

    ///Instructions: "Previous Track: Switches from the current track/file to the previous one.Same functionality as the Next Track
    ///button otherwise."
    public void Previous()
    {
        otherSource.PlayOneShot(otherClips[previousButton], sound_fx_Volume); //Play Previous button soundFX. 
        songSource.pitch = 1;
        if (currentSong == firstSong && (songSource.time >= songClips[fourthSong].length)) //If the currentSong's index...
        ///...is equal to that of "firstSong" (songClips[0]), AND the clip at an index of 3 in the array has a max length...
        ///...that is less than the time that has elapsed in songClips[0], do the following:
        {
            songSource.Pause(); //Pause/Stop the track
            playbackProgress = songSource.time; //Note the time
            songSource.clip = songClips[fourthSong]; //Assign the clip at songClips array index 3 to the songSource audiosource.
            songSource.time = 0; //Set playback time of track to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds to play, so that Previous button soundFX may be heard more clearly.
        }
        else if (currentSong == firstSong && (songSource.time < songClips[fourthSong].length)) //If the current song's index is...
        //...0, AND the playback time noted is less than the length of the song clip in the songSource...
        //...audiosource at index 3, do the following:  
        {
            songSource.Pause(); //Stop/Pause track
            playbackProgress = songSource.time; //Take note of the time.
            songSource.clip = songClips[fourthSong]; //Assign song clip with index 3 to the songSource audiosource.
            songSource.time = playbackProgress; //Set playback time to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before playing song.
        }
        else if (currentSong == secondSong && (songSource.time >= songClips[firstSong].length)) //If the current song's index is...
        //...1, AND the playback time noted is greater than or equal to the length of the song clip in the songSource...
        //...audiosource at index [0], do the following:  
        {
            songSource.Pause(); //Stop/Pause track
            playbackProgress = songSource.time; //Take note of the time
            songSource.clip = songClips[firstSong]; //Assign song clip with index [0] to the songSource audiosource. 
            songSource.time = 0; //Set playback time to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before playing song.
        }
        else if (currentSong == secondSong && (songSource.time < songClips[firstSong].length)) //If the current song's index is...
        //...1, AND the playback time noted is less than the length of the song clip in the songSource...
        //...audiosource at index [0], do the following:  
        {
            songSource.Pause(); //Stop/Pause track
            playbackProgress = songSource.time; //Take note of the time
            songSource.clip = songClips[firstSong]; //Assign song clip with index [0] to the songSource audiosource. 
            songSource.time = playbackProgress; //Set playback time to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before playing song.
        }
        else if (currentSong == thirdSong && (songSource.time >= songClips[secondSong].length)) //If the current song's index is...
        //...2, AND the playback time noted is greater than or equal to the length of the song clip in the songSource...
        //...audiosource at index [1], do the following:  
        {
            songSource.Pause(); //Stop/Pause track
            playbackProgress = songSource.time; //Take note of the time
            songSource.clip = songClips[secondSong]; //Assign song clip with index [secondSong] to the songSource audiosource. 
            songSource.time = 0; //Set playback time to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before playing song.
        }
        else if (currentSong == thirdSong && (songSource.time < songClips[secondSong].length)) //If the current song's index is...
        //...1, AND the playback time noted is greater than or equal to the length of the song clip in the songSource...
        //...audiosource at index [0], do the following:  
        {
            songSource.Pause(); //Stop/Pause track
            playbackProgress = songSource.time; //Take note of the time
            songSource.clip = songClips[secondSong]; //Assign song clip with index [secondSong] to the songSource audiosource. 
            songSource.time = playbackProgress; //Set playback time to zero.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before playing song.
        }

        else if (currentSong == fourthSong && (songSource.time < songClips[thirdSong].length))
        {
            songSource.Pause(); //Stop/Pause track
            playbackProgress = songSource.time; //Take note of the time.
            songSource.clip = songClips[thirdSong]; //Assign song clip with index 2 to the songSource audiosource.
            songSource.time = playbackProgress; //Take note of the time.
            songSource.Stop();
            songSource.PlayDelayed(5); //Wait 5 seconds before playing song.
        }
        else
        {
            if (currentSong == fourthSong && (songSource.time >= songClips[thirdSong].length))
            {
                songSource.Pause(); //Stop/Pause track
                playbackProgress = songSource.time; //Take note of the time.
                songSource.clip = songClips[thirdSong]; //Assign song clip with index 2 to the songSource audiosource.
                songSource.time = 0; //Set playback time to zero.
                songSource.Stop();
                songSource.PlayDelayed(5); //Wait 5 seconds before playing song.
            }
        }
    }

    ///Instructions: "Reset: Button that resets/rewinds the playback position of the audio player to the beginning."
    public void Reset() //Reset to song clip at index 0 in songSource audiosource, and set playback to zero...
    //...prior to playing song.
    {
        otherSource.PlayOneShot(otherClips[resetButton], sound_fx_Volume); //Play Reset button soundFX.
        songSource.pitch = 1;
        if (songSource.isPlaying) //If a song clip from the songSource audiosource is playing, do the...
        //...following:
        {
            songSource.Pause(); //Pause/stop the clip
            playbackProgress = songSource.time; //Take note of the time.
            songSource.clip = songClips[firstSong]; //Reassign the clip at index zero to the audiosource for playback
            songSource.time = 0; //Set the clip to time of zero prior to playback.
            songSource.PlayDelayed(5); //Wait 5 seconds and play.
        }
        else
        {
            playbackProgress = songSource.time; //Implement time monitor.
            songSource.clip = songClips[firstSong]; //Assign clip at 0 index of the audio clip array, to the songSource...
                                                    //...audiosource.
            songSource.time = 0; //Set playback to zero.
            songSource.PlayDelayed(5); //Wait 5 seconds, then play song.
        }
    }
}