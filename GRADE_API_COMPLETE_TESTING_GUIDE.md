# Grade Management APIs - Complete Testing Guide with Valid Data

## ?? Overview
H??ng d?n testing ??y ?? cho Grade Management APIs v?i data th?c t? t? database seeder.

---

## ?? Table of Contents
1. [Authentication Setup](#authentication-setup)
2. [Test Data Reference](#test-data-reference)
3. [API Testing Scenarios](#api-testing-scenarios)
4. [Postman Collection](#postman-collection)
5. [Expected Results](#expected-results)

---

## ?? Authentication Setup

### Step 1: Login to Get Access Token

#### Login as Teacher
```bash
POST {{baseUrl}}/api/auth/login
Content-Type: application/json

{
  "email": "teacher1@fap.edu.vn",
  "password": "123456"
}
```

**Response:**
```json
{
"token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh_token_here",
  "userId": "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb",
  "fullName": "Nguy?n V?n Giáo Viên",
  "role": "Teacher"
}
```

**eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJiYmJiYmJiYi1iYmJiLWJiYmItYmJiYi1iYmJiYmJiYmJiYmIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImJiYmJiYmJiLWJiYmItYmJiYi1iYmJiLWJiYmJiYmJiYmJiYiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlRlYWNoZXIiLCJlbWFpbCI6InRlYWNoZXIxQGZhcC5lZHUudm4iLCJleHAiOjE3NjMwMjA5NDEsImlzcyI6IkZhcC5BcGkiLCJhdWQiOiJGYXAuVXNlcnMifQ.C_mH8IseD-S-Tdx7pXBpJXgQJeJfcBSOZBKrj-eQRk8** for subsequent requests!

#### Login as Student
```bash
POST {{baseUrl}}/api/auth/login
Content-Type: application/json

{
  "email": "student1@fap.edu.vn",
  "password": "123456"
}
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJjY2NjY2NjYy1jY2NjLWNjY2MtY2NjYy1jY2NjY2NjY2NjY2MiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImNjY2NjY2NjLWNjY2MtY2NjYy1jY2NjLWNjY2NjY2NjY2NjYyIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IlN0dWRlbnQiLCJlbWFpbCI6InN0dWRlbnQxQGZhcC5lZHUudm4iLCJleHAiOjE3NjMwMjA5NzQsImlzcyI6IkZhcC5BcGkiLCJhdWQiOiJGYXAuVXNlcnMifQ.uWhuxuP9s2eypbTJG5CAexGXgXpxtyw2zolUuQD90HQ
#### Login as Admin
```bash
POST {{baseUrl}}/api/auth/login
Content-Type: application/json

{
  "email": "admin@fap.edu.vn",
  "password": "123456"
}
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhYWFhYWFhYS1hYWFhLWFhYWEtYWFhYS1hYWFhYWFhYWFhYWEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6ImFhYWFhYWFhLWFhYWEtYWFhYS1hYWFhLWFhYWFhYWFhYWFhYSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZW1haWwiOiJhZG1pbkBmYXAuZWR1LnZuIiwiZXhwIjoxNzYzMDIwOTkzLCJpc3MiOiJGYXAuQXBpIiwiYXVkIjoiRmFwLlVzZXJzIn0.l1v5u4AG9PZfOQOZ-UDhOpCAJF9qpKF7jRre6VER7bY
---

## ?? Test Data Reference

### ?? Students

| Student Code | Name | Email | Student ID | User ID |
|--------------|------|-------|-----------|---------|
| SV001 | Nguy?n V?n An | student1@fap.edu.vn | cccccccc-cccc-cccc-cccc-ccccccccccce | cccccccc-cccc-cccc-cccc-cccccccccccc |
| SV002 | Ph?m Th? Bình | student2@fap.edu.vn | cccccccc-cccc-cccc-cccc-cccccccccce2 | cccccccc-cccc-cccc-cccc-ccccccccccc2 |
| SV003 | Hoàng V?n Châu | student3@fap.edu.vn | cccccccc-cccc-cccc-cccc-cccccccccce3 | cccccccc-cccc-cccc-cccc-ccccccccccc3 |

### ????? Teachers

| Teacher Code | Name | Email | Teacher ID | User ID |
|--------------|------|-------|-----------|---------|
| GV001 | Nguy?n V?n Giáo Viên | teacher1@fap.edu.vn | bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbe | bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb |
| GV002 | Tr?n Th? H?ng | teacher2@fap.edu.vn | bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbe2 | bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2 |
| GV003 | Lê V?n Toán | teacher3@fap.edu.vn | bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbe3 | bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3 |

### ?? Subjects

| Code | Name | Subject ID | Semester | Status |
|------|------|-----------|----------|--------|
| BC101 | Blockchain Fundamentals | e1111111-1111-1111-1111-111111111111 | Spring 2024 | Closed |
| SE201 | Software Engineering | e2222222-2222-2222-2222-222222222222 | Spring 2024 | Closed |
| DB301 | Database Management Systems | e3333333-3333-3333-3333-333333333333 | Summer 2024 | Open |
| BC202 | Smart Contract Development | e4444444-4444-4444-4444-444444444444 | Summer 2024 | Open |
| BC303 | Blockchain Security | e5555555-5555-5555-5555-555555555555 | Fall 2024 | Open |

### ?? Classes with Enrolled Students

| Class Code | Class ID | Subject | Teacher | Students |
|-----------|----------|---------|---------|----------|
| BC101-A | f1111111-1111-1111-1111-111111111111 | BC101 | GV001 | SV001, SV002, SV003 |
| SE201-A | f2222222-2222-2222-2222-222222222222 | SE201 | GV002 | SV001, SV003 |
| DB301-A | f3333333-3333-3333-3333-333333333333 | DB301 | GV003 | SV001, SV002, SV003 |
| BC202-A | f4444444-4444-4444-4444-444444444444 | BC202 | GV001 | SV001, SV002 |
| BC202-B | f5555555-5555-5555-5555-555555555555 | BC202 | GV001 | SV003 |
| BC303-A | f6666666-6666-6666-6666-666666666666 | BC303 | GV001 | SV001, SV003 |

### ?? Grade Components

| Component Name | Component ID | Weight % | Description |
|---------------|-------------|----------|-------------|
| Midterm Exam | a1111111-1111-1111-1111-111111111111 | 30 | Mid-semester examination |
| Final Exam | a2222222-2222-2222-2222-222222222222 | 70 | Final examination |
| Assignment | a3333333-3333-3333-3333-333333333333 | 20 | Homework assignments |
| Project | a4444444-4444-4444-4444-444444444444 | 30 | Course project |

### ?? Existing Grades Summary

#### Student SV001 (Nguy?n V?n An)
- **BC101**: 4 components (Complete) - Avg: 8.6 ? B+
  - Midterm: 8.5, Final: 9.0, Assignment: 8.0, Project: 8.7
- **SE201**: 3 components - Avg: 8.4 ? B+
  - Midterm: 8.0, Final: 8.5, Project: 9.0
- **DB301**: 3 components - Avg: 7.8 ? B
  - Midterm: 7.5, Final: 8.0, Assignment: 7.8
- **BC202**: 2 components (In Progress)
  - Midterm: 8.3, Assignment: 8.0
- **BC303**: 1 component (Just Started)
- Assignment: 8.5

#### Student SV002 (Ph?m Th? Bình)
- **BC101**: 3 components - Avg: 7.5 ? B
  - Midterm: 7.5, Final: 8.0, Assignment: 7.0
- **DB301**: 2 components - Avg: 8.4 ? B+
  - Midterm: 8.2, Final: 8.5
- **BC202**: 1 component (Just Started)
  - Midterm: 7.8

#### Student SV003 (Hoàng V?n Châu) - **Excellent Student**
- **BC101**: 4 components (Complete) - Avg: 9.5 ? A
  - Midterm: 9.5, Final: 9.8, Assignment: 9.2, Project: 9.6
- **SE201**: 4 components (Complete) - Avg: 9.2 ? A
  - Midterm: 9.0, Final: 9.3, Assignment: 8.8, Project: 9.5
- **DB301**: 4 components (Complete) - Avg: 9.4 ? A
  - Midterm: 9.0, Final: 9.5, Assignment: 9.2, Project: 9.8
- **BC202**: 4 components (Complete) - Avg: 9.4 ? A
  - Midterm: 9.2, Final: 9.5, Assignment: 9.0, Project: 9.7
- **BC303**: 2 components - Avg: 9.2 ? A
  - Assignment: 9.0, Project: 9.3

---

## ?? API Testing Scenarios

### Scenario 1: Create New Grade (Teacher)

**Use Case**: Giáo viên GV001 nh?p ?i?m Final Exam cho SV001 trong môn BC202

```bash
POST {{baseUrl}}/api/grades
Authorization: Bearer {{teacher1_token}}
Content-Type: application/json

{
  "studentId": "cccccccc-cccc-cccc-cccc-ccccccccccce",
  "subjectId": "e4444444-4444-4444-4444-444444444444",
  "gradeComponentId": "a2222222-2222-2222-2222-222222222222",
  "score": 8.8
}
```

**Expected Response** (201 Created):
```json
{
  "success": true,
  "message": "Grade created successfully",
  "gradeId": "newly-generated-guid",
  "errors": []
}
```

**Letter Grade Auto-Calculated**: 8.8 ? **B+**

---

### Scenario 2: Create Grade - Duplicate Detection

**Use Case**: Th? t?o grade ?ã t?n t?i (should fail)

```bash
POST {{baseUrl}}/api/grades
Authorization: Bearer {{teacher1_token}}
Content-Type: application/json

{
  "studentId": "cccccccc-cccc-cccc-cccc-ccccccccccce",
  "subjectId": "e1111111-1111-1111-1111-111111111111",
  "gradeComponentId": "a1111111-1111-1111-1111-111111111111",
  "score": 9.0
}
```

**Expected Response** (400 Bad Request):
```json
{
  "success": false,
  "message": "Grade creation failed",
  "gradeId": null,
  "errors": [
    "Grade already exists for this student, subject, and component combination"
  ]
}
```

---

### Scenario 3: Get Grade Details

**Use Case**: Xem chi ti?t m?t grade ?ã t?o

```bash
GET {{baseUrl}}/api/grades/{{gradeId}}
Authorization: Bearer {{any_token}}
```

**Find existing grade ID from database or use one just created**

**Expected Response** (200 OK):
```json
{
  "id": "grade-guid",
  "score": 8.5,
  "letterGrade": "B+",
  "updatedAt": "2024-01-15T10:30:00Z",
  "studentId": "cccccccc-cccc-cccc-cccc-ccccccccccce",
  "studentCode": "SV001",
  "studentName": "Nguy?n V?n An",
  "studentEmail": "student1@fap.edu.vn",
  "subjectId": "e1111111-1111-1111-1111-111111111111",
  "subjectCode": "BC101",
  "subjectName": "Blockchain Fundamentals",
  "credits": 3,
  "gradeComponentId": "a1111111-1111-1111-1111-111111111111",
  "componentName": "Midterm Exam",
  "componentWeight": 30
}
```

---

### Scenario 4: Update Grade

**Use Case**: Giáo viên s?a ?i?m t? 8.5 lên 9.0

```bash
PUT {{baseUrl}}/api/grades/{{gradeId}}
Authorization: Bearer {{teacher1_token}}
Content-Type: application/json

{
  "score": 9.0
}
```

**Expected Response** (200 OK):
```json
{
  "success": true,
  "message": "Grade updated successfully",
  "gradeId": "grade-guid",
  "errors": []
}
```

**Letter Grade Auto-Updated**: 9.0 ? **A**

---

### Scenario 5: Get Class Grade Report (BC101-A)

**Use Case**: Xem b?ng ?i?m c?a l?p BC101-A v?i 3 sinh viên

```bash
GET {{baseUrl}}/api/classes/f1111111-1111-1111-1111-111111111111/grades
Authorization: Bearer {{any_token}}
```

**Optional Query Parameters**:
```bash
# Filter by specific component
GET {{baseUrl}}/api/classes/f1111111-1111-1111-1111-111111111111/grades?gradeComponentId=a1111111-1111-1111-1111-111111111111

# Sort descending
GET {{baseUrl}}/api/classes/f1111111-1111-1111-1111-111111111111/grades?sortOrder=desc
```

**Expected Response** (200 OK):
```json
{
  "classId": "f1111111-1111-1111-1111-111111111111",
  "classCode": "BC101-A",
  "subjectCode": "BC101",
  "subjectName": "Blockchain Fundamentals",
  "teacherName": "Nguy?n V?n Giáo Viên",
  "students": [
    {
      "studentId": "cccccccc-cccc-cccc-cccc-ccccccccccce",
      "studentCode": "SV001",
      "studentName": "Nguy?n V?n An",
      "grades": [
        {
          "gradeId": "guid",
    "gradeComponentId": "a1111111-1111-1111-1111-111111111111",
          "componentName": "Midterm Exam",
          "componentWeight": 30,
       "score": 8.5,
       "letterGrade": "B+"
        },
        {
          "gradeId": "guid",
       "gradeComponentId": "a2222222-2222-2222-2222-222222222222",
          "componentName": "Final Exam",
          "componentWeight": 70,
    "score": 9.0,
      "letterGrade": "A"
   },
        {
          "gradeId": "guid",
          "gradeComponentId": "a3333333-3333-3333-3333-333333333333",
        "componentName": "Assignment",
          "componentWeight": 20,
          "score": 8.0,
          "letterGrade": "B+"
     },
        {
          "gradeId": "guid",
       "gradeComponentId": "a4444444-4444-4444-4444-444444444444",
   "componentName": "Project",
          "componentWeight": 30,
          "score": 8.7,
          "letterGrade": "B+"
        }
      ],
      "averageScore": 8.63,
      "finalLetterGrade": "B+"
    },
  {
      "studentId": "cccccccc-cccc-cccc-cccc-cccccccccce2",
      "studentCode": "SV002",
      "studentName": "Ph?m Th? Bình",
      "grades": [
        {
          "gradeId": "guid",
      "gradeComponentId": "a1111111-1111-1111-1111-111111111111",
        "componentName": "Midterm Exam",
     "componentWeight": 30,
          "score": 7.5,
        "letterGrade": "B"
        },
        {
   "gradeId": "guid",
       "gradeComponentId": "a2222222-2222-2222-2222-222222222222",
   "componentName": "Final Exam",
          "componentWeight": 70,
   "score": 8.0,
          "letterGrade": "B+"
 },
        {
      "gradeId": "guid",
          "gradeComponentId": "a3333333-3333-3333-3333-333333333333",
          "componentName": "Assignment",
   "componentWeight": 20,
          "score": 7.0,
   "letterGrade": "B"
     },
        {
  "gradeId": null,
          "gradeComponentId": "a4444444-4444-4444-4444-444444444444",
        "componentName": "Project",
     "componentWeight": 30,
   "score": null,
          "letterGrade": null
    }
 ],
      "averageScore": 7.53,
      "finalLetterGrade": "B"
    },
    {
      "studentId": "cccccccc-cccc-cccc-cccc-cccccccccce3",
      "studentCode": "SV003",
      "studentName": "Hoàng V?n Châu",
      "grades": [
     {
     "gradeId": "guid",
          "gradeComponentId": "a1111111-1111-1111-1111-111111111111",
          "componentName": "Midterm Exam",
          "componentWeight": 30,
"score": 9.5,
          "letterGrade": "A"
        },
  {
    "gradeId": "guid",
          "gradeComponentId": "a2222222-2222-2222-2222-222222222222",
          "componentName": "Final Exam",
    "componentWeight": 70,
          "score": 9.8,
   "letterGrade": "A+"
        },
        {
          "gradeId": "guid",
          "gradeComponentId": "a3333333-3333-3333-3333-333333333333",
          "componentName": "Assignment",
    "componentWeight": 20,
          "score": 9.2,
          "letterGrade": "A"
        },
    {
          "gradeId": "guid",
          "gradeComponentId": "a4444444-4444-4444-4444-444444444444",
     "componentName": "Project",
  "componentWeight": 30,
          "score": 9.6,
       "letterGrade": "A+"
   }
      ],
      "averageScore": 9.53,
      "finalLetterGrade": "A+"
    }
  ]
}
```

**Key Points**:
- ? Shows all 4 grade components for each student
- ? SV002 missing Project grade (null values)
- ? Weighted average calculated: (score1×weight1 + score2×weight2) / (weight1 + weight2)
- ? Final letter grade based on average

---

### Scenario 6: Get Student Transcript (SV001)

**Use Case**: Sinh viên SV001 xem b?ng ?i?m cá nhân

```bash
GET {{baseUrl}}/api/students/cccccccc-cccc-cccc-cccc-ccccccccccce/grades
Authorization: Bearer {{student1_token}}
```

**Optional Query Parameters**:
```bash
# Filter by semester
GET {{baseUrl}}/api/students/cccccccc-cccc-cccc-cccc-ccccccccccce/grades?semesterId=d1111111-1111-1111-1111-111111111111

# Filter by subject
GET {{baseUrl}}/api/students/cccccccc-cccc-cccc-cccc-ccccccccccce/grades?subjectId=e1111111-1111-1111-1111-111111111111

# Sort descending
GET {{baseUrl}}/api/students/cccccccc-cccc-cccc-cccc-ccccccccccce/grades?sortOrder=desc
```

**Expected Response** (200 OK):
```json
{
  "studentId": "cccccccc-cccc-cccc-cccc-ccccccccccce",
  "studentCode": "SV001",
  "studentName": "Nguy?n V?n An",
  "email": "student1@fap.edu.vn",
  "currentGPA": 8.5,
"subjects": [
 {
      "subjectId": "e1111111-1111-1111-1111-111111111111",
      "subjectCode": "BC101",
    "subjectName": "Blockchain Fundamentals",
      "credits": 3,
      "semesterName": "Spring 2024",
      "componentGrades": [
  {
     "gradeId": "guid",
          "gradeComponentId": "a1111111-1111-1111-1111-111111111111",
   "componentName": "Midterm Exam",
          "componentWeight": 30,
 "score": 8.5,
          "letterGrade": "B+"
    },
        {
      "gradeId": "guid",
          "gradeComponentId": "a2222222-2222-2222-2222-222222222222",
  "componentName": "Final Exam",
          "componentWeight": 70,
          "score": 9.0,
          "letterGrade": "A"
        },
     {
          "gradeId": "guid",
          "gradeComponentId": "a3333333-3333-3333-3333-333333333333",
       "componentName": "Assignment",
     "componentWeight": 20,
          "score": 8.0,
       "letterGrade": "B+"
        },
        {
 "gradeId": "guid",
       "gradeComponentId": "a4444444-4444-4444-4444-444444444444",
          "componentName": "Project",
    "componentWeight": 30,
      "score": 8.7,
 "letterGrade": "B+"
}
      ],
      "averageScore": 8.63,
      "finalLetterGrade": "B+"
    },
    {
      "subjectId": "e2222222-2222-2222-2222-222222222222",
      "subjectCode": "SE201",
      "subjectName": "Software Engineering",
      "credits": 4,
      "semesterName": "Spring 2024",
      "componentGrades": [
        {
          "gradeId": "guid",
        "gradeComponentId": "a1111111-1111-1111-1111-111111111111",
    "componentName": "Midterm Exam",
   "componentWeight": 30,
          "score": 8.0,
          "letterGrade": "B+"
        },
      {
    "gradeId": "guid",
          "gradeComponentId": "a2222222-2222-2222-2222-222222222222",
        "componentName": "Final Exam",
      "componentWeight": 70,
"score": 8.5,
      "letterGrade": "A"
        },
{
        "gradeId": "guid",
      "gradeComponentId": "a4444444-4444-4444-4444-444444444444",
          "componentName": "Project",
          "componentWeight": 30,
     "score": 9.0,
          "letterGrade": "A"
        }
   ],
      "averageScore": 8.42,
      "finalLetterGrade": "B+"
    },
    {
      "subjectId": "e3333333-3333-3333-3333-333333333333",
    "subjectCode": "DB301",
      "subjectName": "Database Management Systems",
   "credits": 3,
    "semesterName": "Summer 2024",
      "componentGrades": [
        {
 "gradeId": "guid",
          "gradeComponentId": "a1111111-1111-1111-1111-111111111111",
          "componentName": "Midterm Exam",
          "componentWeight": 30,
          "score": 7.5,
          "letterGrade": "B"
},
        {
          "gradeId": "guid",
     "gradeComponentId": "a2222222-2222-2222-2222-222222222222",
          "componentName": "Final Exam",
          "componentWeight": 70,
      "score": 8.0,
          "letterGrade": "B+"
        },
        {
"gradeId": "guid",
          "gradeComponentId": "a3333333-3333-3333-3333-333333333333",
          "componentName": "Assignment",
          "componentWeight": 20,
     "score": 7.8,
          "letterGrade": "B"
      }
      ],
      "averageScore": 7.79,
   "finalLetterGrade": "B"
    },
    {
      "subjectId": "e4444444-4444-4444-4444-444444444444",
      "subjectCode": "BC202",
      "subjectName": "Smart Contract Development",
      "credits": 4,
      "semesterName": "Summer 2024",
      "componentGrades": [
{
          "gradeId": "guid",
          "gradeComponentId": "a1111111-1111-1111-1111-111111111111",
        "componentName": "Midterm Exam",
       "componentWeight": 30,
          "score": 8.3,
      "letterGrade": "B+"
  },
        {
          "gradeId": "guid",
          "gradeComponentId": "a3333333-3333-3333-3333-333333333333",
        "componentName": "Assignment",
  "componentWeight": 20,
     "score": 8.0,
          "letterGrade": "B+"
        }
      ],
      "averageScore": 8.12,
      "finalLetterGrade": "B+"
    },
    {
      "subjectId": "e5555555-5555-5555-5555-555555555555",
      "subjectCode": "BC303",
      "subjectName": "Blockchain Security",
      "credits": 3,
      "semesterName": "Fall 2024",
 "componentGrades": [
        {
          "gradeId": "guid",
          "gradeComponentId": "a3333333-3333-3333-3333-333333333333",
  "componentName": "Assignment",
     "componentWeight": 20,
          "score": 8.5,
          "letterGrade": "A"
        }
    ],
"averageScore": 8.5,
      "finalLetterGrade": "A"
    }
  ]
}
```

**Key Points**:
- ? Shows all subjects student is enrolled in
- ? Grouped by subject with all component grades
- ? Shows current semester courses (BC202, BC303) with partial grades
- ? Current GPA displayed at top

---

### Scenario 7: Get All Grade Components

**Use Case**: Xem danh sách các thành ph?n ?i?m

```bash
GET {{baseUrl}}/api/grade-components
Authorization: Bearer {{any_token}}
```

**Expected Response** (200 OK):
```json
[
  {
    "id": "a1111111-1111-1111-1111-111111111111",
  "name": "Midterm Exam",
    "weightPercent": 30,
    "gradeCount": 15
  },
  {
    "id": "a2222222-2222-2222-2222-222222222222",
    "name": "Final Exam",
    "weightPercent": 70,
    "gradeCount": 12
  },
  {
    "id": "a3333333-3333-3333-3333-333333333333",
    "name": "Assignment",
    "weightPercent": 20,
    "gradeCount": 14
  },
  {
    "id": "a4444444-4444-4444-4444-444444444444",
    "name": "Project",
    "weightPercent": 30,
    "gradeCount": 9
  }
]
```

---

### Scenario 8: Create Grade Component (Admin Only)

**Use Case**: Admin t?o thành ph?n ?i?m m?i "Quiz"

```bash
POST {{baseUrl}}/api/grade-components
Authorization: Bearer {{admin_token}}
Content-Type: application/json

{
  "name": "Quiz",
  "weightPercent": 10
}
```

**Expected Response** (201 Created):
```json
{
  "success": true,
  "message": "Grade component created successfully",
  "gradeComponentId": "newly-generated-guid",
  "errors": []
}
```

---

### Scenario 9: Update Grade Component

**Use Case**: Admin c?p nh?t weight c?a Midterm t? 30% lên 35%

```bash
PUT {{baseUrl}}/api/grade-components/a1111111-1111-1111-1111-111111111111
Authorization: Bearer {{admin_token}}
Content-Type: application/json

{
  "name": "Midterm Exam",
  "weightPercent": 35
}
```

**Expected Response** (200 OK):
```json
{
  "success": true,
  "message": "Grade component updated successfully",
  "gradeComponentId": "a1111111-1111-1111-1111-111111111111",
  "errors": []
}
```

---

### Scenario 10: Delete Grade Component (Should Fail)

**Use Case**: Th? xóa component ?ang ???c s? d?ng

```bash
DELETE {{baseUrl}}/api/grade-components/a1111111-1111-1111-1111-111111111111
Authorization: Bearer {{admin_token}}
```

**Expected Response** (400 Bad Request):
```json
{
  "success": false,
  "message": "Grade component deletion failed",
  "gradeComponentId": null,
  "errors": [
    "Cannot delete grade component that is currently in use"
  ]
}
```

---

## ?? Postman Collection

### Environment Variables Setup

Create Postman Environment with these variables:

```json
{
  "baseUrl": "https://localhost:7001",
  "teacher1_token": "",
  "teacher2_token": "",
  "student1_token": "",
  "student2_token": "",
  "student3_token": "",
  "admin_token": "",
  "test_grade_id": ""
}
```

### Collection Structure

```
Grade Management APIs
??? ?? Authentication
?   ??? Login as Teacher1
?   ??? Login as Teacher2
?   ??? Login as Student1
?   ??? Login as Student2
?   ??? Login as Student3
?   ??? Login as Admin
?
??? ?? Grades
?   ??? Create Grade (New)
?   ??? Create Grade (Duplicate - Should Fail)
?   ??? Get Grade by ID
?   ??? Update Grade
?   ??? Update Grade (Invalid Score - Should Fail)
?
??? ?? Class Grades
?   ??? Get BC101-A Grades (Complete)
?   ??? Get SE201-A Grades
?   ??? Get DB301-A Grades
?   ??? Get BC202-A Grades (Partial)
?   ??? Get BC303-A Grades (Early Stage)
?   ??? Get Grades Filtered by Component
?
??? ?? Student Transcripts
?   ??? Get SV001 Transcript (All Subjects)
?   ??? Get SV001 Spring 2024 Grades
?   ??? Get SV001 BC101 Only
?   ??? Get SV002 Transcript
?   ??? Get SV003 Transcript (Excellent)
?
??? ?? Grade Components
 ??? Get All Components
    ??? Get Component by ID
    ??? Create Component (Admin)
    ??? Create Component (Duplicate Name - Should Fail)
    ??? Update Component (Admin)
    ??? Delete Component (In Use - Should Fail)
    ??? Delete Component (Unused - Success)
```

---

## ? Expected Results Summary

### Success Cases

| Test Case | Expected Status | Key Validation |
|-----------|----------------|----------------|
| Create new grade | 201 Created | GradeId returned, letterGrade auto-calculated |
| Get grade details | 200 OK | All student, subject, component info included |
| Update grade | 200 OK | Score updated, letterGrade recalculated, updatedAt changed |
| Get class grades | 200 OK | All students shown with complete/partial grades |
| Get student transcript | 200 OK | All subjects grouped with averages calculated |
| Get grade components | 200 OK | List with grade counts |
| Create component (Admin) | 201 Created | Component ID returned |
| Update component (Admin) | 200 OK | Weight updated successfully |

### Failure Cases

| Test Case | Expected Status | Expected Error Message |
|-----------|----------------|------------------------|
| Create duplicate grade | 400 Bad Request | "Grade already exists for this student, subject, and component combination" |
| Get non-existent grade | 404 Not Found | "Grade with ID {id} not found" |
| Create grade invalid score | 400 Bad Request | "Score must be between 0 and 10" |
| Create grade missing student | 400 Bad Request | "Student with ID '{id}' not found" |
| Update grade (non-Teacher) | 403 Forbidden | Insufficient permissions |
| Delete component in use | 400 Bad Request | "Cannot delete grade component that is currently in use" |
| Create component duplicate name | 400 Bad Request | "Grade component with name '{name}' already exists" |

---

## ?? Letter Grade Reference

| Score Range | Letter Grade | Description |
|-------------|--------------|-------------|
| 9.0 - 10.0 | A+ | Xu?t s?c |
| 8.5 - 8.9 | A | Gi?i |
| 8.0 - 8.4 | B+ | Khá gi?i |
| 7.0 - 7.9 | B | Khá |
| 6.5 - 6.9 | C+ | Trung bình khá |
| 5.5 - 6.4 | C | Trung bình |
| 5.0 - 5.4 | D+ | Y?u khá |
| 4.0 - 4.9 | D | Y?u (Pass) |
| 0.0 - 3.9 | F | R?t (Fail) |

**Passing Grade**: >= 4.0 (D or above)

---

## ?? Weighted Average Calculation

**Formula**:
```
Average = (Score? × Weight? + Score? × Weight? + ... + Score? × Weight?) / (Weight? + Weight? + ... + Weight?)
```

**Example** (SV001 - BC101):
- Midterm (30%): 8.5
- Final (70%): 9.0
- Assignment (20%): 8.0
- Project (30%): 8.7

```
Average = (8.5×30 + 9.0×70 + 8.0×20 + 8.7×30) / (30+70+20+30)
        = (255 + 630 + 160 + 261) / 150
    = 1306 / 150
        = 8.71
   ? B+
```

---

## ?? Testing Tips

### 1. Test Data Reusability
- Use student SV001 for comprehensive testing (has grades in 5 subjects)
- Use student SV003 for excellent performance scenarios
- Use student SV002 for partial grades scenarios

### 2. Edge Cases to Test
? **Create grade with score exactly 9.0** ? Should be "A+" (not "A")  
? **Create grade with score 8.5** ? Should be "A" (not "B+")  
? **Update score from 8.9 to 9.0** ? Letter grade changes "A" ? "A+"  
? **Class with no grades** ? All components show null scores  
? **Student with only 1 component grade** ? Average equals that score  

### 3. Authorization Testing
? Teacher can create/update grades for their classes  
? Student cannot create grades  
? Teacher cannot update other teacher's class grades  
? Admin can do everything  

### 4. Validation Testing
? Score = 10.1 ? Should fail (max 10)  
? Score = -0.1 ? Should fail (min 0)  
? Weight = 101 ? Should fail (max 100)  
? Score = 0 ? Should pass (valid, grade = "F")  
? Score = 10 ? Should pass (valid, grade = "A+")  

---

## ?? Common Issues & Solutions

### Issue 1: 401 Unauthorized
**Problem**: Token expired or not included  
**Solution**: Re-login and get new token, ensure Bearer token in header

### Issue 2: 404 Not Found for Grade ID
**Problem**: Using incorrect GUID or grade doesn't exist  
**Solution**: Query database or use grade ID from create response

### Issue 3: Weighted Average Seems Wrong
**Problem**: Not all components have same total weight  
**Solution**: Average calculated only on components with grades, not fixed at 100%

### Issue 4: Cannot Delete Grade Component
**Problem**: Component is being used by existing grades  
**Solution**: This is correct behavior - cannot delete components in use

### Issue 5: Letter Grade Doesn't Match Score
**Problem**: Possible calculation error  
**Solution**: Check score boundaries - 8.5 is "A", 8.4 is "B+"

---

## ?? Notes

1. **Auto-calculated Fields**: `letterGrade`, `averageScore`, `finalLetterGrade` are all server-calculated, never send from client

2. **Timestamps**: All use UTC timezone, convert to local in frontend

3. **Null Handling**: Components without grades show null score/letterGrade, average excludes these

4. **Grade Updates**: Only score can be updated, letterGrade recalculated automatically

5. **Semester Status**: Grades in closed semesters (BC101, SE201) should not be editable in production

---

## ?? Quick Start Checklist

- [ ] Set up Postman environment with baseUrl
- [ ] Login as each user type and save tokens
- [ ] Test create grade with valid data
- [ ] Test duplicate grade (should fail)
- [ ] View class BC101-A grades (complete data)
- [ ] View class BC202-A grades (partial data)
- [ ] View SV001 transcript (5 subjects)
- [ ] View SV003 transcript (excellent grades)
- [ ] Test update grade and verify letter grade changes
- [ ] Test grade component CRUD as admin
- [ ] Verify authorization (teacher vs student vs admin)
- [ ] Test all edge cases listed above

---

## ?? Support

If you encounter any issues:
1. Check this guide's troubleshooting section
2. Verify data IDs match seeded database
3. Confirm token is valid and not expired
4. Check API logs for detailed error messages

**Happy Testing! ??**
