using Microsoft.Kinect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firefly
{
    public enum HandPosition { Unknown, Up, Center, Down, Left, Right, Backwards, Forwards }

    public class HandPositionChangedArgs
    {
        public HandPosition Position { get; set; }
    }

    public delegate void HandPositionChangedDelegate(object sender, HandPositionChangedArgs args);

    public static class GestureDetection
    {
        public static void FrameReady(Skeleton skeleton)
        {
            ProcessRightHand(skeleton);
            ProcessLeftHand(skeleton);
        }

        // Right Hand
        private static Dictionary<int, HandPosition> rightHandUpDownDictionary = new Dictionary<int, HandPosition>();
        private static Dictionary<int, HandPosition> rightHandLeftRightDictionary = new Dictionary<int, HandPosition>();
        private static Dictionary<int, HandPosition> rightHandBackForwardsDictionary = new Dictionary<int, HandPosition>();

        public static event HandPositionChangedDelegate RightHandUpDownChanged;
        public static event HandPositionChangedDelegate RightHandLeftRightChanged;
        public static event HandPositionChangedDelegate RightHandBackForwardsChanged;

        public static void ProcessRightHand(Skeleton skeleton)
        {
            Joint rightHand = skeleton.Joints[JointType.HandRight];
            Joint rightShoulder = skeleton.Joints[JointType.ShoulderRight];

            SkeletonPoint rightHandPoint = rightHand.Position;
            SkeletonPoint rightShoulderPoint = rightShoulder.Position;

            HandPosition previousRightHandUpDownPosition = (rightHandUpDownDictionary.ContainsKey(skeleton.TrackingId)) ? rightHandUpDownDictionary[skeleton.TrackingId] : HandPosition.Unknown;
            HandPosition newRightHandUpDownPosition = HandPosition.Unknown;

            HandPosition previousRightHandLeftRightPosition = (rightHandLeftRightDictionary.ContainsKey(skeleton.TrackingId)) ? rightHandLeftRightDictionary[skeleton.TrackingId] : HandPosition.Unknown;
            HandPosition newRightHandLeftRightPosition = HandPosition.Unknown;

            HandPosition previousRightHandBackForwardsPosition = (rightHandBackForwardsDictionary.ContainsKey(skeleton.TrackingId)) ? rightHandBackForwardsDictionary[skeleton.TrackingId] : HandPosition.Unknown;
            HandPosition newRightHandBackForwardsPosition = HandPosition.Unknown;

            if ((rightHand.TrackingState == JointTrackingState.NotTracked) || (rightShoulder.TrackingState == JointTrackingState.NotTracked))
            {
                newRightHandUpDownPosition = HandPosition.Unknown;
                newRightHandLeftRightPosition = HandPosition.Unknown;
                newRightHandBackForwardsPosition = HandPosition.Unknown;
            }
            else
            {
                // Up/Down
                if (rightHandPoint.Y - rightShoulderPoint.Y > 0.2)
                {
                    newRightHandUpDownPosition = HandPosition.Up;
                }
                else if (Math.Abs(rightHandPoint.Y - rightShoulderPoint.Y) > 0.2)
                {
                    newRightHandUpDownPosition = HandPosition.Down;
                }
                else
                {
                    newRightHandUpDownPosition = HandPosition.Center;
                }

                // Left/Right
                if (rightHandPoint.X - rightShoulderPoint.X > 0.2)
                {
                    newRightHandLeftRightPosition = HandPosition.Right;
                }
                else if (Math.Abs(rightHandPoint.X - rightShoulderPoint.X) > 0.2)
                {
                    newRightHandLeftRightPosition = HandPosition.Left;
                }
                else
                {
                    newRightHandLeftRightPosition = HandPosition.Center;
                }

                // Backwards/Forwards
                if (rightShoulderPoint.Z - rightHandPoint.Z > 0.5)
                {
                    newRightHandBackForwardsPosition = HandPosition.Forwards;
                }
                else if (rightShoulderPoint.Z - rightHandPoint.Z < 0.25)
                {
                    newRightHandBackForwardsPosition = HandPosition.Backwards;
                }
                else
                {
                    newRightHandBackForwardsPosition = HandPosition.Center;
                }
            }

            if (previousRightHandUpDownPosition != newRightHandUpDownPosition)
            {
                rightHandUpDownDictionary[skeleton.TrackingId] = newRightHandUpDownPosition;
                if (RightHandUpDownChanged != null)
                {
                    RightHandUpDownChanged(skeleton, new HandPositionChangedArgs() { Position = newRightHandUpDownPosition });
                }
            }

            if (previousRightHandLeftRightPosition != newRightHandLeftRightPosition)
            {
                rightHandLeftRightDictionary[skeleton.TrackingId] = newRightHandLeftRightPosition;
                if (RightHandLeftRightChanged != null)
                {
                    RightHandLeftRightChanged(skeleton, new HandPositionChangedArgs() { Position = newRightHandLeftRightPosition });
                }
            }

            if (previousRightHandBackForwardsPosition != newRightHandBackForwardsPosition)
            {
                rightHandBackForwardsDictionary[skeleton.TrackingId] = newRightHandBackForwardsPosition;
                if (RightHandBackForwardsChanged != null)
                {
                    RightHandBackForwardsChanged(skeleton, new HandPositionChangedArgs() { Position = newRightHandBackForwardsPosition });
                }
            }
        }

        // Left Hand
        private static Dictionary<int, HandPosition> leftHandUpDownDictionary = new Dictionary<int, HandPosition>();
        private static Dictionary<int, HandPosition> leftHandLeftRightDictionary = new Dictionary<int, HandPosition>();
        private static Dictionary<int, HandPosition> leftHandBackForwardsDictionary = new Dictionary<int, HandPosition>();

        public static event HandPositionChangedDelegate LeftHandUpDownChanged;
        public static event HandPositionChangedDelegate LeftHandLeftRightChanged;
        public static event HandPositionChangedDelegate LeftHandBackForwardsChanged;

        public static void ProcessLeftHand(Skeleton skeleton)
        {
            Joint leftHand = skeleton.Joints[JointType.HandLeft];
            Joint leftShoulder = skeleton.Joints[JointType.ShoulderLeft];

            SkeletonPoint leftHandPoint = leftHand.Position;
            SkeletonPoint leftShoulderPoint = leftShoulder.Position;

            HandPosition previousLeftHandUpDownPosition = (leftHandUpDownDictionary.ContainsKey(skeleton.TrackingId)) ? leftHandUpDownDictionary[skeleton.TrackingId] : HandPosition.Unknown;
            HandPosition newLeftHandUpDownPosition = HandPosition.Unknown;

            HandPosition previousLeftHandLeftRightPosition = (leftHandLeftRightDictionary.ContainsKey(skeleton.TrackingId)) ? leftHandLeftRightDictionary[skeleton.TrackingId] : HandPosition.Unknown;
            HandPosition newLeftHandLeftRightPosition = HandPosition.Unknown;

            HandPosition previousLeftHandBackForwardsPosition = (leftHandBackForwardsDictionary.ContainsKey(skeleton.TrackingId)) ? leftHandBackForwardsDictionary[skeleton.TrackingId] : HandPosition.Unknown;
            HandPosition newLeftHandBackForwardsPosition = HandPosition.Unknown;

            if ((leftHand.TrackingState == JointTrackingState.NotTracked) || (leftShoulder.TrackingState == JointTrackingState.NotTracked))
            {
                newLeftHandUpDownPosition = HandPosition.Unknown;
                newLeftHandLeftRightPosition = HandPosition.Unknown;
                newLeftHandBackForwardsPosition = HandPosition.Unknown;
            }
            else
            {
                // Up/Down
                if (leftHandPoint.Y - leftShoulderPoint.Y > 0.2)
                {
                    newLeftHandUpDownPosition = HandPosition.Up;
                }
                else if (Math.Abs(leftHandPoint.Y - leftShoulderPoint.Y) > 0.2)
                {
                    newLeftHandUpDownPosition = HandPosition.Down;
                }
                else
                {
                    newLeftHandUpDownPosition = HandPosition.Center;
                }

                // Left/Right
                if (leftHandPoint.X - leftShoulderPoint.X > 0.2)
                {
                    newLeftHandLeftRightPosition = HandPosition.Right;
                }
                else if (Math.Abs(leftHandPoint.X - leftShoulderPoint.X) > 0.2)
                {
                    newLeftHandLeftRightPosition = HandPosition.Left;
                }
                else
                {
                    newLeftHandLeftRightPosition = HandPosition.Center;
                }

                // Backwards/Forwards
                if (leftShoulderPoint.Z - leftHandPoint.Z > 0.5)
                {
                    newLeftHandBackForwardsPosition = HandPosition.Forwards;
                }
                else if (leftShoulderPoint.Z - leftHandPoint.Z < 0.25)
                {
                    newLeftHandBackForwardsPosition = HandPosition.Backwards;
                }
                else
                {
                    newLeftHandBackForwardsPosition = HandPosition.Center;
                }
            }

            if (previousLeftHandUpDownPosition != newLeftHandUpDownPosition)
            {
                leftHandUpDownDictionary[skeleton.TrackingId] = newLeftHandUpDownPosition;
                if (LeftHandUpDownChanged != null)
                {
                    LeftHandUpDownChanged(skeleton, new HandPositionChangedArgs() { Position = newLeftHandUpDownPosition });
                }
            }

            if (previousLeftHandLeftRightPosition != newLeftHandLeftRightPosition)
            {
                leftHandLeftRightDictionary[skeleton.TrackingId] = newLeftHandLeftRightPosition;
                if (LeftHandLeftRightChanged != null)
                {
                    LeftHandLeftRightChanged(skeleton, new HandPositionChangedArgs() { Position = newLeftHandLeftRightPosition });
                }
            }

            if (previousLeftHandBackForwardsPosition != newLeftHandBackForwardsPosition)
            {
                leftHandBackForwardsDictionary[skeleton.TrackingId] = newLeftHandBackForwardsPosition;
                if (LeftHandBackForwardsChanged != null)
                {
                    LeftHandBackForwardsChanged(skeleton, new HandPositionChangedArgs() { Position = newLeftHandBackForwardsPosition });
                }
            }
        }
    }
}