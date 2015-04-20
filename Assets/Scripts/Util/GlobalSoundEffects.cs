using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class GlobalSoundEffects : MonoBehaviour {
	public static GlobalSoundEffects main;

	public AudioMixer mixer;
	public static AudioMixerGroup[] mixerGroups;

	private Dictionary<string, AudioClip> soundEffectsDict;
	private AudioSource bgmSource;

	private float bgmFadeTarget;
	private float bgmFadeVelocity;
	private string nextBGM;
	private string onCompleteBGMName;

	private List<string> playlist;

	void Awake(){
		main = this;
		soundEffectsDict = new Dictionary<string, AudioClip>();
		AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
		for(int i = 0; i < clips.Length; i++){
			string name = clips[i].name;
			int counter = 1;
			while (soundEffectsDict.ContainsKey(name)){
				name = clips[i].name + "(" + counter++ + ")";
			}
			soundEffectsDict.Add(name, clips[i]);
		}
		mixerGroups = mixer.FindMatchingGroups("");
	}

	public AudioClip GetClip(string name){
		AudioClip clip;
		soundEffectsDict.TryGetValue(name, out clip);
		if (clip == null) {Debug.LogError("No clip named "+name+" found!");return null;}
		return clip;
	}

	public bool GetClip(string name, out AudioClip clip){
		soundEffectsDict.TryGetValue(name, out clip);
		if (clip == null) {Debug.LogError("No clip named "+name+" found!");return false;}
		return true;
	}

	public AudioSource PlayClipAtPoint(string name){ 
		return PlayClipAtPoint(name, Vector3.zero);
	}

	public AudioSource PlayClipAtPoint(string name, Vector3 position, string group, float volume = .2f){
		AudioMixerGroup g = GetGroup(group);
		AudioSource s = PlayClipAtPoint(name, position, g, volume);
		return s;
	}

	public AudioSource PlayClipAtPoint(string name, Vector3 position, AudioMixerGroup group, float volume = .2f){
		AudioSource s = PlayClipAtPoint(name, position, volume);
		s.outputAudioMixerGroup = group;
		return s;
	}

	public AudioSource PlayClipAtPoint(string name, Vector3 position, float min, float max, float volume = .2f){
		AudioSource s = PlayClipAtPoint(name, position, volume);
		s.pitch = Random.Range(min, max);
		return s;
	}

	public AudioSource PlayClipAtPoint(string name, Vector3 position, float volume = .2f){
		AudioClip clip;
		GetClip(name, out clip);
		return this.PlayClipAtPoint(clip, position, volume);
	}

	public float SetVolumeMMF(float volume){
		AudioListener.volume = volume/10f;
		return volume;
	}

	public AudioSource PlayClipAtPoint(AudioClip clip, Vector3 position, float pitch = 1f, float volume = .2f){
		GameObject g = new GameObject("One Shot Audio");
		g.transform.position = position;
		if (g == null) return null;
		AudioSource audio = g.AddComponent<AudioSource>();
		audio.clip = clip;
		audio.volume = volume;
		audio.pitch = pitch;
		audio.rolloffMode = AudioRolloffMode.Logarithmic;
		audio.Play();
		audio.spatialBlend = 1f;
		Destroy(g, clip.length * 2f);
		return audio;
	}

	public static AudioMixerGroup GetGroup(string name){
		AudioMixerGroup g = GlobalSoundEffects.main.mixer.FindMatchingGroups(name)[0];
		return g;
	}

}
