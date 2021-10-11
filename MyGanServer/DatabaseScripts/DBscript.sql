Use master
Create Database MyGanDB
Go

Use MyGanDB
Go





create table Events(
eventID INT identity(1,1) PRIMARY KEY NOT NULL,
eventName NVARCHAR (250) NOT NULL,
eventDate DATETIME NOT NULL,
duration INT NOT NULL,
groupID INT NOT NULL
);

create table Photos(
ID INT identity(1,1) PRIMARY KEY NOT NULL,
type NVARCHAR(255) NOT NULL,
description NVARCHAR(255) NOT NULL,
date DATETIME NOT NULL,
eventID INT NOT NULL,
CONSTRAINT FK_EventPhotos FOREIGN KEY (eventID)
REFERENCES Events(eventID),
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

create table Students(
StudentID INT identity(1,1) PRIMARY KEY NOT NULL,
SastName NVARCHAR(255) NOT NULL,
BirthDate DATETIME NOT NULL DEFAULT GETDATE(),
FirstName INT NOT NULL,
Gender NVARCHAR(255) NOT NULL,
ArrivedFrom NVARCHAR(255) NOT NULL,
Allergies NVARCHAR(255) NOT NULL,
PhotoID INT NOT NULL,
Grade INT NOT NULL,
GroupID INT NOT NULL,
CONSTRAINT FK_StudentGroup FOREIGN KEY (GroupID)
REFERENCES Groups(GroupID)
);

create table StudentOfUsers(
StudentID INT NOT NULL,
CONSTRAINT FK_StudentOfParent FOREIGN KEY (StudentID)
REFERENCES Students(StudentID),
UserID INT NOT NULL,
CONSTRAINT FK_ParentOfStudent FOREIGN KEY (UserID)
REFERENCES Users(UserID),
RelationToStudentID NVARCHAR(255) NOT NULL,
CONSTRAINT FK_RelationToStudent FOREIGN KEY (RelationToStudentID)
REFERENCES RelationToStudent(relationToStudentID),
CONSTRAINT PK_StudentOfUsers PRIMARY KEY (StudentID,UserID),
Vaad BIT DEFAULT 0 NOT NULL
);


create table Kindergarten(
KindergartenID INT identity(1,1) PRIMARY KEY NOT NULL,
Name NVARCHAR(255) NOT NULL
);


create table KindergartenManagers(
UserID INT NOT NULL,
KindergartenID INT  NOT NULL,
CONSTRAINT FK_KindergartenManager FOREIGN KEY (KindergartenID)
REFERENCES Kindergarten(KindergartenID),
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

create table Approvals(
ApprovalID INT PRIMARY KEY NOT NULL,
CONSTRAINT FK_GroupApproval FOREIGN KEY (ApprovalID)
REFERENCES Groups(GroupID),
Waiting INT NOT NULL,
Approved INT NOT NULL,
Status NVARCHAR(255) NOT NULL
);

create table Signatures(
ApprovalID INT NOT NULL,
CONSTRAINT FK_GroupSignature FOREIGN KEY (ApprovalID)
REFERENCES Approvals(ApprovalID),
UserID INT NOT NULL,
CONSTRAINT FK_UserSignature FOREIGN KEY (UserID)
REFERENCES Users(UserID),
);

create table RelationToStudent(
relationToStudentID INT identity(1,1) PRIMARY KEY NOT NULL,
relationType NVARCHAR(255) NOT NULL
)
;

create table Allergies(
allergyID INT identity(1,1) PRIMARY KEY NOT NULL,
allergyName NVARCHAR(255) NOT NULL
);

create table StudentAllergies(
studentID INT NOT NULL,
CONSTRAINT FK_StudentAllergies FOREIGN KEY (studentID)
REFERENCES Students(studentID),
allergyID INT NOT NULL,
CONSTRAINT FK_StudentAllergyName FOREIGN KEY (allergyID)
REFERENCES Allergies(allergyID),
PRIMARY KEY(studentID, allergyID)
);


