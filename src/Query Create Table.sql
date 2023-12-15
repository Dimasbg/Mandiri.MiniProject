CREATE TABLE Users(
	UserId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UserName varchar(255),
	PasswordHash varchar(255),
	Email varchar(255)
)

CREATE TABLE Roles(
	RoleId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] varchar(255)
)

CREATE TABLE UserRoles(
	UserId int NOT NULL,
	RoleId int NOT NULL,
	FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
	FOREIGN KEY (RoleId) REFERENCES Roles(RoleId) ON DELETE CASCADE
)

CREATE TABLE [Events](
	EventId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	[Name] varchar(255),
	[Description] varchar(max),
	[Location] varchar(255),
	StartDateTime datetime,
	EndDateTime datetime
)

CREATE TABLE Registrations(
	RegistrationId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	EventId int NOT NULL,
	UserId int NOT NULL,
	RegistrationDateTime datetime,
	FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
	FOREIGN KEY (EventId) REFERENCES [Events](EventId) ON DELETE CASCADE
)


CREATE TABLE Tickets(
	TicketId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	EventId int NOT NULL,
	TicketType varchar(50),
	Price decimal(18,0),
	FOREIGN KEY (EventId) REFERENCES [Events](EventId) ON DELETE CASCADE
)

CREATE TABLE Purchases(
	PurchaseId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UserId int NOT NULL,
	TicketId int NOT NULL,
	PurchaseDateTime datetime,
	Quantity int,
	FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
	FOREIGN KEY (TicketId) REFERENCES Tickets(TicketId) ON DELETE CASCADE
)

CREATE TABLE Notifications(
	NotificationId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UserId int NOT NULL,
	EventId int NOT NULL,
	Content varchar(255),
	[Timestamp] datetime,
	FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
	FOREIGN KEY (EventId) REFERENCES [Events](EventId) ON DELETE CASCADE
)

CREATE TABLE Feedbacks(
	FeedbackId int NOT NULL IDENTITY(1,1) PRIMARY KEY,
	UserId int NOT NULL,
	EventId int NOT NULL,
	Rating int,
	Comment varchar(max),
	FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
	FOREIGN KEY (EventId) REFERENCES [Events](EventId) ON DELETE CASCADE
)

CREATE UNIQUE INDEX Index_Users
ON Users(UserId,Email)

CREATE UNIQUE INDEX Index_Roles
ON Roles(RoleId,[Name])

CREATE UNIQUE INDEX Index_UserRoles
ON UserRoles(UserId,RoleId)

CREATE UNIQUE INDEX Index_Events
ON [Events](EventId,[Name],StartDateTime,EndDateTime)

CREATE UNIQUE INDEX Index_Registrations
ON Registrations(RegistrationId,EventId,UserId)

CREATE UNIQUE INDEX Index_Tickets
ON Tickets(TicketId,EventId)

CREATE UNIQUE INDEX Index_Purchases
ON Purchases(PurchaseId,UserId,TicketId,PurchaseDateTime)

CREATE UNIQUE INDEX Index_Notifications
ON Notifications(NotificationId,UserId,EventId,[Timestamp])

CREATE UNIQUE INDEX Index_Feedbacks
ON Feedbacks(FeedbackId,UserId,EventId,Rating)