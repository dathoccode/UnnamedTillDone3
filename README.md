# UnnamedTillDone3
# Dev log
## 24/3/2026
- Trying to create castling function
- Add attack map support further function (castle, checkmate, check, stalemate...)
- Split valid moves and attack squares logic 
    - I thought valid moves were kinda same as attackable squares but I was wrong. A square can be attacked by a piece but the piece may cant move to the square because it was pinned by other piece. Or king can attack surround squares but cant move to these square if it is currently attacked by an opponent piece
