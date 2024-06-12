**Key Features of the Game**

Tetromino Distribution:

• 🎲 7-Bag Algorithm: The 7-Bag Algorithm is used for fair distribution of Tetrominos.

🛡️ Collision and Boundary Control: Tetrominos are shifted 1 unit left or right during rotations within the grid to ensure they stay within the game boundaries.

• ⏳ Drop Speed: Tetrominos fall at a set speed, which can be adjusted based on user input.

Game Area Management:

• 🗺️ Grid System: The game area is defined as a grid system consisting of 10 columns and 20 rows.

• 📌 Position Tracking: The positions of Tetromino pieces are tracked within the grid system.

• 🧩 Block Registration and Clearing: When Tetromino pieces reach the bottom, they are registered in the grid system, and filled rows are cleared. Blocks above the cleared rows are shifted down by one row.

Adding Next Tetromino to UI:

• 📋 Retrieving Tetromino Information: Each Tetromino piece is defined as a data object containing prefab and sprite information. Positions and sprite details are extracted and stored as tuples.

• 🖥️ UI Update: When Tetromino information is retrieved as tuples, the visual elements in the UI are updated. This dynamically shows the next Tetromino piece to the player, enriching the gaming experience.
