Use master
Create Database MyGanDB
Go


Use MyGanDB


create table Kindergarten(
KindergartenID INT identity(1,1) PRIMARY KEY NOT NULL,
Name NVARCHAR(255) NOT NULL
);

create table StatusType(
StatusID INT identity(1,1) PRIMARY KEY NOT NULL,
Description NVARCHAR(255) NOT NULL
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
REFERENCES Kindergarten(KindergartenID)
);

create table PendingTeachers(
UserID INT NOT NULL,
CONSTRAINT FK_PendingTeacher FOREIGN KEY (UserID)
REFERENCES Users(UserID),
GroupID INT NOT NULL,
CONSTRAINT FK_PendingTeacherGroup FOREIGN KEY (GroupID)
REFERENCES Groups(GroupID),
StatusID INT NOT NULL,
CONSTRAINT FK_PendingTeacherStatus FOREIGN KEY (StatusID)
REFERENCES StatusType(StatusID),
CONSTRAINT PK_PendingTeachers PRIMARY KEY(UserID, GroupID),
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
StudentID NVARCHAR(255) PRIMARY KEY NOT NULL,
LastName NVARCHAR(255) NOT NULL,
BirthDate DATETIME NOT NULL DEFAULT GETDATE(),
FirstName NVARCHAR(255) NOT NULL,
Gender NVARCHAR(255) NOT NULL,
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
StudentID NVARCHAR(255) NOT NULL,
CONSTRAINT FK_StudentOfParent FOREIGN KEY (StudentID)
REFERENCES Students(StudentID),
UserID INT NOT NULL,
CONSTRAINT FK_ParentOfStudent FOREIGN KEY (UserID)
REFERENCES Users(UserID),
RelationToStudentID INT NOT NULL,
CONSTRAINT FK_RelationToStudent FOREIGN KEY (RelationToStudentID)
REFERENCES RelationToStudent(relationToStudentID),
CONSTRAINT PK_StudentOfUsers PRIMARY KEY (StudentID,UserID),
Vaad BIT DEFAULT 0 NOT NULL,
StatusID INT,
CONSTRAINT FK_UserStatus FOREIGN KEY (StatusID)
REFERENCES StatusType(StatusID),
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
StudentID NVARCHAR(255) NOT NULL,
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


insert into RelationToStudent (RelationType) VALUES (N'אבא')
insert into RelationToStudent (RelationType) VALUES (N'אמא')

INSERT INTO StatusType(Description)
VALUES (N'נדחה');

INSERT INTO StatusType(Description)
VALUES (N'מאושר');

INSERT INTO StatusType(Description)
VALUES (N'בהמתנה');







--scaffold-dbcontext "Server=localhost\sqlexpress;Database=MyGanDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models –force
select * from Kindergarten
select * from Groups
insert into Groups (TeacherID, GroupName, KindergartenID) VALUES (1, 'Kuku', 1)
insert into Groups (TeacherID, GroupName, KindergartenID) VALUES (1, 'Kaka', 1)

select * from RelationToStudent

select * from Users
select * from StudentOfUsers
select * from Students
select * from KindergartenManagers
select * from PendingTeachers
select * from StudentAllergies

UPDATE PendingTeachers
SET StatusID= 3 WHERE UserID = 5