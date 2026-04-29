# Đánh giá tổ chức hệ thống & định hướng thêm en passant

## Nhận xét nhanh kiến trúc hiện tại

- **Tách lớp cơ bản khá rõ**: `GameManager` xử lý input/turn, `Board` quản lý lưới quân, `ChessPiece` + lớp con xử lý luật di chuyển.
- **Điểm mạnh**: logic spawn + lưu trạng thái bàn cờ (`grid`) đủ đơn giản để mở rộng luật.
- **Điểm cần cải thiện**:
  - `GameManager` đang trộn nhiều trách nhiệm (input, kiểm tra nước đi, đổi lượt, cập nhật board).
  - `Board.MoveOnBoard` chỉ xử lý bắt quân tại ô đích, chưa có hook cho các luật đặc biệt (en passant, castling, promotion).
  - `Pawn.hasMoved` có nhưng chưa được set nhất quán sau khi đi.
  - `GetAllValidMove()` hiện trả về **offset** thay vì nước đi cấu trúc đầy đủ, khiến luật đặc biệt khó cài.

## Định hướng triển khai en passant (không phá vỡ nhiều code)

### 1) Thêm trạng thái ván để nhớ nước tốt hai ô gần nhất

Tạo kiểu dữ liệu lưu nước đi gần nhất (ít nhất):

- quân vừa đi (`ChessPiece`)
- từ ô nào (`from`)
- đến ô nào (`to`)
- lượt số hoặc cờ "vừa đi ngay lượt trước"

Lưu ở `GameManager` hoặc tách `GameState` riêng (khuyến nghị).

### 2) Mở rộng mô hình nước đi

Thay vì chỉ dùng `Vector2Int` offset, tạo struct/class `Move`:

- `from`, `to`
- `MoveType` (Normal, Capture, EnPassant, Castling, Promotion)
- `capturedSquare` (cho en passant sẽ khác `to`)

Giữ lại API cũ tạm thời nếu muốn migrate dần.

### 3) Sinh nước đi en passant trong `Pawn`

Khi tính nước đi tốt:

- xác định hai ô ngang trái/phải có tốt đối phương.
- kiểm tra nước trước đó của đối phương có phải tốt đi **2 ô** và dừng cạnh mình.
- nếu đúng, thêm `MoveType.EnPassant` với:
  - `to = ô chéo phía trước`
  - `capturedSquare = ô ngang chứa tốt đối phương`

### 4) Áp dụng nước đi ở một chỗ trung tâm

Tạo hàm kiểu `ApplyMove(Move move)` tại `Board`/`GameRules`:

- `Normal/Capture`: như hiện tại.
- `EnPassant`: di chuyển quân đi, xóa quân ở `capturedSquare` (không phải ô đích).

Điều này giúp thêm luật khác sau này dễ hơn.

### 5) Cập nhật vòng đời lượt

Sau mỗi nước hợp lệ:

- set `hasMoved = true` cho tốt.
- ghi lại `lastMove`.
- đổi lượt.

En passant chỉ hợp lệ ở **lượt ngay sau** nước tốt đi 2 ô của đối thủ.

## Checklist test cho en passant

1. Trắng đi tốt 2 ô cạnh tốt đen -> đen ăn en passant được.
2. Nếu đen không ăn ngay mà đi nước khác -> quyền en passant biến mất.
3. En passant xóa đúng quân bị ăn (ở ô ngang), không phải ô đích.
4. Không cho en passant nếu tốt đối thủ chỉ đi 1 ô.
5. Cả hai phía (trắng/đen) đều hoạt động đúng.

## Gợi ý refactor ngắn hạn

- Đổi tên `GetAllValidMove` thành `GetLegalMoves` và trả về `Move`.
- Tách input khỏi luật (ví dụ `InputController` gọi `GameRules`).
- Tránh import dư (`NUnit.Framework`, `Unity.VisualScripting`) trong runtime scripts.

