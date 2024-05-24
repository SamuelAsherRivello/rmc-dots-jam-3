using RMC.DOTS.Systems.Audio;

namespace RMC.DOTS.Systems.Destroyable
{
    //TODO: Add this to AudioComponent as static methods
    public static class AudioComponent2
    {
        public static AudioComponent FromAudioClipName (string audioClipName) 
        {
            return new AudioComponent(audioClipName);
        }
    }
}