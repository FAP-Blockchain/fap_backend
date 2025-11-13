# H??ng d?n tri?n khai Backend cho Frontend Team b?ng Docker

Tài li?u này h??ng d?n cách nhanh chóng kh?i ch?y toàn b? h? th?ng backend (API + Database) trên b?t k? máy tính nào ch? v?i Docker.

## 1. Yêu c?u c?n có

- **Cài ??t Docker Desktop**: ??m b?o b?n ?ã cài ??t và ?ang ch?y Docker Desktop trên máy tính c?a mình.
  - T?i v? t?i: [https://www.docker.com/products/docker-desktop/](https://www.docker.com/products/docker-desktop/)

## 2. Nh?ng gì b?n c?n

- B?n ch? c?n duy nh?t t?p `docker-compose.yml`.
- **Không c?n** t?i xu?ng toàn b? mã ngu?n c?a backend.

## 3. Các b??c th?c hi?n

1.  **T?o m?t th? m?c m?i**: Trên máy tính c?a b?n, t?o m?t th? m?c tr?ng ?? ch?a t?p c?u hình, ví d?: `fap-backend-env`.

2.  **Sao chép t?p**: ??t t?p `docker-compose.yml` vào bên trong th? m?c b?n v?a t?o.

3.  **M? Terminal**: M? m?t c?a s? dòng l?nh (PowerShell, Command Prompt, ho?c Terminal) và ?i?u h??ng ??n th? m?c ?ó.
    ```sh
    # Ví d?:
    cd C:\Users\YourUser\Desktop\fap-backend-env
    ```

4.  **Kh?i ??ng Backend**: Ch?y l?nh sau.
    ```sh
    docker compose up -d
    ```

**Ch? m?t chút!** L?n ??u tiên ch?y, Docker s? c?n t?i các "image" (b?n ?óng gói c?a API và SQL Server) t? trên m?ng v?. Quá trình này có th? m?t vài phút. Nh?ng l?n kh?i ??ng sau s? nhanh h?n nhi?u.

L?nh trên s? t? ??ng:
- T?i v? và ch?y container cho **API Backend**.
- T?i v? và ch?y container cho **SQL Server Database**.
- C?u hình m?ng ?? hai container có th? giao ti?p v?i nhau.

## 4. Ki?m tra ho?t ??ng

Sau khi l?nh ch?y xong, b?n có th? ki?m tra xem backend ?ã ho?t ??ng ?úng ch?a:

- **API Endpoint**: Backend s? ch?y t?i ??a ch? `http://localhost:8080`.
- **Swagger UI (Tài li?u API)**: M? trình duy?t và truy c?p `http://localhost:8080/swagger`. B?n s? th?y danh sách t?t c? các API có s?n ?? frontend có th? g?i.
- **Health Check**: Truy c?p `http://localhost:8080/health` ?? ??m b?o API ?ang "kh?e m?nh".

## 5. Các l?nh Docker h?u ích khác

- **?? d?ng toàn b? h? th?ng backend**:
  ```sh
  docker compose down
  ```
- **?? xem nh?t ký (logs) c?a API n?u có l?i**:
  ```sh
  docker compose logs -f api
  ```
- **?? kh?i ??ng l?i h? th?ng**:
  ```sh
  docker compose restart
  ```

V?i các b??c trên, ??i ng? frontend có th? d? dàng có m?t môi tr??ng backend hoàn ch?nh và nh?t quán trên b?t k? máy tính nào mà không c?n ph?i build hay c?u hình mã ngu?n.
