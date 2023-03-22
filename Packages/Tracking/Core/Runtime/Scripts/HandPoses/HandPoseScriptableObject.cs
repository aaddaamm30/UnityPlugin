using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Leap.Unity
{
    public class NamedListAttribute : PropertyAttribute
    {
        public readonly string[] names;
        public NamedListAttribute(string[] names) { this.names = names; }
    }

    [CustomPropertyDrawer(typeof(NamedListAttribute))]
    public class NamedArrayDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            try
            {
                int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
                EditorGUI.PropertyField(rect, property, new GUIContent(((NamedListAttribute)attribute).names[pos]), true);
            }
            catch
            {
                EditorGUI.PropertyField(rect, property, label, true);
            }
        }
    }

    [CreateAssetMenu(fileName = "HandPose", menuName = "ScriptableObjects/HandPose")]
    public class HandPoseScriptableObject : ScriptableObject
    {
        [System.Serializable]
        public struct FingerJointThresholds
        {
            public Vector2[] jointThresholds;
        }

        [HideInInspector]
        public bool DetectThumb = true;
        [HideInInspector]
        public bool DetectIndex = true;
        [HideInInspector]
        public bool DetectMiddle = true;
        [HideInInspector]
        public bool DetectRing = true;
        [HideInInspector]
        public bool DetectPinky = true;

        private List<int> fingerIndexesToCheck = new List<int>();
        
        public List<int> GetFingerIndexesToCheck()
        {
            ApplyFingersToUse();
            return fingerIndexesToCheck;
        }

        #region Finger Thresholds

        [HideInInspector]
        public float globalRotation = 15;

        [HideInInspector]
        public FingerJointThresholds[] fingerJointRotationThresholds = new FingerJointThresholds[5];

        #endregion

        [SerializeField, Attributes.Disable]
        private Hand serializedHand;
        public Hand GetSerializedHand()
        {
            return serializedHand;
        }

        [SerializeField, Attributes.Disable]
        private Hand mirroredHand;
        public Hand GetMirroredHand()
        {
            return mirroredHand;
        }

        /// <summary>
        /// The distance a bone must move away from being detected before the pose is no longer enabled.
        /// This means that users cannot hover just on the edge of a detection and cause it to send rapid detections while straying still.
        /// E.g. Detection threshold is 15 degrees, so when the user gets within 15 degrees, detection will occur.
        /// Hysteresis threshold is 5 so the user need to move 20 degrees from the pose before the detection will drop.
        /// </summary>
        [SerializeField, Tooltip("When a joint is within the rotation threshold, how many degrees away from the original threshold " +
            "must the user move to stop the detection of each joint for the pose. This helps to avoid flickering detection when on the boundaries of thresholds")]

        [HideInInspector]
        public float hysteresisThreshold = 5;

        public float GetHysteresisThreshold()
        {
            return hysteresisThreshold;
        }

        public void SaveHandPose(Hand handToSerialise)
        {
            serializedHand = new Hand();
            serializedHand = serializedHand.CopyFrom(handToSerialise);
            MirrorHand(handToSerialise);
            SetAllBoneThresholds(globalRotation, true);
        }

        void MirrorHand(Hand handSource)
        {
            mirroredHand = new Hand();
            mirroredHand = mirroredHand.CopyFrom(handSource);
            LeapTransform leapTransform = new LeapTransform(Vector3.zero, Quaternion.Euler(Vector3.zero));
            leapTransform.MirrorX();
            mirroredHand.Transform(leapTransform);
            mirroredHand.IsLeft = !mirroredHand.IsLeft;
            return;
        }

        public Vector2 GetBoneRotationthreshold(int fingerNum, int boneNum)
        {
            if(fingerJointRotationThresholds.Length > 0)
            {
                return fingerJointRotationThresholds[fingerNum].jointThresholds[boneNum];
            }
            else
            {
                return Vector2.zero;
            }
        }

        private void OnValidate()
        {
            MirrorHand(serializedHand);
            ApplyFingersToUse();
        }

        private void ApplyFingersToUse()
        {
            fingerIndexesToCheck.Clear();
            if (DetectThumb) { fingerIndexesToCheck.Add(0); }
            if (DetectIndex) { fingerIndexesToCheck.Add(1); }
            if (DetectMiddle) { fingerIndexesToCheck.Add(2); }
            if (DetectRing) { fingerIndexesToCheck.Add(3); }
            if (DetectPinky) { fingerIndexesToCheck.Add(4); }
        }

        public void SetAllBoneThresholds(float threshold, bool forceAll = false)
        {
            Vector2 newRotation = new Vector2(threshold, threshold);

            for(int fingerIndex = 0; fingerIndex < fingerJointRotationThresholds.Length; fingerIndex++)
            {
                if (forceAll)
                {
                    fingerJointRotationThresholds[fingerIndex].jointThresholds = new Vector2[] { newRotation, newRotation, newRotation };
                }
                else
                {
                    for(int jointIndex = 0; jointIndex < fingerJointRotationThresholds[fingerIndex].jointThresholds.Length; jointIndex++)
                    {
                        if(fingerJointRotationThresholds[fingerIndex].jointThresholds[jointIndex].x == globalRotation)
                        {
                            fingerJointRotationThresholds[fingerIndex].jointThresholds[jointIndex].x = threshold;
                        }

                        if (fingerJointRotationThresholds[fingerIndex].jointThresholds[jointIndex].y == globalRotation)
                        {
                            fingerJointRotationThresholds[fingerIndex].jointThresholds[jointIndex].y = threshold;
                        }
                    }
                }
            }

            globalRotation = threshold;
        }
    }
}