## Dependencies
# pip install cvzone
# pip install mediapipe
# pip install protobuf==3.20.*

from cvzone.HandTrackingModule import HandDetector
import cv2
import socket
import time
 
# Setup videocapture
print("Setting up videocapture...")
cap = cv2.VideoCapture(0)
cap.set(3, 1280)
cap.set(4, 720)
success, img = cap.read()
height, width, _ = img.shape
detector = HandDetector(detectionCon=0.8, maxHands=2)

# Setup socket
print("Setting up socket...")
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
serverAddressPort = ("127.0.0.1", 5052)

def track_hands():
    # Get image frame
    success, img = cap.read()
    # Find the hand and its landmarks
    hands, img = detector.findHands(img)  # with draw
    # hands = detector.findHands(img, draw=False)  # without draw
 
    # Send data
    if hands:
        data = []
        # Hand 1
        hand = hands[0]
        lmList = hand["lmList"]  # List of 21 Landmark points
        for lm in lmList:
            data.extend([lm[0], height - lm[1], lm[2]])
 
        sock.sendto(str.encode(str(data)), serverAddressPort)
 
    # Display
    cv2.imshow("Video", img)

# Main loop
print("Begin tracking!") 
while True:
    startTime = time.time()
    
    
    print("deltaTime " + str(time.time() - startTime))
    # Press ESC to close windows
    k = cv2.waitKey(1) & 0xFF
    if k == 27: # ESC
        cv2.destroyAllWindows()
        break