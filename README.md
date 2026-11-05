[README.md](https://github.com/user-attachments/files/34427272/README.md)
# 🎬 CineLuxe Cinema Management System

Hệ thống **Đặt vé xem phim & Quản lý rạp chiếu** được xây dựng bằng **.NET 10 WinForms**, sử dụng **Entity Framework Core (Code-First)** và cơ sở dữ liệu **SQLite**. Ứng dụng mô phỏng quy trình mua vé thực tế tại các rạp như CGV, Lotte Cinema, thiết kế theo mô hình phân tầng, gồm 3 phân hệ: **Quản trị (Admin)**, **Ki-ốt tự phục vụ (Kiosk)** và **Quầy bán vé (POS/Staff)**.

> 📚 Đồ án cuối kỳ môn Lập trình .NET — thời gian thực hiện: 02/11/2026 – 22/11/2026.
>
> 📴 **Ứng dụng chạy hoàn toàn offline** (không cần Internet): dữ liệu lưu trong SQLite cục bộ, phần thanh toán/quét mã QR được **mô phỏng** và mặc định trả về *thanh toán thành công*.

---

## 📑 Mục lục

1. [Tính năng nổi bật](#-tính-năng-nổi-bật)
2. [Danh sách 18 màn hình](#-danh-sách-18-màn-hình)
3. [Quy tắc nghiệp vụ chính](#-quy-tắc-nghiệp-vụ-chính)
4. [Công nghệ sử dụng](#-công-nghệ-sử-dụng)
5. [Cấu trúc thư mục](#-cấu-trúc-thư-mục-dự-án)
6. [Thiết kế cơ sở dữ liệu](#-thiết-kế-cơ-sở-dữ-liệu)
7. [Cài đặt & chạy ứng dụng](#-cài-đặt--chạy-ứng-dụng)
8. [Luồng hoạt động](#-luồng-hoạt-động)
9. [Quy chuẩn làm việc nhóm (Git)](#-quy-chuẩn-làm-việc-nhóm-git)
10. [Hướng phát triển](#-hướng-phát-triển)
11. [Thành viên thực hiện](#-thành-viên-thực-hiện)

---

## 🚀 Tính năng nổi bật

### 🛠 Phân hệ Quản trị (Admin)
* **Quản lý phim:** tên phim, poster, thời lượng, thể loại, độ tuổi, trailer/mô tả.
* **Quản lý phòng chiếu:** cấu hình số hàng/cột, **tự động sinh sơ đồ ghế** (Thường, VIP, Đôi).
* **Quản lý lịch chiếu:** tạo suất chiếu theo từng phòng (Phim + Phòng + Giờ chiếu), **tự động chặn xếp 2 suất đè giờ nhau trong cùng 1 phòng**.
* **Quản lý combo bắp nước:** thêm, sửa, xóa danh mục đồ ăn/thức uống.
* **Quản lý tài khoản & phân quyền:** thêm/sửa/khóa tài khoản, phân vai trò **Admin** hoặc **Staff**; đăng nhập bằng mật khẩu được băm (PBKDF2).
* **Báo cáo thống kê:** doanh thu theo từng bộ phim (vé + combo), **tỷ lệ lấp đầy rạp** theo suất chiếu (phim cháy vé / phim vắng), dashboard tổng quan.

### 🖥 Phân hệ Ki-ốt tự phục vụ (Kiosk)
* Khách tự chọn **Phim → Suất chiếu → Ghế → Combo → Thanh toán**.
* **Sơ đồ ghế sinh động theo phòng** (ví dụ 10 hàng × 10 cột):
  * ⬜ Ghế trống (màu trắng) → click chuyển sang 🟨 **vàng** (đang chọn).
  * ⬛ Ghế đã bán ở suất đó → **màu xám**, khóa không cho click.
  * Ghế đang bị người khác giữ → khóa cho đến khi hết hạn giữ.
* **Giữ ghế 3 phút (`SeatHold`):** thời điểm hết hạn được ghi vào CSDL; `Timer` trên form chỉ là đồng hồ đếm ngược. Hết giờ chưa thanh toán → tự động nhả ghế.
* **Thanh toán mô phỏng (offline):** hiển thị mã QR giả lập, hệ thống mặc định coi giao dịch là **thành công**, sau đó tạo mã vé và hiển thị **mã QR của vé**.

### 🧾 Phân hệ Quầy POS (Staff)
* Nhân viên bán vé trực tiếp tại quầy: chọn phim, suất, ghế, combo.
* Tính tổng tiền, **tiền khách đưa và tiền thừa**.
* Hỗ trợ **Tiền mặt** và **Chuyển khoản/QR (mô phỏng)**; cả hai đều ghi nhận thành công khi xác nhận.
* Xuất hóa đơn / in vé.

---

## 🖼 Danh sách 18 màn hình

| Phân hệ | Màn hình |
|---------|----------|
| **Chung & Tài khoản** | `FrmRoleSelection`, `FrmLogin`, `FrmAdminUsers` |
| **Admin** | `FrmAdminDashboard`, `FrmAdminMovies`, `FrmMovieDetail`, `FrmAdminRooms`, `FrmAdminShowtimes` |
| **Kiosk** | `FrmKioskHome`, `FrmKioskShowtime`, `FrmKioskSeat`, `FrmKioskCombo`, `FrmKioskPayment`, `FrmKioskSuccess` |
| **POS & Báo cáo** | `FrmStaffPOS`, `FrmAdminCombos`, `FrmAdminReports`, `FrmOccupancyReport` |

`FrmRoleSelection` là màn hình khởi đầu: chọn **Kiosk** thì vào thẳng (không cần đăng nhập); chọn **Quản trị / Nhân viên** thì qua `FrmLogin`, hệ thống kiểm tra vai trò rồi mở đúng phân hệ.

---

## 📏 Quy tắc nghiệp vụ chính

| # | Quy tắc | Cách xử lý |
|---|---------|------------|
| 1 | **Chống trùng lịch chiếu** trong cùng 1 phòng | `existing.StartTime < newEndTime && existing.EndTime > newStartTime`. `EndTime` = giờ bắt đầu + thời lượng phim (+ thời gian dọn phòng), lưu sẵn trong bảng `Showtime` |
| 2 | **Trạng thái ghế động**, không lưu `IsSold` cố định trong bảng `Seat` | Trạng thái `Available / Held / Sold` được xác định theo từng suất chiếu dựa trên `Ticket` và `SeatHold` |
| 3 | **Giữ ghế 3 phút** | Thời điểm hết hạn ghi vào CSDL (`SeatHold.ExpiresAt`); `Timer` giao diện chỉ đếm ngược. Hết hạn → xóa/bỏ qua hold |
| 4 | **Chống bán trùng ghế** | Kiểm tra lại trong **transaction** khi thanh toán + unique index `(ShowtimeId, SeatId)` trên `Ticket` |
| 5 | **Ràng buộc thanh toán** | Chỉ giao dịch thành công mới chuyển ghế sang `Sold` và được ghi vào báo cáo doanh thu |
| 6 | **Thanh toán mô phỏng offline** | Quét QR/chuyển khoản được giả lập và mặc định thành công |
| 7 | **Đăng nhập & phân quyền** | Mật khẩu chỉ lưu dạng băm (PBKDF2 kèm salt). Admin dùng mọi màn hình quản trị và POS; Staff chỉ dùng POS; Kiosk không cần đăng nhập |

---

## 🛠 Công nghệ sử dụng

| Thành phần | Công nghệ |
|------------|-----------|
| Ngôn ngữ | C# |
| Framework | **.NET 10** Windows Forms (WinForms) |
| ORM | Entity Framework Core 10 (Code-First, Migrations) |
| Cơ sở dữ liệu | SQLite (`cinema.db`) |
| IDE | JetBrains Rider |
| Kiểm soát phiên bản | Git & GitHub (phân nhánh theo module) |
| Chế độ hoạt động | Offline (không cần Internet) |

---

## 📂 Cấu trúc thư mục dự án

```text
CinemaManagement/
│
├── CinemaManagement.csproj          # File cấu hình dự án (.NET 10)
├── Program.cs                       # Entry point khởi chạy ứng dụng
│
├── Models/                          # Các Entity (Database Entities)
│   ├── Movie.cs                     # Thông tin phim
│   ├── Room.cs                      # Phòng chiếu
│   ├── Seat.cs                      # Ghế ngồi của phòng
│   ├── Showtime.cs                  # Lịch suất chiếu
│   ├── SeatHold.cs                  # Tạm giữ ghế khi đang chọn
│   ├── Booking.cs                   # Hóa đơn đặt vé
│   ├── Ticket.cs                    # Chi tiết vé bán
│   ├── ComboItem.cs                 # Danh mục bắp nước
│   ├── BookingCombo.cs              # Chi tiết combo trong hóa đơn
│   ├── Payment.cs                   # Giao dịch thanh toán
│   └── User.cs                      # Tài khoản & vai trò (Admin / Staff)
│
├── Data/
│   ├── CinemaDbContext.cs           # DbContext dùng chung (SQLite + DbSet)
│   └── DbInitializer.cs             # Dữ liệu mẫu (Seed Data)
│
├── Services/                        # Toàn bộ logic nghiệp vụ & truy vấn LINQ/EF Core
│   ├── MovieService.cs
│   ├── RoomService.cs
│   ├── ShowtimeService.cs           # Có thuật toán chống trùng lịch chiếu
│   ├── BookingService.cs
│   ├── SeatHoldService.cs
│   ├── ComboService.cs
│   ├── PaymentService.cs
│   ├── ReportService.cs
│   └── AuthService.cs               # Đăng nhập, kiểm tra vai trò
│
├── Controls/
│   └── SeatMapControl.cs            # Sơ đồ ghế dùng chung cho Kiosk & POS
│
├── Helpers/
│   ├── ValidationHelper.cs          # Kiểm tra dữ liệu nhập, định dạng
│   ├── PasswordHelper.cs            # Băm và kiểm tra mật khẩu (PBKDF2)
│   ├── AppSession.cs                # Người dùng đang đăng nhập
│   └── QrHelper.cs                  # Sinh mã QR cho vé
│
├── Forms/                           # Giao diện Windows Forms
│   ├── Auth/                        # Đăng nhập, chọn vai trò
│   ├── Admin/                       # Phân hệ Quản trị
│   ├── Kiosk/                       # Phân hệ Ki-ốt tự phục vụ
│   └── Staff/                       # Phân hệ POS
│
└── Migrations/                      # EF Core Migrations
```

### Phân chia trách nhiệm các tầng

| Tầng | Nhiệm vụ |
|------|----------|
| Models | Khai báo đối tượng và thuộc tính dữ liệu |
| Data | Kết nối và quản lý cơ sở dữ liệu (`CinemaDbContext`) |
| Services | Xử lý logic nghiệp vụ và truy vấn LINQ/EF Core |
| Controls | UserControl dùng chung giữa nhiều Form (`SeatMapControl`) |
| Forms | Hiển thị giao diện, bắt sự kiện, nhận thao tác người dùng |
| Helpers | Hàm tiện ích dùng chung |

### Quy tắc phát triển
* **Không** viết nghiệp vụ hoặc truy vấn CSDL phức tạp trong sự kiện `Button_Click`; hãy gọi qua **Services**.
* Form **không** tạo hay giữ `DbContext`; mọi truy cập dữ liệu đi qua Services, mỗi thao tác dùng một `DbContext` ngắn hạn (`using var`) để tránh dữ liệu cũ bị cache khi nhiều Form cùng chạy.
* Mọi thao tác **Thêm, Sửa, Xóa, Thanh toán** phải bọc trong `try-catch` và hiển thị thông báo hợp lý.
* Kiểm tra dữ liệu nhập bằng `ErrorProvider` kết hợp `ValidationHelper`.

---

## 🗄 Thiết kế cơ sở dữ liệu

### Các bảng chính

| Bảng | Mô tả |
|------|-------|
| `Movie` | Tên, poster, thời lượng, thể loại, độ tuổi, mô tả/trailer |
| `Room` | Phòng chiếu: tên, số hàng, số cột |
| `Seat` | Ghế của phòng: hàng, số ghế, loại ghế (Thường/VIP/Đôi) — **không** lưu trạng thái đã bán |
| `Showtime` | Suất chiếu: phim, phòng, `StartTime`, `EndTime`, giá vé |
| `SeatHold` | Ghế đang giữ tạm: suất chiếu, ghế, phiên giữ, `ExpiresAt` |
| `Booking` | Hóa đơn: tổng tiền, thời gian, kênh bán (Kiosk/POS) |
| `Ticket` | Vé đã bán: suất chiếu, ghế, hóa đơn, mã vé |
| `ComboItem` | Danh mục combo bắp nước |
| `BookingCombo` | Combo trong hóa đơn (quan hệ nhiều-nhiều) |
| `Payment` | Giao dịch thanh toán: phương thức, số tiền, trạng thái |
| `User` | Tài khoản: tên đăng nhập, mật khẩu băm (PBKDF2), vai trò (Admin/Staff), trạng thái hoạt động |

### Quan hệ chính

```text
Movie 1 ─── n Showtime n ─── 1 Room 1 ─── n Seat
Showtime 1 ─── n Ticket n ─── 1 Seat
Showtime 1 ─── n SeatHold n ─── 1 Seat
Booking 1 ─── n Ticket
Booking 1 ─── n BookingCombo n ─── 1 ComboItem
Booking 1 ─── 1 Payment
---

## ⚙️ Cài đặt & chạy ứng dụng

### Yêu cầu
* Windows 10/11 (WinForms chỉ chạy trên Windows)
* [.NET SDK 10](https://dotnet.microsoft.com/download)
* **JetBrains Rider**

### Các bước

```bash
# 1. Clone dự án
git clone https://github.com/LatteLz/DotNet-CinemaGroup-27.git
cd DotNet-CinemaGroup-27

# 2. Khôi phục các gói NuGet
dotnet restore

# 3. Chạy ứng dụng
dotnet run
```

### Chạy bằng JetBrains Rider

1. Mở Rider → **Open** → chọn file `CinemaManagement.csproj` (hoặc file `.sln` nếu có).
2. Đợi Rider tự **Restore NuGet packages**.
3. Chọn cấu hình chạy `CinemaManagement`, nhấn **Run** (`Shift + F10`) hoặc **Debug** (`Shift + F9`).
4. Xem CSDL trực tiếp trong Rider: **Database** tool window → **+** → **Data Source** → **SQLite** → trỏ tới file `cinema.db`.

### Cơ sở dữ liệu & Migration
* Lần chạy đầu tiên, ứng dụng tự tạo `cinema.db` và nạp **dữ liệu mẫu** qua `DbInitializer`.
* Muốn đặt lại dữ liệu: xóa file `cinema.db` rồi chạy lại ứng dụng.
* Chỉ **Nhóm trưởng** được tạo/sửa Model, DbContext và Migration:

```bash
dotnet tool install --global dotnet-ef
dotnet ef migrations add <TenMigration>
dotnet ef database update
```

* Toàn bộ ứng dụng hoạt động **offline**, không gọi API bên ngoài.

### Tài khoản mặc định

> ⚠️ Cập nhật bảng dưới đây cho khớp với dữ liệu trong `DbInitializer.cs`. Mật khẩu trong CSDL được lưu dạng băm, không lưu mật khẩu thô.

| Vai trò | Tên đăng nhập | Mật khẩu |
|---------|---------------|----------|
| Admin | `admin` | `<mật khẩu>` |
| Nhân viên (Staff) | `staff` | `<mật khẩu>` |

Phân hệ **Kiosk** dành cho khách hàng nên không yêu cầu đăng nhập.

### Gói NuGet sử dụng

```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package QRCoder        # (tùy chọn) sinh mã QR offline cho vé
```

---

## 🔄 Luồng hoạt động

### Luồng đăng nhập & phân quyền

```text
FrmRoleSelection
   ├── Kiosk → vào thẳng phân hệ Kiosk (không đăng nhập)
   └── Quản trị / Nhân viên → FrmLogin
          ├── Sai tài khoản/mật khẩu → báo lỗi
          ├── Vai trò Admin → các màn hình quản trị + POS
          └── Vai trò Staff → chỉ FrmStaffPOS
```

### Luồng đặt vé (Kiosk / POS)

```text
Chọn Phim
   ↓
Chọn Suất chiếu
   ↓
Hiển thị Sơ đồ ghế của phòng (trạng thái tính động theo suất chiếu)
   ↓
Chọn ghế (ghế → vàng, tạo SeatHold 3 phút, bật đồng hồ đếm ngược)
   ↓                                   ↘ Hết giờ → tự động nhả ghế
Chọn thêm Combo bắp nước
   ↓
Tính tổng tiền
   ↓
Bấm Thanh toán
   ↓
QR giả lập → mặc định THÀNH CÔNG (offline)
   ↓
Ghế chuyển "Đã bán" cho suất chiếu → Tạo mã vé + QR → In vé
```

### Luồng tạo lịch chiếu (Admin)

```text
Chọn Phim + Chọn Phòng + Chọn Giờ chiếu
   ↓
Tính EndTime = StartTime + thời lượng phim (+ dọn phòng)
   ↓
Kiểm tra trùng giờ trong cùng phòng
   ├── Trùng  → Báo lỗi, không cho lưu
   └── Hợp lệ → Lưu suất chiếu
```

---

## 🌿 Quy chuẩn làm việc nhóm (Git)

* Mỗi tính năng làm trên **nhánh riêng** đặt tên `feature/<phân-hệ>-<tính-năng>` (ví dụ `feature/admin-movies`, `feature/kiosk-seat-map`, `feature/pos-payment`); sửa lỗi dùng `fix/<mô-tả>`. **Không commit trực tiếp lên `main`.** Nhánh đã merge thì xóa và tạo nhánh mới từ `main`.
* Commit message rõ nghĩa theo tính năng, ví dụ: `feat: add movie management`, `fix: prevent overlapping showtimes`.
* Hoàn thành tính năng → tạo **Pull Request** mô tả rõ nội dung để Nhóm trưởng review trước khi merge.
* Nên `git pull origin main` hằng ngày để tránh xung đột lớn.
* File `.gitignore` cần loại: `bin/`, `obj/`, `.idea/`, `*.db` (mỗi người dùng DB cục bộ riêng).
* Nhóm trưởng điều phối kiến trúc, review code; mỗi thành viên tự chịu trách nhiệm kiểm thử phân hệ của mình.

---

## 🔮 Hướng phát triển

- [ ] Tích hợp cổng thanh toán thật (MoMo, VNPay) thay cho phần thanh toán mô phỏng — chỉ cần thay lớp xử lý trong `PaymentService`, các phần còn lại giữ nguyên.
- [ ] Chương trình khách hàng thân thiết, mã giảm giá.
- [ ] Gửi email xác nhận vé.
- [ ] Phiên bản web/mobile cho khách đặt vé online.

---

## 👥 Thành viên thực hiện

| STT | Họ và tên | MSSV | Vai trò & phụ trách |
|-----|-----------|------|---------------------|
| 1 | Trần Thị Hiền | 24130089 | **Nhóm trưởng** — Admin, CSDL & kiến trúc: Models, DbContext, Migration, `MovieService`, `RoomService`, `ShowtimeService`, chống trùng lịch chiếu, 6 màn hình Admin |
| 2 | Nguyễn Hoàng Phúc Lộc | 24130175 | **Ki-ốt & Đặt vé:** `BookingService`, `SeatHoldService`, `SeatMapControl` (sơ đồ ghế động), giữ ghế 3 phút, thanh toán giả lập, 6 màn hình Kiosk |
| 3 | Đặng Ngọc Thảo Hân | 24130084 | **POS, Combo & Báo cáo:** `ValidationHelper`, `PasswordHelper`, `AppSession`, `AuthService`, `ComboService`, `PaymentService`, `ReportService`, báo cáo LINQ, `ErrorProvider`, đăng nhập & phân quyền, 6 màn hình |

**Giảng viên hướng dẫn:** `Thầy Phan Đình Long`
