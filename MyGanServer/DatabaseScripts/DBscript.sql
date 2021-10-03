Use master
Create Database MyGanDB
Go

Use MyGanDB
Go


Create Table Users (
ID int Identity primary key,
Email nvarchar(100) not null,
FirstName nvarchar(30) not null,
LastName nvarchar(30) not null,
UserPswd nvarchar(30) not null,
CONSTRAINT UC_Email UNIQUE(Email)
)

Go

INSERT INTO Users VALUES ('kuku@kuku.com','kuku','kaka','1234');
GO




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
userID INT identity(1,1) PRIMARY KEY NOT NULL ,
email NVARCHAR(255) UNIQUE NOT NULL,
password NVARCHAR(255) NOT NULL,
name NVARCHAR(255) NOT NULL,
lastName NVARCHAR(255) NOT NULL,
phoneNumber NVARCHAR(255) NOT NULL,
isSystemManager BIT DEFAULT 0 NOT NULL,
);

create table Students(
studentID INT identity(1,1) PRIMARY KEY NOT NULL,
ID INT NOT NULL,
lastName NVARCHAR(255) NOT NULL,
birthDate DATETIME NOT NULL DEFAULT GETDATE(),
firstName INT NOT NULL,
gender NVARCHAR(255) NOT NULL,
arrivedFrom NVARCHAR(255) NOT NULL,
allergies NVARCHAR(255) NOT NULL,
parentID INT NOT NULL,
CONSTRAINT FK_StudentParent FOREIGN KEY (parentID)
REFERENCES Users(userID),
photoID INT NOT NULL,
grade INT NOT NULL,
kinderGartenID INT NOT NULL
);

create table StudentOfUsers(
studentID INT NOT NULL,
CONSTRAINT FK_StudentOfParent FOREIGN KEY (studentID)
REFERENCES Students(StudentID),
userID INT NOT NULL,
CONSTRAINT FK_ParentOfStudent FOREIGN KEY (userID)
REFERENCES Users(userID),
PRIMARY KEY(studentID,userID),
relationToStudentID NVARCHAR(255) NOT NULL,
CONSTRAINT FK_RelationToStudent FOREIGN KEY (relationToStudentID)
REFERENCES RelationToStudent(relationToStudentID),
CONSTRAINT PK_StudentOfUsers PRIMARY KEY (studentID,userID)
);

create table Groups(
groupID INT identity(1,1) PRIMARY KEY NOT NULL,
teacher NVARCHAR(255) NOT NULL,
groupName NVARCHAR(255) NOT NULL
);

create table RelationToStudent(
relationToStudentID INT identity(1,1) PRIMARY KEY NOT NULL,
relationType NVARCHAR(255) NOT NULL
);

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


