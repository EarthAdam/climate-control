//====================== Copyright 2016-2018, HTC.Corporation. All rights reserved. ======================
/**
*   release version:    1.0.0.0
*   script version:     1.0.0.0
*/

using UnityEngine;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace HTC.UnityPlugin.Vive3DSP
{
    public enum OccComputeMode
    {
        VeryLight = 0,
        Simple,
        Normal,
        Detailed
    }

    public enum OccEngineMode
    {
        Sphere = 0,
        Box
    }

    public enum OccRaycastMode
    {
        MonoOcclusion = 0,                      /**< Mono raycast mode. The ray is casted from one audio source to the listener. */
        BinauralOcclusion                       /**< Binaural mode. Two rays are casted from one audio source to the listener's ear. */
    }

    public enum ChannelType
    {
        Mono = 0,                               /**< A single speaker, typically in front of the user. */
        Stereo
    }

    public struct Vive3DSPQuaternion
    {
        public float x;                         /**< The x-coordinate of the vector part. */
        public float y;                         /**< The y-coordinate of the vector part. */
        public float z;                         /**< The z-coordinate of the vector part. */
        public float w;
    }

    public enum OccMaterial
    {
        Curtain = 0,
        ThinDoor,
        WoodWall,
        Window,
        HardwoodDoor,
        SeparateWindow,
        HollowCinderConcreteWall,
        SingleLeafBrickWall,
        MetalDoor,
        StoneWall,
        UserDefine
    }

    public enum RoomPlane
    {
        Floor = 0,
        Ceiling,
        LeftWall,
        RightWall,
        FrontWall,
        BackWall
    }

    public enum RoomPlateMaterial
    {
        None = 0,
        Concrete,
        Carpet,
        Wood,
        Glass,
        CoarseConcrete,
        Curtain,
        FiberGlass,
        Foam,
        Sheetrock,
        Plywood,
        Plaster,
        Brick,
        UserDefine
    }

    public enum RoomReverbPreset
    {
        Generic = 0,
        Bathroom,
        Livingroom,
        Church,
        Arena,
        UserDefine
    }
    public enum ReverbMode
    {
        Mono = 0,
        Binaural
    }
    public enum BinauralEngine
    {
        SpeedUp = 0,
        Normal
    }

    [System.Obsolete("This will be deprecated. Please use SoundDecayMode instead.")]
    public enum Ambisonic3dDistanceMode
    {
        RealWorldDecay = 0,
        PointSourceDecay,
        LineSourceDecay,
        LinearDecay,
        NoDecay,
    }

    public enum SoundDecayMode
    {
        RealWorldDecay = 0,
        PointSourceDecay,
        LineSourceDecay,
        LinearDecay,
        NoDecay,
    }

    public enum RoomBackgroundAudioType
    {
        None = 0,
        BigRoom,
        SmallRoom,
        AirConditioner,
        Refrigerator,
        PinkNoise,
        BrownNoise,
        WhiteNoise,
        UserDefine,
    }

    public enum RaycastQuality
    {
        High = 0,
        Median,
        Low
    }

    public static class Vive3DSPAudio
    {
        private static float MaxRayBiasAngle = 30;
        private static float MinRayBiasAngle = 10f;
        private static int RayEmitLayer = 2;
        private static int RayDisperseOrder = 10;
        private static float RayDisperseAngle = 360f / RayDisperseOrder;
        private static float totalRays = RayDisperseOrder * RayEmitLayer + 1;
        public static OccRaycastMode OcclusionMode;
        private static List<Vive3DSPAudioSource> srcList = new List<Vive3DSPAudioSource>();
        private static List<Vive3DSPAudioRaycastOcclusion> occList = new List<Vive3DSPAudioRaycastOcclusion>();
        private static Vive3DSPAudioListener MainListener = null;
        public static Vive3DSPAudioNative native3dsp = new Vive3DSPAudioNative();

        public struct Vive3DSPVersion
        {
            public uint major;
            public uint minor;
            public uint build;
            public uint revision;
        }
        
        // Source
        public static void CreateSource(Vive3DSPAudioSource srcComponent)
        {
            srcList.Add(srcComponent);
        }
        public static void DestroySource(Vive3DSPAudioSource source)
        {
            srcList.Remove(source);
        }
        
        // Listener
        public static void CreateAudioListener(Vive3DSPAudioListener listener)
        {
            MainListener = listener;
            native3dsp.Create3DSPEngine();
            native3dsp.Set3DSPListenerHeadset(listener.headsetType);
        }
        public static void DestroyAudioListener()
        {
            native3dsp.Destroy3DSPEngine();
        }

        public static void UpdateAudioListener()
        {
            if (MainListener == null)
            {
                return;
            }

            if (MainListener.occlusionMode == OccRaycastMode.MonoOcclusion)
            {
                native3dsp.Set3DSPOcclusionChnNumber(1);
                OcclusionMode = OccRaycastMode.MonoOcclusion;
            }
            else
            {
                native3dsp.Set3DSPOcclusionChnNumber(2);
                OcclusionMode = OccRaycastMode.BinauralOcclusion;
            }

            native3dsp.Set3DSPListenerGain(ConvertAmplitudeFromDb(MainListener.globalGain));
        }
        public static void UpdateOcclusionCoverRatio()
        {
            float ratio = 0.0f;

            if (MainListener == null)
            {
                NoListenerWarning();
                return;
            }

            foreach (var occ in occList)
            {
                foreach (var src in srcList)
                {
                    if (src.isVirtual)
                        continue;

                    var StoL_direction = MainListener.transform.position - src.transform.position;

                    if (OcclusionMode == OccRaycastMode.MonoOcclusion)
                    {
                        if (occ.occlusionEngine == (OccEngineMode)2)
                        {
                            MaxRayBiasAngle = 30;
                            if (occ.RayNumber < 16)
                            {
                                MinRayBiasAngle = 30f;
                                RayEmitLayer = 1;
                                RayDisperseOrder = occ.RayNumber - 1;
                                totalRays = occ.RayNumber;
                            }
                            else
                            {
                                MinRayBiasAngle = 10f;
                                RayEmitLayer = 2;
                                RayDisperseOrder = (int)((occ.RayNumber - 1) / 2);
                                totalRays = RayDisperseOrder * RayEmitLayer + 1;
                            }
                            RayDisperseAngle = 360f / RayDisperseOrder;

                            ratio = getRaycastOcclusionRatio(src.transform.position, StoL_direction, occ);
                            src.audioSource.SetSpatializerFloat((int)(Vive3DSPAudioSource.EffectData.MonoCoverOcclusion),
                            BitConverter.ToSingle(BitConverter.GetBytes(occ.OcclusionObject.ToInt32()), 0));
                            src.audioSource.SetSpatializerFloat((int)(Vive3DSPAudioSource.EffectData.MonoCoverRatio), ratio);
                            //Debug.Log("[Mono] ratio = " + ratio);
                        }
                    }
                    else // binaural mode
                    {
                        var direct_head_to_right_ear = 0.1f * MainListener.transform.right / MainListener.transform.right.magnitude;
                        var right_ear_position = MainListener.Position + direct_head_to_right_ear;
                        var left_ear_position = MainListener.Position - direct_head_to_right_ear;

                        var direct_rightear_to_src = src.Position - right_ear_position;
                        var direct_leftear_to_src = src.Position - left_ear_position;

                        float ratio1, ratio2;

                        if (occ.occlusionEngine == (OccEngineMode)2)
                        {
                            MaxRayBiasAngle = 30;
                            if (occ.RayNumber < 16)
                            {
                                MinRayBiasAngle = 30f;
                                RayEmitLayer = 1;
                                RayDisperseOrder = occ.RayNumber - 1;
                                totalRays = occ.RayNumber;
                            }
                            else
                            {
                                MinRayBiasAngle = 10f;
                                RayEmitLayer = 2;
                                RayDisperseOrder = (int)((occ.RayNumber - 1) / 2);
                                totalRays = RayDisperseOrder * RayEmitLayer + 1;
                            }
                            RayDisperseAngle = 360f / RayDisperseOrder;

                            ratio1 = getRaycastOcclusionRatio(src.transform.position, -direct_rightear_to_src, occ);
                            ratio2 = getRaycastOcclusionRatio(src.transform.position, -direct_leftear_to_src, occ);

                            src.audioSource.SetSpatializerFloat((int)(Vive3DSPAudioSource.EffectData.StereoCoverOcclusion),
                            BitConverter.ToSingle(BitConverter.GetBytes(occ.OcclusionObject.ToInt32()), 0));
                            src.audioSource.SetSpatializerFloat((int)(Vive3DSPAudioSource.EffectData.StereoCoverRatioL), ratio2);
                            src.audioSource.SetSpatializerFloat((int)(Vive3DSPAudioSource.EffectData.StereoCoverRatioR), ratio1);
                            //Debug.Log("[Binaural] ratioL = " + ratio2 + ", ratioR = " + ratio1);
                        }
                    }
                }
            }
        }
        
        public static IntPtr CreateRaycastOcclusion(Vive3DSPAudioRaycastOcclusion occObj)
        {
            IntPtr occobj = IntPtr.Zero;

            occobj = native3dsp.Create3DSPOcclusionObject();
            if (occobj != IntPtr.Zero)
            {
                native3dsp.Set3DSPOcclusionProperty(occobj, occObj.OcclusionPorperty);
                occList.Add(occObj);
            }

            return occobj;
        }
        public static void DestroyRaycastOcclusion(Vive3DSPAudioRaycastOcclusion occObj)
        {
            if (occObj.OcclusionObject != IntPtr.Zero)
            {
                native3dsp.Destroy3DSPOcclusionObject(occObj.OcclusionObject);
            }
            occList.Remove(occObj);
            occObj.OcclusionObject = IntPtr.Zero;
        }
        public static IntPtr CreateGeometricOcclusion(Vive3DSPAudioGeometricOcclusion occObj)
        {
            IntPtr occobj = IntPtr.Zero;

            occobj = native3dsp.Create3DSPOcclusionObject();
            if (occobj != IntPtr.Zero)
            {
                native3dsp.Set3DSPOcclusionProperty(occobj, occObj.OcclusionPorperty);
            }

            return occobj;
        }
        public static void DestroyGeometricOcclusion(Vive3DSPAudioGeometricOcclusion occObj)
        {
            if (occObj.OcclusionObject != IntPtr.Zero)
            {
                native3dsp.Destroy3DSPOcclusionObject(occObj.OcclusionObject);
            }
            occObj.OcclusionObject = IntPtr.Zero;
        }
        public static void EnableOcclusion(IntPtr occ)
        {
            native3dsp.Enable3DSPOcclusion(occ, true);
        }
        public static void DisableOcclusion(IntPtr occ)
        {
            native3dsp.Enable3DSPOcclusion(occ, false);
        }
        public static void UpdateOcclusion(IntPtr occobj, bool OcclusionEffect, VIVE_3DSP_OCCLUSION_PROPERTY OcclusionPorperty)
        {
            if (occobj == IntPtr.Zero)
            {
                return;
            }
            native3dsp.Enable3DSPOcclusion(occobj, OcclusionEffect);
            native3dsp.Set3DSPOcclusionProperty(occobj, OcclusionPorperty);
        }
        private static float getRaycastOcclusionRatio(Vector3 srcPos, Vector3 direction, Vive3DSPAudioRaycastOcclusion occ)
        {
            float ratio = 0f;
            int ListenerColliderNumber = 0, SourceColliderNumber = 0;
            var dist = direction.magnitude;
            RaycastHit[] hit;
            hit = Physics.RaycastAll(srcPos, direction, dist);
            //Debug.DrawRay(srcPos, direction, Color.red);
            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].transform == occ.transform)
                {
                    SourceColliderNumber++;
                    ListenerColliderNumber++;
                }
            }
            if (occ.RayNumber == 1)
            {
                if (SourceColliderNumber == 0)
                    return 0.0f;
                else
                    return 1.0f;
            }
            var max_direct_forward_up = Quaternion.AngleAxis(MaxRayBiasAngle, Vector3.up) * direction;
            var max_direct_up = max_direct_forward_up - direction;
            var min_direct_forward_up = Quaternion.AngleAxis(MinRayBiasAngle, Vector3.up) * direction;
            var min_direct_up = min_direct_forward_up - direction;

            float maxmin_direct_up_mag_ratio = max_direct_up.magnitude / min_direct_up.magnitude;

            Vector3 rot_up, direct_up, direct_forward_up;
            for (int i = 0; i < RayEmitLayer; ++i)
            {
                if (i == 0)
                    direct_up = min_direct_up;
                else if (i == RayEmitLayer - 1)
                    direct_up = max_direct_up;
                else
                    direct_up = (i * (maxmin_direct_up_mag_ratio - 1) / RayEmitLayer + 1) * min_direct_up;
                
                for (int j = 0; j < RayDisperseOrder; ++j)
                {
                    // source part
                    rot_up = Quaternion.AngleAxis(RayDisperseAngle * j, direction) * direct_up;
                    direct_forward_up = direction + rot_up;
                    //Debug.DrawRay(srcPos, rot_up, Color.green);
                    //Debug.DrawRay(srcPos, direct_forward_up, Color.blue);
                    hit = Physics.RaycastAll(srcPos, direct_forward_up, direct_forward_up.magnitude);
                    for (int h = 0; h < hit.Length; h++)
                    {
                        if (hit[h].transform == occ.transform)
                        {
                            ListenerColliderNumber++;
                        }
                    }

                    // listener part
                    direct_forward_up = -direction + rot_up;
                    //Debug.DrawRay(MainListener.transform.position, rot_up, Color.green);
                    //Debug.DrawRay(MainListener.transform.position, direct_forward_up, Color.blue);
                    hit = Physics.RaycastAll(srcPos + direction, direct_forward_up, direct_forward_up.magnitude);
                    for (int h = 0; h < hit.Length; h++)
                    {
                        if (hit[h].transform == occ.transform)
                        {
                            SourceColliderNumber++;
                        }
                    }
                }
            }

            ratio = (float)Math.Max(SourceColliderNumber, ListenerColliderNumber) / totalRays;
            return ratio;
        }
        
        // Room
        public static void CreateRoom(Vive3DSPAudioRoom room)
        {
            room.RoomObject = native3dsp.Create3DSPRoomObject();
        }
        public static void DestroyRoom(Vive3DSPAudioRoom room)
        {
            native3dsp.Destroy3DSPRoomObject(room.RoomObject);
        }
        public static void UpdateRoom(Vive3DSPAudioRoom roomComponent)
        {
            if (roomComponent.RoomEffect)
            {
                native3dsp.Enable3DSPRoom(roomComponent.RoomObject, true);
            }
            else
            {
                native3dsp.Enable3DSPRoom(roomComponent.RoomObject, false);
            }
            native3dsp.Set3DSPRoomProperty(roomComponent.RoomObject, roomComponent.RoomPorperty);
        }

        public static void CheckIfListenerInRoom(Vive3DSPAudioRoom room)
        {
            if (MainListener == null)
            {
                return;
            }
            if (IsObjectInsideRoom(MainListener.transform.position, room))
            {
                if (room.isListenerInsideRoomUpdated == false)
                {
                    MainListener.CurrentRoom = room;
                    room.isListenerInsideRoomUpdated = true;
                    MainListener.RoomList.Add(room);
                }
            }
            else // the listener is outside the room
            {
                room.isListenerInsideRoomUpdated = false;
                MainListener.RoomList.Remove(room);
                if (MainListener.CurrentRoom == room)
                {
                    MainListener.CurrentRoom = null;
                }

                if (MainListener.RoomList.Count > 0)
                {
                    // Set another room
                    foreach (var anotherRoom in MainListener.RoomList)
                    {
                        if (MainListener.CurrentRoom == null)
                        {
                            anotherRoom.isListenerInsideRoomUpdated = true;
                            MainListener.CurrentRoom = anotherRoom;
                        }
                        break;
                    }
                }
            }
        }
        public static bool IsObjectInsideRoom(Vector3 object_pos, Vive3DSPAudioRoom room)
        {
            bool isInside = false;
            Bounds bounds = new Bounds(Vector3.zero, Vector3.zero); ;

            Vector3 relativePosition = object_pos - room.transform.position;
            Quaternion rotationInverse = Quaternion.Inverse(room.transform.rotation);

            bounds.size = Vector3.Scale(room.transform.lossyScale, room.Size);
            isInside = bounds.Contains(rotationInverse * relativePosition);

            return isInside;
        }

        public static float[] GetBGAudioData(int file_id)
        {
            float[] buffer = new float[48000];
            IntPtr bg_data = native3dsp.Get3DSPBgaudio(file_id);
            if (bg_data == IntPtr.Zero)
                return null;
            Marshal.Copy(bg_data, buffer, 0, buffer.Length);
            native3dsp.Destroy3DSPBgaudio(bg_data);
            return buffer;
        }
        public static float ConvertAmplitudeFromDb(float db)
        {
            return Mathf.Pow(10.0f, 0.05f * db);
        }
        private static void NoListenerWarning()
        {
            Debug.LogWarning("[Vive3DSP] Check if the audio listener added the component Vive3DSPAudioListener.");
        }
    }
}
