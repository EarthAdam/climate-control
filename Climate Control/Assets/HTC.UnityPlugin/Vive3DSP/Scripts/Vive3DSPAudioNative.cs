
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace HTC.UnityPlugin.Vive3DSP
{
    public enum HeadsetType
    {
        Generic = 0,
        VIVEPro
    }

    public struct VIVE_3DSP_ROOM_PROPERTY
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 size;
        public RoomReverbPreset preset;
        public RoomPlateMaterial material_left;
        public RoomPlateMaterial material_right;
        public RoomPlateMaterial material_back;
        public RoomPlateMaterial material_front;
        public RoomPlateMaterial material_ceiling;
        public RoomPlateMaterial material_floor;
        public float reflection_rate_left;
        public float reflection_rate_right;
        public float reflection_rate_back;
        public float reflection_rate_front;
        public float reflection_rate_ceiling;
        public float reflection_rate_floor;
        public float gain;
        public float dry_level;
        public float reflection_level;
        public float reverb_level;
    }

    public struct VIVE_3DSP_OCCLUSION_PROPERTY
    {
        public Vector3 position;
        public Vector3 size;
        public Quaternion rotation;
        public OccMaterial material;
        public float rhf;
        public float lfratio;
        public float density;
        public float radius;
        public OccEngineMode mode;
        public OccComputeMode computeMode;
    }

    public class Vive3DSPAudioNative
    {
        private const string pluginName = "audioplugin_vive3dsp";

        public Vive3DSPAudioNative() { }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_create_engine_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_create_engine_plugin();
        public Int32 Create3DSPEngine()
        {
            try
            {
                return vive_3dsp_create_engine_plugin();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_destroy_engine_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_destroy_engine_plugin();
        public Int32 Destroy3DSPEngine()
        {
            try
            {
                return vive_3dsp_destroy_engine_plugin();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }
        
        [DllImport(pluginName, EntryPoint = "vive_3dsp_listener_set_gain_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_listener_set_gain_plugin(float gain);
        public Int32 Set3DSPListenerGain(float gain)
        {
            try
            {
                return vive_3dsp_listener_set_gain_plugin(gain);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_listener_set_headset_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_listener_set_headset_plugin(HeadsetType mode);
        public Int32 Set3DSPListenerHeadset(HeadsetType mode)
        {
            try
            {
                return vive_3dsp_listener_set_headset_plugin(mode);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }
        
        [DllImport(pluginName, EntryPoint = "vive_3dsp_room_create_object_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr vive_3dsp_room_create_object_plugin();
        public IntPtr Create3DSPRoomObject()
        {
            try
            {
                return vive_3dsp_room_create_object_plugin();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return IntPtr.Zero;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_room_destroy_object_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_room_destroy_object_plugin(IntPtr room);
        public Int32 Destroy3DSPRoomObject(IntPtr room)
        {
            try
            {
                return vive_3dsp_room_destroy_object_plugin(room);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_room_enable_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_room_enable_plugin(IntPtr room, bool enable);
        public Int32 Enable3DSPRoom(IntPtr room, bool enable)
        {
            try
            {
                return vive_3dsp_room_enable_plugin(room, enable);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_room_get_bgaudio_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr vive_3dsp_room_get_bgaudio_plugin(int audio_id);
        public IntPtr Get3DSPBgaudio(int audio_id)
        {
            try
            {
                return vive_3dsp_room_get_bgaudio_plugin(audio_id);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return IntPtr.Zero;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_room_free_bgaudio_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_room_free_bgaudio_plugin(IntPtr pBGAudio);
        public Int32 Destroy3DSPBgaudio(IntPtr pBGAudio)
        {
            try
            {
                return vive_3dsp_room_free_bgaudio_plugin(pBGAudio);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }


        [DllImport(pluginName, EntryPoint = "vive_3dsp_room_set_property_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_room_set_property_plugin(IntPtr room, VIVE_3DSP_ROOM_PROPERTY prop);
        public Int32 Set3DSPRoomProperty(IntPtr room, VIVE_3DSP_ROOM_PROPERTY prop)
        {
            try
            {
                return vive_3dsp_room_set_property_plugin(room, prop);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_occlusion_create_object_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr vive_3dsp_occlusion_create_object_plugin();
        public IntPtr Create3DSPOcclusionObject()
        {
            try
            {
                return vive_3dsp_occlusion_create_object_plugin();
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return IntPtr.Zero;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_occlusion_destroy_object_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_occlusion_destroy_object_plugin(IntPtr occ);
        public Int32 Destroy3DSPOcclusionObject(IntPtr occ)
        {
            try
            {
                return vive_3dsp_occlusion_destroy_object_plugin(occ);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_occlusion_enable_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_occlusion_enable_plugin(IntPtr occ, bool enable);
        public Int32 Enable3DSPOcclusion(IntPtr occ, bool enable)
        {
            try
            {
                return vive_3dsp_occlusion_enable_plugin(occ, enable);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_occlusion_set_channel_num_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_occlusion_set_channel_num_plugin(int chnNum);
        public Int32 Set3DSPOcclusionChnNumber(int chnNum)
        {
            try
            {
                return vive_3dsp_occlusion_set_channel_num_plugin(chnNum);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_occlusion_set_property_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_occlusion_set_property_plugin(IntPtr occlusion, VIVE_3DSP_OCCLUSION_PROPERTY prop);
        public Int32 Set3DSPOcclusionProperty(IntPtr occlusion, VIVE_3DSP_OCCLUSION_PROPERTY prop)
        {
            try
            {
                return vive_3dsp_occlusion_set_property_plugin(occlusion, prop);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_get_version_plugin", CallingConvention = CallingConvention.Cdecl)]
        private static extern int vive_3dsp_get_version_plugin(IntPtr ver);
        public Int32 Get3DSPNativeVersion(IntPtr ver)
        {
            try
            {
                return vive_3dsp_get_version_plugin(ver);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }

        [DllImport(pluginName, EntryPoint = "vive_3dsp_parametric_eq_get_coefs_plugin", CallingConvention = CallingConvention.Cdecl)]
        public static extern int vive_3dsp_parametric_eq_get_coefs_plugin(IntPtr frequency, IntPtr gain, IntPtr Q, IntPtr coefs);
        public Int32 Get3DSPParametricEQCoeffs(IntPtr frequency, IntPtr gain, IntPtr Q, IntPtr coefs)
        {
            try
            {
                return vive_3dsp_parametric_eq_get_coefs_plugin(frequency, gain, Q, coefs);
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
                return -1;
            }
        }
    }
}
