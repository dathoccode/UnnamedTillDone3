# UnnamedTillDone3
# Dev log
## 24/3/2026
- Trying to create castling function
- Add attack map support further function (castle, checkmate, check, stalemate...)
- Split valid moves and attack squares logic 
    - I thought valid moves were kinda same as attackable squares but I was wrong. A square can be attacked by a piece but the piece may cant move to the square because it was pinned by other piece. Or king can attack surround squares but cant move to these square if it is currently attacked by an opponent piece
## 4/5/2026
- Completed: Castling function, en passant move
- Issue 1: 
    - current attack map system need optimizing cus it's repeated too many times in a single move.
    - King cant move to some safe square -> may be due to attack map system
    > Solution: Solved by optimizing attack map system usage. Now it's not called every time a new piece is selected. It's called only when there's pending change in grid
- Issue 2: 
    - Previously, castling logic was handled by king piece itself which caused a missmatching between grid and castled rook's world position (The king only changes its position on grid but not the rook)
    > Plan: move special move's logic to Gamanager, let's it decide what to move in both grid and world (just deciding not executing) 