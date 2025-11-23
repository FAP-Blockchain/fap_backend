# IPFS Integration Guide

## Tổng quan
Dự án đã tích hợp IPFS (InterPlanetary File System) thông qua Pinata để lưu trữ file phi tập trung.

## Cấu hình

### 1. Đăng ký tài khoản Pinata
1. Truy cập: https://pinata.cloud
2. Đăng ký tài khoản miễn phí (Free tier: 1GB storage)
3. Vào **API Keys** → Tạo API Key mới
4. Copy **API Key** và **API Secret**

### 2. Cập nhật appsettings.json
```json
"IpfsSettings": {
  "Provider": "Pinata",
  "ApiKey": "YOUR_PINATA_API_KEY_HERE",
  "ApiSecret": "YOUR_PINATA_API_SECRET_HERE",
  "GatewayUrl": "https://gateway.pinata.cloud/ipfs/",
  "ApiBaseUrl": "https://api.pinata.cloud",
  "MaxFileSizeMB": 100,
  "EnablePinning": true
}
```

### 3. Cập nhật appsettings.Development.json (cho môi trường dev)
Tạo file riêng cho môi trường development để không commit API key lên Git:
```json
{
  "IpfsSettings": {
    "ApiKey": "your-dev-api-key",
    "ApiSecret": "your-dev-api-secret"
  }
}
```

⚠️ **LƯU Ý**: Thêm các API key thật vào `.gitignore` hoặc dùng User Secrets trong .NET

## Sử dụng

### API Endpoints

#### 1. Upload file
```http
POST /api/ipfs/upload
Content-Type: multipart/form-data
Authorization: Bearer {jwt_token}

Body: file (form-data)
```

**Response:**
```json
{
  "cid": "QmXxxx...",
  "url": "https://gateway.pinata.cloud/ipfs/QmXxxx...",
  "size": 1024567,
  "uploadedAt": "2025-11-23T10:30:00Z",
  "fileName": "document.pdf"
}
```

#### 2. Upload metadata (JSON)
```http
POST /api/ipfs/upload-metadata
Content-Type: application/json
Authorization: Bearer {jwt_token}

Body:
{
  "studentId": "SE123456",
  "semester": "Fall 2024",
  "grades": [
    { "subject": "Blockchain", "score": 9.5 }
  ]
}
```

#### 3. Get file URL
```http
GET /api/ipfs/url/{cid}
```

#### 4. Download file
```http
GET /api/ipfs/download/{cid}
```

### Sử dụng trong Code

#### Inject IIpfsService vào Controller/Service
```csharp
public class CredentialService
{
    private readonly IIpfsService _ipfsService;
    
    public CredentialService(IIpfsService ipfsService)
    {
        _ipfsService = ipfsService;
    }
    
    public async Task<string> IssueCertificate(byte[] pdfData, string studentName)
    {
        // Upload PDF to IPFS
        var cid = await _ipfsService.UploadBytesAsync(pdfData, $"{studentName}_certificate.pdf");
        
        // Get public URL
        var publicUrl = _ipfsService.GetFileUrl(cid);
        
        // Save CID to blockchain
        // await _blockchainService.IssueCredential(studentAddress, cid);
        
        return cid;
    }
}
```

#### Upload bảng điểm lên IPFS
```csharp
public async Task<string> UploadTranscript(TranscriptDto transcript)
{
    var metadata = new
    {
        studentId = transcript.StudentId,
        semester = transcript.Semester,
        year = transcript.Year,
        courses = transcript.Courses.Select(c => new
        {
            code = c.CourseCode,
            name = c.CourseName,
            credits = c.Credits,
            grade = c.Grade,
            gpa = c.GradePoint
        }),
        totalCredits = transcript.TotalCredits,
        gpa = transcript.GPA,
        generatedAt = DateTime.UtcNow
    };
    
    var cid = await _ipfsService.UploadMetadataAsync(metadata, $"transcript_{transcript.StudentId}_{transcript.Semester}.json");
    return cid;
}
```

## Luồng hoạt động với Blockchain

### Quy trình cấp bằng tốt nghiệp
1. **Admin duyệt tốt nghiệp** → Tạo file PDF bằng cấp
2. **Upload PDF lên IPFS** → Nhận về CID (hash)
3. **Lưu CID vào Smart Contract** → Gọi `issueCredential(studentAddress, ipfsCid, ...)`
4. **Tạo QR Code** → Chứa link verify: `https://yourapp.com/certificates/verify/{transactionHash}`

### Quy trình xác thực bằng cấp (Public)
1. Người tuyển dụng scan QR Code
2. Frontend gọi API: `GET /api/certificates/verify/{transactionHash}`
3. Backend:
   - Đọc dữ liệu từ Smart Contract (on-chain)
   - Lấy IPFS CID từ contract
   - Tạo URL để xem file: `https://gateway.pinata.cloud/ipfs/{cid}`
4. Hiển thị thông tin bằng cấp + PDF gốc từ IPFS

## Best Practices

### 1. Lưu gì lên IPFS?
✅ **NÊN lưu:**
- File PDF bằng tốt nghiệp, chứng chỉ
- Ảnh thẻ sinh viên
- File JSON metadata (bảng điểm chi tiết)
- Documents quan trọng cần chống giả mạo

❌ **KHÔNG NÊN lưu:**
- Thông tin cá nhân nhạy cảm (CMND, địa chỉ nhà)
- Dữ liệu thường xuyên thay đổi
- Log hệ thống

### 2. Bảo mật
- File upload lên IPFS là **công khai** (ai có CID đều truy cập được)
- **Nếu cần bảo mật**: Mã hóa file trước khi upload
```csharp
var encryptedData = await _encryptionService.EncryptAsync(pdfData);
var cid = await _ipfsService.UploadBytesAsync(encryptedData, "encrypted_cert.pdf");
```

### 3. Quản lý chi phí
- Pinata Free: 1GB storage
- Pinata Pro: $20/tháng cho 100GB
- **Nên unpin** các file không còn dùng để tiết kiệm storage

### 4. Tối ưu hóa
- Upload metadata dạng JSON thay vì lưu từng điểm số lên blockchain (tiết kiệm gas fee)
- Cache CID trong database để giảm số lần query blockchain

## Troubleshooting

### Lỗi: "401 Unauthorized"
→ Kiểm tra lại API Key và API Secret trong appsettings.json

### Lỗi: "File size exceeds maximum"
→ Tăng `MaxFileSizeMB` trong settings hoặc nén file trước khi upload

### Lỗi: "Gateway timeout"
→ IPFS gateway có thể chậm, thử lại sau vài giây hoặc dùng gateway khác

### File không tải được từ gateway
→ Đợi vài phút sau khi upload (file cần thời gian để replicate trên IPFS network)

## Tài liệu tham khảo
- Pinata Documentation: https://docs.pinata.cloud
- IPFS Documentation: https://docs.ipfs.tech
- Nethereum (Blockchain integration): https://docs.nethereum.com
