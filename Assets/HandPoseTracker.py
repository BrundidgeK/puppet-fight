import socket

import cv2
from cvzone.HandTrackingModule import HandDetector
import mediapipe as mp
import math

from FingerPlacement import FingerPlacement

# window dimensions
width, height = 1280, 720

# Initialize video capture
cap = cv2.VideoCapture(0)
cap.set(3, width)
cap.set(4, height)

detector = HandDetector(maxHands=1, detectionCon=.8)

# Define features of circles
circle_radius = 50
circle_positions = [(0, 0), (0, 0), (0, 0), (0, 0), (0, 0)]
tracking = True  # If finger positions are being tracked by circles
fingerPlacement = FingerPlacement(circle_radius)  # Initialize object that tracks finger placement
wrist_position = (0, 0)

tracked_rad = circle_radius
tracked_positions = circle_positions

# Creating sockets for unity communication
GameplaySock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 1352)
GameplaySock.connect(serverAddressPort)
mainMenuSock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort2 = ("127.0.0.1", 1351)
mainMenuSock.connect(serverAddressPort2)


def tracking_placements(cur_lm):
    hand_scale = (math.hypot(cur_lm[17][1] - cur_lm[0][1], cur_lm[17][0] - cur_lm[0][0]) /
                  math.hypot(set_landmarks[17][1] - set_landmarks[0][1], set_landmarks[17][0] - set_landmarks[0][0]))
    global tracked_rad
    tracked_rad = hand_scale * circle_radius

    for i in range(len(circle_positions)):
        fin_index = i * 4 + 4
        wrist = 0
        hand_rotation = (
                math.atan2(cur_lm[wrist][1] - cur_lm[fin_index][1], cur_lm[wrist][0] - cur_lm[fin_index][0]) -
                math.atan2(set_landmarks[wrist][1] - set_landmarks[fin_index][1],
                           set_landmarks[wrist][0] - set_landmarks[fin_index][0]))

        angle = math.atan2(set_landmarks[fin_index][1] - set_landmarks[wrist][1],
                           set_landmarks[fin_index][0] - set_landmarks[wrist][0])
        distance = math.hypot(set_landmarks[fin_index][1] - set_landmarks[0][1],
                              set_landmarks[fin_index][0] - set_landmarks[0][0])
        distance *= hand_scale

        tracked_positions[i] = (distance * math.cos(angle + hand_rotation), distance * math.sin(angle + hand_rotation))


# Main loop to process video frames
while True:
    tracked_rad = circle_radius

    data = []
    ref, img = cap.read()

    # Detects the hand in frame
    hands, img = detector.findHands(img)

    # Sends if there were any hands detected to the main menu
    mainMenuSock.send(str.encode(str(hands)))

    if hands:
        hand = hands[0]  # Finds the first hand detected
        lmList = hand['lmList']  # Gets the list of landmarks
        index = 0
        lm_positions = []

        for lm in lmList:
            if index == 0:
                wrist_position = (lm[0], lm[1])  # updates wrist position
            elif index % 4 == 0:
                # follows finger positions until user sets their "up" position
                if tracking:
                    circle_positions[(index // 4) - 1] = (lm[0] - wrist_position[0], lm[1] - wrist_position[1])
            lm_positions.append((lm[0], lm[1]))
            index += 1
        # Sends processed finger positions to the gameplay socket
        if not tracking:
            tracking_placements(lmList)
            fingerPlacement.set_radius(tracked_rad)
            placements = str(fingerPlacement.get_fingers(tracked_positions, lm_positions))
            print(placements)
            GameplaySock.send(str.encode(placements))
            img = cv2.putText(img, placements, (25, 50), 5, 1, (0, 0, 0))
        else:
            set_landmarks = lmList
            tracked_positions = circle_positions

    # draws circles where each fingers' "up" position is
    for pos in tracked_positions:
        if tracking:
            color = (255, 0, 0)
        else:
            color = (0, 255, 0)

        center = (int(pos[0] + wrist_position[0]), int(pos[1] + wrist_position[1]))  # Adjust based on wrist position
        cv2.circle(img, center, int(tracked_rad), color, 2)

    # Display instructions on screen
    img = cv2.putText(img, 'Press E to set the "down" positions for fingers', (25, 25),
                      5, 1, (0, 0, 0))

    cv2.imshow("Finger Tracking", img)
    if cv2.waitKey(1) == ord('e'):  # Toggles finger tracking
        tracking = not tracking
    if cv2.getWindowProperty('Finger Tracking', cv2.WND_PROP_VISIBLE) < 1:
        break

cv2.destroyAllWindows()
