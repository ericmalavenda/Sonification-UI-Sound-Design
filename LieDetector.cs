using System.Collections;
using UnityEngine;
using UnityEngine.UI; /// "allows us to create variables based on Unity UI components"

[RequireComponent(typeof(AudioSource))]
public class LieDetector : MonoBehaviour
{
    public AudioSource ambientSource;

    /// <summary>
    /// pulse = Ambient Channel #1: represented by
    /// respiration = Ambient Channel #2: Right channel extraction of ticking clock sound
    /// with the original source of the sound clip being the song "The Great Gig in the Sky" from Pink Floyd's 1973
    /// album "Dark Side of the Moon." Sound clip was copied with copies being merged for 
    /// resulting clip imported into Unity. Clip was also first modified to correct for extraneous
    /// noise from the clipped segment prior to importing, using:
    /// Noise Reduction/Restoration --> Automatic Click Remover --> Heavy Reduction 
    /// setting in adobe audition. 
    /// </summary>
    public AudioSource lieDetector;
    public AudioClip[] lieDetectorClip = new AudioClip[5];

    public int pulseClipIndex = 0;
    public int respirationClipIndex = 1;
    public int gsrClipIndex = 2;
    public int truthClipIndex = 3;
    public int lieClipIndex = 4;
    public int currentClipIndex;

    public AudioSource pulseSource;
    public AudioClip pulseClip;

    [Range(0.0f, 3.0f)]
    public float pulsePitch = 1.0f; // Adjust pitch of pulse/'river water flow' AudioSource to denote change in pulse rate.

    public AudioSource pulsePitchControl; // 1st Slider g.o. for pulse: rate/pitch control

    [Range(0.0f, 1.0f)]
    public float pulseVolume = 0.5f; // Adjust volume of pulse/'ticking clock' AudioSource to denote 
                                     // change in pulse amplitude/intensity.
    public AudioSource pulseVolControl; // 2nd Slider g.o. for pulse: volume control

    /// <summary>
    /// respiration = Ambient Channel #2: Left channel extraction of cash register "kaChing" sound
    /// with the original source of the sound clip being the song "Money" from Pink Floyd's album
    /// "Dark Side of the Moon." The same protocol was followed for this sound prior to import into
    /// Unity, as was followed for the pulse sound. A segment of the song was first copied, with those
    /// copies being merged for the resulting clip imported into Unity. Clip was also first modified
    /// to reduce extraneous noise in my clipped segment prior to importing, using:
    /// Noise Reduction/Restoration --> Automatic Click Remover --> Heavy Reduction 
    /// setting in adobe audition. 
    /// 
    /// [HW2] INSTRUCTIONS: "Ambient sound channel representing the respiration rate of 
    /// the subject, manipulated by a slider (to simulate low and high breathing). 
    /// There should be a second slider that controls the volume of this source, as well."
    /// </summary>
    public AudioSource respirationSource;
    public AudioClip respirationClip;

    [Range(0.25f, 3.0f)]
    public float respirationPitch = 1.0f;  // Adjust pitch of respiration "wind" AudioSource to change in respiration 
                                           // rate.
    public AudioSource respirationPitchControl; // 1st Slider g.o. for respiration: rate/pitch control

    [Range(0.25f, 1.0f)]
    public float respirationVolume = 0.50f; // Adjust volume of respiration AudioSource to denote change in respiration
                                            // amplitude/intensity.
    public AudioSource respirationVolControl; // 2nd Slider g.o. for respiration: volume control

    ///<summary>
    /// INSTRUCTOR NOTES: "[Range(low, high)] will restrict the values in the Inspector to this range.
    /// When tweaking values in the Editor -> Inspector,...
    /// ...it documents the values you expect this variable to receive"
    ///</summary>

    /// Alert AudioSources (4)

    /// <summary>
    /// My alert sounds for 'truthClip' and 'lieClip' were also 
    /// clipped from the song "Time" on Pink Floyd's album, 
    /// "Dark Side of the Moon," which has been  one of my favorite 
    /// albums (strangely, to some) since I was 12-years-old. 
    /// What I and many others perceive to be the brilliance 
    /// of Dark Side of the Moon, also happens to be the product
    /// of work done by sound engineer, Alan Parsons, on the album.
    /// 
    /// Using Adobe Audition: Both the 'truth' and 'lie' sound clips
    /// were obtained as short segments of the the song Time 
    /// that were appropiate for playing each of the sounds as a 
    /// loop. The 'truth' sound is a clip from the unaltered 
    /// song having an audibly lower pitch and softer tone relative to the 
    /// the sound I clipped for the 'lie' sound. I chose a 'lie' sound that
    /// was indelible as an alert, consisting of the alarm clock sound at the 
    /// beginning of "Time." I felt these sound clip selections were 
    /// representative of a dichotomy that seemed 
    /// natural to express the contrasting messages 
    /// conveyed by their respective alert AudioSource game 
    /// object buttons: 'alertTruth' & 'alertLie'...One soft, and one brutal.
    /// I reduced the decibel setting to -15 dB to make it more tolerable.
    /// </summary>
    public AudioSource alertSource;

    /// <summary>
    /// truth: please see above
    /// </summary>
    public AudioSource truthSource;
    public AudioClip truthClip;
    public AudioSource alertTruth; // alert button g.o.
    [Range(0.25f, 3.0f)]
    public float truthConfidence = 1.0f;
    /// <summary>
    /// pitch of the sound clip is adjusted up or down according to the lie detector's
    /// confidence in its assessment of a truth.
    /// </summary>
    public AudioSource truthConfidenceLevel; // slider g.o. 
    public float truthVolume = 1.0f;
    
    /// <summary>
    /// lie: Lowest amplitude variation of the sound used that is included in the song
    /// "Time" in Pink Floyd's "Dark Side of the Moon" album. Please see above for 
    /// additional breakdown of justification for use of this sound and how it was modified
    /// from its original source.
    /// </summary>
    public AudioSource lieSource;
    public AudioClip lieClip;
    public AudioSource alertLie; // alert button g.o[Range(0.0f, 1.0f)]
    [Range(0.25f, 3.0f)]
    public float lieConfidence = 1.0f;

    /// <summary>
    /// pitch of the sound clip is adjusted up or down according to the lie detector's
    /// confidence in its assessment of a lie.
    /// </summary>
    public AudioSource lieConfidenceLevel; // slider g.o.

    public float lieVolume = 1.0f;

    /// <summary>
    /// GSR: The song "Time" was once again used, with right channel of song, 
    /// drum solo segment + with a variation of the sound used for 'truthClip'
    /// (not as stern...more questioning in its tone) being extracted for 
    /// this alert. Sound clip was cut from the original song to accommodate 
    /// looping of sound. Sound copies were merged linearly and used to extend
    /// the sound that was ultimately imported into unity so that its length
    /// totaled just over 15 seconds. Prior to copying and merging, the clip
    /// was modified to reduce extraneous noise in the clipped segment, using:
    /// Noise Reduction/Restoration --> Automatic Click Remover --> Heavy Reduction 
    /// setting in adobe audition.  
    /// </summary>
    public AudioSource gsrSource; // audio source for alerting the user/tester to the detection of a spike in a subject's galvanic skin response
    public AudioClip gsrClip;
    public AudioSource alertGSR; // alert button g.o.

    [Range(0.25f, 3.0f)]
    public float gsrConfidence = 1.0f; // pitch

    /// <summary>
    /// pitch of the sound clip is adjusted up or down according to the lie detector's
    /// confidence in its assessment of a galvanic skin response (gsr).
    /// </summary>
    public AudioSource gsrConfidenceLevel; // slider g.o.

    public float gsrVolume = 1.0f;

    /// <summary>
    /// As per Instructor's response on Piazza: "The sliders are used to adjust 
    /// the confidence level of the Truth/Lie and the strength of the GSR signal. 
    /// Higher values of the slider on Truth/Lie indicate the system is more confident
    /// that the answer is truthful or not. Higher values on the GSR indicate more of 
    /// an emotional response. You will choose how that changes the sound output." 
    /// </summary>

    /// <summary>
    /// The threshold pulsePitch values I've selected are based on a normal HR of 60-100bpm according 
    /// to the following resource: http://my.clevelandclinic.org/services/heart/prevention/exercise/pulse-target-heart-rate
    /// Resting HR of 60-80bpm: http://share.upmc.com/2015/07/what-is-a-normal-heart-rate/
    /// Value of 190bpm as a max HR for a 30-yr-old and a target exercise HR between 95-162bpm. 
    /// Given these values, I've selected a range of 80bpm < moderatePulsePitch < 95bpm 
    /// I standardized values according to those expected for a 30-yr-old, so that ranges 
    /// could be included in my script as relative percentages within a range 0.00 (0%) and 2.50 (250%),
    /// with 80bpm[max CALM pulse] being represented by a value of 1 (100%) within that range, 
    /// 60bpm[typical min CALM pulse] being represented by a value of 0.75 (75%), 
    /// 95bpm[min EXCITED TRUE pulse] being represented by a value of 1.1875 (118.75%),
    /// 128.5[min EXCITED FALSE pulse] being represented by a value of 1.60625 (160.625%),
    /// and 190bpm[max pulse] being represented by a value of 2.375 (237.5%).
    /// </summary>

    /// <summary>
    /// The following are pre-set pitch values for the pulse AudioSource, 
    /// for sonification of a characteristic ('minimum-within-range' pulse * factor(float)) 
    /// experienced by a subject who is having one of the following physiological responses:
    /// a (1)calm," (2)"excited but telling the truth," (3)"excited but telling a lie," 
    /// or (4)"moderate" physiological response:
    /// </summary>

    /// <summary>
    /// I've multiplied each pitch (except for pre-defined "calm" pitch setting) by a sliding/increasing factor 
    /// of either 1.05f, 1.15f, and 1.25f. Justification for use is that it helps to increase the number of 
    /// values within each range, helping to improve recognition of a change in the sonification parameter settings.
    /// Highest pitch value (excitedFalsePulePitch) remains below 3. 
    /// </summary>
    public float calmPulsePitch = 0.50f; // The pre-set pitch value(40/80), with a pulse of 80 = 1.0f for the pulse AudioSource, 
                                         // for sonification of pulse data of a physiologically calm response made by the subject.
    public float excitedTruePulsePitch = 1.1875f * 1.15f; // The pre-set pitch value(95/80)*1.25 for the pulse AudioSource, 
                                                          // for sonification of a physiological response to an 
                                                          // excited 'true' statement. The appointment of this setting 
                                                          // will also trigger the "True" alert. 
    public float excitedFalsePulsePitch = 1.60625f * 1.25f; // Pre-set pitch variation(128.5/80)*1.25 on pulse AudioSource pitch 
                                                            // for sonification of a subject's physiological response to an 
                                                            // excited 'false' statement. 
                                                            // The appointment of this setting will also trigger the "False" alert.  
    public float moderatePulsePitch = 1.00f * 1.05f; // Variation on pulse audioSource pitch for sonification of 
                                                     // a moderate physiological response to a given statement. 
                                                     // In this case, the statement's identification as either 
                                                     // 'true' or 'false' has yet to be determined.   

    /// <summary>
    /// Pre-set lower range values of pulse AudioSource volume for autoconfig buttons also trigger 'truth' alert 
    /// (for 'excitedTrue' pre-set volume value), 'lie' alert (for 'excitedFalse' pre-set pitch or volume value), 
    /// and 'startGSR' alert (for 'moderate' pre-set volume value) 
    /// increases in intensity as one goes from calm --> truth ---> moderate ----> lie. Values were otherwise 
    /// selected arbitrarily, with an equal number of values within each range.
    /// </summary>
    public float calmPulseVolume = 0.25f;
    public float excitedTruePulseVolume = 0.75f; // The appointment of this setting will also trigger the "True" alert
    public float excitedFalsePulseVolume = 1.00f; // The appointment of this setting will also trigger the "False" alert
    public float moderatePulseVolume = 0.50f;

    /// <summary>
    /// Pre-set volume of alerts increases in intensity/amplitude as one goes from calm --> truth ---> moderate ----> lie
    /// </summary>

    /// <summary>
    /// The respirationPitch values selected are based on a normal respiration rate of 12-20 breaths/min,
    /// resting respiration rate of 12-16 breaths/min, moderate respiration rate of 17-20 breaths/min:
    /// 12<=[typical min CALM respiration rate] being represented by 12/17 (or 0.70588 (70.588%) of 17 
    /// [min MODERATE respiration rate], 17 breaths per minute thus being represented by a value of 1 (100%),
    /// by a value of 1 (100%), 21 breaths/minute being represented by 1.23529 (123.529%), 
    /// 25 breaths/min [max respiration rate, and also used as a respiration rate indicative of min EXCITED FALSE respiration rate]
    /// being represented by 1.47059 (147.059%). I've also increased the default spatial blend values by 0.1 for the other settings,
    /// to improve user experience when determining whether a change in sonification parameter settings has occurred.
    /// These values were considered upon review of the following resources: 
    /// http://my.clevelandclinic.org/health/diagnostics/hic_Vital_Signs
    /// http://www.hopkinsmedicine.org/healthlibrary/conditions/cardiovascular_diseases/vital_signs_body_temperature_pulse_rate_respiration_rate_blood_pressure_85,P00866/
    /// Values have been standardized according to those expected for a 30-yr-old, so that ranges 
    /// could be included in my script as relative percentages.
    /// </summary>

    /// <summary>
    /// The following are pre-set pitch values for the respiration AudioSource, for sonification of a characteristic 
    /// 'minimum-within-range' respiration rate experienced by a subject who is having one of 
    /// the following physiological responses:
    /// a (1)calm," (2)"excited but telling the truth," (3)"excited but telling a lie," 
    /// or (4)"moderate" physiological response:
    /// </summary>
    /// Multiply excitedTrue pre-defined pitch by a factor of 1.25f, and excitedFalse by a factor of 2.0f, and 1.05f for moderate. 
    /// Justification for use is that it helps to increase the number of values within each range, helping to improve recognition 
    /// of a change in the sonification parameter settings. Highest pitch value (excitedFalsePulePitch) remains below 3. 
    public float calmRespirationPitch = 0.70588f; // The pre-set pitch value (12/17) for the respiration AudioSource, 
                                                  // for sonification of resp. rate data of a physiologically calm response made by the subject.
    public float excitedTrueRespirationPitch = 1.23529f * 1.25f; // Variation on pitch(21/17) for sonification of a 
                                                                 // subject's respiration rate upon having a 
                                                                 // physiological response to an excited 'true' statement. 
                                                                 // The appointment of this setting will also trigger 
                                                                 // the "True" alert. 
    public float excitedFalseRespirationPitch = 1.470588f * 2.00f; // Variation on pitch(25/17) for sonification of a subject's 
                                                                   // respiration rate upon having a physiological response 
                                                                   // to an excited 'false' statement. 
                                                                   // The appointment of this setting will also trigger 
                                                                   // the "False" alert. 
    public float moderateRespirationPitch = 1.00f * 1.05f; // Variation on pitch for sonification of a subject's respiration rate upon
                                                           // having a  moderate physiological response to a given statement. In
                                                           // his case, the statement's identification as either 'true' or 'false'
                                                           // has yet to be determined.   

    /// <summary>
    /// Pre-set lower range values of respiration AudioSource volume for autoconfig buttons also trigger 'truth' alert 
    /// (for 'excitedTrue' pre-set volume value), 'lie' alert (for 'excitedFalse' pre-set pitch or volume value), 
    /// and 'startGSR' alert (for 'moderate' pre-set volume value) 
    /// increases in intensity as one goes from calm --> truth ---> moderate ----> lie. Values were otherwise 
    /// selected arbitrarily, with an equal number of values within each range.
    /// </summary>
    public float calmRespirationVolume = 0.25f;
    public float excitedTrueRespirationVolume = 0.75f; // The appointment of this setting will also trigger the "True" alert
    public float excitedFalseRespirationVolume = 1.00f; // The appointment of this setting will also trigger the "False" alert
    public float moderateRespirationVolume = 0.50f;

    /// <summary>
    /// Pre-set volume of alerts increases in intensity/amplitude as one goes from calm --> truth ---> moderate ----> lie
    /// </summary>

    public AudioSource StartAmbientSounds; // button g.o.
    public bool playingAmbientSounds = true; // [INSTRUCTOR notes]: "allows us to turn the functionality on and off, you can do this from the Inspector"
    public bool playingAlertSounds = false;
    public AudioSource StopAllSounds;  // button g.o.

    public AudioSource stopAlertTruth; // button g.o.

    public AudioSource stopAlertLie; // button g.o.

    public AudioSource stopAlertGSR; // button g.o.

    public AudioSource calm;
    public AudioSource excitedTrue;
    public AudioSource excitedFalse;
    public AudioSource moderate;

    public float repeatRateInSeconds = 1.0f;

    public float pulsePlaybackElapsedTime = 0.0f;
    public float respirationPlaybackElapsedTime = 0.0f;
    public float gsrPlaybackElapsedTime = 0.0f;
    public float truthPlaybackElapsedTime = 0.0f;
    public float liePlaybackElapsedTime = 0.0f;

    public float lieDetectorPlaybackElapsedTime;

    public float lieDetectorPlaybackProgress = 0.0f;
    private float lieDetectorPitch;
    private float lieDetectorVolume;
    private float playbackElapsedTime;
    private float p_lieDetectorPlaybackProgress;
    private float r_lieDetectorPlaybackProgress;

    /// <summary>
    /// Awake: Adding gameObject components 
    /// </summary>
    void Awake()
    {
        calm = gameObject.AddComponent<AudioSource>();
        excitedTrue = gameObject.AddComponent<AudioSource>();
        excitedFalse = gameObject.AddComponent<AudioSource>();
        moderate = gameObject.AddComponent<AudioSource>();

        pulseSource = gameObject.AddComponent<AudioSource>(); // Get pulse audio source game object.
        pulseVolControl = gameObject.AddComponent<AudioSource>(); // Get pulse volume control game object.
        pulseVolume = gameObject.AddComponent<AudioSource>().volume; // Get pulse volume object to vol control g.o.
        pulsePitchControl = gameObject.AddComponent<AudioSource>(); // Get pulse pitch control game object.
        pulsePitch = gameObject.AddComponent<AudioSource>().pitch; // Get pulse pitch object to pitch control g.o. 

        respirationSource = gameObject.AddComponent<AudioSource>(); // Get resp. rate audio source game object.
        respirationVolControl = gameObject.AddComponent<AudioSource>(); // Assign resp. rate volume control game object. 
        respirationVolume = gameObject.AddComponent<AudioSource>().volume; // Assign resp. rate volume object to vol control g.o. 
        respirationPitchControl = gameObject.AddComponent<AudioSource>(); // Assign resp. rate pitch control game object.
        respirationPitch = gameObject.AddComponent<AudioSource>().pitch; // Assign resp. rate pitch object to pitch control g.o.

        ambientSource = gameObject.AddComponent<AudioSource>();
        alertSource = gameObject.AddComponent<AudioSource>();

        //truth AudioSource + button + slider
        truthSource = gameObject.AddComponent<AudioSource>(); // audio source
        alertTruth = gameObject.AddComponent<AudioSource>(); // alert button g.o.
        truthConfidenceLevel = gameObject.AddComponent<AudioSource>(); // slider g.o.
        truthConfidence = gameObject.AddComponent<AudioSource>().pitch;
        truthSource.volume = truthVolume; /// Alert volumes are statically set to 1.0f

        //lie AudioSource + button + slider
        lieSource = gameObject.AddComponent<AudioSource>(); // audio source
        alertLie = gameObject.AddComponent<AudioSource>(); // alert button g.o.
        lieConfidenceLevel = gameObject.AddComponent<AudioSource>(); // slider g.o.
        lieConfidence = gameObject.AddComponent<AudioSource>().pitch;
        lieSource.volume = lieVolume; /// Alert volumes are statically set to 1.0f

        //gsr AudioSource + button + slider
        gsrSource = gameObject.AddComponent<AudioSource>(); // audio source
        alertGSR = gameObject.AddComponent<AudioSource>(); // alert button g.o.
        gsrConfidenceLevel = gameObject.AddComponent<AudioSource>(); // slider g.o.
        gsrConfidence = gameObject.AddComponent<AudioSource>().pitch;
        gsrSource.volume = gsrVolume; /// Alert volumes are statically set to 1.0f

        StartAmbientSounds = gameObject.AddComponent<AudioSource>(); // Button g.o.
        StopAllSounds = gameObject.AddComponent<AudioSource>();  // Button g.o.

        stopAlertTruth = gameObject.AddComponent<AudioSource>(); // Button g.o.
        stopAlertLie = gameObject.AddComponent<AudioSource>(); // Button g.o.
        stopAlertGSR = gameObject.AddComponent<AudioSource>(); // Button g.o.
    }
            
    /// <summary>
    /// Start: Getting gameObject components + initializing var values/settings
    /// </summary>
    void Start()
    {
        ambientSource = gameObject.GetComponent<AudioSource>();
        alertSource = gameObject.GetComponent<AudioSource>();
        gsrClip = gameObject.GetComponent<AudioClip>();
        truthClip = gameObject.GetComponent<AudioClip>();
        lieClip = gameObject.GetComponent<AudioClip>();

        pulseSource = gameObject.GetComponent<AudioSource>();
        pulseClip = gameObject.GetComponent<AudioClip>(); // Get pulse audio clip game object.

        respirationSource = gameObject.GetComponent<AudioSource>();
        respirationClip = gameObject.GetComponent<AudioClip>(); // Get respiration audio clip game object.

        gsrSource = gameObject.GetComponent<AudioSource>();
        gsrClip = gameObject.GetComponent<AudioClip>(); // audio clip

        truthSource = gameObject.GetComponent<AudioSource>();
        truthClip = gameObject.GetComponent<AudioClip>(); // audio clip

        lieSource = gameObject.GetComponent<AudioSource>();
        lieClip = gameObject.GetComponent<AudioClip>(); // audio clip

        /// Initialize alert confidence (pitch) levels
        lieSource.pitch = lieConfidence;
        lieSource.volume = 0.75f;
        truthSource.pitch = truthConfidence;
        truthSource.volume = 0.75f;
        gsrSource.pitch = gsrConfidence;
        gsrSource.volume = 0.75f;

        /// Initialize pulse data sonification
        pulseSource.pitch = pulsePitch;
        pulseSource.volume = pulseVolume;

        /// Initialize respiration data sonification
        respirationSource.pitch = respirationPitch;
        respirationSource.volume = respirationVolume;

        /// Initialize time counter
        pulseSource.time = pulsePlaybackElapsedTime;
        respirationSource.time = respirationPlaybackElapsedTime;
        gsrSource.time = gsrPlaybackElapsedTime;
        truthSource.time = truthPlaybackElapsedTime;
        lieSource.time = liePlaybackElapsedTime;
        if (playingAmbientSounds == true)
        {
            lieDetector = gameObject.GetComponent<AudioSource>();
            lieDetectorClip[currentClipIndex] = gameObject.GetComponent < AudioClip>();
            lieDetectorClip[pulseClipIndex] = pulseClip;
            lieDetectorClip[respirationClipIndex] = respirationClip;
            
             
         }
    }

    // "Update is called once per frame"
    void Update()
    {
        if (lieDetector.clip == lieDetectorClip[respirationClipIndex])
        {
            currentClipIndex = respirationClipIndex;
        }
        else if (lieDetector.clip == lieDetectorClip[gsrClipIndex])
        {
            currentClipIndex = gsrClipIndex;
        }
        else if (lieDetector.clip == lieDetectorClip[truthClipIndex])
        {
            currentClipIndex = truthClipIndex;
        }
        else
        {
            if (lieDetector.clip == lieDetectorClip[truthClipIndex])
            {
                currentClipIndex = truthClipIndex;
            }
        }
        if (playingAmbientSounds)
        {
            p_lieDetectorPlaybackProgress += Time.deltaTime;
            r_lieDetectorPlaybackProgress += Time.deltaTime;
            if (p_lieDetectorPlaybackProgress >= pulseClip.length)
            {
                pulseSource.Play();
            }
            else
            {
                if (r_lieDetectorPlaybackProgress >= respirationClip.length)
                {
                    respirationSource.Play();
                }
            }
        }
        else
        {
            StopAll();
        }
    }
              
    
        

        /***
        }
        else 

        if (currentClipIndex == pulseClipIndex)
        {
            lieDetector.clip = lieDetectorClip[pulseClipIndex];
            pulsePlaybackElapsedTime = pulseSource.time;
            if (playingAmbientSounds)
            {
                lieDetectorPlaybackProgress += Time.deltaTime;
                if (lieDetectorPlaybackProgress >= lieDetectorClip[pulseClipIndex].length)
                {
                    pulsePlaybackElapsedTime = lieDetectorPlaybackProgress;
                    //StartPulse(); //playbackElapsedTime is reset to zero in this function
                    StartRespiration();
                }
            }
        }
        else if (currentClipIndex == respirationClipIndex)
        {
            lieDetector.clip = lieDetectorClip[respirationClipIndex];
            respirationPlaybackElapsedTime = respirationSource.time;
            
            if (playingAmbientSounds)
            {
                lieDetectorPlaybackProgress += Time.deltaTime;
                if (lieDetectorPlaybackProgress >= lieDetectorClip[respirationClipIndex].length)
                {
                    respirationSource.time = lieDetectorPlaybackProgress;
                    StartRespiration(); //playbackElapsedTime is reset to zero in this function
                }
            }
        }
    }

    /***
        else if ((playingAlertSounds == true) && (currentClipIndex == gsrClipIndex))
        {
            lieDetector.clip = lieDetectorClip[gsrClipIndex];
            gsrConfidence = gsrSource.pitch;
            gsrPlaybackElapsedTime = gsrSource.time;
            if (playingAmbientSounds)
            {
                lieDetectorPlaybackProgress += Time.deltaTime;
                if (lieDetectorPlaybackProgress >= lieDetectorClip[gsrClipIndex].length)
                {
                    AlertForGSR(); //playbackElapsedTime is reset to zero in this function
                }
            }
        }
        else if ((playingAlertSounds == true) && (currentClipIndex == truthClipIndex))
        {
            lieDetector.clip = lieDetectorClip[truthClipIndex];
            truthConfidence = truthSource.pitch;
            truthPlaybackElapsedTime = truthSource.time;
            if (playingAmbientSounds)
            {
                lieDetectorPlaybackProgress += Time.deltaTime;
                if (lieDetectorPlaybackProgress >= lieDetectorClip[truthClipIndex].length)
                {
                    PlayTruthAlert(); //playbackElapsedTime is reset to zero in this function
                }
            }
        }
        else
        {
            if ((playingAlertSounds == true) && (currentClipIndex == lieClipIndex))
            {
                lieDetector.clip = lieDetectorClip[lieClipIndex];
                lieConfidence = lieSource.pitch;
                liePlaybackElapsedTime = lieSource.time;
                if (playingAmbientSounds)
                {
                    lieDetectorPlaybackProgress += Time.deltaTime;
                    if (lieDetectorPlaybackProgress >= lieDetectorClip[lieClipIndex].length)
                    {
                        PlayLieAlert(); //playbackElapsedTime is reset to zero in this function
                    }
                }
            }
        }
    }
    ***/
    
    public void StartPulse()
    {

        /***
        pulseSource.time = lieDetectorPlaybackProgress;
        pulsePlaybackElapsedTime = pulseSource.time;
        pulseSource.pitch = pulsePitch;
        pulseSource.volume = pulseVolume;
        
        respirationSource.time = lieDetectorPlaybackProgress;
        respirationPlaybackElapsedTime = respirationSource.time;
        respirationSource.pitch = respirationPitch;
        respirationSource.volume = respirationVolume;
        ***/
        lieDetector.clip = pulseClip;
        pulsePlaybackElapsedTime = pulseSource.time;
        pulseSource.Play();
        p_lieDetectorPlaybackProgress = 0;





    }

    public void StartRespiration()
    {
        lieDetector.clip = respirationClip;
        respirationPlaybackElapsedTime = respirationSource.time;
        lieDetector.Play();
        r_lieDetectorPlaybackProgress = 0;
    }

    public void StopAll()
    {
        playingAmbientSounds = false;
        playingAlertSounds = false;
        lieDetector.Stop();
        pulseSource.Stop();
        respirationSource.Stop();
        gsrSource.Stop();
        truthSource.Stop();
        lieSource.Stop();
        alertSource.Stop();
        ambientSource.Stop();
        lieDetectorPlaybackProgress = 0;
    }

    public void StopAlert()
    {
        playingAmbientSounds = true;
        playingAlertSounds = false;
        gsrSource.Stop();
        truthSource.Stop();
        lieSource.Stop();
        alertSource.Stop();
        lieDetectorPlaybackProgress = 0;
    }

    /// <summary>
    /// Alarms are either activated by their corresponding g.o. button on the Unity interface, or
    /// they are idling silently. Pitch and volume have pre-defined values upon initialization. 
    /// Pitch will increase or decrease according to confidence levels, adjusted on corresponding sliders.
    /// </summary> 

    /// <summary> 
    /// Alert that notifies that a question has been answered truthfully.
    /// This response should be triggered by a button and should also have a slider that tweaks the sound based 
    /// on the degree of confidence the system has that it is a true answer.
    /// </summary>
    public void PlayTruthAlert()
    {
        /// <summary>
        /// Also triggers autoConfigExcitedTrue() function which changes the ambient settings to 
        /// reflect a moderate physiological response,  which have pre-defined,  
        /// identifying pitch and volume threshold values corresponding to pulse 
        /// and respiration [2 pre-defined pitch values(1 for pulse and 1 for respiration)... 
        /// ...+ 2 pre-defined volume values, attached to the lieDetectorSource AudioSource in Unity].
        /// </summary>
        playingAlertSounds = true;
        truthConfidence = truthSource.pitch;
        truthPlaybackElapsedTime = truthSource.time;
        truthSource.Play();
        lieDetectorPlaybackProgress = 0;
        autoConfigExcitedTrue();
    }

    /// <summary>
    /// INSTRUCTIONS: "Alert that notifies that a rapid change in galvanic skin response (GSR) has taken place. 
    /// This typically happens at the onset of an emotional response. 
    /// This response should be triggered by a button and should also have a slider that tweaks the sound based 
    /// on the severity of the emotional response."
    /// </summary>
    public void AlertForGSR()
    {
        /// <summary>
        /// Also triggers autoConfigModerate() function which changes the ambient settings to 
        /// reflect a moderate physiological response,  which have pre-defined,  
        /// identifying pitch and volume threshold values corresponding to pulse 
        /// and respiration [2 pre-defined pitch values(1 for pulse and 1 for respiration)... 
        /// ...+ 2 pre-defined volume values, attached to the lieDetectorSource AudioSource in Unity].
        /// </summary>
        if (alertGSR.isActiveAndEnabled)
        playingAlertSounds = true;
        gsrConfidence = gsrSource.pitch;
        gsrPlaybackElapsedTime = gsrSource.time;
        gsrSource.Play();
        lieDetectorPlaybackProgress = 0;
        
    }

    /// <summary>
    /// Instructions: "Alert that notifies that a question has been answered falsely (a lie). This response should be triggered 
    /// by a button and should also have a slider that tweaks the sound based on the degree of confidence the 
    /// system has that it is a false answer."
    /// </summary>
    public void PlayLieAlert()
    {
        /// <summary>
        /// Also triggers autoConfigExcitedFalse() function which changes the ambient settings to 
        /// reflect an "excited-while-lying" physiological response, which have pre-defined,  
        /// identifying pitch and volume thresholds corresponding to pulse 
        /// and respiration [2 pre-defined pitch values(1 for pulse and 1 for respiration)... 
        /// ...+ 2 pre-defined volume values, attached to the lieDetectorSource AudioSource in Unity].
        /// </summary>
        playingAlertSounds = true;
        lieConfidence = lieSource.pitch;
        liePlaybackElapsedTime = lieSource.time;
        lieSource.Play();
        lieDetectorPlaybackProgress = 0;
      
    }

    /// <summary>
    /// The function below stops alerts when called in void Update() by a corresponding button press. 
    /// The following 4 functions automatically configure pre-defined pitch and volume 
    /// settings for corresponding pulse and respiration rates, assisting the user/'person testing' with 
    /// identification of either a "calm," "excitedTrue," excitedFalse," physiological response in the
    /// 'person being tested' using a sonification of the data.
    /// </summary>
    public void autoConfigCalm() // Pre-set for autoconfig of playback sound indicating "The subject is calm."
    {
        
            StopAll();
            respirationPitch = calmRespirationPitch;
            respirationSource.pitch = respirationPitch;
            respirationVolume = calmRespirationVolume;
            respirationSource.volume = respirationVolume;
            respirationPlaybackElapsedTime = respirationSource.time;
            pulsePitch = calmPulsePitch;
            pulseSource.pitch = pulsePitch;
            pulseVolume = calmPulseVolume;
            pulseSource.volume = pulseVolume;
            pulsePlaybackElapsedTime = pulseSource.time;
            pulseSource.Play();
            respirationSource.Play();
            lieDetectorPlaybackProgress = 0;
        
    }

    public void autoConfigExcitedTrue()
    {
       
            StopAll();
            respirationPitch = excitedTrueRespirationPitch;
            respirationSource.pitch = respirationPitch;
            respirationVolume = excitedTrueRespirationVolume;
            respirationSource.volume = respirationVolume;
            respirationPlaybackElapsedTime = respirationSource.time;
            pulsePitch = excitedTruePulsePitch;
            pulseSource.pitch = pulsePitch;
            pulseVolume = excitedTruePulseVolume;
            pulseSource.volume = pulseVolume;
            pulsePlaybackElapsedTime = pulseSource.time;
            pulseSource.Play();
            respirationSource.Play();
            lieDetectorPlaybackProgress = 0;
       
    }

    public void autoConfigExcitedFalse()
    {


        StopAll();
        respirationPitch = excitedFalseRespirationPitch;
        respirationSource.pitch = respirationPitch;
        respirationVolume = excitedFalseRespirationVolume;
        respirationSource.volume = respirationVolume;
        respirationPlaybackElapsedTime = respirationSource.time;
        pulsePitch = excitedFalsePulsePitch;
        pulseSource.pitch = pulsePitch;
        pulseVolume = excitedFalsePulseVolume;
        pulseSource.volume = pulseVolume;
        pulsePlaybackElapsedTime = pulseSource.time;
        pulseSource.Play();
        respirationSource.Play();
        lieDetectorPlaybackProgress = 0;
    }

    public void autoConfigModerate()
    {
       
        
            StopAll();
            respirationPitch = moderateRespirationPitch;
            respirationSource.pitch = respirationPitch;
            respirationVolume = moderateRespirationVolume;
            respirationSource.volume = respirationVolume;
            respirationPlaybackElapsedTime = respirationSource.time;
            pulsePitch = moderatePulsePitch;
            pulseSource.pitch = pulsePitch;
            pulseVolume = moderatePulseVolume;
            pulseSource.volume = pulseVolume;
            pulsePlaybackElapsedTime = pulseSource.time;
            pulseSource.Play();
            respirationSource.Play();
            lieDetectorPlaybackProgress = 0;
       
    }
} 
