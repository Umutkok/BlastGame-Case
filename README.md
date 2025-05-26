# Blast-Game-Case
 
GJG case

In this project, I used Unity2D to create a game with a Collapse or Blast mechanic. The player can tap on more than two same-colored candies that are next to each other to make them explode, and new candies fall into the grid. My main goal was to build a grid-based system and work on game logic and algorithms.

I also added a Smart Shuffle system. This system changes the candy sprites when they are next to each other and helps the player if there are no more possible moves. While working on the project, I focused on performance. I tried to reduce garbage collection, CPU and GPU usage, and memory usage. I only used the Update function to test the MS (milliseconds) value. Because of this, garbage collection is much lower.

I used BFS (Breadth-First Search) only when there is a null item in the grid. This means that the system does not always scan the whole grid after every move, which helps performance. I didn’t use any plugins in this project. Since I only needed simple animations, I used Coroutines instead of DoTween.

I also marked the items that don’t have neighbors as static. This allowed me to use static batching, which helps reduce render calls and improves performance, especially on mobile devices.

After removing the Update functions, GC allocation dropped from 0.5 MB to 0.02 MB per frame. The MS value is usually between 1 and 2 on mobile devices when v-sync is off. For an 8x8 grid, memory usage is around 248 MB, and you can check this using Unity’s Memory Profiler.
