﻿Use master
Create Database MyGanDB
Go

Use MyGanDB


create table Kindergarten(
KindergartenID INT identity(1,1) PRIMARY KEY NOT NULL,
Name NVARCHAR(255) NOT NULL
);

create table Users(
UserID INT identity(1,1) PRIMARY KEY NOT NULL ,
Email NVARCHAR(255) UNIQUE NOT NULL,
Password NVARCHAR(255) NOT NULL,
FName NVARCHAR(255) NOT NULL,
LastName NVARCHAR(255) NOT NULL,
PhoneNumber NVARCHAR(255) NOT NULL,
IsSystemManager BIT DEFAULT 0 NOT NULL,
);

create table Groups(
GroupID INT identity(1,1) PRIMARY KEY NOT NULL,
TeacherID INT NOT NULL,
CONSTRAINT FK_GroupTeacher FOREIGN KEY (TeacherID)
REFERENCES Users(UserID),
GroupName NVARCHAR(255) NOT NULL,
KindergartenID INT NOT NULL,
CONSTRAINT FK_KinderGartenGroup FOREIGN KEY (KindergartenID)
REFERENCES Kindergarten(KindergartenID),
Code NVARCHAR(255) NOT NULL
);

create table Events(
EventID INT identity(1,1) PRIMARY KEY NOT NULL,
EventName NVARCHAR (250) NOT NULL,
EventDate DATETIME NOT NULL DEFAULT GETDATE(),
Duration INT NOT NULL,
GroupID INT NOT NULL,
CONSTRAINT FK_GroupEvent FOREIGN KEY (GroupID)
REFERENCES Groups(GroupID)
);

create table Photos(
ID INT identity(1,1) PRIMARY KEY NOT NULL,
Type NVARCHAR(255) NOT NULL,
Description NVARCHAR(255) NOT NULL,
Date DATETIME NOT NULL,
EventID INT NOT NULL,
CONSTRAINT FK_EventPhotos FOREIGN KEY (EventID)
REFERENCES Events(EventID),
);


create table Grade(
GradeID INT identity(1,1) PRIMARY KEY NOT NULL,
GradeName NVARCHAR(255) NOT NULL
);

create table Students(
StudentID INT PRIMARY KEY NOT NULL,
LastName NVARCHAR(255) NOT NULL,
BirthDate DATETIME NOT NULL DEFAULT GETDATE(),
FirstName INT NOT NULL,
Gender NVARCHAR(255) NOT NULL,
ArrivedFrom NVARCHAR(255) NOT NULL,
PhotoID INT NOT NULL,
GradeID INT NOT NULL,
CONSTRAINT FK_StudentGrade FOREIGN KEY (GradeID)
REFERENCES Grade(GradeID),
GroupID INT NOT NULL,
CONSTRAINT FK_StudentGroup FOREIGN KEY (GroupID)
REFERENCES Groups(GroupID)
);

create table RelationToStudent(
RelationToStudentID INT identity(1,1) PRIMARY KEY NOT NULL,
RelationType NVARCHAR(255) NOT NULL
);

create table StudentOfUsers(
StudentID INT NOT NULL,
CONSTRAINT FK_StudentOfParent FOREIGN KEY (StudentID)
REFERENCES Students(StudentID),
UserID INT NOT NULL,
CONSTRAINT FK_ParentOfStudent FOREIGN KEY (UserID)
REFERENCES Users(UserID),
RelationToStudentID INT NOT NULL,
CONSTRAINT FK_RelationToStudent FOREIGN KEY (RelationToStudentID)
REFERENCES RelationToStudent(relationToStudentID),
CONSTRAINT PK_StudentOfUsers PRIMARY KEY (StudentID,UserID),
Vaad BIT DEFAULT 0 NOT NULL
);

create table KindergartenManagers(
UserID INT NOT NULL,
CONSTRAINT FK_KindergartenUsers FOREIGN KEY (UserID)
REFERENCES Users(UserID),
KindergartenID INT  NOT NULL,
CONSTRAINT FK_KindergartenManagersKindergarten FOREIGN KEY (KindergartenID )
REFERENCES Kindergarten(KindergartenID),
CONSTRAINT PK_KindergartenManager PRIMARY KEY (UserID,KindergartenID)
);

create table StatusType(
StatusID INT identity(1,1) PRIMARY KEY NOT NULL,
Description NVARCHAR(255) NOT NULL
);

create table Approvals(
ApprovalID INT PRIMARY KEY NOT NULL,
CONSTRAINT FK_GroupApproval FOREIGN KEY (ApprovalID)
REFERENCES Groups(GroupID),
Waiting INT NOT NULL,
Approved INT NOT NULL,
StatusID INT NOT NULL,
CONSTRAINT FK_ApprovalStatusType FOREIGN KEY (StatusID)
REFERENCES StatusType(StatusID),
);

create table Signatures(
ApprovalID INT NOT NULL,
CONSTRAINT FK_GroupSignature FOREIGN KEY (ApprovalID)
REFERENCES Approvals(ApprovalID),
UserID INT NOT NULL,
CONSTRAINT FK_UserSignature FOREIGN KEY (UserID)
REFERENCES Users(UserID),
SignatureDate DATETIME NOT NULL DEFAULT GETDATE(),
CONSTRAINT PK_UserApprovals PRIMARY KEY (ApprovalID,UserID)
);

create table Allergies(
allergyID INT identity(1,1) PRIMARY KEY NOT NULL,
allergyName NVARCHAR(255) NOT NULL
);

create table StudentAllergies(
StudentID INT NOT NULL,
CONSTRAINT FK_StudentAllergiesStudents FOREIGN KEY (StudentID)
REFERENCES Students(StudentID),
AllergyID INT NOT NULL,
CONSTRAINT FK_StudentAllergyName FOREIGN KEY (AllergyID)
REFERENCES Allergies(AllergyID),
CONSTRAINT PK_StudentAllergy PRIMARY KEY(StudentID, AllergyID)
);

create table Messages(
MessageID INT PRIMARY KEY NOT NULL,
CONSTRAINT FK_GroupMessage FOREIGN KEY (MessageID)
REFERENCES Groups(GroupID),
Content NVARCHAR(255) NOT NULL,
MessageDate DATETIME NOT NULL DEFAULT GETDATE(),
);


INSERT INTO Grade(GradeName)
VALUES (N'חובה');

INSERT INTO Grade(GradeName)
VALUES (N'טרום חובה');

INSERT INTO Grade(GradeName)
VALUES (N'טרום טרום חובה');

INSERT INTO Users(Email,Password,FName,LastName,PhoneNumber,IsSystemManager)
VALUES('Shira@gmail.com','shira',N'שירה',N'יוסוב','0544963452',1);


INSERT INTO Allergies(allergyName)
VALUES (N'גלוטן');

INSERT INTO Allergies(allergyName)
VALUES (N'שיבולת שועל');

INSERT INTO Allergies(allergyName)
VALUES (N'כוסמת');

INSERT INTO Allergies(allergyName)
VALUES (N'אורז');

INSERT INTO Allergies(allergyName)
VALUES (N'חיטה');

INSERT INTO Allergies(allergyName)
VALUES (N'תירס');

ALTER TABLE Users
ADD IsApproved BIT DEFAULT 0 NOT NULL

--scaffold-dbcontext "Server=localhost\sqlexpress;Database=MyGanDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models –force
