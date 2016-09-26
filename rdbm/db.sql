


CREATE TABLE Address
    (
        Country VARCHAR(50) NOT NUll,
        PostCode VARCHAR(10) NOT NUll,
        HouseNumber VARCHAR(10) NOT NUll,
        City VARCHAR(50),
        Street VARCHAR(50),
        PRIMARY KEY(Country, PostCode, HouseNumber)
    );

CREATE TABLE HeadQuater(
    BuildingName VARCHAR(50) NOT NULL PRIMARY KEY,
    Rooms INT NOT NULL,
    Rent money NOT NULL,
    Country VARCHAR(50) NOT NUll,
    PostCode VARCHAR(10) NOT NUll,
    HouseNumber VARCHAR(10) NOT NUll,
    CONSTRAINT HeadQuater_Address FOREIGN KEY (Country, PostCode, HouseNumber) REFERENCES Address (Country,PostCode,HouseNumber)
);

CREATE TABLE Employe
    (
        BSN int PRIMARY KEY NOT NUll,
        Name VARCHAR(100) NOT NULL,
        SurName VARCHAR(100) NOT NUll,
        BuildingName VARCHAR(50) NOT NULL,
        CONSTRAINT Employe_HeadQuater FOREIGN KEY (BuildingName) REFERENCES HeadQuater (BuildingName)
    );

CREATE TABLE EmployeAddress
    (
        BSN int NOT NUll,
        Country VARCHAR(50) NOT NUll,
        PostCode VARCHAR(10) NOT NUll,
        HouseNumber VARCHAR(10) NOT NUll,
        Residence BIT NOT NULL,
        CONSTRAINT Employe_EmployeAddress FOREIGN KEY (BSN) REFERENCES Employe (BSN),
        CONSTRAINT Address_EmployeAddress FOREIGN KEY (Country, PostCode, HouseNumber) REFERENCES Address (Country,PostCode,HouseNumber),
        CONSTRAINT EmployeAddress_PK PRIMARY KEY  (BSN, Country, PostCode, HouseNumber)
    );

--choose key

CREATE TABLE Degree
    (
        Course VARCHAR(150) NOT NUll,
        School VARCHAR(150) NOT NUll,
        Level VARCHAR(100) NOT NULL,
        BSN int Not NULL,
        CONSTRAINT Epmloye_Degree FOREIGN KEY (BSN) REFERENCES Employe (BSN)
    );



CREATE TABLE Project(
    ProjectID int IDENTITY PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Budget money NOT NULL,
    Hours int NOT NULL,
    BuildingName VARCHAR(50) NOT NULL,
    CONSTRAINT Project_HeadQuater FOREIGN KEY (BuildingName) REFERENCES HeadQuater (BuildingName)
);

CREATE TABLE Position(
    PositionName VARCHAR(100) NOT NUll,
    Description TEXT,
    HourFee money NOT NULL,
    BSN int NOT NULL,
	ProjectID int,
    CONSTRAINT Employe_Position FOREIGN KEY (BSN) REFERENCES Employe (BSN),
	CONSTRAINT Project_Position FOREIGN KEY (ProjectID) REFERENCES Project (ProjectID),
	PRIMARY KEY (BSN, PositionName, ProjectID)
);



