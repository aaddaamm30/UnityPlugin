/******************************************************************************
 * Copyright (C) Ultraleap, Inc. 2011-2021.                                   *
 *                                                                            *
 * Use subject to the terms of the Apache License 2.0 available at            *
 * http://www.apache.org/licenses/LICENSE-2.0, or another agreement           *
 * between Ultraleap and you, your company or other organization.             *
 ******************************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace Leap.Unity.HandsModule
{

    /// <summary>
    /// A data structure to define all the fingers in a hand, the wrist and elbow
    /// </summary>
    [System.Serializable]
    public class BoundHand
    {
        public BoundFinger[] fingers = new BoundFinger[5];
        public BoundBone wrist = new BoundBone();
        public BoundBone elbow = new BoundBone();
        public float baseScale;
        public Vector3 startScale;
    }

    /// <summary>
    /// A data structure to define a finger
    /// </summary>
    [System.Serializable]
    public class BoundFinger
    {
        public BoundBone[] boundBones = new BoundBone[4];
    }

    /// <summary>
    /// A data structure to define starting position, an offset and the Transform reference found in the scene
    /// </summary>
    [System.Serializable]
    public class BoundBone
    {
        public Transform boundTransform;
        public TransformStore startTransform = new TransformStore();
        public TransformStore offset = new TransformStore();
    }

    /// <summary>
    /// A data structure to store a transforms position and rotation
    /// </summary>
    [System.Serializable]
    public class TransformStore
    {
        public Vector3 position = Vector3.zero;
        public Vector3 rotation = Vector3.zero;
    }

    /// <summary>
    /// A data structure to store information about a transform and a Gameobject
    /// </summary>
    [System.Serializable]
    public class SerializedTransform
    {
        public TransformStore transform;
        public GameObject reference;
    }

    /// <summary>
    /// ENUM types for bones of the hand the hand binder can attach to
    /// </summary>
    public enum BoundTypes
    {
        THUMB_METACARPAL,
        THUMB_PROXIMAL,
        THUMB_INTERMEDIATE,
        THUMB_DISTAL,

        INDEX_METACARPAL,
        INDEX_PROXIMAL,
        INDEX_INTERMEDIATE,
        INDEX_DISTAL,

        MIDDLE_METACARPAL,
        MIDDLE_PROXIMAL,
        MIDDLE_INTERMEDIATE,
        MIDDLE_DISTAL,

        RING_METACARPAL,
        RING_PROXIMAL,
        RING_INTERMEDIATE,
        RING_DISTAL,

        PINKY_METACARPAL,
        PINKY_PROXIMAL,
        PINKY_INTERMEDIATE,
        PINKY_DISTAL,

        WRIST,
        ELBOW,
    }

    public class HandBinderUtilities
    {

        /// <summary>
        /// The mapping that allows a BoundType and Leap FingerType/BoneType to map back to the HandBinders Data structure
        /// </summary>
        public readonly static Dictionary<BoundTypes, (Finger.FingerType, Bone.BoneType)> boundTypeMapping = new Dictionary<BoundTypes, (Finger.FingerType, Bone.BoneType)>
            {
            {BoundTypes.THUMB_METACARPAL, (Finger.FingerType.TYPE_THUMB, Bone.BoneType.TYPE_METACARPAL)},
            {BoundTypes.THUMB_PROXIMAL, (Finger.FingerType.TYPE_THUMB, Bone.BoneType.TYPE_PROXIMAL)},
            {BoundTypes.THUMB_INTERMEDIATE, (Finger.FingerType.TYPE_THUMB, Bone.BoneType.TYPE_INTERMEDIATE)},
            {BoundTypes.THUMB_DISTAL, (Finger.FingerType.TYPE_THUMB, Bone.BoneType.TYPE_DISTAL)},
            {BoundTypes.INDEX_METACARPAL, (Finger.FingerType.TYPE_INDEX, Bone.BoneType.TYPE_METACARPAL)},
            {BoundTypes.INDEX_PROXIMAL, (Finger.FingerType.TYPE_INDEX, Bone.BoneType.TYPE_PROXIMAL)},
            {BoundTypes.INDEX_INTERMEDIATE, (Finger.FingerType.TYPE_INDEX, Bone.BoneType.TYPE_INTERMEDIATE)},
            {BoundTypes.INDEX_DISTAL, (Finger.FingerType.TYPE_INDEX, Bone.BoneType.TYPE_DISTAL)},
            {BoundTypes.MIDDLE_METACARPAL, (Finger.FingerType.TYPE_MIDDLE, Bone.BoneType.TYPE_METACARPAL)},
            {BoundTypes.MIDDLE_PROXIMAL, (Finger.FingerType.TYPE_MIDDLE, Bone.BoneType.TYPE_PROXIMAL)},
            {BoundTypes.MIDDLE_INTERMEDIATE, (Finger.FingerType.TYPE_MIDDLE, Bone.BoneType.TYPE_INTERMEDIATE)},
            {BoundTypes.MIDDLE_DISTAL, (Finger.FingerType.TYPE_MIDDLE, Bone.BoneType.TYPE_DISTAL)},
            {BoundTypes.RING_METACARPAL, (Finger.FingerType.TYPE_RING, Bone.BoneType.TYPE_METACARPAL)},
            {BoundTypes.RING_PROXIMAL, (Finger.FingerType.TYPE_RING, Bone.BoneType.TYPE_PROXIMAL)},
            {BoundTypes.RING_INTERMEDIATE, (Finger.FingerType.TYPE_RING, Bone.BoneType.TYPE_INTERMEDIATE)},
            {BoundTypes.RING_DISTAL, (Finger.FingerType.TYPE_RING, Bone.BoneType.TYPE_DISTAL)},
            {BoundTypes.PINKY_METACARPAL, (Finger.FingerType.TYPE_PINKY, Bone.BoneType.TYPE_METACARPAL)},
            {BoundTypes.PINKY_PROXIMAL, (Finger.FingerType.TYPE_PINKY, Bone.BoneType.TYPE_PROXIMAL)},
            {BoundTypes.PINKY_INTERMEDIATE, (Finger.FingerType.TYPE_PINKY, Bone.BoneType.TYPE_INTERMEDIATE)},
            {BoundTypes.PINKY_DISTAL, (Finger.FingerType.TYPE_PINKY, Bone.BoneType.TYPE_DISTAL)},
        };
    }
}