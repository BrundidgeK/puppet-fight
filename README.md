Puppet Fight - README
Welcome to Puppet Fight, a unique and interactive fighting game built in Unity, where players control their characters' attacks through finger tracking! The game combines the power of Unity for game development with Python for tracking hand and finger movements, allowing for a new level of immersion and control in fighting games.

Features
Finger Tracking Combat: Players' finger movements are captured using Python libraries and translated into in-game attacks.
Responsive Controls: Each finger's placement correlates to different moves and attacks, making gameplay more intuitive and engaging.
Immersive Gameplay: Real-time tracking creates an immersive experience, allowing you to "puppet" your fighter with precise hand gestures.
Unity-powered Game Engine: The game leverages Unity's robust graphics and physics engine for smooth, high-quality performance.
Table of Contents
Requirements
Installation
How it Works
Usage
Controls
Contribution
License
Requirements
To run Puppet Fight, ensure you have the following prerequisites:

Game Requirements:
Unity (version 2021.3 or newer)
Python (version 3.7 or newer)
Python Dependencies:
OpenCV (for camera and image processing)
Mediapipe (for finger tracking)
PySerial (for communication with Unity)
Unity Packages:
Unity's Input System (for character controls)
Unity's networking system (if playing multiplayer)
Installation
Follow these steps to set up Puppet Fight on your local machine:

1. Clone the Repository:
bash
Copy code
git clone https://github.com/BrundidgeK/puppet-fight.git
2. Python Setup:
Install required Python libraries:

bash
Copy code
pip install opencv-python mediapipe pyserial
3. Unity Setup:
Open Unity Hub, click on Add Project, and select the cloned puppet-fight directory.
Make sure all Unity dependencies are correctly installed (the project should prompt you if something is missing).
Verify that the Input System and necessary packages are installed via the Unity Package Manager.
4. Connect Python and Unity:
The Python script handles the finger tracking and sends the data to Unity via a socket or serial communication.
Make sure the Python script and Unity are able to communicate by running the Python script first, then starting the game in Unity.
How it Works
Finger Tracking in Python:

Python uses Mediapipe and OpenCV to capture live video input through a webcam, detect hand landmarks, and track finger movements.
The finger data is mapped to corresponding attack actions based on finger position and movement.
Communication with Unity:

The Python script sends the processed finger tracking data to Unity using a socket or serial connection (PySerial).
In-Game Control:

Unity receives the finger tracking data and uses it to trigger character attacks and movements.
For example, bending your fingers might execute a punch or block, while extending them could launch a special move.
Usage
Running the Game:
Start the Finger Tracking:

Run the Python finger tracking script:
bash
Copy code
python finger_tracking.py
Start the Game in Unity:

Open the game scene in Unity and press Play to start the game.
Ensure your webcam is active, and Unity is receiving data from the Python script.
Puppet Fight uses hand gestures to control the character’s movements and attacks. Here's a basic guide to how finger movements map to in-game actions:

Punch: Close your thumb or pinkie finger
Block: Close both thumg and pinkie
Duck: Close both the index and ring fingers
Lean to the side: Close either the index or ring fingers
These are examples, and you can modify the mappings in the Python script or Unity to match your preferred hand gestures.

Contribution
We welcome contributions! Here’s how you can help:

Fork the repository.
Create a new branch with descriptive name (feature-finger-combo-attack).
Make your changes and commit them.
Submit a pull request.
Make sure to document your changes and test thoroughly before submitting your contribution.

License
This project is licensed under the MIT License. See the LICENSE file for more information.

Thank you for checking out Puppet Fight! We hope you enjoy this innovative and interactive gaming experience. If you encounter any issues or have any feedback, feel free to reach out through the GitHub Issues section.
